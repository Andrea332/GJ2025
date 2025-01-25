using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "GGJ/Pick Up Animation")]
public class PickUpAnimation : CoroutineAnimation
{
    [SerializeField] Prsd_PoolObject iconPool;
    [SerializeField] float duration = 2f;
    [SerializeField] AnimationCurve curve;

    static Canvas canvas;

    public override IEnumerator Animation(GameObject target, Action onComplete)
    {
        var cam = Camera.main;
        var item = target.GetComponent<Item>();
        InventorySlotUI.registeredSlots.TryGetValue(item.ItemData.Id, out InventorySlotUI uiSlot);
        if (!canvas) canvas = FindFirstObjectByType<Canvas>();

        if (!cam || !item || !uiSlot || duration <= 0f || !canvas)
        {
            onComplete?.Invoke();
            yield break;
        }

        var startPosition = cam.WorldToScreenPoint(target.transform.position);

        var icon = iconPool.Get();
        icon.GetComponent<Image>().sprite = item.ItemData.InGameSprite;
        var transform = icon.transform;
        transform.SetParent(canvas.transform);

        Vector2 endPosition = uiSlot.transform.position;

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
