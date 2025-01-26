using System;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [Serializable]
    public struct MapSection
    {
        public string id;
        public Transform pos;
    }

    public Transform dot;
    public MapSection[] sections;

    private void Start()
    {
        WorldManager.PartitionChanged += WorldManager_PartitionChanged;
    }

    private void WorldManager_PartitionChanged()
    {
        Vector2 pos = dot.position;
        string currentId = WorldManager.CurrentPartitionVisitedId;
        for (int i = 0; i < sections.Length; i++)
        {
            if (sections[i].id == currentId)
            {
                pos = sections[i].pos.position;
                break;
            }
        }
        dot.position = pos;
    }
}
