using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PLAYER : MonoBehaviour {

    public int numCloseNodes = 6, numFarNodes = 12;
    public float playerCloseDistance = 1.5f, closeFarDistance = 3f;

    private PLAYER player;

	void Awake () {
        player = GetComponent<PLAYER>();
        NavNodes test = SetPlayerNavNodes();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    NavNodes SetPlayerNavNodes() {

        // Setting Close Nodes
        List<Vector3> closeNodes = new List<Vector3>();
        float angleSpread = 360f / numCloseNodes;
        Vector3 startDirVec = Vector3.forward * playerCloseDistance;   
        while (closeNodes.Count < numCloseNodes) {
            float thisNodeAngle = angleSpread * closeNodes.Count;
            Vector3 nodePosition = Quaternion.Euler(0, thisNodeAngle, 0) * startDirVec;
            closeNodes.Add(nodePosition + player.transform.position);
        }
            

        return new NavNodes(closeNodes, farNodes);
    }

    public struct NavNodes {
        public List<Transform> close { get; set; }
        public List<Transform> far { get; set; }

        public NavNodes(List<Transform> closeNodes, List<Transform> farNodes) {
            close = closeNodes;
            far = farNodes;
        }
    }

}
