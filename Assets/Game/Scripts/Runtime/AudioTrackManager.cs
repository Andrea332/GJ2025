using UnityEngine;

public class AudioTrackManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] Prsd_AudioSet[] incrementalTracks;

    void OnEnable()
    {
        inventory.ItemAmountChanged += OnItemAmountChanged;
    }

    void OnDisable()
    {
        inventory.ItemAmountChanged -= OnItemAmountChanged;
    }

    private void OnItemAmountChanged(ItemData item, int amount)
    {
        amount = Mathf.Clamp(amount - 1, 0, incrementalTracks.Length - 1);
        Prsd_AudioManager.TrackCrossFadeRequest(incrementalTracks[amount], true);
    }
}
