using UnityEngine;

public class Prsd_AudioSetPlayer : MonoBehaviour
{
    enum PlayMode { Instant, CrossFade, CrossFadeSync }

    [SerializeField] Prsd_AudioSet audioSet;
    [SerializeField] PlayMode playMode;
    [SerializeField] bool playOnEnable = true;

    void OnEnable()
    {
        if (playOnEnable) Play();
    }

    public void Play()
    {
        if (!audioSet) return;
        switch (playMode)
        {
            case PlayMode.Instant:
                Prsd_AudioManager.PlaySoundSet(audioSet);
                break;
            case PlayMode.CrossFade:
                Prsd_AudioManager.TrackCrossFadeRequest(audioSet);
                break;
            case PlayMode.CrossFadeSync:
                Prsd_AudioManager.TrackCrossFadeTimeSyncedRequest(audioSet);
                break;
        }
    }

    public void PlayInstant(Prsd_AudioSet set)
    {
        if (set) Prsd_AudioManager.PlaySoundSet(set);
    }
}
