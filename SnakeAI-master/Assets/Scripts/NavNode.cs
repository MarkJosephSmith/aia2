using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//little class for storing nav information.  could have been a struct.  
public class NavNode : MonoBehaviour {

	//Store the node, its best parent node, gcost, hcost, fcost
	public GraphNode MyGraphNode = new GraphNode();
	public GraphNode MyParentNode = new GraphNode();
	public int GCost;
	public int HCost;
	public int FCost;

	public NavNode(GraphNode Me, GraphNode MyParent, int G = 0, int H = 0, int F = 0)
	{
		MyGraphNode = Me;
		MyParentNode = MyParent;
		GCost = G;
		HCost = H;
		FCost = F;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
