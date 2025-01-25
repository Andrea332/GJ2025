using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public WorldPartition[] worldPartitions;
    public WorldPartition startPartition;

    private void Start()
    {
        LoadPartition(startPartition.Id);
    }

    public void LoadPartition(string id)
    {
        for (int i = 0; i < worldPartitions.Length; i++)
        {
            worldPartitions[i].gameObject.SetActive(worldPartitions[i].Id == id);
        }
    }
}
