using RAIN.Action;
using RAIN.Core;
using System.Collections.Generic;
using UnityEngine;

//public override void Start (RAIN.Core.AI ai)
//{
//	base.Start(ai);
//}

//public override ActionResult Execute (RAIN.Core.AI ai)
//{
//	return ActionResult.SUCCESS;
//}

//public override void Stop (RAIN.Core.AI ai)
//{
//	base.Stop(ai);
//}

[RAINAction]
public class PopulateConstants : RAINAction
{
    private EnemyAI enemy;
    public override void Start (AI ai)
    {
        ai.WorkingMemory.SetItem("BeatDuration", GAME.BeatDuration);
        ai.WorkingMemory.SetItem("WaitTime", GAME.BeatDuration);
        ai.WorkingMemory.SetItem("IsAttacking", false);
        ai.WorkingMemory.SetItem("IsWaitingToAttack", false);
        ai.WorkingMemory.SetItem("Player", GAME.Player.gameObject);
        base.Start(ai);
    }

    //public override ActionResult Execute (RAIN.Core.AI ai)
    //{
    //	ai.WorkingMemory.SetItem("BeatDuration", GAME.BeatDuration);
    //	ai.WorkingMemory.SetItem("IsAttacking", false);
    //	return ActionResult.SUCCESS;
    //}

    //public override void Stop (RAIN.Core.AI ai)
    //{
    //	base.Stop(ai);
    //}
}

[RAINAction]
public class UpdateVariables : RAINAction
{
    private EnemyAI enemy;

    public override void Start (AI ai)
    {
        enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
        base.Start(ai);
    }

    public override ActionResult Execute (AI ai)
    {
        enemy.UpdatePosition();
        return ActionResult.FAILURE;
    }
}

[RAINAction]
public class GetClosestUnoccupiedNode : RAINAction
{
    private EnemyAI enemy;
    private NavNetwork navNet;

    public override void Start (AI ai)
    {
        enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
        navNet = GAME.Player.NavNetwork;
        base.Start(ai);
    }

    public override ActionResult Execute (RAIN.Core.AI ai)
    {
        Vector3 myPosition = ai.Body.transform.position;
        Wedge myWedge = navNet.GetWedgeFromPosition(myPosition);
        int maxTier = navNet.GetTierFromPosition(myPosition);
        maxTier = Mathf.Max(maxTier, 0);
        List<Node> theseNodes = new List<Node>(navNet.GetNodesFromWedge(myWedge, null, maxTier, false, myPosition));
        Node thisNode;
        for ( int i = 0; i < 2; i++ )
        {
            theseNodes.Reverse();
            while ( theseNodes.Count > 0 )
            {
                thisNode = theseNodes.Pop();
                //Debug.Log(enemy.name + ": " + myWedge.name + " -- " + thisNode.name);
                if ( enemy.ClaimNode(thisNode) )
                    return ActionResult.SUCCESS;
            }
            List<Wedge> myWedges = new List<Wedge>(myWedge.Neighbours);
            theseNodes = new List<Node>(navNet.GetNodesFromWedge(myWedges, null, maxTier, false, myPosition));
        }
        return ActionResult.FAILURE;
    }
}

[RAINAction]
public class WithdrawFromArena : RAINAction
{
    private EnemyAI enemy;
    private PLAYER player;

    public override void Start (AI ai)
    {
        enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
        player = GAME.Player;
        base.Start(ai);
    }

    public override ActionResult Execute (RAIN.Core.AI ai)
    {
        Wedge myWedge = player.NavNetwork.GetWedgeFromPosition(ai.Body.transform.position);
        Node distantNode = player.NavNetwork.GetNodesFromWedge(myWedge, GAME.BIGINT).Random();
        //Debug.Log("Withdraw Node: " + distantNode.name + ", Distance: " + distantNode.transform.position.Distance2D(GAME.Player.transform.position));
        Vector3 newDest = (distantNode.transform.position - GAME.Player.transform.position) * Random.Range(1.1f, 1.7f) + GAME.Player.transform.position;
        //Debug.Log("... New Dest: " + newDest.ToString() + ", Distance: " + newDest.Distance2D(GAME.Player.transform.position));
        enemy.MyNode = null;
        enemy.MyDestination = newDest;
        return ActionResult.SUCCESS;
    }
}


[RAINAction]
public class RegisterAttack : RAINAction
{
    private EnemyAI enemy;

    public override void Start (AI ai)
    {
        enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
        base.Start(ai);
    }

    public override ActionResult Execute (AI ai)
    {
        if ( GAME.Threats.RegisterAttacker(enemy) )
        {
            ai.WorkingMemory.SetItem("IsWaitingToAttack", true);
            return ActionResult.SUCCESS;
        }
        else
            return ActionResult.FAILURE;
    }
}

[RAINAction]
public class TryToAdvance : RAINAction
{
    private EnemyAI enemy;

    public override void Start (AI ai)
    {
        enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
        base.Start(ai);
    }

    public override ActionResult Execute (AI ai)
    {
        GameObject node = ai.WorkingMemory.GetItem<GameObject>("MyNode");

        if ( node == null )
            return ActionResult.FAILURE;
        else
        {
            Node myNode = node.GetComponent<Node>();
            List<Node> neighbours = myNode.GetNeighbours(Mathf.Max(0, myNode.Tier - 1), Mathf.Max(0, myNode.Tier - 1));
            //Debug.Log("myNode.Tier = " + myNode.Tier + ", Mathf.Max = " + Mathf.Max(0, myNode.Tier - 1) + ", Min = " + Mathf.Max(0, myNode.Tier - 1) + ". Neighbour Count = " + neighbours.Count);
            while ( neighbours.Count > 0 )
                if ( enemy.ClaimNode(neighbours.Pop(true)) )
                    return ActionResult.SUCCESS;
            return ActionResult.FAILURE;
        }
    }
}

[RAINAction]
public class TryToSpread : RAINAction
{
    private EnemyAI enemy;

    public override void Start (AI ai)
    {
        enemy = ai.Body.gameObject.GetComponent<EnemyAI>();
        base.Start(ai);
    }

    public override ActionResult Execute (AI ai)
    {
        GameObject node = ai.WorkingMemory.GetItem<GameObject>("MyNode");

        if ( node == null )
            return ActionResult.FAILURE;
        else
        {
            Node myNode = node.GetComponent<Node>();
            List<Node> neighbours = node.GetComponent<Node>().GetNeighbours(myNode.Tier, myNode.Tier);
            while ( neighbours.Count > 0 )
            {
                if ( enemy.ClaimNode(neighbours.Pop(true)) )
                    return ActionResult.SUCCESS;
            }
            return ActionResult.FAILURE;
        }
    }
}