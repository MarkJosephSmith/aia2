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

	public GraphNode (){
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
