/*WARNING!  THESE ARE ACTUALLY THE GRAPH EDGES, NOT VERTICIES.*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
/*WARNING!  THESE ARE ACTUALLY THE GRAPH EDGES, NOT VERTICIES.*/
public class GraphVert {    // : MonoBehaviour {

	public GraphNode FirstNode;
	public GraphNode SecondNode;


	public bool FirstNodeOpen;
	public bool SecondNodeOpen;
	public bool LinksToWall;

	public int TravelCost = 1;

	// Use this for initialization
	void Start () {
		 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool HasFirst()
	{ 
		if (FirstNode == null)
		{
			return false;
		}
		return true;
	}

	public bool HasSecond()
	{ 
		if (SecondNode == null)
		{
			return false;
		}
		return true;
	}

	public bool OpenPath()
	{
		return (FirstNodeOpen && SecondNodeOpen && (!LinksToWall));
	}

	//given a graph node, return the other node connected by this vert
	public GraphNode GetConnectedNode(GraphNode StartingNode)
	{
		if (StartingNode == FirstNode)
		{
			return SecondNode;
		} else if (StartingNode == SecondNode)
		{
			return FirstNode;
		} else
		{
			Assert.IsTrue (false);
			return null;
		}

	}



}
