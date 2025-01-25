using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "GGJ/Unlock Animation")]
public class UnlockAnimation : CoroutineAnimation
{
    [SerializeField] Prsd_PoolObject iconPool;
    [SerializeField] float duration = 2f;
    [SerializeField] AnimationCurve curve;

    static Canvas canvas;

    public override IEnumerator Animation(GameObject target, Action onComplete)
    {
        var cam = Camera.main;
        var item = target.TryGetComponent(out Lock lockObj) ? lockObj.RequiredItem : null;
        InventorySlotUI.registeredSlots.TryGetValue(item, out InventorySlotUI uiSlot);
        if (!canvas) canvas = FindFirstObjectByType<Canvas>();

        if (!cam || !item || !uiSlot || duration <= 0f || !canvas)
        {
            onComplete?.Invoke();
            yield break;
        }

        Vector2 startPosition = uiSlot.transform.position;
        var endPosition = cam.WorldToScreenPoint(target.transform.position);

        var icon = iconPool.Get();
        icon.GetComponent<Image>().sprite = item.InventorySprite;
        var transform = icon.transform;
        transform.SetParent(canvas.transform);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            t = Mathf.Clamp01(t);

            transform.position = Vector2.LerpUnclamped(startPosition, endPosition, curve.Evaluate(t));
            yield return null;
        }
        icon.SetActive(false);

        onComplete?.Invoke();
    }
}
