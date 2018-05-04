/*WARNING!  THESE ARE ACTUALLY THE GRAPH EDGES, NOT VERTICIES.*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*WARNING!  THESE ARE ACTUALLY THE GRAPH EDGES, NOT VERTICIES.*/
public class GraphVert {    // : MonoBehaviour {

	public GraphNode FirstNode;
	public GraphNode SecondNode;


	public bool FirstNodeOpen;
	public bool SecondNodeOpen;
	public bool LinksToWall;

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


}
