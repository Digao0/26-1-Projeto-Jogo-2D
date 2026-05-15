using System.Collections;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    public float dashInterval = 5f;
    public float dashSpeed = 7f;
    public float dashDuration = 1f;

    private EnemyFollow enemyFollow;
    private float baseSpeed;

    IEnumerator Start()
    {
        yield return null; // espera todos os Start() rodarem (EnemyFollow aplica seus valores)
        enemyFollow = GetComponent<EnemyFollow>();
        baseSpeed = enemyFollow.speed;
        StartCoroutine(DashLoop());
    }

    IEnumerator DashLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(dashInterval);
            if (enemyFollow != null && enemyFollow.enabled)
                yield return StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        enemyFollow.speed = dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        if (enemyFollow != null)
            enemyFollow.speed = baseSpeed;
    }
}
