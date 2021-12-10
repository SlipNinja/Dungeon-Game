using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MazeRoom : ScriptableObject {

	public int settingsIndex;

	public MazeRoomSettings settings;
	
	private List<MazeCell> cells = new List<MazeCell>();
	
	public void Add (MazeCell cell) {
		cell.room = this;
		cells.Add(cell);
    }

    public void Assimilate (MazeRoom room) {
        for (int i = 0; i < room.cells.Count; i++) {
            Add(room.cells[i]);
        }
	}

	public List<MazeCell> GetCellsWithoutDoor()
	{
		List<MazeCell> noDoorsCells = new List<MazeCell>();

		foreach (MazeCell cell in cells)
		{
			if(!cell.HasEdgeDoor())
			{
				noDoorsCells.Add(cell);
			}
		}

		return noDoorsCells;
	}

	public int Size()
	{
		return cells.Count;
	}

	private List<MazeCell> GetOneWallCells()
	{
		return new List<MazeCell>();
	}

	public bool Contains(MazeCell cell)
	{
		return cells.Contains(cell);
	}

	public bool Contains(Vector2Int coords)
	{
		foreach (MazeCell c in cells)
		{
			if(c.coordinates == coords)
			{
				return true;
			}
		}

		return false;
	}
}