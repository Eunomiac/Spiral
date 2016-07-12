using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FIR_FireBolt1_FX : MonoBehaviour
{

    public int numBolts = 1;
    public float searchAngle = 60f;
    public float damage = 25f;
    public float knockback = 1000f;
    public float speed = 1000;
    public Material boltMaterial;

    private PLAYER player;
    private THREATS threats;
    private Color[] colors = { Color.red, Color.yellow, new Vector4(1f, 0.5f, 0f, 1f) };
    private List<LineRenderer> fireBoltLines = new List<LineRenderer>();

    void Awake ()
    {
        player = GAME.Player;
        threats = GAME.Threats;
    }

    void Start ()
    {
        float facingAngle = transform.forward.FacingAngle();
        float minAngle = (facingAngle - 0.5f * searchAngle).Clamp();
        float maxAngle = (facingAngle + 0.5f * searchAngle).Clamp();
        Debug.Log("Min: " + minAngle.ToString() + ", Max: " + maxAngle.ToString());
        List<EnemyAI> enemyTargets = threats.GetClosestEnemies(minAngle, maxAngle, numBolts);
        foreach ( EnemyAI enemy in enemyTargets )
        {
            Color color = colors.Random();
            LineRenderer thisLine = new GameObject("Firebolt", typeof(LineRenderer)).GetComponent<LineRenderer>();
            thisLine.SetColors(color, color);
            Vector3[] targetVertices = new Vector3[2];
            targetVertices[0] = player.transform.position;
            targetVertices[1] = enemy.transform.position;
            thisLine.SetPositions(targetVertices);
            thisLine.material = boltMaterial;
            thisLine.SetWidth(0.15f, 0.08f);
            thisLine.transform.SetParent(transform);
            fireBoltLines.Add(thisLine);
            Vector3 knockbackVec = Vector3.Normalize(enemy.transform.position - player.transform.position) * knockback;
            enemy.TakeHit(damage);
            StartCoroutine(ApplyKnockback(enemy, knockbackVec));
        }
        Invoke("DestroySelf", 5f);
    }

    IEnumerator ApplyKnockback (EnemyAI enemy, Vector3 knockbackVec)
    {
        for ( int i = 0; i < 20; i++ )
        {
            enemy.Knockback(knockbackVec * (1 - i / 20f));
            yield return new WaitForFixedUpdate();
        }
    }

    void DestroySelf ()
    {
        foreach ( LineRenderer line in fireBoltLines )
        {
            Destroy(line.gameObject);
        }
        Destroy(gameObject);
    }

}
