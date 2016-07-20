using System.Collections.Generic;
using UnityEngine;

public class ARENA : MonoBehaviour
{
    public Node navNodePrefab;
    public bool showNodes = false;
    public bool showConnections = false;
    public bool showLabels = false;
    public Material lineMaterial;                                           // Debug

    private List<NavNetwork> navNetworks = new List<NavNetwork>();

    [HideInInspector]
    public List<NavNetwork> NavNetworks { get { return navNetworks; } }
    public NavNetwork PlayerNavNetwork { get { return navNetworks[0]; } }

    public NavNetwork InitializeNavNetwork (GameObject networkCore, int[] nodesPerTier, float[] distOfTier, float maxNeighbourDistMult)
    {
        NavNetwork thisNetwork = new GameObject(networkCore.name + " NavNet", typeof(NavNetwork)).GetComponent<NavNetwork>();
        NavNetworks.Add(thisNetwork);
        thisNetwork.gameObject.SetParent(gameObject, true, networkCore);
        thisNetwork.Initialize(networkCore, nodesPerTier, distOfTier, maxNeighbourDistMult);
        return thisNetwork;
    }

    public NavNetwork GetNavNetwork (GameObject networkCore)
    {
        foreach ( NavNetwork network in NavNetworks )
            if ( network.Core == networkCore )
                return network;
        Debug.LogError("No navigation network found for network core '" + networkCore.name + "'.");
        return null;
    }
}
