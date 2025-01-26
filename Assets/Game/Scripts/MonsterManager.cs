using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public int playerHeath = 3;
    public int stepCounter = 6;
    [Space]
    public string startPartitionId = "1";
    public string monsterActivationPartitionId = "5";
    public string animationState = "Attack";
    public float animationDuration = 1f;
    public Game.DataEventString loadPartitionEvent;
    public List<string> monsterSpawnPartitionsIds;
    public string[] warpPartitionsIds;

    public Animator monster;

    public int CurrentPlayerHealth { get; private set; }
    public int Count { get; private set; }

    string partitionIdAfterAnimation;

    public bool MonsterActive => Count >= stepCounter;

    static MonsterManager instance;

    void OnEnable()
    {
        instance = this;
        CurrentPlayerHealth = playerHeath;
        WorldManager.PartitionChanged += WorldManager_PartitionChanged;
    }

    void OnDisable()
    {
        WorldManager.PartitionChanged -= WorldManager_PartitionChanged;
    }

    void WorldManager_PartitionChanged()
    {
        monster.gameObject.SetActive(false);
        Count++;
    }

    public static bool MonsterCheck(Interactable interactable)
    {
        if (!WorldManager.HasVisitedPartition(instance.monsterActivationPartitionId)) return true;
        if (!instance.monsterSpawnPartitionsIds.Contains(WorldManager.CurrentPartitionVisitedId)) return true;
        if (instance.Count < instance.stepCounter) return true;

        if (interactable is Teleporter t)
        {
            if (t.TargetPartitionId == WorldManager.LastPartitionVisitedId)
            {
                instance.Count = 0;
                return true;
            }
            else
            {
                if (--instance.CurrentPlayerHealth > 0)
                {
                    instance.partitionIdAfterAnimation = instance.warpPartitionsIds[Random.Range(0, instance.warpPartitionsIds.Length)];
                }
                else
                {
                    instance.Death();
                }
            }
        }
        else
        {
            instance.Death();
        }

        instance.Count = 0;
        instance.TriggerAnimation();
        return false;
    }

    void Death()
    {
        instance.partitionIdAfterAnimation = instance.startPartitionId;
        Count = 0;
        CurrentPlayerHealth = playerHeath;
    }

    void TriggerAnimation()
    {
        monster.gameObject.SetActive(true);
        monster.Play(animationState);
        Invoke(nameof(LoadPartition), animationDuration);
    }

    void LoadPartition()
    {
        loadPartitionEvent.Raise(partitionIdAfterAnimation);
    }
}
