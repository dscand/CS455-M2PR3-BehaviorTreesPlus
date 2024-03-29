using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Character : MonoBehaviour
{
	public bool HasKey;
	public GameObject KeyObject;

	[HideInInspector]
	public BehaviorTree.Task characterTaskTree;

	private Vector3 startPos;
	public Transform roomTargetObject;
	private Vector3 roomTarget;

	public Door targetDoor;
	


	void Start()
	{
		startPos = transform.position;
		roomTarget = roomTargetObject.position;

		KeyObject.SetActive(HasKey);

		characterTaskTree = new BehaviorTree.Selector() {
			children = new BehaviorTree.Task[] {
				new BehaviorTree.Sequence() {
					children = new BehaviorTree.Task[] {
						new DoorOpen_q() {
							transform = transform,
							target = targetDoor,
						},
						new Move() {
							transform = transform,
							target = roomTarget,
						}
					}
				},
				new BehaviorTree.Sequence() {
					children = new BehaviorTree.Task[] {
						new Move() {
							transform = transform,
							target = targetDoor.DoorTarget.position,
						},
						new BehaviorTree.Selector() {
							children = new BehaviorTree.Task[] {
								new BehaviorTree.Sequence() {
									children = new BehaviorTree.Task[] {
										new DoorLocked_q() {
											transform = transform,
											target = targetDoor,
										},
										new OpenDoor() {
											transform = transform,
											target = targetDoor,
										},
										new Move() {
											transform = transform,
											target = roomTarget,
										}
									}
								},
								new BehaviorTree.Sequence() {
									children = new BehaviorTree.Task[] {
										/*new DoorOpen_q() {
											transform = transform,
											target = targetDoor,
										},*/
										new BargeDoor() {
											transform = transform,
											target = targetDoor,
										},
										new Move() {
											transform = transform,
											target = roomTarget,
										}
									}
								}
							}
						},
						new Move() {
							transform = transform,
							target = roomTarget,
						}
					}
				}
			}
		};

		//CharacterRun();
	}

	public void CharacterRun()
	{
		CharacterReset();
		bool success = characterTaskTree.run();
		Debug.Log(success);
	}

	public void CharacterReset()
	{
		transform.position = startPos;
		targetDoor.Reset();
	}


	class CharacterTask : BehaviorTree.Task
	{
		public Transform transform;
	}

	class Move : CharacterTask
	{
		public Vector3 target;

		public override bool run()
		{
			Debug.Log("Move");
			transform.position = target;
			return true;
		}
	}

	class OpenDoor : CharacterTask
	{
		public Door target;
		public override bool run()
		{
			Debug.Log("OpenDoor");
			return target.OpenDoor();
		}
	}

	class CloseDoor : CharacterTask
	{
		public Door target;
		public override bool run()
		{
			Debug.Log("CloseDoor");
			return target.CloseDoor();
		}
	}

	class BargeDoor : CharacterTask
	{
		public Door target;
		public override bool run()
		{
			Debug.Log("BargeDoor");
			return target.BargeDoor();
		}
	}


	class DoorOpen_q : CharacterTask
	{
		public Door target;
		public override bool run()
		{
			Debug.Log("DoorOpen_q");
			return target.DoorOpen;
		}
	}

	class DoorLocked_q : CharacterTask
	{
		public Door target;
		public override bool run()
		{
			Debug.Log("DoorLocked_q");
			return !target.DoorLocked;
		}
	}
}