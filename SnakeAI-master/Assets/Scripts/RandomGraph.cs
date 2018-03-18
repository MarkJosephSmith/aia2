using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class RandomGraph : MonoBehaviour {

	int DesiredNodes; //how many nodes we want in the graph.  Cannot be more than MaxXNodes * MaxYNodes
	int MaxXNodes; //the maximum X value the graph allows
	int MaxYNodes; //the maximum Y value the graph allows
	int CurrentNodes; //Total numbe of nodes currently in the graph
	//Array TheGraph; //two dimensional array of graph nodes
	GraphNode[] TheGraph;

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

	void GenerateGraph()
	{
		
	}
}
