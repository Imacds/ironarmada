using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	public bool displayGridGizmos;
	public LayerMask unwalkableMask;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	Node[,] grid;

	Transform myTransform;
	float nodeDiameter;
	float nodeMargin = 0.02f;
	int gridSizeX, gridSizeY;

	void Awake() 
	{
		myTransform = transform;
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	public int MaxSize 
	{
		get 
		{
			return gridSizeX * gridSizeY;
		}
	}

	void CreateGrid() 
	{
		grid = new Node[gridSizeX,gridSizeY];
		Vector2 transPos2D = myTransform.position;
		Vector2 worldBottomLeft = transPos2D - Vector2.right * gridWorldSize.x/2 - Vector2.up * gridWorldSize.y/2;

		for (int x = 0; x < gridSizeX; x ++) 
		{
			for (int y = 0; y < gridSizeY; y ++) 
			{
				Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius-nodeMargin, unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint, x,y);
			}
		}
	}

	public List<Node> GetNeighbours(Node node) 
	{
		List<Node> neighbours = new List<Node>();

		bool rightBlocked = true;
		bool topBlocked = true;
		bool leftBlocked = true;
		bool bottomBlocked = true;

		//add adjacent neighbors
		for (int x = -1; x <= 1; x++) 
		{
			for (int y = -1; y <= 1; y++) 
			{
				if (x == 0 && y == 0)
					continue;
				
				if (x == 0 || y == 0)
				{
					int checkX = node.gridX + x;
					int checkY = node.gridY + y;

					if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) 
					{
						// If directly adjacent, always add as neighbor
						neighbours.Add(grid[checkX,checkY]);

						// Test for blocked nodes
						if (x == 1)
							rightBlocked = !grid[checkX,checkY].walkable;
						else if (y == 1)
							topBlocked = !grid[checkX,checkY].walkable;
						else if (x == -1)
							leftBlocked = !grid[checkX,checkY].walkable;
						else if (y == -1)
							bottomBlocked = !grid[checkX,checkY].walkable;
					}
				}
			}
		}

		// Diagonal bools are derived from adjacent
		bool topRightBlocked = rightBlocked || topBlocked;
		bool topLeftBlocked = topBlocked || leftBlocked;
		bool bottomLeftBlocked = leftBlocked || bottomBlocked;
		bool bottomRightBlocked = bottomBlocked || rightBlocked;

		//add diagonal neighbors
		for (int x = -1; x <= 1; x++) 
		{
			for (int y = -1; y <= 1; y++) 
			{
				if (x == 0 && y == 0)
					continue;
				
				if (x == 0 || y == 0)
					continue;


				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) 
				{
					// if not a diagonal or is a non-blocked diagonal, add to neighbor list
					if ( (x == 1 && y == 1 && !topRightBlocked) 
						|| (x == -1 && y == 1 && !topLeftBlocked)
						|| (x == -1 && y == -1 && !bottomLeftBlocked)
						|| (x == 1 && y == -1 && !bottomRightBlocked) )
					{
						neighbours.Add(grid[checkX,checkY]);
					}
				}
			}
		}

		return neighbours;
	}


	public Node NodeFromWorldPoint(Vector2 worldPosition) 
	{
		Vector2 transPos2D = myTransform.position;

		float percentX = (worldPosition.x + gridWorldSize.x/2 - transPos2D.x) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y/2 - transPos2D.y) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX) * percentX - 0.5f);
		int y = Mathf.RoundToInt((gridSizeY) * percentY - 0.5f);
		return grid[x,y];
	}

	void OnDrawGizmos() 
	{
		Gizmos.DrawWireCube(myTransform.position,new Vector2(gridWorldSize.x, gridWorldSize.y));
		if (grid != null && displayGridGizmos) 
		{
			foreach (Node n in grid) 
			{
				Gizmos.color = (n.walkable) ? Color.white : Color.red;
				Gizmos.DrawCube(n.worldPosition, Vector2.one * (nodeDiameter-nodeMargin));
			}
		}
	}
}