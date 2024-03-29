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
	[HideInInspector]
	public bool running = false;

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
		if (!running) {
			StartCoroutine(CharacterRun_Enumerator());
		}
	}

	public IEnumerator CharacterRun_Enumerator()
	{
		CharacterReset();
		yield return characterTaskTree.run();
		bool success = characterTaskTree.result;
		Debug.Log(success);
		running = false;
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
		public float speed = 5f;

		public override IEnumerator run()
		{
			Vector3 currentPos = transform.position;
			float elapsedTime = 0;

			float waitTime = 1/speed * Vector3.Distance(transform.position, target);

			while (elapsedTime < waitTime) {
				transform.position = Vector3.Lerp(currentPos, target, elapsedTime / waitTime);
				elapsedTime += Time.deltaTime;

				yield return null;
			}  

			// Make sure we got there
			transform.position = target;
			result = true;
			Debug.Log("Move (" + target + "): " + result);
			yield break;
		}
	}

	class OpenDoor : CharacterTask
	{
		public Door target;
		public override IEnumerator run()
		{
			yield return target.OpenDoor();
			result = target.DoorOpen;
			Debug.Log("OpenDoor: " + result);
			yield break;
		}
	}

	class CloseDoor : CharacterTask
	{
		public Door target;
		public override IEnumerator run()
		{
			yield return target.CloseDoor();
			result = !target.DoorOpen;
			Debug.Log("CloseDoor: " + result);
			yield break;
		}
	}

	class BargeDoor : CharacterTask
	{
		public Door target;
		public override IEnumerator run()
		{
			yield return target.BargeDoor();
			result = target.DoorOpen;
			Debug.Log("BargeDoor: " + result);
			yield break;
		}
	}


	class DoorOpen_q : CharacterTask
	{
		public Door target;
		public override IEnumerator run()
		{
			result = target.DoorOpen;
			Debug.Log("DoorOpen_q: " + result);
			yield break;
		}
	}

	class DoorLocked_q : CharacterTask
	{
		public Door target;
		public override IEnumerator run()
		{
			result = !target.DoorLocked;
			Debug.Log("DoorLocked_q: " + result);
			yield break;
		}
	}
}