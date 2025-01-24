using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public List<GameObject> worldPartitions;
    public GameObject currentWorldPartition;

    private void Start()
    {
        foreach (var worldPartition in worldPartitions)
        {
            if (currentWorldPartition == worldPartition)
            {
                currentWorldPartition.SetActive(true);
                continue;
            }

            worldPartition.SetActive(false);
        }
    }

    public void LoadAndOverrideWorldPartition(string worldName)
    {
        currentWorldPartition.SetActive(false);
        if (currentWorldPartition.name == worldName) return;
        currentWorldPartition = worldPartitions.Find(worldPartition => worldPartition.name == worldName);
        currentWorldPartition.SetActive(true);
    }
}
