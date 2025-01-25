using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ/Pick Up Animation")]
public class PickUpAnimation : CoroutineAnimation
{
    public override IEnumerator Animation(GameObject target, Action onComplete)
    {
        yield return null;
        onComplete?.Invoke();
    }
}
