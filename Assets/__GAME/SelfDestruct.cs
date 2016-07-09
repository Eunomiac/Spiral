using UnityEngine;

public class SelfDestruct : MonoBehaviour {

	public float fuseTime;

	// Use this for initialization
	void Start () {
		Invoke("DestroySelf", fuseTime);
	}

	void DestroySelf()
	{
		Destroy(gameObject);
	}
}
