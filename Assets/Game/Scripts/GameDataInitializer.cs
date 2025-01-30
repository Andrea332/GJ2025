using UnityEngine;

public class GameDataInitializer : MonoBehaviour
{
    [SerializeField] Inventory inventory;

    void Awake()
    {
        GameData.Init(inventory);
    }
}
