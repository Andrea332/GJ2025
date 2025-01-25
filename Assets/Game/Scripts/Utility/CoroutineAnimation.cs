using System;
using System.Collections;
using UnityEngine;

public abstract class CoroutineAnimation : ScriptableObject
{
    static CoroutineAnimationPlayer player;

    public void Play(GameObject target, Action onComplete = null)
    {
        if (!player)
        {
            player = new GameObject(nameof(CoroutineAnimationPlayer)).AddComponent<CoroutineAnimationPlayer>();
        }

        player.StartCoroutine(Animation(target, onComplete));
    }

    public virtual IEnumerator Animation(GameObject target, Action onComplete)
    {
        yield return null;
        onComplete?.Invoke();
    }

    public static void StopAllAnimations()
    {
        if (player) player.StopAllCoroutines();
    }
}
