using UnityEditor;
using UnityEngine;

public static class SetObstacleMaskTool
{
    [MenuItem("Tools/Configurar Obstacle Mask nos Inimigos")]
    public static void Run()
    {
        LayerMask mask = 1 << 2; // Layer 2
        int ok = 0;

        foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Enemies" }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string prefabName = System.IO.Path.GetFileNameWithoutExtension(path);

            using var scope = new PrefabUtility.EditPrefabContentsScope(path);
            EnemyFollow follow = scope.prefabContentsRoot.GetComponent<EnemyFollow>();

            if (follow != null)
            {
                follow.obstacleMask = mask;
                ok++;
            }
            else
            {
                Debug.LogWarning($"[SetObstacleMask] EnemyFollow nao encontrado: {prefabName}");
            }
        }

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("Obstacle Mask", $"{ok} prefabs atualizados com Layer 2.", "OK");
    }
}
