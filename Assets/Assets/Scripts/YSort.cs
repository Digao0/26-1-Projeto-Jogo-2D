using UnityEngine;

public class YSort : MonoBehaviour
{
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (sr != null)
            sr.sortingOrder = Mathf.RoundToInt(-sr.bounds.min.y * 100);
    }
}
