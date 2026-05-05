using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(EnemyData))]
public class EnemyDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Criar Prefab", GUILayout.Height(35)))
            CreateEnemyPrefab((EnemyData)target);
    }

    static void CreateEnemyPrefab(EnemyData data)
    {
        EnsureFolder("Assets/Prefabs");
        EnsureFolder("Assets/Prefabs/Enemies");
        EnsureFolder("Assets/Assets/Animations");
        EnsureFolder("Assets/Assets/Animations/Enemies");

        string controllerPath = $"Assets/Assets/Animations/Enemies/{data.enemyName}_Animator.controller";
        var controller = BuildAnimatorController(data, controllerPath);

        var go = new GameObject(data.enemyName);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = data.defaultSprite;

        var rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        go.AddComponent<CapsuleCollider2D>();

        var anim = go.AddComponent<Animator>();
        anim.runtimeAnimatorController = controller;

        var follow = go.AddComponent<EnemyFollow>();
        follow.speed = data.speed;
        follow.damage = data.damage;

        var health = go.AddComponent<EnemyHealth>();
        health.maxHealth = data.maxHealth;
        health.knockbackForce = data.knockbackForce;
        health.knockbackDuration = data.knockbackDuration;
        health.deathAnimDuration = data.deathClip != null ? data.deathClip.length : 1f;

        go.AddComponent<EnemyAnimator>();

        go.tag = "Enemy";

        string prefabPath = $"Assets/Prefabs/Enemies/{data.enemyName}.prefab";
        PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
        DestroyImmediate(go);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        EditorGUIUtility.PingObject(prefab);
        Debug.Log($"Prefab criado: {prefabPath}");
    }

    static AnimatorController BuildAnimatorController(EnemyData data, string path)
    {
        var controller = AnimatorController.CreateAnimatorControllerAtPath(path);

        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        controller.AddParameter("Hit", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Attack", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Die", AnimatorControllerParameterType.Trigger);

        var sm = controller.layers[0].stateMachine;

        var idle   = sm.AddState("Idle");
        var walk   = sm.AddState("Walk");
        var hurt   = sm.AddState("Hurt");
        var attack = sm.AddState("Attack");
        var death  = sm.AddState("Death");

        if (data.idleClip   != null) idle.motion   = data.idleClip;
        if (data.walkClip   != null) walk.motion   = data.walkClip;
        if (data.hurtClip   != null) hurt.motion   = data.hurtClip;
        if (data.attackClip != null) attack.motion = data.attackClip;
        if (data.deathClip  != null) death.motion  = data.deathClip;

        sm.defaultState = idle;

        var idleToWalk = idle.AddTransition(walk);
        idleToWalk.AddCondition(AnimatorConditionMode.Greater, 0.1f, "Speed");
        idleToWalk.hasExitTime = false;
        idleToWalk.duration = 0f;

        var walkToIdle = walk.AddTransition(idle);
        walkToIdle.AddCondition(AnimatorConditionMode.Less, 0.1f, "Speed");
        walkToIdle.hasExitTime = false;
        walkToIdle.duration = 0f;

        var anyToHurt = sm.AddAnyStateTransition(hurt);
        anyToHurt.AddCondition(AnimatorConditionMode.If, 0, "Hit");
        anyToHurt.hasExitTime = false;
        anyToHurt.duration = 0f;
        anyToHurt.canTransitionToSelf = false;

        var hurtToIdle = hurt.AddTransition(idle);
        hurtToIdle.hasExitTime = true;
        hurtToIdle.exitTime = 1f;
        hurtToIdle.duration = 0f;

        var anyToAttack = sm.AddAnyStateTransition(attack);
        anyToAttack.AddCondition(AnimatorConditionMode.If, 0, "Attack");
        anyToAttack.hasExitTime = false;
        anyToAttack.duration = 0f;
        anyToAttack.canTransitionToSelf = false;

        var attackToIdle = attack.AddTransition(idle);
        attackToIdle.hasExitTime = true;
        attackToIdle.exitTime = 1f;
        attackToIdle.duration = 0f;

        var anyToDeath = sm.AddAnyStateTransition(death);
        anyToDeath.AddCondition(AnimatorConditionMode.If, 0, "Die");
        anyToDeath.hasExitTime = false;
        anyToDeath.duration = 0f;
        anyToDeath.canTransitionToSelf = false;

        EditorUtility.SetDirty(controller);
        AssetDatabase.SaveAssets();

        return controller;
    }

    static void EnsureFolder(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            string parent = Path.GetDirectoryName(path).Replace("\\", "/");
            string folder = Path.GetFileName(path);
            AssetDatabase.CreateFolder(parent, folder);
        }
    }
}
