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

    private float moveSpeed = 0.5f;
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
                    //Debug.Log("AT THE TOP");
                    upperCell.gameObject.SetActive(true);
                    player.GetComponent<CharacterController>().currentFloor += 1;
                    done = true;
                }
                
                else
                {
                    MoveUp();
                }
            }
            
            else
            {
                player.SetParent(null);
                if(platform.transform.position.y <= minHeight)
                {
                    // At the bottom
                    //Debug.Log("AT THE BOTTOM");
                    upperCell.gameObject.SetActive(false);
                }

                else
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
            //Debug.Log("CELL " + upperCell.coordinates.x + ":" + upperCell.coordinates.y);
        }
    }

    private void MoveUp()
    {
        platform.transform.position += move;
        //player.GetComponent<CharacterController>().elevatorVelocity = move.y;
        // player.GetComponent<CharacterController>().elevatorVelocity = platform.GetComponent<Rigidbody>().velocity.y;
    }

    private void MoveDown()
    {
        platform.transform.position -= move;
        //player.GetComponent<CharacterController>().elevatorVelocity = 0f;
    }
}
