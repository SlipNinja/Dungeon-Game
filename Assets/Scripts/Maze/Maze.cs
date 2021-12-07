using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
	public MazeRoomSettings[] roomSettings;
    public Vector2Int size;
	public MazePassage passagePrefab;
	public MazeWall wallPrefab;
	public MazeCell cellPrefab;
	public MazeDoor doorPrefab;
	public int floor;

	private float doorProbability = 0.05f;
	private MazeCell[,] cells;
    public float cellSizeX, cellSizeZ;
	private float generationStepDelay = 0.0001f;
	private List<MazeRoom> rooms = new List<MazeRoom>();

    public MazeCell GetCell (Vector2Int coordinates) {
		return cells[coordinates.x, coordinates.y];
	}

    // public IEnumerator Generate () {
	// 	WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
	// 	cells = new MazeCell[size.x, size.y];

    //     List<MazeCell> activeCells = new List<MazeCell>();
	// 	DoFirstGenerationStep(activeCells);

	// 	while (activeCells.Count > 0) {
	// 		yield return delay;
	// 		DoNextGenerationStep(activeCells);
	// 	}
	// }

	public void Generate() {
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.y];

        List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);

		while (activeCells.Count > 0) {
			DoNextGenerationStep(activeCells);
		}
	}

	private MazeCell CreateCell (Vector2Int coords) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;

		cells[coords.x, coords.y] = newCell;
        
		newCell.name = "Maze Cell " + coords.x + ", " + coords.y;
		newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coords.x * cellSizeX + 0.5f, 0f, coords.y * cellSizeZ + 0.5f);
        newCell.coordinates = coords;

        return newCell;
	}

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
		MazePassage passage = Instantiate(prefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(prefab) as MazePassage;
		if (passage is MazeDoor) {
			otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
		}
		else {
			otherCell.Initialize(cell.room);
		}
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as MazeWall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	private void CreatePassageInSameRoom (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());

		if (cell.room != otherCell.room)
		{
			MazeRoom roomToAssimilate = otherCell.room;
			cell.room.Assimilate(roomToAssimilate);
			rooms.Remove(roomToAssimilate);
			Destroy(roomToAssimilate);
		}
	}
	

    public Vector2Int RandomCoordinates() {
		return new Vector2Int(Random.Range(0, size.x), Random.Range(0, size.y));
	}

	public bool ContainsCoordinates (Vector2Int coordinate) {
		return (coordinate.x >= 0)
            && (coordinate.x < size.x)
            && (coordinate.y >= 0)
            && (coordinate.y < size.y);
	}
	
	private MazeRoom CreateRoom (int indexToExclude) {
		MazeRoom newRoom = ScriptableObject.CreateInstance<MazeRoom>();
		newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
		if (newRoom.settingsIndex == indexToExclude) {
			newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
		}
		newRoom.settings = roomSettings[newRoom.settingsIndex];
		rooms.Add(newRoom);
		return newRoom;
	}

    private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		MazeCell newCell = CreateCell(RandomCoordinates());
		newCell.Initialize(CreateRoom(-1));
		activeCells.Add(newCell);
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells)
	{
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];

		if (currentCell.IsFullyInitialized()) {
			activeCells.RemoveAt(currentIndex);
			return;
		}

		MazeDirection direction = currentCell.RandomUninitializedDirection();
		Vector2Int coordinates = currentCell.coordinates + direction.ToVector2Int();

		if (ContainsCoordinates(coordinates))
		{
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null) {
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}

			else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex)
			{
				CreatePassageInSameRoom(currentCell, neighbor, direction);
			}

			else
			{
				CreateWall(currentCell, neighbor, direction);
			}
		}

		else
		{
			CreateWall(currentCell, null, direction);
		}
	}
    
}
