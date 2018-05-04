using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using RandomGraph;

public class MakeGraphOnGameStart : MonoBehaviour {

	public int maxX;
	public int maxY;
	public int desiredNodes;
	public RandomGraph MyGraph;

	// Use this for initialization
	void Start () {

		MyGraph = new RandomGraph (maxX, maxY, desiredNodes);
		MyGraph.PopulateGraph ();

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
