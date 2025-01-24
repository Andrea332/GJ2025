using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ItemData itemData;
        public ItemData ItemData => itemData;
    }
}
