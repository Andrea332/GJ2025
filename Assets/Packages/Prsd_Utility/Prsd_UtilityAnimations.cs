using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// Behaviour for quickly set up simple animations, mainly thought for UI elements.
/// </summary>
public class Prsd_UtilityAnimations : MonoBehaviour
{
    public enum PopOptions { DefaultScale, Fade, CustomAnimation }

    [HorizontalGroup("cb"), ToggleLeft]
    public bool resetValuesOnPopIn = true;
    [HorizontalGroup("cb"), ToggleLeft]
    public bool allowCombiningAnimations;

    public Animation[] customAnimations;

    [HorizontalGroup("oe")]
    public PopOptions popInAnimation;
    [HorizontalGroup("oe", MarginLeft = 10f), ToggleLeft, LabelWidth(80f)]
    public bool popInOnEnable = true;
    [Indent, ShowIf("@" + nameof(popInAnimation) + "!=" + nameof(PopOptions) + "." + nameof(PopOptions.CustomAnimation))]
    public Ease popInEase = Ease.OutBack;
    [Indent, ShowIf("@" + nameof(popInAnimation) + "==" + nameof(PopOptions) + "." + nameof(PopOptions.CustomAnimation)), LabelText("Index")]
    public int popInAnimationIndex;
    [Indent, ShowIf("@" + nameof(popInAnimation) + "==" + nameof(PopOptions) + "." + nameof(PopOptions.Fade)), LabelText("Target Alpha")]
    public float popInTargetFade = 1f;
    [HorizontalGroup("od")]
    public PopOptions popOutAnimation;
    [HorizontalGroup("od", MarginLeft = 10f), ToggleLeft, LabelWidth(80f)]
    public bool disableOnPopOut = true;
    [Indent, ShowIf("@" + nameof(popOutAnimation) + "!=" + nameof(PopOptions) + "." + nameof(PopOptions.CustomAnimation))]
    public Ease popOutEase = Ease.InBack;
    [Indent, ShowIf("@" + nameof(popOutAnimation) + "==" + nameof(PopOptions) + "." + nameof(PopOptions.CustomAnimation)), LabelText("Index")]
    public int popOutAnimationIndex;
    [Indent, ShowIf("@" + nameof(popOutAnimation) + "==" + nameof(PopOptions) + "." + nameof(PopOptions.Fade)), LabelText("Target Alpha")]
    public float popOutTargetFade = 0f;

    public float duration = 0.3f;
    public float punchIntensity = 0.1f;
    public float shakeIntensity = 0.2f;

    Vector3 startPosition;
    Vector3 startRotation;
    Vector3 startScale;

    Action pendingOnComplete;

    public float Duration
    {
        get => duration;
        set
        {
            duration = Mathf.Max(0.01f, value);
        }
    }

    void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = transform.localEulerAngles;
        startScale = transform.localScale;
    }

    void OnEnable()
    {
        if (popInOnEnable)
        {
            PopIn(pendingOnComplete);
            pendingOnComplete = null;
        }
    }

    void DisableIfRequested()
    {
        if (disableOnPopOut)
        {
            gameObject.SetActive(false);
        }
    }

    public void Stop()
    {
        transform.DOKill();
        if (TryGetComponent(out CanvasGroup group))
        {
            group.DOKill();
        }
        else if (TryGetComponent(out Graphic graphic))
        {
            graphic.DOKill();
        }
    }

    [Button, HorizontalGroup("btn"), DisableInEditorMode]
    public void PopIn()
    {
        PopIn(null);
    }

    public void PopIn(Action onComplete)
    {
        if (popInOnEnable && !gameObject.activeSelf)
        {
            pendingOnComplete = onComplete;
            gameObject.SetActive(true);
        }
        else
        {
            if (resetValuesOnPopIn)
            {
                transform.DOKill();
                transform.localPosition = startPosition;
                transform.localEulerAngles = startRotation;
                transform.localScale = startScale;
                if (popOutAnimation == PopOptions.Fade)
                    if (TryGetComponent(out CanvasGroup group))
                    {
                        group.alpha = 0f;
                    }
                    else if (TryGetComponent(out Graphic graphic))
                    {
                        graphic.color = graphic.color.ChangeAlpha(0f);
                    }
            }

            if (TryGetComponent(out Canvas canvas))
            {
                canvas.enabled = true;
            }

            Action onCompleteModified = onComplete;
            if (popInOnEnable)
            {
                onCompleteModified = () => { onComplete?.Invoke(); SetInteractable(true); };
            }

            switch (popInAnimation)
            {
                case PopOptions.DefaultScale:
                    PopIn(0f, 0f, onCompleteModified);
                    break;
                case PopOptions.Fade:
                    Fade(popInTargetFade, popInEase, 0f, 0f, onCompleteModified);
                    break;
                case PopOptions.CustomAnimation:
                    PlayCustomAnimation(popInAnimationIndex, 0f, 0f, onCompleteModified);
                    break;
            }
        }
    }

    public void PopIn(float durationOverride, float delay = 0f, Action onComplete = null)
    {
        if (durationOverride <= 0f) durationOverride = duration;
        switch (popInAnimation)
        {
            case PopOptions.DefaultScale:
                if (!allowCombiningAnimations) transform.DOKill();
                transform.localScale = Vector3.zero;
                transform.DOScale(1f, durationOverride).SetDelay(delay).SetUpdate(true).SetEase(popInEase).OnComplete(() => onComplete?.Invoke());
                break;
            case PopOptions.Fade:
                Fade(popInTargetFade, popInEase, durationOverride, delay, onComplete);
                break;
            case PopOptions.CustomAnimation:
                PlayCustomAnimation(popInAnimationIndex, durationOverride, delay, onComplete);
                break;
        }
    }

    [Button, HorizontalGroup("btn"), DisableInEditorMode]
    public void PopOut()
    {
        PopOut(null);
    }

    public void PopOut(Action onComplete)
    {
        if (disableOnPopOut) SetInteractable(false);
        if (TryGetComponent(out Canvas canvas))
        {
            onComplete += () => canvas.enabled = false;
        }
        switch (popOutAnimation)
        {
            case PopOptions.DefaultScale:
                PopOut(0f, 0f, onComplete);
                break;
            case PopOptions.Fade:
                Fade(popOutTargetFade, popOutEase, 0f, 0f, () => { DisableIfRequested(); onComplete?.Invoke(); });
                break;
            case PopOptions.CustomAnimation:
                PlayCustomAnimation(popOutAnimationIndex, 0f, 0f, () => { DisableIfRequested(); onComplete?.Invoke(); });
                break;
        }
    }

    public void PopOut(float durationOverride, float delay = 0f, Action onComplete = null)
    {
        if (durationOverride <= 0f) durationOverride = duration;
        switch (popOutAnimation)
        {
            case PopOptions.DefaultScale:
                if (!allowCombiningAnimations) transform.DOKill();
                transform.DOScale(0f, durationOverride).SetDelay(delay).SetUpdate(true).SetEase(popOutEase).OnComplete(() => { DisableIfRequested(); onComplete?.Invoke(); });
                break;
            case PopOptions.Fade:
                Fade(popOutTargetFade, popOutEase, durationOverride, delay, onComplete);
                break;
            case PopOptions.CustomAnimation:
                PlayCustomAnimation(popOutAnimationIndex, durationOverride, delay, () => { DisableIfRequested(); onComplete?.Invoke(); });
                break;
        }
    }

    public void PopInAndOut(float durationOverride, float showTime = 0f, Action onPopIn = null, Action onComplete = null)
    {
        if (durationOverride <= 0f) durationOverride = duration;
        PopIn(durationOverride, 0f, () => { onPopIn?.Invoke(); PopOut(durationOverride, Mathf.Max(0f, showTime), onComplete); });
    }

    public void PopOutAndIn(float durationOverride, float hideTime = 0f, Action onPopOut = null, Action onComplete = null)
    {
        if (durationOverride <= 0f) durationOverride = duration;
        PopOut(durationOverride, 0f, () => { onPopOut?.Invoke(); PopIn(durationOverride, Mathf.Max(0f, hideTime), onComplete); });
    }

    [Button, HorizontalGroup("btn"), DisableInEditorMode]
    public void Punch()
    {
        Punch(duration);
    }

    public void Punch(float durationOverride, float delay = 0, Action onComplete = null)
    {
        if (durationOverride <= 0f) durationOverride = duration;
        if (!allowCombiningAnimations) transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(punchIntensity * Vector3.one, durationOverride).SetDelay(delay).SetUpdate(true).OnComplete(() => onComplete?.Invoke());
    }

    [Button, HorizontalGroup("btn"), DisableInEditorMode]
    public void Shake()
    {
        Shake(duration);
    }

    public void Shake(float durationOverride, float delay = 0, Action onComplete = null)
    {
        if (durationOverride <= 0f) durationOverride = duration;
        if (!allowCombiningAnimations) transform.DOKill();
        transform.DOShakePosition(durationOverride, shakeIntensity).SetDelay(delay).SetUpdate(true).OnComplete(() => onComplete?.Invoke());
    }

    public void FadeIn()
    {
        Fade(1f, popInEase);
    }

    public void FadeOut()
    {
        Fade(0f, popOutEase);
    }

    public void FadeIn(float durationOverride)
    {
        Fade(1f, popInEase, durationOverride);
    }

    public void FadeOut(float durationOverride)
    {
        Fade(0f, popOutEase, durationOverride);
    }

    public void FadeIn(Action onComplete)
    {
        Fade(1f, popInEase, 0f, 0f, onComplete);
    }

    public void FadeOut(Action onComplete)
    {
        Fade(0f, popOutEase, 0f, 0f, onComplete);
    }

    public void Fade(float endValue, Ease ease = 0, float durationOverride = 0f, float delay = 0f, Action onComplete = null)
    {
        if (durationOverride <= 0f) durationOverride = duration;
        var group = GetComponent<CanvasGroup>();
        if (group)
        {
            if (!allowCombiningAnimations) group.DOKill();
            group.DOFade(endValue, durationOverride).SetDelay(delay).SetUpdate(true).SetEase(ease).OnComplete(() => onComplete?.Invoke());
        }
        else
        {
            var graphic = GetComponent<Graphic>();
            if (graphic)
            {
                if (!allowCombiningAnimations) graphic.DOKill();
                graphic.DOFade(endValue, durationOverride).SetDelay(delay).SetUpdate(true).SetEase(ease).OnComplete(() => onComplete?.Invoke());
            }
        }
    }

    public void FadeInAndOut(float minValue = 0f, float maxValue = 1f, Ease ease = 0, float durationOverride = 0f, float delay = 0f, float delayInBetween = 0f, Action onComplete = null)
    {
        Fade(maxValue, ease, durationOverride, delay, () => Fade(minValue, ease, durationOverride, delayInBetween, onComplete));
    }

    public void SetInteractable(bool value)
    {
        if (TryGetComponent(out CanvasGroup group)) group.interactable = value;
        else if (TryGetComponent(out Button button)) button.interactable = value;
    }

    #region CustomAnimations
    public enum AnimationType { Move, Rotate, Scale, Fade }
    public enum AnimationMode { Absolute, Relative }

    [Serializable]
    public struct AnimationElement
    {
        [HorizontalGroup("a"), HideLabel]
        public AnimationType type;
        [HorizontalGroup("a"), HideIf("type", AnimationType.Fade), HideLabel]
        public AnimationMode mode;
        [EnumToggleButtons, HideIf("type", AnimationType.Fade), HideLabel]
        public SnapAxis axis;
        [HorizontalGroup("c"), HideIf("type", AnimationType.Fade), DisableIf(nameof(startFromCurrent))]
        public Vector3 startValue;
        [HorizontalGroup("c"), ShowIf("type", AnimationType.Fade), DisableIf(nameof(startFromCurrent))]
        public float startFloatValue;
        [HorizontalGroup("c", MarginLeft = 10f), ToggleLeft, LabelText("Current"), LabelWidth(50f)]
        public bool startFromCurrent;
        [HorizontalGroup("d"), DisableIf(nameof(endToCurrent)), HideIf("type", AnimationType.Fade)]
        public Vector3 endValue;
        [HorizontalGroup("d"), DisableIf(nameof(endToCurrent)), ShowIf("type", AnimationType.Fade)]
        public float endFloatValue;
        [HorizontalGroup("d", MarginLeft = 10f), ToggleLeft, LabelText("Current"), LabelWidth(50f)]
        public bool endToCurrent;
        public Ease ease;

        public Vector3 GetStartValue(Vector3 current)
        {
            Vector3 v = current;
            if (!startFromCurrent)
            {
                if (mode == AnimationMode.Absolute)
                {
                    if ((axis & SnapAxis.X) != 0) v.x = startValue.x;
                    if ((axis & SnapAxis.Y) != 0) v.y = startValue.y;
                    if ((axis & SnapAxis.Z) != 0) v.z = startValue.z;
                }
                else
                {
                    if ((axis & SnapAxis.X) != 0) v.x += startValue.x;
                    if ((axis & SnapAxis.Y) != 0) v.y += startValue.y;
                    if ((axis & SnapAxis.Z) != 0) v.z += startValue.z;
                }
            }
            return v;
        }

        public Vector3 GetEndValue(Vector3 current)
        {
            Vector3 v = current;
            if (!endToCurrent)
            {
                if (mode == AnimationMode.Absolute)
                {
                    if ((axis & SnapAxis.X) != 0) v.x = endValue.x;
                    if ((axis & SnapAxis.Y) != 0) v.y = endValue.y;
                    if ((axis & SnapAxis.Z) != 0) v.z = endValue.z;
                }
                else
                {
                    if ((axis & SnapAxis.X) != 0) v.x += endValue.x;
                    if ((axis & SnapAxis.Y) != 0) v.y += endValue.y;
                    if ((axis & SnapAxis.Z) != 0) v.z += endValue.z;
                }
            }
            return v;
        }

        public float GetStartFloatValue(float current)
        {
            float v = current;
            if (!startFromCurrent)
            {
                return startFloatValue;
            }

            return v;
        }

        public float GetEndFloatValue(float current)
        {
            float v = current;
            if (!endToCurrent)
            {
                return endFloatValue;
            }
            return v;
        }
    }

    [Serializable]
    public struct Animation
    {
        public float duration;
        public AnimationElement[] elements;
    }

    [ShowIf("@" + nameof(customAnimations) + ".Length>0"), Button(Expanded = true)]
    public void PlayCustomAnimation(int index)
    {
        if (index >= 0 && index < customAnimations.Length)
        {
            PlayCustomAnimation(index, customAnimations[index].duration, 0f, null);
        }
    }

    public void PlayCustomAnimation(int index, float durationOverride = 0f, float delay = 0f, Action onComplete = null)
    {
        if (index >= 0 && index < customAnimations.Length)
        {
            PlayCustomAnimation(customAnimations[index], durationOverride, delay, onComplete);
        }
    }

    public void PlayCustomAnimation(Animation a, float durationOverride = 0f, float delay = 0f, Action onComplete = null)
    {
        if (!allowCombiningAnimations) transform.DOKill();
        if (durationOverride <= 0f) durationOverride = a.duration;
        for (int i = 0; i < a.elements.Length; i++)
        {
            Action _onComplete = null;

            if (i == a.elements.Length - 1)
            {
                _onComplete = onComplete;
            }

            var e = a.elements[i];
            Vector3 end;
            switch (e.type)
            {
                case AnimationType.Move:
                    end = e.GetEndValue(transform.localPosition);
                    transform.localPosition = e.GetStartValue(transform.localPosition);
                    transform.DOLocalMove(end, durationOverride).SetDelay(delay).SetUpdate(true).SetEase(e.ease).OnComplete(() => _onComplete?.Invoke());
                    break;
                case AnimationType.Rotate:
                    end = e.GetEndValue(transform.localEulerAngles);
                    transform.localEulerAngles = e.GetStartValue(transform.localEulerAngles);
                    transform.DOLocalRotate(end, durationOverride, RotateMode.FastBeyond360).SetDelay(delay).SetUpdate(true).SetEase(e.ease).OnComplete(() => _onComplete?.Invoke());
                    break;
                case AnimationType.Scale:
                    end = e.GetEndValue(transform.localScale);
                    transform.localScale = e.GetStartValue(transform.localScale);
                    transform.DOScale(end, durationOverride).SetDelay(delay).SetUpdate(true).SetEase(e.ease).OnComplete(() => _onComplete?.Invoke());
                    break;
                case AnimationType.Fade:

                    var group = GetComponent<CanvasGroup>();
                    if (group)
                    {
                        group.DOKill();
                        group.alpha = e.GetStartFloatValue(group.alpha);
                    }
                    else
                    {
                        var graphic = GetComponent<Graphic>();
                        if (graphic)
                        {
                            graphic.DOKill();
                            Color _color = graphic.color;
                            _color.a = e.GetStartFloatValue(_color.a);
                            graphic.color = _color;
                        }
                    }

                    Fade(e.endFloatValue, e.ease, durationOverride, delay, () => _onComplete?.Invoke());
                    break;
            }
        }
    }
    #endregion
}


public static class Prsd_UtilityAnimationsExtensions
{
    /// <summary>
    /// Returns the MN_UtilityAnimation component attached on the game object.
    /// </summary>
    public static Prsd_UtilityAnimations MNAnimations(this Component m)
    {
        return m.GetComponent<Prsd_UtilityAnimations>();
    }

    /// <summary>
    /// Returns the MN_UtilityAnimation component attached on the game object.
    /// </summary>
    public static Prsd_UtilityAnimations MNAnimations(this GameObject m)
    {
        return m.GetComponent<Prsd_UtilityAnimations>();
    }
}
