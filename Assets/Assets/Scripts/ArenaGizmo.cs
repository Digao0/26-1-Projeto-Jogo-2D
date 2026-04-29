using UnityEngine;

public class ArenaGizmo : MonoBehaviour
{
    public Color borderColor = new Color(1f, 0.3f, 0f, 0.8f);

    void OnDrawGizmos()
    {
        Gizmos.color = borderColor;
        foreach (BoxCollider2D col in GetComponentsInChildren<BoxCollider2D>())
        {
            Vector3 center = col.transform.TransformPoint(col.offset);
            Vector3 size = new Vector3(
                col.transform.lossyScale.x * col.size.x,
                col.transform.lossyScale.y * col.size.y,
                1f
            );
            Gizmos.DrawWireCube(center, size);
        }
    }
}
