using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class RandomGraph : MonoBehaviour {

	int DesiredNodes; //how many nodes we want in the graph.  Cannot be more than MaxXNodes * MaxYNodes THIS VALUE IS NOT ZERO BASED
	int MaxXNodes; //the maximum X value the graph allows THIS VALUE IS NOT ZERO BASED
	int MaxYNodes; //the maximum Y value the graph allows THIS VALUE IS NOT ZERO BASED
	int CurrentNodes; //Total numbe of nodes currently in the graph
	//Array TheGraph; //two dimensional array of graph nodes
	GraphNode[] TheGraph;
	Queue<GraphNode> NodesWithOpenVerts; //Nodes that contain an open vert without a node on the other end
	Queue<GraphNode> NodesWithAvailibleVerts; //Nodes that contain a closed vert

	// Use this for initialization
	void Start () {

		if ((MaxXNodes * MaxYNodes) < DesiredNodes) 
		{
			Assert.IsTrue (false);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	RandomGraph(int MaxHoriz, int MaxVertical, int DesiredN)
	{
		DesiredNodes = DesiredN;
		MaxXNodes = MaxHoriz;
		MaxYNodes = MaxVertical;
		CurrentNodes = 0;
		TheGraph = new GraphNode[MaxXNodes * MaxYNodes];

	}

	//Create the first node which will be at a random location and will be used as the starting or seed node.
	GraphNode GenerateFirstNodeInGraph()
	{



		return null;
	}

	//x and y should be the zero based x and y coordinates in the graph.  Y will be converted inside.  So if y = 1 it will be converted to (1 * MaxXNodes).
	GraphNode GenerateNodeInsideGraph(int x, int y)
	{
		GraphNode ToReturn = new GraphNode ();

		int ActualYOffset = y * MaxXNodes;

		//sanity checks
		if (  ((MaxYNodes-1) < y)  ||  ((MaxXNodes-1) < x)  ||  ( TheGraph[ActualYOffset + x] !=null) || (x < 0) || (y < 0)  ) 
		{
			Assert.IsTrue (false);
		}

		//Look around this node for walls, other nodes with open verts, open spaces, and use some sort of randomization to decide what connections we want.
		int PossibleVerts = 4;

		//these checks are all individual, no else's or else if's because the graph could be a single square.
		if (x == 0) 
		{
			PossibleVerts--;
		}
		if( x == (MaxXNodes-1))
		{
			PossibleVerts--;
		}

		if (y == 0) 
		{
			PossibleVerts--;
		}
		if( y == (MaxYNodes-1))
		{
			PossibleVerts--;
		}

		int OpenVerts = (PossibleVerts > 0 ?  UnityEngine.Random.Range(1,PossibleVerts) : 0);
		int FilledVerts = 0;





		return ToReturn;
	}

	void PopulateGraph()
	{
		
	}
}
