using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class RandomGraph { // : MonoBehaviour {

	int DesiredNodes; //how many nodes we want in the graph.  Cannot be more than MaxXNodes * MaxYNodes THIS VALUE IS NOT ZERO BASED
	int MaxXNodes; //the maximum X value the graph allows THIS VALUE IS NOT ZERO BASED
	int MaxYNodes; //the maximum Y value the graph allows THIS VALUE IS NOT ZERO BASED
	int CurrentNodes; //Total numbe of nodes currently in the graph
	//Array TheGraph; //two dimensional array of graph nodes
	GraphNode[] TheGraph;
	Queue<GraphNode> NodesWithOpenVerts = new Queue<GraphNode>(); //Nodes that contain an open vert without a node on the other end
	Queue<GraphNode> NodesWithAvailibleVerts= new Queue<GraphNode>(); //Nodes that contain a closed vert

	// Use this for initialization
	void Start () {


		//NodesWithAvailibleVerts = new Queue<GraphNode>();
		//NodesWithOpenVerts = new Queue<GraphNode>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//parameters are counts and are thus not zero based.  They start at 1 if you want to actually have a graph.  An array with 3 colums and 2 rows would have columns X0, X1, X2 and rows Y0, Y1.
	public RandomGraph(int MaxHoriz, int MaxVertical, int DesiredN)
	{
		DesiredNodes = DesiredN;
		MaxXNodes = MaxHoriz;
		MaxYNodes = MaxVertical;
		CurrentNodes = 0;
		TheGraph = new GraphNode[MaxXNodes * MaxYNodes];
		Array.Clear (TheGraph, 0, TheGraph.Length - 1);


		if ( ((MaxXNodes * MaxYNodes) < DesiredNodes) || (MaxHoriz < 1)  ||  (MaxVertical <1) || (DesiredNodes < 1)  ) 
		{
			Assert.IsTrue (false);
		}


	}

	/*
	//Create the first node which will be at a random location and will be used as the starting or seed node.
	GraphNode GenerateFirstNodeInGraph()
	{



		return null;
	}
	*/

	//x and y should be the zero based x and y coordinates in the graph.  Y will be converted inside.  So if y = 1 it will be converted to (1 * MaxXNodes).  
	//Creator is the node that spawned us from an open vert, we must be open to them.  0,1,2,3 for left, right, up, down. 1 means my creator made me to his left, he is to my right.   
	//-1 for the first node of the graph
	GraphNode GenerateNodeInsideGraph(int x, int y, int Creator)
	{
		GraphNode ToReturn = new GraphNode ();
		ToReturn.X = x;
		ToReturn.Y = y;
		int ReserveVertForCreator = (Creator >= 0) ? 1 : 0;

		int ActualYOffset = y * MaxXNodes;

		//sanity checks
		if (  ((MaxYNodes) < y)  ||  ((MaxXNodes) < x)  ||  ( TheGraph[ActualYOffset + x] !=null) || (x < 0) || (y < 0) || Creator > 3 || Creator < -1 ) 
		{
			Assert.IsTrue (false);
		}

		//Look around this node for walls, other nodes with open verts, open spaces, and use some sort of randomization to decide what connections we want.
		int PossibleVerts = 4;

		//these checks are all individual, no else's or else if's because the graph could be a single square.
		if (x == 0) 
		{
			PossibleVerts--;
			ToReturn.LeftVert.LinksToWall = true;
			ToReturn.LeftVert.FirstNodeOpen = false;
			ToReturn.LeftVert.SecondNodeOpen = false;
			Assert.IsTrue (Creator != 0);
		}
		if( x == (MaxXNodes-1))
		{
			PossibleVerts--;
			ToReturn.RightVert.LinksToWall = true;
			ToReturn.RightVert.FirstNodeOpen = false;
			ToReturn.RightVert.SecondNodeOpen = false;
			Assert.IsTrue (Creator != 1);
		}

		if (y == 0) 
		{
			PossibleVerts--;
			ToReturn.UpVert.LinksToWall = true;
			ToReturn.UpVert.FirstNodeOpen = false;
			ToReturn.UpVert.SecondNodeOpen = false;
			Assert.IsTrue (Creator != 2);
		}
		if( y == (MaxYNodes-1))
		{
			PossibleVerts--;
			ToReturn.DownVert.LinksToWall = true;
			ToReturn.DownVert.FirstNodeOpen = false;
			ToReturn.DownVert.SecondNodeOpen = false;
			Assert.IsTrue (Creator != 3);
		}

		int OpenVerts = (PossibleVerts > 0 ?  UnityEngine.Random.Range(1,PossibleVerts+1) : 0);
		int FilledVerts = 0 + ReserveVertForCreator;

		//set up the node's variables:
		ToReturn.NumVerts = OpenVerts;
		ToReturn.NumPossibleVerts = PossibleVerts;


		/**/
		//set to open for the vert that spawned us
		if (Creator == 0) //my creator made me to his right, he is to my left.
		{
			GraphNode MyParent = GetNodeAtLocation(x-1, y);
			ToReturn.LeftVert = MyParent.RightVert;
			ToReturn.LeftVert.SecondNode = ToReturn;
			ToReturn.LeftVert.SecondNodeOpen = true;
			ToReturn.LeftVert.FirstNodeOpen = true;
		}
		else if(Creator == 1)//my creator made me to his left, he is to my right.
		{
			GraphNode MyParent = GetNodeAtLocation(x+1, y);
			ToReturn.RightVert = MyParent.LeftVert;
			ToReturn.RightVert.SecondNode = ToReturn;
			ToReturn.RightVert.SecondNodeOpen = true;
			ToReturn.RightVert.FirstNodeOpen = true;
		}
		else if (Creator == 2) //my creator made me to his down, he is to my up.
		{
			GraphNode MyParent = GetNodeAtLocation(x, y-1);
			ToReturn.UpVert = MyParent.DownVert;
			ToReturn.UpVert.SecondNode = ToReturn;
			ToReturn.UpVert.SecondNodeOpen = true;
			ToReturn.UpVert.FirstNodeOpen = true;
		}
		else if (Creator == 3) //my creator made me to his up, he is to my down.
		{
			GraphNode MyParent = GetNodeAtLocation(x, y+1);
			ToReturn.DownVert = MyParent.UpVert;
			ToReturn.DownVert.SecondNode = ToReturn;
			ToReturn.DownVert.SecondNodeOpen = true;
			ToReturn.DownVert.FirstNodeOpen = true;
		}


			

		//Check for surounding open verts, do they point to an empty space or a taken one?  Are we already linked?
		if (!(ToReturn.LeftVert.LinksToWall)) 
		{
			//check for a node with an open connection
			GraphNode ToCheck = GetNodeAtLocation(x-1, y);


			if (ToCheck != null)
			{
				ToReturn.LeftVert = ToCheck.RightVert;
				ToReturn.LeftVert.SecondNode = ToReturn;

				//sanity check to make sure I remember how object references work in C++
				Assert.IsTrue(ToCheck.RightVert.SecondNode != null);
				Assert.IsTrue(ToCheck.RightVert.SecondNode == ToReturn);


				//set the status of the verts and register with the appropriate list
				if (Creator == 0 && !ToReturn.LeftVert.SecondNodeOpen) //my creator made me to his right, he is to my left.
				{
					ToReturn.LeftVert.SecondNodeOpen = true;				//set to open for the vert that spawned us
				}
				else if (OpenVerts > FilledVerts) 
				{
					ToReturn.LeftVert.SecondNodeOpen = true;
					FilledVerts++;

				} 
				else
				{
					NodesWithAvailibleVerts.Enqueue (ToReturn);
				}




			}
			else
			{
				ToReturn.LeftVert.FirstNode = ToReturn;
				if (OpenVerts > FilledVerts)
				{
					FilledVerts++;
					ToReturn.LeftVert.FirstNodeOpen = true;
					NodesWithOpenVerts.Enqueue (ToReturn);
				} 
				else
				{
					NodesWithAvailibleVerts.Enqueue (ToReturn);
				}

			}


		}

		if (!(ToReturn.RightVert.LinksToWall)) 
		{
			//check for a node with an open connection
			GraphNode ToCheck = GetNodeAtLocation(x+1, y);

			if (ToCheck != null)
			{
				ToReturn.RightVert = ToCheck.LeftVert;
				ToReturn.RightVert.SecondNode = ToReturn;

				//sanity check to make sure I remember how object references work in C++
				Assert.IsTrue(ToCheck.LeftVert.SecondNode != null);
				Assert.IsTrue (ToCheck.LeftVert.SecondNode == ToReturn);

				//set the status of the verts and register with the appropriate list
				if (Creator == 1 && !ToReturn.RightVert.SecondNodeOpen) //my creator made me to his left, he is to my right.
				{
					ToReturn.RightVert.SecondNodeOpen = true;				//set to open for the vert that spawned us
				}
				else if (OpenVerts > FilledVerts)
				{
					ToReturn.RightVert.SecondNodeOpen = true;
					FilledVerts++;

				}
				else
				{
					NodesWithAvailibleVerts.Enqueue (ToReturn);
				}



			}
			else
			{
				ToReturn.RightVert.FirstNode = ToReturn;
				if (OpenVerts > FilledVerts)
				{
					FilledVerts++;
					ToReturn.RightVert.FirstNodeOpen = true;
					NodesWithOpenVerts.Enqueue (ToReturn);
				}
				else
				{
					NodesWithAvailibleVerts.Enqueue (ToReturn);
				}

			}
		}

		if (!(ToReturn.UpVert.LinksToWall)) 
		{
			//check for a node with an open connection
			GraphNode ToCheck = GetNodeAtLocation(x, y-1);

			if (ToCheck != null)
			{
				ToReturn.UpVert = ToCheck.DownVert;
				ToReturn.UpVert.SecondNode = ToReturn;



				//sanity check to make sure I remember how object references work in C++
				Assert.IsTrue(ToCheck.DownVert.SecondNode != null);
				Assert.IsTrue (ToCheck.DownVert.SecondNode == ToReturn);

				//set the status of the verts and register with the appropriate list
				if (Creator == 2 && !ToReturn.UpVert.SecondNodeOpen) //my creator made me to his down, he is to my up.
				{
					ToReturn.UpVert.SecondNodeOpen = true;				//set to open for the vert that spawned us
				}
				else if (OpenVerts > FilledVerts)
				{
					ToReturn.UpVert.SecondNodeOpen = true;
					FilledVerts++;

				}
				else
				{
					NodesWithAvailibleVerts.Enqueue (ToReturn);
				}



			}
			else
			{
				ToReturn.UpVert.FirstNode = ToReturn;
				if (OpenVerts > FilledVerts)
				{
					FilledVerts++;
					ToReturn.UpVert.FirstNodeOpen = true;
					NodesWithOpenVerts.Enqueue (ToReturn);
				}
				else
				{
					NodesWithAvailibleVerts.Enqueue (ToReturn);
				}

			}
		}

		if (!(ToReturn.DownVert.LinksToWall)) 
		{
			//check for a node with an open connection
			GraphNode ToCheck = GetNodeAtLocation(x, y+1);

			if (ToCheck != null)
			{
				ToReturn.DownVert = ToCheck.UpVert;
				ToReturn.DownVert.SecondNode = ToReturn;



				//sanity check to make sure I remember how object references work in C++
				Assert.IsTrue(ToCheck.UpVert.SecondNode != null);
				Assert.IsTrue (ToCheck.UpVert.SecondNode == ToReturn);

				//set the status of the verts and register with the appropriate list
				//set to open for the vert that spawned us
				if (Creator == 3 && !ToReturn.DownVert.SecondNodeOpen) //my creator made me to his up, he is to my down.
				{
					ToReturn.DownVert.SecondNodeOpen = true;
				}
				else if (OpenVerts > FilledVerts)
				{
					ToReturn.DownVert.SecondNodeOpen = true;
					FilledVerts++;

				}
				else
				{
					NodesWithAvailibleVerts.Enqueue (ToReturn);
				}



			}
			else
			{
				ToReturn.DownVert.FirstNode = ToReturn;
				if (OpenVerts > FilledVerts)
				{
					FilledVerts++;
					ToReturn.DownVert.FirstNodeOpen = true;
					NodesWithOpenVerts.Enqueue (ToReturn); //register with the appropriate lists
				}
				else
				{
					NodesWithAvailibleVerts.Enqueue (ToReturn);
				}

			}

		}



		if (OpenVerts > FilledVerts)
		{
			Assert.IsTrue (false); //don't think this should ever fail to fill all of its open verts, inserting in the list should be in the else statement where we create a new empty vert
		}
		if (PossibleVerts > OpenVerts)//register with the appropriate lists
		{
			NodesWithAvailibleVerts.Enqueue (ToReturn);
		}

		ToReturn.MyNodeNumber = CurrentNodes;
		CurrentNodes++;
		TheGraph [ToReturn.X + (ToReturn.Y * MaxXNodes)] = ToReturn;



		if (Creator == 0) //my creator made me to his right, he is to my left.
		{
			GraphNode MyParent = GetNodeAtLocation(x-1, y);

			if ((ToReturn.LeftVert.FirstNode == null) || (ToReturn.LeftVert.FirstNodeOpen == false))
			{
				Assert.IsTrue (false);
			}


			Assert.IsFalse ((ToReturn.LeftVert.FirstNode == null) || (ToReturn.LeftVert.FirstNodeOpen == false));
			/**/
			Assert.IsFalse((ToReturn.LeftVert.FirstNode != MyParent));

			Assert.IsFalse ((ToReturn.LeftVert.SecondNodeOpen != true) || ToReturn.LeftVert.SecondNode == null);

			Assert.IsFalse((ToReturn.LeftVert.SecondNode != ToReturn));

			Assert.IsFalse (MyParent.RightVert.SecondNodeOpen != true);
			Assert.IsFalse (MyParent.RightVert.SecondNode != ToReturn);

			Assert.IsTrue (ToReturn.LeftVert == MyParent.RightVert);



		}
		else if(Creator == 1)//my creator made me to his left, he is to my right.
		{
			GraphNode MyParent = GetNodeAtLocation(x+1, y);


			if ((ToReturn.RightVert.FirstNode == null) || (ToReturn.RightVert.FirstNodeOpen == false))
			{
				Assert.IsTrue (false);
			}


			Assert.IsFalse ((ToReturn.RightVert.FirstNode == null) || (ToReturn.RightVert.FirstNodeOpen == false));

			Assert.IsFalse((ToReturn.RightVert.FirstNode != MyParent));

			Assert.IsFalse ((ToReturn.RightVert.SecondNodeOpen != true) || ToReturn.RightVert.SecondNode == null);

			Assert.IsFalse((ToReturn.RightVert.SecondNode != ToReturn));

			Assert.IsFalse (MyParent.LeftVert.SecondNodeOpen != true);
			Assert.IsFalse (MyParent.LeftVert.SecondNode != ToReturn);

			Assert.IsTrue (ToReturn.RightVert == MyParent.LeftVert);
		}
		else if (Creator == 2) //my creator made me to his down, he is to my up.
		{
			GraphNode MyParent = GetNodeAtLocation(x, y-1);

			if ((ToReturn.UpVert.FirstNode == null) || (ToReturn.UpVert.FirstNodeOpen == false))
			{
				Assert.IsTrue (false);
			}

			Assert.IsFalse ((ToReturn.UpVert.FirstNode == null) || (ToReturn.UpVert.FirstNodeOpen == false));

			Assert.IsFalse((ToReturn.UpVert.FirstNode != MyParent));

			Assert.IsFalse ((ToReturn.UpVert.SecondNodeOpen != true) || ToReturn.UpVert.SecondNode == null);

			Assert.IsFalse((ToReturn.UpVert.SecondNode != ToReturn));

			Assert.IsFalse (MyParent.DownVert.SecondNodeOpen != true);
			Assert.IsFalse (MyParent.DownVert.SecondNode != ToReturn);

			Assert.IsTrue (ToReturn.UpVert == MyParent.DownVert);
		}
		else if (Creator == 3) //my creator made me to his up, he is to my down.
		{
			GraphNode MyParent = GetNodeAtLocation(x, y+1);

			if ((ToReturn.DownVert.FirstNode == null) || (ToReturn.DownVert.FirstNodeOpen == false))
			{
				Assert.IsTrue (false);
			}

			Assert.IsFalse ((ToReturn.DownVert.FirstNode == null) || (ToReturn.DownVert.FirstNodeOpen == false));

			Assert.IsFalse((ToReturn.DownVert.FirstNode != MyParent));

			Assert.IsFalse ((ToReturn.DownVert.SecondNodeOpen != true) || ToReturn.DownVert.SecondNode == null);

			Assert.IsFalse((ToReturn.DownVert.SecondNode != ToReturn));

			Assert.IsFalse (MyParent.UpVert.SecondNodeOpen != true);
			Assert.IsFalse (MyParent.UpVert.SecondNode != ToReturn);

			Assert.IsTrue (ToReturn.DownVert == MyParent.UpVert);
		}


		return ToReturn;
	}

	public void PopulateGraph()
	{
		GraphNode TempNode;
		int firstNodeX = UnityEngine.Random.Range (1, MaxXNodes);
		int firstNodeY = UnityEngine.Random.Range (1, MaxYNodes);

		//make the first node
		TempNode = GenerateNodeInsideGraph(firstNodeX, firstNodeY, -1);

		//while we have not reached the desired number of nodes, keep making nodes
		while (CurrentNodes < DesiredNodes)
		{
			int Shuffle = UnityEngine.Random.Range(0,2);

			if (NodesWithOpenVerts.Count > 0)
			{
				//shuffle
				if (Shuffle == 1)
				{
					TempNode = NodesWithOpenVerts.Dequeue ();
					NodesWithOpenVerts.Enqueue (TempNode);
				} 
				else
				{

					//get a node
					TempNode = NodesWithOpenVerts.Dequeue();

					//check for availible connections
					int NewNodeX = TempNode.X;
					int NewNodeY = TempNode.Y;
					bool FoundOpenEmpty = false;

					//scan all 4 directions
					if (TempNode.LeftVert.FirstNodeOpen && !TempNode.LeftVert.LinksToWall && !FoundOpenEmpty)
					{
						if (TempNode.LeftVert.SecondNodeOpen)
						{
							//both are already open, do nothing.  
						} 
						else if ( (GetNodeAtLocation (TempNode.X - 1, TempNode.Y)) == null)
						{
							//found an open space, insert a node there and be done with this popped node
							FoundOpenEmpty = true;
							GenerateNodeInsideGraph (TempNode.X - 1, TempNode.Y, 1);
						}
					}

					if (TempNode.RightVert.FirstNodeOpen && !TempNode.RightVert.LinksToWall && !FoundOpenEmpty)
					{
						if (TempNode.RightVert.SecondNodeOpen)
						{
							//both are already open, do nothing.  
						} 
						else if (GetNodeAtLocation (TempNode.X + 1, TempNode.Y) == null)
						{
							//found an open space, insert a node there and be done with this popped node
							FoundOpenEmpty = true;
							GenerateNodeInsideGraph (TempNode.X + 1, TempNode.Y, 0);
						}
					}

					if (TempNode.UpVert.FirstNodeOpen && !TempNode.UpVert.LinksToWall && !FoundOpenEmpty)
					{
						if (TempNode.UpVert.SecondNodeOpen)
						{
							//both are already open, do nothing.  
						} 
						else if (GetNodeAtLocation (TempNode.X , TempNode.Y - 1) == null)
						{
							//found an open space, insert a node there and be done with this popped node
							FoundOpenEmpty = true;
							GenerateNodeInsideGraph (TempNode.X , TempNode.Y - 1, 3);
						}
					}

					if (TempNode.DownVert.FirstNodeOpen && !TempNode.DownVert.LinksToWall && !FoundOpenEmpty)
					{
						if (TempNode.DownVert.SecondNodeOpen)
						{
							//both are already open, do nothing.  
						} 
						else if (GetNodeAtLocation (TempNode.X , TempNode.Y +1 ) == null)
						{
							//found an open space, insert a node there and be done with this popped node
							FoundOpenEmpty = true;
							GenerateNodeInsideGraph (TempNode.X, TempNode.Y + 1, 2);
						}
					}
						
				}				

			} 
			else  //if we ever run out of nodes to make from open links, open a new link
			{
				Assert.IsTrue (NodesWithAvailibleVerts.Count > 0);

				//shuffle
				if (Shuffle == 1)
				{
					TempNode = NodesWithAvailibleVerts.Dequeue ();
					NodesWithAvailibleVerts.Enqueue (TempNode);
				} 
				else
				{
					//get a node
					TempNode = NodesWithAvailibleVerts.Dequeue();

					//check for availible connections
					int NewNodeX = TempNode.X;
					int NewNodeY = TempNode.Y;
					bool FoundOpenAvailible = false;

					//check for availible connections

					//scan all 4 directions
					if(!TempNode.LeftVert.LinksToWall && !FoundOpenAvailible)                                             //(TempNode.LeftVert.FirstNodeOpen && !TempNode.LeftVert.LinksToWall && !FoundOpenEmpty)
					{

						if (TempNode.LeftVert.FirstNodeOpen && TempNode.LeftVert.SecondNodeOpen)
						{
							//This has become an open connection, do nothing
						} 
						else if (!TempNode.LeftVert.FirstNodeOpen && !TempNode.LeftVert.SecondNodeOpen && !(GetNodeAtLocation (TempNode.X-1 , TempNode.Y ) == null))
						{
							//This is a closed off connection between existing nodes, do nothing
						} 
						else if (GetNodeAtLocation (TempNode.X-1 , TempNode.Y ) != null)
						{
							//One of us is keeping the link closed, do nothing for now.  may change this behavior in the future.
						} 
						else //There's a possible connection, make it happen
						{
							FoundOpenAvailible = true;
							GenerateNodeInsideGraph (TempNode.X-1 , TempNode.Y , 1);
						}

					}
					///////////////////////
					if(!TempNode.RightVert.LinksToWall && !FoundOpenAvailible)                                             //(TempNode.LeftVert.FirstNodeOpen && !TempNode.LeftVert.LinksToWall && !FoundOpenEmpty)
					{

						if (TempNode.RightVert.FirstNodeOpen && TempNode.RightVert.SecondNodeOpen)
						{
							//This has become an open connection, do nothing
						} 
						else if (!TempNode.RightVert.FirstNodeOpen && !TempNode.RightVert.SecondNodeOpen && !(GetNodeAtLocation (TempNode.X+1 , TempNode.Y ) == null))
						{
							//This is a closed off connection between existing nodes, do nothing
						} 
						else if (GetNodeAtLocation (TempNode.X+1 , TempNode.Y ) != null)
						{
							//One of us is keeping the link closed, do nothing for now.  may change this behavior in the future.
						} 
						else //There's a possible connection, make it happen
						{
							FoundOpenAvailible = true;
							GenerateNodeInsideGraph (TempNode.X+1 , TempNode.Y , 0);
						}

					}

					if(!TempNode.UpVert.LinksToWall && !FoundOpenAvailible)                                             //(TempNode.LeftVert.FirstNodeOpen && !TempNode.LeftVert.LinksToWall && !FoundOpenEmpty)
					{

						if (TempNode.UpVert.FirstNodeOpen && TempNode.UpVert.SecondNodeOpen)
						{
							//This has become an open connection, do nothing
						} 
						else if (!TempNode.UpVert.FirstNodeOpen && !TempNode.UpVert.SecondNodeOpen && !(GetNodeAtLocation (TempNode.X , TempNode.Y-1 ) == null))
						{
							//This is a closed off connection between existing nodes, do nothing
						} 
						else if ((GetNodeAtLocation (TempNode.X , TempNode.Y-1 ) != null))
						{
							//One of us is keeping the link closed, do nothing for now.  may change this behavior in the future.
						} 
						else //There's a possible connection, make it happen
						{
							FoundOpenAvailible = true;
							GenerateNodeInsideGraph (TempNode.X , TempNode.Y-1 , 3);
						}

					}

					if(!TempNode.DownVert.LinksToWall && !FoundOpenAvailible)                                             //(TempNode.LeftVert.FirstNodeOpen && !TempNode.LeftVert.LinksToWall && !FoundOpenEmpty)
					{

						if (TempNode.DownVert.FirstNodeOpen && TempNode.DownVert.SecondNodeOpen)
						{
							//This has become an open connection, do nothing
						} else if (!TempNode.DownVert.FirstNodeOpen && !TempNode.DownVert.SecondNodeOpen && !(GetNodeAtLocation (TempNode.X, TempNode.Y + 1) == null))
						{
							//This is a closed off connection between existing nodes, do nothing
						} else if ((GetNodeAtLocation (TempNode.X, TempNode.Y + 1) != null))
						{
							//One of us is keeping the link closed, do nothing for now.  may change this behavior in the future.
						}
						else //There's a possible connection, make it happen
						{
							FoundOpenAvailible = true;
							GenerateNodeInsideGraph (TempNode.X , TempNode.Y+1 , 2);
						}

					}


				}

			}


		}

		PrintGraph ();
		
	}

	//Y val will be recalculated inside the function
	GraphNode GetNodeAtLocation(int x, int y)
	{
		GraphNode ToReturn = TheGraph [(x + (y * MaxXNodes))];
		return ToReturn;

	}

	void PrintGraph()
	{
		int printedNodes = 0;
		int currentX = 0;
		int currentY = 0;
		string ToPrint = "";

		while (printedNodes < (MaxXNodes * MaxYNodes))
		{
			GraphNode TempNode = TheGraph [currentX + (currentY * MaxXNodes)];

			if (TempNode != null)
			{
				ToPrint = ToPrint + "{" + TempNode.X + "," + TempNode.Y + "(" + TempNode.LeftVert.FirstNodeOpen + "," + TempNode.RightVert.FirstNodeOpen + "," + TempNode.UpVert.FirstNodeOpen + "," + TempNode.DownVert.FirstNodeOpen + ")} | ";
				//Debug.Log ("{"+ TempNode.X + "," + TempNode.Y +"(" + TempNode.LeftVert.FirstNodeOpen + "," + TempNode.RightVert.FirstNodeOpen + "," +TempNode.UpVert.FirstNodeOpen + ","+ TempNode.DownVert.FirstNodeOpen + ")}     |     ");
			}
			else
			{
				ToPrint = ToPrint + "{" + currentX + "," + currentY + "(" + " )}";
				//Debug.Log("{" + currentX + "," + currentY + "(" + "                    )}");
			}



			if (currentX == (MaxXNodes -1))
			{
				Debug.Log (ToPrint + "\n");
				//Debug.Log ("\n");
				currentX = 0;
				currentY++;
				ToPrint = "";
				//print a new line
				//set x to 0
				//move y down
				
			} 
			else
			{
				currentX++;
			}
			printedNodes++;
		}


	}
}
