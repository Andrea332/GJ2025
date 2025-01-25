using UnityEngine;
using UnityEngine.UI;

public class NotePanel : MonoBehaviour
{
    public Image noteImage;
    public Prsd_UtilityAnimations anim;

    static NotePanel instance;

    void Awake()
    {
        instance = this;
    }

    public static void ShowNote(Sprite sprite)
    {
        if (!instance) return;
        instance.noteImage.sprite = sprite;
        instance.anim.PopIn();
    }
}
