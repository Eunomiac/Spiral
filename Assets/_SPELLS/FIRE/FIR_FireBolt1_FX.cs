using UnityEngine;

public class FIT_FireBolt1_FX : MonoBehaviour
{

	Rigidbody body;
	public float damage = 25f;
	public float extraPerWisp = 150f;
	public float torque = 1000;
	public float force = 10000;

	void Awake ()
	{
		body = GetComponent<Rigidbody>();
	}

	void Start ()
	{
		//body.AddTorque(torque);
		body.AddForce(transform.forward * force);
		Invoke("DestroySelf", 5f);
	}

	void DestroySelf ()
	{
		Destroy(gameObject);
	}

}
