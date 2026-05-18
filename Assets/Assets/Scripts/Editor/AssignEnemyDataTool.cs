using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class AssignEnemyDataTool
{
    // Mapeamento: nome do prefab -> nome do arquivo EnemyData
    static readonly Dictionary<string, string> PrefabToData = new()
    {
        { "ArmoredAxeman",   "ArmoredAxeman"     },
        { "ArmoredOrc",      "ArmoredOrc"        },
        { "ArmoredSkeleton", "ArmoredSkeleton"   },
        { "EliteOrc",        "EliteOrc"          },
        { "Greatsword",      "GreatswordSkeleton" },
        { "KnightTemplar",   "KnightTemplar"     },
        { "Lancer",          "Lancer"            },
        { "orc",             "Orc"               },
        { "RiderOrc",        "RiderOrc"          },
        { "Skeleton",        "Skeleton"          },
        { "Archer",          "SkeletonArcher"    },
        { "Slime",           "Slime"             },
        { "Soldier",         "Soldier"           },
        { "Werebear",        "Werebear"          },
        { "Werewolf",        "Werewolf"          }
    };

    [MenuItem("Tools/Atribuir EnemyData aos Prefabs")]
    public static void Run()
    {
        // Carrega todos os EnemyData
        var dataByName = new Dictionary<string, EnemyData>();
        foreach (string guid in AssetDatabase.FindAssets("t:EnemyData", new[] { "Assets/Assets/EnemyData" }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            EnemyData d = AssetDatabase.LoadAssetAtPath<EnemyData>(path);
            if (d != null)
                dataByName[Path.GetFileNameWithoutExtension(path)] = d;
        }

        if (dataByName.Count == 0)
        {
            EditorUtility.DisplayDialog("Erro", "Nenhum EnemyData encontrado em Assets/Assets/EnemyData.", "OK");
            return;
        }

        int ok = 0, erros = 0;

        foreach (string guid in AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Enemies" }))
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string prefabName = Path.GetFileNameWithoutExtension(path);

            if (!PrefabToData.TryGetValue(prefabName, out string dataName))
            {
                Debug.LogWarning($"[AssignEnemyData] Sem mapeamento para prefab: {prefabName}");
                erros++;
                continue;
            }

            if (!dataByName.TryGetValue(dataName, out EnemyData data))
            {
                Debug.LogWarning($"[AssignEnemyData] EnemyData nao encontrado: {dataName}");
                erros++;
                continue;
            }

            using var scope = new PrefabUtility.EditPrefabContentsScope(path);
            GameObject root = scope.prefabContentsRoot;

            EnemyFollow follow = root.GetComponent<EnemyFollow>();
            EnemyHealth health = root.GetComponent<EnemyHealth>();

            if (follow != null) follow.data = data;
            if (health != null) health.data = data;

            if (follow != null || health != null)
            {
                Debug.Log($"[AssignEnemyData] OK: {prefabName} -> {dataName}");
                ok++;
            }
            else
            {
                Debug.LogWarning($"[AssignEnemyData] EnemyFollow/EnemyHealth nao encontrados em: {prefabName}");
                erros++;
            }
        }

        AssetDatabase.SaveAssets();

        string msg = $"{ok} prefab(s) atualizados com sucesso.";
        if (erros > 0) msg += $"\n{erros} prefab(s) com problema (veja o Console).";
        EditorUtility.DisplayDialog("Atribuir EnemyData", msg, "OK");
    }
}
