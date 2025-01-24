using UnityEngine;
using Sirenix.OdinInspector;

[HideMonoScript]
[CreateAssetMenu(fileName = "AudioSet", menuName = "Prsd/Audio/AudioSet")]
public class Prsd_AudioSet : ScriptableObject
{
    public enum Type { SFX, Track }

    [EnumToggleButtons, LabelText(" "), GUIColor(nameof(TypeColor))]
    public Type type;
    public AudioClip[] soundsElements;
    public float volume = 1f;
    [ShowIf(nameof(IsSfx))]
    public int sfxSource = 0;
    [ShowIf(nameof(CanLoop))]
    public bool loop = false;
    [Space]
    [Multiline(2)]
    public string description = "";

    public AudioClip GetSound()
    {
        return soundsElements[Random.Range(0, soundsElements.Length)];
    }

    bool CanLoop => type == Type.Track;
    bool IsSfx => type == Type.SFX;
    Color TypeColor => type == Type.SFX ? new Color(1f, 1f, 0.5f) : new Color(0.5f, 1f, 0.5f);
}
