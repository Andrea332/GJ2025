using System;
using UnityEngine;

public class BackgroundSelector : MonoBehaviour
{
    [SerializeField] SpriteRenderer background;
    [SerializeField] Sprite defaultSprite;

    [SerializeField] Lock[] lockConditions;

    [Serializable]
    public struct SpriteCondition
    {
        public Sprite sprite;
        public bool[] conditions;
    }

    [SerializeField] SpriteCondition[] sprites;

    void OnEnable()
    {
        SelectBackground();

        for (int i = 0; i < lockConditions.Length; i++)
        {
            lockConditions[i].LockUnlocked += SelectBackground;
        }
    }

    void OnDisable()
    {
        for (int i = 0; i < lockConditions.Length; i++)
        {
            lockConditions[i].LockUnlocked -= SelectBackground;
        }
    }

    public void SelectBackground()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            bool success = true;
            for (int j = 0; j < sprites[i].conditions.Length; j++)
            {
                if (sprites[i].conditions[j] != lockConditions[j].Unlocked)
                {
                    success = false;
                    break;
                }
            }

            if (success)
            {
                background.sprite = sprites[i].sprite;
                return;
            }
        }

        background.sprite = defaultSprite;
    }

    void OnValidate()
    {
        if (sprites == null || lockConditions == null) return;
        int count = lockConditions.Length;
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].conditions.Length != count)
            {
                Array.Resize(ref sprites[i].conditions, count);
            }
        }
    }
}
