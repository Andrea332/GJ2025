using Sirenix.OdinInspector;
using UnityEngine;

public class Teleporter : Interactable
{
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] string targetPartitionId;
    [SerializeField] Game.DataEventString loadPartitionEvent;
    [SerializeField] Prsd_AudioSet moveAudio;

    public string TargetPartitionId => targetPartitionId;

    protected override void OnInteract(Vector2 worldInteractPosition)
    {
        loadPartitionEvent.Raise(targetPartitionId);
        if (moveAudio) Prsd_AudioManager.PlaySoundFx(moveAudio);
    }
}
