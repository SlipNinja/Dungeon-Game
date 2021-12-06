using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public MazePassage passagePrefab;
	public MazeWall wallPrefab;
    public Vector2Int size;
	public MazeCell cellPrefab;

	private MazeCell[,] cells;
    private float cellSizeX, cellSizeZ;
	private float generationStepDelay = 0.001f;

    void Awake()
    {
        MazeCell tmpCell = Instantiate(cellPrefab) as MazeCell;
        cellSizeX = tmpCell.transform.Find("floor").GetComponent<Renderer>().bounds.size.x;
        cellSizeZ = tmpCell.transform.Find("floor").GetComponent<Renderer>().bounds.size.z;
        Destroy(tmpCell.gameObject);
    }

    public MazeCell GetCell (Vector2Int coordinates) {
		return cells[coordinates.x, coordinates.y];
	}

    public IEnumerator Generate () {
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new MazeCell[size.x, size.y];

        List<MazeCell> activeCells = new List<MazeCell>();
		DoFirstGenerationStep(activeCells);

		while (activeCells.Count > 0) {
			yield return delay;
			DoNextGenerationStep(activeCells);
		}
	}

	private MazeCell CreateCell (Vector2Int coords) {
		MazeCell newCell = Instantiate(cellPrefab) as MazeCell;

		cells[coords.x, coords.y] = newCell;
        
		newCell.name = "Maze Cell " + coords.x + ", " + coords.y;
		newCell.transform.parent = transform;
		Debug.Log(coords.x + " " + cellSizeX + " " + coords.y + " " + cellSizeZ);
        newCell.transform.localPosition = new Vector3(coords.x * cellSizeX + 0.5f, 0f, coords.y * cellSizeZ + 0.5f);
        newCell.coordinates = coords;

        return newCell;
	}

	private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazePassage passage = Instantiate(passagePrefab) as MazePassage;
		passage.Initialize(cell, otherCell, direction);
		// passage = Instantiate(passagePrefab) as MazePassage;
		// passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction) {
		MazeWall wall = Instantiate(wallPrefab) as MazeWall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			// wall = Instantiate(wallPrefab) as MazeWall;
			// wall.Initialize(otherCell, cell, direction.GetOpposite());
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

    private void DoFirstGenerationStep (List<MazeCell> activeCells) {
		activeCells.Add(CreateCell(RandomCoordinates()));
	}

	private void DoNextGenerationStep (List<MazeCell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		MazeCell currentCell = activeCells[currentIndex];

		if (currentCell.IsFullyInitialized()) {
			activeCells.RemoveAt(currentIndex);
			return;
		}

		MazeDirection direction = currentCell.RandomUninitializedDirection();
		Vector2Int coordinates = currentCell.coordinates + direction.ToVector2Int();

		if (ContainsCoordinates(coordinates)) {
			MazeCell neighbor = GetCell(coordinates);
			if (neighbor == null) {
				neighbor = CreateCell(coordinates);
				CreatePassage(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			else {
				CreateWall(currentCell, neighbor, direction);
			}
		}
		else {
			CreateWall(currentCell, null, direction);
		}
	}
    
}
