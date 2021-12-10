using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeElevator : MonoBehaviour
{

    public ParticleSystem particles;

    public ElevatorPlatform platform;
    public Vector2Int coordinates;
    public float maxHeight;
    public float minHeight;

    private float moveSpeed = 1f;
    private Vector3 move;
    private Transform player;
    private MazeCell upperCell;
    private bool done = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!upperCell)
        {
            GetUpperCell();
        }

        else if(!done)
        {

            move = platform.transform.up * Time.deltaTime * moveSpeed;

            if(platform.playerOnPlatform)
            {
                player.SetParent(platform.transform);
                if(platform.transform.position.y >= maxHeight)
                {
                    // At the top
                    upperCell.SetFloor(true);
                    player.GetComponent<CharacterControl>().currentFloor += 1;
                    InterfaceHandler.SetFloor(player.GetComponent<CharacterControl>().currentFloor);
                    done = true;
                    Destroy(gameObject);
                }
                
                else
                {
                    upperCell.SetFloor(false);
                    MoveUp();
                }
            }
            
            else
            {
                player.SetParent(null);
                upperCell.SetFloor(true);

                if(platform.transform.position.y > minHeight)
                {
                    MoveDown();
                }
            }
        }
    }

    private void GetUpperCell()
    {
        Maze parent = platform.transform.GetComponentInParent<Maze>();
        MazeTower tower = parent.transform.GetComponentInParent<MazeTower>();

        Maze m = tower.GetMaze(parent.floor + 1);

        if(m)
        {
            upperCell = m.GetCell(coordinates);
        }
    }

    private void MoveUp()
    {
        platform.transform.position += move;
    }

    private void MoveDown()
    {
        platform.transform.position -= move;
    }
}
