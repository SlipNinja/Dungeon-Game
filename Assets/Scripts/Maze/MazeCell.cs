using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public Vector2Int coordinates;
    public MazeRoom room;
	
    private int initializedEdgeCount;

    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

    public void Initialize (MazeRoom room) {
		room.Add(this);
		transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	}

	public MazeCellEdge GetEdge (MazeDirection direction)
    {
		return edges[(int)direction];
	}

	public void SetEdge (MazeDirection direction, MazeCellEdge edge)
    {
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}

	public bool IsFullyInitialized()
    {
		return initializedEdgeCount == MazeDirections.Count;
	}

    public MazeDirection RandomUninitializedDirection()
    {
        int numEdgesLeft = MazeDirections.Count - initializedEdgeCount;
        int[] edgesLeft = new int[numEdgesLeft];
        int edgesLeftIndice = 0;

        for(int i = 0; i < MazeDirections.Count; i++)
        {
            if(edges[i] is null)
            {
                edgesLeft[edgesLeftIndice] = i;
                edgesLeftIndice ++;
            }
        }

        int newDirectionIndice = Random.Range(0, numEdgesLeft); // (int inclusive, int exclusive)
        MazeDirection newDirection = (MazeDirection)edgesLeft[newDirectionIndice];

        return newDirection;
	}
}
