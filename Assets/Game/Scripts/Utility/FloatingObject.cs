using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] float frequency;
    [SerializeField] float amplitude;
    [SerializeField] float rotationSpeed;

    Vector2 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        Vector2 pos = startPos;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;

        transform.localPosition = pos;
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
    }
}
