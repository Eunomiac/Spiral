using UnityEngine;
using System.Collections;

public class GeneratePlayerNavNodes : MonoBehaviour {

    public int numCloseNodes, numFarNodes;
    public float playerCloseDistance, closeFarDistance;

    private ARENA arena;
    private GameObject navFloor;

	void Awake () {
        arena = FindObjectOfType<ARENA>();
        navFloor = arena.navFloor;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
