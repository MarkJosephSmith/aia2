using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour {

	//GraphVert [] MyVerts = new GraphVert[4];
	GraphVert UpVert;
	GraphVert DownVert;
	GraphVert LeftVert;
	GraphVert RightVert;
	int NumVerts { get; set; } //how many verts I currently have
	int NumOpenVerts {get; set;}
	int X { get; set;}
	int Y { get; set;}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
