using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public void Play(Prsd_AudioSet set)
    {
        Prsd_AudioManager.PlaySoundFx(set);
    }
}
