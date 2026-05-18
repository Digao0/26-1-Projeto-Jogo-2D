using System.Collections;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    // follow: se informado, o número acompanha esse transform (ex: player com knockback)
    public static void Spawn(Vector3 position, int damage, Color color, Transform follow = null)
    {
        GameObject go = new GameObject("DamageNumber");
        go.transform.position = position + Vector3.up * 0.3f;

        TextMeshPro tmp = go.AddComponent<TextMeshPro>();
        var font = Resources.Load<TMP_FontAsset>("Fonts/MedievalSharp-Regular SDF");
        if (font != null) tmp.font = font;
        tmp.text = "-" + damage;
        tmp.fontSize = 5f;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.outlineWidth = 0.2f;
        tmp.outlineColor = Color.black;
        tmp.sortingLayerID = SortingLayer.NameToID("Light");
        tmp.sortingOrder = 50;

        go.AddComponent<DamageNumber>().StartCoroutine(Animate(tmp, follow));
    }

    static IEnumerator Animate(TextMeshPro tmp, Transform follow)
    {
        float duration = 0.8f;
        float elapsed = 0f;
        Color startColor = tmp.color;
        Vector3 spawnOffset = Vector3.up * 0.3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector3 basePos = follow != null ? follow.position + spawnOffset : tmp.transform.position - Vector3.up * (t * 0.8f);
            tmp.transform.position = basePos + Vector3.up * (t * 0.8f);
            tmp.color = new Color(startColor.r, startColor.g, startColor.b, 1f - t);
            yield return null;
        }

        Destroy(tmp.gameObject);
    }
}
