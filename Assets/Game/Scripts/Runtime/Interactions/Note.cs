using UnityEngine;

public class Note : Interactable
{
    public Sprite note;

    protected override void OnInteract(Vector2 worldInteractPosition)
    {
        NotePanel.ShowNote(note);
    }
}
