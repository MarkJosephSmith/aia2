using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVert : MonoBehaviour {

	private GraphNode FirstNode { set; get;}
	private GraphNode SecondNode { set; get;}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	bool HasFirst()
	{ 
		if (FirstNode == null)
		{
			return false;
		}
		return true;
	}

	bool HasSecond()
	{ 
		if (SecondNode == null)
		{
			return false;
		}
		return true;
	}


}
