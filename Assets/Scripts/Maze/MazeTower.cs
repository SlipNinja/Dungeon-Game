using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazeTower : MonoBehaviour
{
    public MazeCell cellPrefab;
    public MazeWall wallPrefab;
    public Maze mazePrefab;

    private int floor = 0;
    private List<Maze> mazes;
    private float cellSizeX, cellSizeZ, wallHeight;
    private int currentMazeIndex = 0;
    public CharacterControl player;

    private bool removingMazes = false;

    void Awake()
    {
        mazes = new List<Maze>();

        MazeCell tmpCell = Instantiate(cellPrefab) as MazeCell;
        cellSizeX = tmpCell.transform.Find("floor").GetComponent<Renderer>().bounds.size.x;
        cellSizeZ = tmpCell.transform.Find("floor").GetComponent<Renderer>().bounds.size.z;
        Destroy(tmpCell.gameObject);

        MazeWall tmpWall = Instantiate(wallPrefab) as MazeWall;
        wallHeight = tmpWall.transform.Find("wall").GetComponent<Renderer>().bounds.size.y;
        Destroy(tmpWall.gameObject);
    }

    void Update()
    {
        if(!removingMazes)
        {
            StartCoroutine(RemovePastMazes());
        }

        if(player.currentFloor >= floor-1)// always 2 floors
        {
            GenerateNewMaze();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.currentFloor ++;
        }
    }

    private IEnumerator RemovePastMazes()
    {
        removingMazes = true;

        yield return new WaitForSeconds(10);

        for (int i = mazes.Count - 1; i >= 0; i--)
        {
            Maze maze = mazes.ElementAt(i);

            if(maze.floor < player.currentFloor)
            {
                DeleteMaze(i);
            }
        }

        removingMazes = false;
    }

    public Maze GenerateNewMaze()
    {
        Debug.Log("Generating new Maze");
        Vector3 newMazePos = new Vector3(0f, wallHeight * floor, 0f);
        Maze newMaze = Instantiate(mazePrefab) as Maze;
        newMaze.transform.SetParent(transform);
        newMaze.transform.position = newMazePos;
        newMaze.cellSizeX = cellSizeX;
        newMaze.cellSizeZ = cellSizeZ;
        newMaze.floor = floor;
        newMaze.Generate();

        mazes.Add(newMaze);
        floor ++;

        return newMaze;
    }

    public void DeleteMaze(int index)
    {
        Maze m = mazes.ElementAt(index);
        Debug.Log("Deleting floor " + m.floor + " Maze");
        mazes.RemoveAt(index);
        Destroy(m.gameObject);

    }

    public Maze GetMaze(int index)
    {
        return mazes.ElementAt(index);
    }

    public int GetCurrentMazeIndex()
    {
        return currentMazeIndex;
    }

    public int GetCurrentFloor()
    {
        return floor;
    }
}
