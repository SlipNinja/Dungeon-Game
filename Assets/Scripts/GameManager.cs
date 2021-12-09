using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public MazeTower mazeTowerPrefab;
    public CharacterController playerPrefab;

	private CharacterController playerInstance;
	private MazeTower mazeTowerInstance;

    void Start()
    {
        BeginGame();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

	private void BeginGame () {
		mazeTowerInstance = Instantiate(mazeTowerPrefab) as MazeTower;
        playerInstance = Instantiate(playerPrefab) as CharacterController;
        playerInstance.name = "Player";
        mazeTowerInstance.player = playerInstance;

        Maze firstMaze = mazeTowerInstance.GenerateNewMaze();
		playerInstance.SetLocation(firstMaze.GetCell(firstMaze.RandomCoordinates()));
	}

	private void RestartGame () {
        StopAllCoroutines();
		Destroy(mazeTowerInstance.gameObject);

        if (playerInstance != null) {
			Destroy(playerInstance.gameObject);
		}

		BeginGame();
	}
}
