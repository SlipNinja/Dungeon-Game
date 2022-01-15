using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public Transform floor, ceiling;
    public Vector2Int coordinates;
    public MazeRoom room;
    public int floorNumber;
	
    private int initializedEdgeCount;

    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

    public void Initialize (MazeRoom room) {
		room.Add(this);
		transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
	}

    public void InitializeCeiling(Material mat)
    {
        transform.GetChild(1).GetComponent<Renderer>().material =  mat;
    }

    public List<MazeWall> GetWalls()
    {
        List<MazeWall> walls = new List<MazeWall>();

        foreach (MazeCellEdge edge in edges)
        {
            if(!edge)
            {
                continue;
            }

            if(edge is MazeWall)
            {
                walls.Add(edge as MazeWall);
            }
        }

        return walls;
    }

	public MazeCellEdge GetEdge (MazeDirection direction)
    {
		return edges[(int)direction];
	}

    public bool HasEdgeDoor()
    {
        foreach (MazeCellEdge edge in edges)
        {
            if(edge is MazeDoor)
            {
                return true;
            }
        }

        return false;
    }

    public void SetFloor(bool state)
    {
        floor.gameObject.SetActive(state);
        ceiling.gameObject.SetActive(state);
    }

    public void OnCell()
    {
        foreach (MazeCellEdge edge in edges)
        {
            if(edge is MazeDoor)
            {
                if(!((MazeDoor)edge).isOpen)
                {
                    ((MazeDoor)edge).OpenDoor();
                }
            }
        }
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
