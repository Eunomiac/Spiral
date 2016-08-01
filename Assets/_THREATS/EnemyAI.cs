using RAIN.Core;
using RAIN.Memory;
using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    const string ATTACKNODE = "Attack";

    public ParticleSystem attackAlertPrefab;

    /* USING RAIN AI FOR NAVIGATION
     * - 
     * - 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * */
    public float strength = 1f;

    public bool IsImmobile { get; set; }


    private AIRig aiRig;
    private RAINMemory memory;
    private Rigidbody body;
    private Vector3 prevPosition;
    private ParticleSystem attackAlert;
    //private BasicMind mind;
    //private BTPriorityNode mainPriorityNode;
    //private int attackNodeIndex;

    private Node myNode;
    private Vector3? myDest;
    private bool isAttacking;


    private PLAYER player;

    public Node MyNode
    {
        get { return myNode; }
        set {
            if ( myNode != null && myNode != value )
                myNode.Unclaim();
            myNode = value;
            memory.SetItem("MyNode", value == null ? null : value.gameObject);
            if ( myNode != null )
            {
                memory.SetItem("CurrentTier", myNode.Tier);
                MyDestination = myNode.transform.position;
            }
            else
            {
                memory.SetItem<int?>("CurrentTier", null);
                MyDestination = null;
            }
        }
    }

    public Vector3? MyDestination
    {
        get { return myDest; }
        set {
            myDest = value;
            memory.SetItem("Destination", value);
            if ( myDest != null )
                memory.SetItem("DistFromDest", transform.position.Distance2D((Vector3) myDest));
            else
                memory.SetItem<float?>("DistFromDest", null);
        }
    }

    public bool IsAttacking
    {
        get { return isAttacking; }
        set {
            isAttacking = value;
            memory.SetItem("IsAttacking", value);
            if ( isAttacking )
                memory.SetItem("IsWaitingToAttack", false);
        }
    }

    public void UpdatePosition ()
    {
        memory.SetItem("MyPosition", transform.position);
        MyDestination = MyDestination;
    }

    void Awake ()
    {
        player = GAME.Player;
        aiRig = GetComponentInChildren<AIRig>();
        memory = aiRig.AI.WorkingMemory;
        body = GetComponent<Rigidbody>();

        //mind = aiRig.AI.Mind as BasicMind;
        memory.SetItem("AttackStartPriority", 0);
        memory.SetItem("AttackRunPriority", 0);
        IsImmobile = false;
    }

    void Start ()
    {
        prevPosition = transform.position;
        attackAlert = Instantiate(attackAlertPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z) + Vector3.up + 2.5f * Vector3.forward, Quaternion.identity) as ParticleSystem;
        attackAlert.transform.SetParent(transform, true);
        attackAlert.Stop();

        //BTNode rootNode = mind.BehaviorRoot;
        //for ( int i = 0; i < rootNode.GetChildCount(); i++ )
        //{
        //	if ( rootNode.GetChild(i).GetType() == typeof(BTPriorityNode) )
        //	{
        //		mainPriorityNode = rootNode.GetChild(i) as BTPriorityNode;
        //		break;
        //	}
        //}
        //attackNodeIndex = mainPriorityNode.GetChildIndex(ATTACKNODE);
    }

    void LateUpdate ()
    {
        if ( IsImmobile )
            transform.position = prevPosition;
        else
            prevPosition = transform.position;
    }

    public void TakeHit (float damage)
    {
        SendMessage("DamageHealth", damage);
    }

    public void Knockback (Vector3 knockbackVec)
    {
        if ( body )
            StartCoroutine(ApplyKnockback(knockbackVec));
    }

    IEnumerator ApplyKnockback (Vector3 knockbackVec)
    {
        for ( int i = 0; i < 20; i++ )
        {
            body.AddForce(knockbackVec * (1 - 0.05f * i));
            yield return new WaitForFixedUpdate();
        }
    }

    public void DeathBlow ()
    {
        // play death animation
        Destroy(gameObject);
    }

    public bool ClaimNode (Node node)
    {
        if ( node.Claim(gameObject) )
        {
            MyNode = node;
            return true;
        }
        else
            return false;
    }

    void UnclaimNode ()
    {
        MyNode = null;
    }

    public void Attack ()
    {
        if ( attackAlert )
            attackAlert.Play();
        Invoke("GoAttack", attackAlert.duration);
    }

    void GoAttack ()
    {
        attackAlert.Stop();
        SetPriority(ATTACKNODE, 100);
    }

    void SetPriority (string nodeName, int start, int? run = null)
    {
        memory.SetItem(nodeName + "StartPriority", start);
        memory.SetItem(nodeName + "RunPriority", run ?? start);
    }

    void OnTriggerEnter (Collider col)
    {
        if ( col.gameObject.GetComponent<PLAYER>() )
        {
            player.TakeHit(strength);
            IsAttacking = false;
            //SetPriority(ATTACKNODE, 0);
            MyDestination = null;
            //float myAngle = player.NavNetwork.GetAngleFromPosition(transform.position);
            //int tier = player.nodesPerTier.Length;
            //Node targetNode = null;
            //while ( targetNode == null && tier > 0 )
            //{
            //	float angleSpread = 30f;
            //	while ( targetNode == null && angleSpread <= 90f )
            //	{
            //		targetNode = player.NavNetwork.GetRandomNode((myAngle - angleSpread).Clamp(), (myAngle + angleSpread).Clamp(), Mathf.RoundToInt(Mathf.Infinity));
            //		angleSpread += 30f;
            //	}
            //	tier -= 1;
            //}
            //if ( targetNode == null )
            //{
            //	Debug.Log("No unoccupied node to retreat to!");
            //	Destroy(gameObject);
            //} else
            //{
            //	ClaimNode(targetNode);
            //}
        }
    }
}
