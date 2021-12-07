using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeDoor : MazePassage {
	
	public Transform hinge;
	private bool isMirrored = false;
	public bool isOpen = false; 
	private static Quaternion normalRotation = Quaternion.Euler(0f, -90f, 0f);
	private static Quaternion mirroredRotation = Quaternion.Euler(0f, 90f, 0f);

    private MazeDoor OtherSideOfDoor()
	{
		return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
	}

	public void OpenDoor () {
		isOpen = true;
		OtherSideOfDoor().isOpen = true;
		OtherSideOfDoor().hinge.localRotation = hinge.localRotation = isMirrored ? mirroredRotation : normalRotation;
		
	}
	
	public override void Initialize (MazeCell primary, MazeCell other, MazeDirection direction) {
		base.Initialize(primary, other, direction);
		if (OtherSideOfDoor() != null) {
			isMirrored = true;
			hinge.localScale = new Vector3(-1f, 1f, 1f);
			Vector3 p = hinge.localPosition;
			p.x = -p.x;
			hinge.localPosition = p;
		}
		for (int i = 0; i < transform.childCount; i++) {
			Transform child = transform.GetChild(i);
			if (child != hinge) {
				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
			}
		}
	}
}