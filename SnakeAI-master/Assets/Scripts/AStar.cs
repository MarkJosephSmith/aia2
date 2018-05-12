using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;



public class AStar : MonoBehaviour {
	//hcost = distance to end
	//gcost = distance from start
	//fcost = gcost+hcost
	//choose lowest f, if equal choose lowestt h, if equal random


	//need a reference to the graph
	MakeGraphOnGameStart GameGraph; 

	//public Queue<GraphNode> ToVisit;
	//public SimplePriorityQueue<GraphNode> ToVisit = new SimplePriorityQueue<GraphNode>();
	public Queue<GraphNode> ToVisit;
	public HashSet<GraphNode> Visited = new HashSet<GraphNode>();

	//these are for the table.
	float TimeToComplete = 0;
	int NumberVisited = 0;
	int NumberExpanded = 0;

	void Start()
	{
		GameGraph = GetComponent<MakeGraphOnGameStart>();
	}

	Stack<GraphNode> Search(int StartX, int StartY, int GoalX, int GoalY, out string TimeInMili, out int NumVisited, out int NumExpanded)
	{
		Stopwatch SW = new Stopwatch ();
		SW.Start;
		//setup start and end nodes
		GraphNode StartNode = GameGraph.MyGraph.GetNodeAtLocation(StartX, StartY);
		GraphNode GoalNode = GameGraph.MyGraph.GetNodeAtLocation(GoalX, GoalY);

		StartNode.GCost = 0;
		StartNode.HCost = StartNode.DistanceBetweenNodes (GoalNode);
		StartNode.FCost = StartNode.HCost;
		StartNode.MyParentNode = null;

		GoalNode.GCost = StartNode.DistanceBetweenNodes (StartNode);
		GoalNode.HCost = 0;
		GoalNode.FCost = GoalNode.GCost;

		//NavNode Start = new NavNode (StartNode, null, StartNode.DistanceBetweenNodes(GoalNode), 0, StartNode.DistanceBetweenNodes(GoalNode));

		ToVisit.Enqueue(StartNode);
		NumberExpanded++;


		//loop while there are nodes to find
		GraphNode TempPoppedNode = new GraphNode();

		while (ToVisit.Count > 0)
		{
			//get new node to ToVisit
			TempPoppedNode = ToVisit.Dequeue();

			//Reture node
			Visited.Add(TempPoppedNode);
			NumberVisited++;

			if (TempPoppedNode == GoalNode) //if we found the goal, we are done
			{
				break;
			}


			//calc costs of surrounding nodes.

			//check left
			GraphNode TempSuroundingNode = TempPoppedNode.LeftVert.GetConnectedNode(TempPoppedNode);
			if (TempPoppedNode.LeftVert.OpenPath() && (!Visited.Contains(TempSuroundingNode)))
			{
				UpdateSuroundingNode (TempPoppedNode, TempPoppedNode.LeftVert, StartNode, GoalNode);

			}

			//check right
			TempSuroundingNode = TempPoppedNode.RightVert.GetConnectedNode(TempPoppedNode);
			if (TempPoppedNode.RightVert.OpenPath() && (!Visited.Contains(TempSuroundingNode)))
			{
				UpdateSuroundingNode (TempPoppedNode, TempPoppedNode.RightVert, StartNode, GoalNode);

			}
			//check up
			TempSuroundingNode = TempPoppedNode.UpVert.GetConnectedNode(TempPoppedNode);
			if (TempPoppedNode.UpVert.OpenPath() && (!Visited.Contains(TempSuroundingNode)))
			{
				UpdateSuroundingNode (TempPoppedNode, TempPoppedNode.UpVert, StartNode, GoalNode);

			}
			//check down
			TempSuroundingNode = TempPoppedNode.DownVert.GetConnectedNode(TempPoppedNode);
			if (TempPoppedNode.DownVert.OpenPath() && (!Visited.Contains(TempSuroundingNode)))
			{
				UpdateSuroundingNode (TempPoppedNode, TempPoppedNode.DownVert, StartNode, GoalNode);

			}
				
		}

		//When the loop is finished, return the path
		Stack<GraphNode> BestPath = TracePath(TempPoppedNode);

		SW.Stop ();
		TimeInMili = SW.Elapsed.TotalMilliseconds.ToString();
		NumExpanded = NumberExpanded;
		NumVisited = NumberVisited;
		return BestPath;


	}

	public void UpdateSuroundingNode(GraphNode CurrentNode, GraphVert ToCheck, GraphNode StartNode, GraphNode GoalNode)
	{
		GraphNode OtherNode = ToCheck.GetConnectedNode(StartNode);

		//calc cost to visit
		int NewGCost = StartNode.GCost + ToCheck.TravelCost;
		int NewHCost = GoalNode.DistanceBetweenNodes (OtherNode);
		int NewFCost = NewHCost + NewGCost;

		//update costs if they haven't been
		if (OtherNode.FCost < 0 ||  OtherNode.GCost < 0 || OtherNode.HCost < 0)
		{
			OtherNode.FCost = NewFCost;
			OtherNode.GCost = NewGCost;
			OtherNode.HCost = NewHCost;
		}

		//check for a better FCost and update if needed
		if (OtherNode.FCost < NewFCost || (OtherNode.FCost == NewFCost && OtherNode.HCost < NewHCost))
		{
			//update the cost and parent info
			OtherNode.MyParentNode = CurrentNode;
			OtherNode.FCost = NewFCost;
			OtherNode.GCost = NewGCost;
			OtherNode.HCost = NewHCost;
			 

			if (ToVisit.Contains (OtherNode)) //We have changed the cost of our node, rebalance the heap
			{
				//ToVisit.UpdatePriority (OtherNode, OtherNode.FCost);
			} 

		}

		//also need to set parent
		if (!ToVisit.Contains (OtherNode))
		{
			OtherNode.MyParentNode = CurrentNode;
			//ToVisit.Enqueue (OtherNode, OtherNode.FCost);
			NumberExpanded++;
		}
	}


	public Stack<GraphNode> TracePath(GraphNode EndOfPath)
	{
		Stack<GraphNode> ToReturn = new Stack<GraphNode> ();
		GraphNode TempNode = EndOfPath;
		while (TempNode.MyParentNode != null)
		{
			ToReturn.Push (TempNode);
			TempNode = TempNode.MyParentNode;
		}

		return ToReturn;
	}

}
