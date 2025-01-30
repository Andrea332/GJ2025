using UnityEngine;

public class CameraSizeSetter : MonoBehaviour
{
    [SerializeField] float targetWidth;
    [SerializeField] float targetHeight;

    Camera cam;
    float targetAspect;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!cam) return;

        targetAspect = targetWidth / targetHeight;

        if (cam.aspect >= targetAspect)
        {
            cam.orthographicSize = targetHeight * 0.5f;
        }
        else
        {
            float size = targetWidth / cam.aspect;
            cam.orthographicSize = size * 0.5f;
        }
    }
}
