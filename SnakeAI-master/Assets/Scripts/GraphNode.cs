using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode{ // : MonoBehaviour {                   //public class GraphNode{   //public class GraphNode : MonoBehaviour {

	//GraphVert [] MyVerts = new GraphVert[4];
	public GraphVert UpVert = new GraphVert();
	public GraphVert DownVert = new GraphVert();
	public GraphVert LeftVert = new GraphVert();
	public GraphVert RightVert = new GraphVert();
	public int NumVerts {  get;  set; } //how many verts I currently have
	public int NumPossibleVerts {  get;  set;} //max number of verts I could have.
	public int X {  get;  set;}
	public int Y {  get;  set;}
	public bool IsWall {  get;  set;} //A wall node has only one node connected to it and that vert is always closed.
	public int MyNodeNumber;

	public bool IsStart = false;
	public bool IsGoal = false;

	public GraphNode MyParentNode;
	public int GCost = -1;
	public int HCost = -1;
	public int FCost = -1;

	public GraphNode (){
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//gives the magnitude of the distance between two nodes by getting the difference in X and the difference in Y
	public int DistanceBetweenNodes(GraphNode Node2)
	{
		int ToReturn = 0;

		ToReturn = Mathf.Abs (Mathf.Abs (this.X) - Mathf.Abs (Node2.X)) + Mathf.Abs (Mathf.Abs (this.Y) - Mathf.Abs (Node2.Y));
		return ToReturn;
	}
}
