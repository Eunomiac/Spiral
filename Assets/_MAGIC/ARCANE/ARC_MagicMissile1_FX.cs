using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARC_MagicMissile1_FX : MonoBehaviour {

    Rigidbody body;
    public float damage = 25f;
    public float extraPerWisp = 150f;
    public float torque = 1000;
    public float force = 10000;

    //private List<ARC_Wisp_SpellFX> wisps = new List<ARC_Wisp_SpellFX>();
    //private List<EnemyHandler> enemies = new List<EnemyHandler>();

    void Awake() {
         body = GetComponent<Rigidbody>();
    }
    // Use this for initialization
    void Start() {
        //body.AddTorque(torque);
        body.AddForce(transform.forward * force);
        Invoke("DestroySelf", 5f);
    }

    void DestroySelf() {
        Destroy(gameObject);
    }

    //void OnTriggerEnter2D(Collider2D col) {
    //    if (col.gameObject.tag == "Enemy" && !enemies.Contains(col.gameObject.GetComponent<EnemyHandler>())) {
    //        enemies.Add(col.gameObject.GetComponent<EnemyHandler>());
    //        col.gameObject.GetComponent<EnemyHandler>().TakeHit(damage + extraPerWisp * wisps.Count);
    //    } else if (col.gameObject.tag == "ARC_Wisp" && !wisps.Contains(col.gameObject.GetComponent<ARC_Wisp_SpellFX>())) {
    //        wisps.Add(col.gameObject.GetComponent<ARC_Wisp_SpellFX>());
    //    }
    //}

    //void OnTriggerExit2D(Collider2D col) {
    //    if (col.gameObject.tag == "Enemy" && enemies.Contains(col.gameObject.GetComponent<EnemyHandler>())) {
    //        enemies.Remove(col.gameObject.GetComponent<EnemyHandler>());
    //    } else if (col.gameObject.tag == "ARC_Wisp" && wisps.Contains(col.gameObject.GetComponent<ARC_Wisp_SpellFX>())) {
    //        wisps.Remove(col.gameObject.GetComponent<ARC_Wisp_SpellFX>());
    //    }
    //}
}
