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

		characterTaskTree = new BehaviorTree.Selector() {
			children = new BehaviorTree.Task[] {
				new BehaviorTree.Sequence() {
					children = new BehaviorTree.Task[] {
						new DoorOpen_q() {
							transform = transform,
							characterData = this,
							target = targetDoor,
						},
						new Move() {
							transform = transform,
							characterData = this,
							target = roomTarget,
						}
					}
				},
				new BehaviorTree.Sequence() {
					children = new BehaviorTree.Task[] {
						new Move() {
							transform = transform,
							characterData = this,
							target = targetDoor.DoorTarget.position,
						},
						new BehaviorTree.Selector() {
							children = new BehaviorTree.Task[] {
								new BehaviorTree.Sequence() {
									children = new BehaviorTree.Task[] {
										new DoorNotLocked_q() {
											transform = transform,
											characterData = this,
											target = targetDoor,
										},
										new OpenDoor() {
											transform = transform,
											characterData = this,
											target = targetDoor,
										}
									}
								},
								new BehaviorTree.Sequence() {
									children = new BehaviorTree.Task[] {
										/*new FindKey() {
											transform = transform,
											characterData = this,
											target = targetDoor,
										},*/
										new HasKey_q() {
											transform = transform,
											characterData = this,
										},
										new UnlockDoor() {
											transform = transform,
											characterData = this,
											target = targetDoor,
										},
										new OpenDoor() {
											transform = transform,
											characterData = this,
											target = targetDoor,
										},
										new Move() {
											transform = transform,
											characterData = this,
											target = targetDoor.DoorTarget2.position,
										},
										new CloseDoor() {
											transform = transform,
											characterData = this,
											target = targetDoor,
										},
										new LockDoor() {
											transform = transform,
											characterData = this,
											target = targetDoor,
										},
									}
								},
								new BehaviorTree.Sequence() {
									children = new BehaviorTree.Task[] {
										/*new DoorOpen_q() {
											transform = transform,
											characterData = this,
											target = targetDoor,
										},*/
										new BargeDoor() {
											transform = transform,
											characterData = this,
											target = targetDoor,
										},
									}
								}
							}
						},
						new Move() {
							transform = transform,
							characterData = this,
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
		if (running) {
			StopAllCoroutines();
			CharacterReset();
		}

		running = true;
		StartCoroutine(CharacterRun_Enumerator());
	}

	public IEnumerator CharacterRun_Enumerator()
	{
		yield return characterTaskTree.run();
		bool success = characterTaskTree.result;
		Debug.Log(success);
		running = false;
	}

	public void CharacterReset()
	{
		transform.position = startPos;
		KeyObject.SetActive(HasKey);
		//targetDoor.Reset();
	}


	class CharacterTask : BehaviorTree.Task
	{
		public Transform transform;
		public Character characterData;
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


	class UnlockDoor : CharacterTask
	{
		public Door target;
		public override IEnumerator run()
		{
			if (!characterData.HasKey) {
				result = false;
				yield break;
			}

			yield return target.UnlockDoor();
			result = !target.DoorLocked;
			Debug.Log("UnlockDoor: " + result);
			yield break;
		}
	}

	class LockDoor : CharacterTask
	{
		public Door target;
		public override IEnumerator run()
		{
			if (!characterData.HasKey) {
				result = false;
				yield break;
			}

			yield return target.LockDoor();
			result = target.DoorLocked;
			Debug.Log("LockDoor: " + result);
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

	class DoorNotLocked_q : CharacterTask
	{
		public Door target;
		public override IEnumerator run()
		{
			result = !target.DoorLocked;
			Debug.Log("DoorNotLocked_q: " + result);
			yield break;
		}
	}

	class HasKey_q : CharacterTask
	{
		public override IEnumerator run()
		{
			result = characterData.HasKey;
			Debug.Log("HasKey_q: " + result);
			yield break;
		}
	}
}