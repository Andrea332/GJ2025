using UnityEngine;

public class Ending : MonoBehaviour
{

    public GameObject[] uiToClose;
    public GameObject[] uiToOpen;

    private void Start()
    {
        for (int i = 0; i < uiToClose.Length; i++) uiToClose[i].SetActive(false);

    }

    public void End()
    {
        for (int i = 0; i < uiToOpen.Length; i++) uiToOpen[i].SetActive(true);
    }
}
