using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

[HideMonoScript]
public class Prsd_AudioManager : MonoBehaviour
{
    [SerializeField] bool showLogs;
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource[] sfxSources;
    [SerializeField] AudioSource trackSource;
    [SerializeField] AudioSource transitionSource;
    [SerializeField] float defaultFadeDuration = 0.5f;

    public static void PlaySoundFx(Prsd_AudioSet set) { if (instance) instance.PlaySound(set); }
    public static void TrackFadeInRequest(Prsd_AudioSet set) { if (instance) instance.FadeIn(set); }
    public static void TrackFadeOutRequest(Prsd_AudioSet set) { if (instance) instance.FadeOut(); }
    public static void TrackCrossFadeRequest(Prsd_AudioSet set, bool syncTime) { if (instance) instance.CrossFade(set, syncTime); }

    readonly List<Prsd_AudioSet> playedThisFrame = new();

    float lastTrackVolume = 1f;

    static Prsd_AudioManager instance;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void OnEnable()
    {
        transitionSource.enabled = false;
    }

    void LateUpdate()
    {
        playedThisFrame.Clear();
    }

    public void PlaySound(Prsd_AudioSet sound)
    {
        if (!playedThisFrame.Contains(sound))
        {
            playedThisFrame.Add(sound);
            if (sound.type == Prsd_AudioSet.Type.SFX)
            {
                if (sfxSources.Length > 0)
                {
                    int index = Mathf.Clamp(sound.sfxSource, 0, sfxSources.Length);
                    sfxSources[index].PlayOneShot(sound.GetSound(), sound.volume);
                }
            }
            else
            {
                trackSource.clip = sound.GetSound();
                trackSource.volume = sound.volume;
                trackSource.loop = sound.loop;
                trackSource.Play();
            }

            if (showLogs) Debug.Log($"<color=#4cff99>Audio: {sound.name}</color>");
        }
    }

    public void PlaySound(AudioClip clip, float volumeScale = 1f)
    {
        if (clip)
        {
            trackSource.PlayOneShot(clip, volumeScale);
        }
    }

    #region FADE

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(trackSource, lastTrackVolume, defaultFadeDuration));
    }

    public void FadeOut()
    {
        lastTrackVolume = trackSource.volume;
        StopAllCoroutines();
        StartCoroutine(Fade(trackSource, 0f, defaultFadeDuration));
    }

    public void FadeIn(Prsd_AudioSet track)
    {
        if (track.type != Prsd_AudioSet.Type.Track) return;
        StopAllCoroutines();
        PlaySound(track);
        trackSource.volume = 0f;
        StartCoroutine(Fade(trackSource, track.volume, defaultFadeDuration));
    }

    public void Fade(float targetVolume)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(trackSource, targetVolume, defaultFadeDuration));
    }

    public void CrossFade(Prsd_AudioSet newTrack, bool syncTime)
    {
        if (newTrack.type != Prsd_AudioSet.Type.Track) return;
        StopAllCoroutines();
        transitionSource.Stop();
        StartCoroutine(CrossFade(newTrack, defaultFadeDuration, syncTime));
    }

    IEnumerator Fade(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;
        float t = 0f;
        float mul = 1f / duration;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * mul;
            t = Mathf.Clamp01(t);
            source.volume = Mathf.SmoothStep(startVolume, targetVolume, t);
            yield return null;
        }
    }

    IEnumerator CrossFade(Prsd_AudioSet newTrack, float duration, bool syncTime)
    {
        transitionSource.enabled = true;
        (trackSource, transitionSource) = (transitionSource, trackSource);
        StartCoroutine(Fade(transitionSource, 0f, duration));
        PlaySound(newTrack);
        if (syncTime) trackSource.time = transitionSource.time;
        trackSource.volume = 0f;
        yield return Fade(trackSource, newTrack.volume, duration);
        transitionSource.Stop();
        transitionSource.enabled = false;
    }

    #endregion
}