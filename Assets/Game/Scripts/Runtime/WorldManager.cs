using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public WorldPartition[] worldPartitions;
    public WorldPartition startPartition;
    public Prsd_UtilityAnimations transitionPanel;

    static readonly HashSet<string> visitedPartitions = new();

    public static event Action PartitionChanged;

    public static string LastPartitionVisitedId { get; private set; }
    public static string CurrentPartitionVisitedId { get; private set; }

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
        StopAllCoroutines();
        StartCoroutine(Transition(id));
    }

    IEnumerator Transition(string id)
    {
        if (transitionPanel)
        {
            transitionPanel.PopIn();
            yield return new WaitForSeconds(transitionPanel.Duration);
        }

        for (int i = 0; i < worldPartitions.Length; i++)
        {
            bool active = worldPartitions[i].Id == id;
            worldPartitions[i].gameObject.SetActive(active);
            if (active)
            {
                if (!visitedPartitions.Contains(id)) visitedPartitions.Add(id);
            }
        }

        LastPartitionVisitedId = CurrentPartitionVisitedId;
        CurrentPartitionVisitedId = id;

        PartitionChanged?.Invoke();

        if (transitionPanel)
        {
            transitionPanel.PopOut();
        }
    }
}
