using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public Prsd_AudioSet menuTrack;

    private void Start()
    {
        Prsd_AudioManager.TrackFadeInRequest(menuTrack);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
