using UnityEngine;

public class WorldPartition : MonoBehaviour
{
    [SerializeField] string id;
    [SerializeField] bool canHaveMonster;

    public string Id => id;

    public bool CanHaveMonster => canHaveMonster;
}
