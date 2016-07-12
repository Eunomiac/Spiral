using System.Collections;
using UnityEngine;

public class FIR_FireBolt1_Bolt : MonoBehaviour
{

    public float damage = 25f;
    public float knockback = 1000f;

    public Material boltMaterial;

    private Color[] colors = { Color.red, Color.yellow, new Vector4(1f, 0.5f, 0f, 1f) };
    private LineRenderer fireBolt;

    public void setTarget (EnemyAI enemy)
    {
        Color color = colors.Random();
        fireBolt = new GameObject("Firebolt", typeof(LineRenderer)).GetComponent<LineRenderer>();
        fireBolt.SetColors(color, color);
        Vector3[] targetVertices = new Vector3[2];
        targetVertices[0] = GAME.Player.transform.position;
        targetVertices[1] = enemy.transform.position;
        fireBolt.SetPositions(targetVertices);
        fireBolt.material = boltMaterial;
        fireBolt.SetWidth(0.15f, 0.08f);
        fireBolt.transform.SetParent(transform);
        Vector3 knockbackVec = Vector3.Normalize(enemy.transform.position - GAME.Player.transform.position) * knockback;
        enemy.TakeHit(damage);
        StartCoroutine(ApplyKnockback(enemy, knockbackVec));
    }

    IEnumerator ApplyKnockback (EnemyAI enemy, Vector3 knockbackVec)
    {
        for ( int i = 0; i < 20; i++ )
        {
            enemy.Knockback(knockbackVec * (1 - 0.05f * i));
            yield return new WaitForFixedUpdate();
        }
        Destroy(fireBolt.gameObject);
        Destroy(gameObject);
    }
}
