using System;
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
    [SerializeField] TrackSlot[] trackSlots;
    [SerializeField] float defaultFadeDuration = 0.5f;

    List<Coroutine>[] coroutines;

    [Serializable]
    public class TrackSlot
    {
        public AudioSource trackSource;
        public AudioSource transitionSource;

        public Prsd_AudioSet ActiveSet { get; set; }
    }

    public static void PlaySoundSet(Prsd_AudioSet set) { if (instance) instance.PlaySound(set); }
    public static void TrackFadeInRequest(Prsd_AudioSet set) { if (instance) instance.FadeIn(set, set.playbackSlotIndex); }
    public static void TrackFadeOutRequest(int trackSlotIndex = 0) { if (instance) instance.FadeOut(trackSlotIndex); }
    public static void TrackCrossFadeRequest(Prsd_AudioSet set) { if (instance) instance.CrossFade(set, false, set.playbackSlotIndex); }
    public static void TrackCrossFadeTimeSyncedRequest(Prsd_AudioSet set) { if (instance) instance.CrossFade(set, true, set.playbackSlotIndex); }

    readonly List<Prsd_AudioSet> playedThisFrame = new();

    static Prsd_AudioManager instance;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        coroutines = new List<Coroutine>[trackSlots.Length];
        for (int i = 0; i < coroutines.Length; i++)
        {
            coroutines[i] = new List<Coroutine>();
        }
    }

    void OnEnable()
    {
        for (int i = 0; i < trackSlots.Length; i++)
        {
            trackSlots[i].transitionSource.enabled = false;
        }
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
                    int index = Mathf.Clamp(sound.playbackSlotIndex, 0, sfxSources.Length);
                    sfxSources[index].PlayOneShot(sound.GetSound(), sound.volume);
                }
            }
            else
            {
                if (trackSlots.Length > 0)
                {
                    int index = Mathf.Clamp(sound.playbackSlotIndex, 0, trackSlots.Length);
                    trackSlots[index].ActiveSet = sound;
                    var trackSource = trackSlots[index].trackSource;
                    trackSource.clip = sound.GetSound();
                    trackSource.volume = sound.volume;
                    trackSource.loop = sound.loop;
                    trackSource.Play();
                }
            }

            if (showLogs) Debug.Log($"<color=#4cff99>Audio: {sound.name}</color>");
        }
    }

    void StopCoroutines(int trackSlot)
    {
        for (int i = coroutines[trackSlot].Count - 1; i >= 0; i--)
        {
            StopCoroutine(coroutines[trackSlot][i]);
        }
        coroutines[trackSlot].Clear();
    }

    #region FADE

    public void FadeIn(int trackSlot = 0)
    {
        if (trackSlot < 0 || trackSlot >= trackSlots.Length) return;
        if (!trackSlots[trackSlot].ActiveSet) return;

        StopCoroutines(trackSlot);
        coroutines[trackSlot].Add(StartCoroutine(Fade(trackSlots[trackSlot].trackSource, trackSlots[trackSlot].ActiveSet.volume, defaultFadeDuration)));
    }

    public void FadeOut(int trackSlot = 0)
    {
        if (trackSlot < 0 || trackSlot >= trackSlots.Length) return;

        StopCoroutines(trackSlot);
        coroutines[trackSlot].Add(StartCoroutine(Fade(trackSlots[trackSlot].trackSource, 0f, defaultFadeDuration)));
    }

    public void FadeIn(Prsd_AudioSet track, int trackSlot = 0)
    {
        if (trackSlot < 0 || trackSlot >= trackSlots.Length) return;
        if (track.type != Prsd_AudioSet.Type.Track) return;

        StopCoroutines(trackSlot);
        PlaySound(track);
        var trackSource = trackSlots[trackSlot].trackSource;
        trackSource.volume = 0f;
        coroutines[trackSlot].Add(StartCoroutine(Fade(trackSource, track.volume, defaultFadeDuration)));
    }

    public void Fade(float targetVolume, int trackSlot = 0)
    {
        if (trackSlot < 0 || trackSlot >= trackSlots.Length) return;

        StopCoroutines(trackSlot);
        coroutines[trackSlot].Add(StartCoroutine(Fade(trackSlots[trackSlot].trackSource, targetVolume, defaultFadeDuration)));
    }

    public void CrossFade(Prsd_AudioSet newTrack, bool syncTime, int trackSlot = 0)
    {
        if (trackSlot < 0 || trackSlot >= trackSlots.Length) return;
        if (newTrack.type != Prsd_AudioSet.Type.Track) return;
        if (newTrack == trackSlots[trackSlot].ActiveSet)
        {
            FadeIn(trackSlot);
            return;
        }

        StopCoroutines(trackSlot);
        trackSlots[trackSlot].transitionSource.Stop();
        coroutines[trackSlot].Add(StartCoroutine(CrossFade(newTrack, trackSlot, defaultFadeDuration, syncTime)));
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

    IEnumerator CrossFade(Prsd_AudioSet newTrack, int trackSlot, float duration, bool syncTime)
    {
        var slot = trackSlots[trackSlot];
        slot.transitionSource.enabled = true;
        (slot.trackSource, slot.transitionSource) = (slot.transitionSource, slot.trackSource);
        coroutines[trackSlot].Add(StartCoroutine(Fade(slot.transitionSource, 0f, duration)));
        PlaySound(newTrack);
        if (syncTime) slot.trackSource.time = slot.transitionSource.time;
        slot.trackSource.volume = 0f;
        yield return Fade(slot.trackSource, newTrack.volume, duration);
        slot.transitionSource.Stop();
        slot.transitionSource.enabled = false;
    }

    #endregion
}