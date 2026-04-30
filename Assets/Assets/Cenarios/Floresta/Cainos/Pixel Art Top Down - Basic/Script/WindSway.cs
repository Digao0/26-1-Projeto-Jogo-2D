using UnityEngine;

public class WindSway : MonoBehaviour
{
    public float swayAmount = 0.05f;
    public float swaySpeed = 1.5f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.position = startPos + new Vector3(sway, 0, 0);
    }
}