using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName = "Enemy";

    [Header("Visuals")]
    public Sprite defaultSprite;

    [Header("Stats")]
    public int maxHealth = 50;
    public float speed = 2f;
    public int damage = 10;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.3f;

    [Header("Animations")]
    public AnimationClip idleClip;
    public AnimationClip walkClip;
    public AnimationClip hurtClip;
    public AnimationClip deathClip;
}
