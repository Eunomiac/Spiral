using UnityEngine;

public class Health : MonoBehaviour
{

    public float health = 1f;

    void DamageHealth (float damage)
    {
        health -= damage;
        if ( health <= 0 )
            SendMessage("DeathBlow");
    }

}
