using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public WorldPartition[] worldPartitions;
    public WorldPartition startPartition;

    static readonly HashSet<string> visitedPartitions = new();

    public static bool HasVisitedPartition(string id)
    {
        return visitedPartitions.Contains(id);
    }

    private void Start()
    {
        LoadPartition(startPartition.Id);
    }

    public void LoadPartition(string id)
    {
        for (int i = 0; i < worldPartitions.Length; i++)
        {
            bool active = worldPartitions[i].Id == id;
            worldPartitions[i].gameObject.SetActive(active);
            if (active && !visitedPartitions.Contains(id)) visitedPartitions.Add(id);
        }
    }
}
