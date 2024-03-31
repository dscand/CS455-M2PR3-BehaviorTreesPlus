using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
	public bool DoorOpen = false;
	public bool DoorLocked = false;
	public bool DoorBroken = false;
	public Transform DoorTarget;
	public Transform DoorTarget2;

	[SerializeField]
	private GameObject OpenedDoor;
	[SerializeField]
	private GameObject ClosedDoor;
	[SerializeField]
	private GameObject BrokenDoor;
	[SerializeField]
	private GameObject LockIndicator;
	[SerializeField]
	private GameObject AnimatedDoor;

	[SerializeField]
	private Animator DoorAnimator;


	void Start()
	{
		/*DoorOpenUi.onValueChanged.AddListener(delegate {
			DoorOpen = DoorOpenUi.isOn;
			Reset();
		});
		DoorLockedUi.onValueChanged.AddListener(delegate {
			DoorLocked = DoorLockedUi.isOn;
			Reset();
		});*/
		
		//Reset();
	}

	public void Reset()
	{
		DoorBroken = false;
		if (DoorLocked) {
			DoorOpen = false;
		}

		//BrokenDoor.SetActive(DoorBroken);
		
		//ClosedDoor.SetActive(!DoorOpen);
		//OpenedDoor.SetActive(DoorOpen);

		LockIndicator.SetActive(DoorLocked);


		OpenedDoor.SetActive(false);
		ClosedDoor.SetActive(false);
		BrokenDoor.SetActive(false);
		AnimatedDoor.SetActive(true);

		DoorAnimator.SetBool("DoorOpen", DoorOpen);
		DoorAnimator.SetBool("DoorLocked", DoorLocked);
		DoorAnimator.SetBool("DoorBroken", DoorBroken);
	}

	public IEnumerator OpenDoor()
	{
		if (DoorLocked) yield break;
		//ClosedDoor.SetActive(false);
		//OpenedDoor.SetActive(true);
		DoorOpen = true;
		DoorAnimator.SetBool("DoorOpen", DoorOpen);
		//DoorOpenUi.isOn = true;

		Debug.Log("Door: OpenDoor");
		yield return new WaitForSeconds(0.5f);
		yield break;
	}

	public IEnumerator CloseDoor()
	{
		//OpenedDoor.SetActive(false);
		//ClosedDoor.SetActive(true);
		DoorOpen = false;
		DoorAnimator.SetBool("DoorOpen", DoorOpen);
		//DoorOpenUi.isOn = true;

		Debug.Log("Door: CloseDoor");
		yield return new WaitForSeconds(0.5f);
		yield break;
	}

	public IEnumerator BargeDoor()
	{
		DoorLocked = false;
		DoorAnimator.SetBool("DoorLocked", DoorLocked);
		//DoorLockedUi.isOn = false;
		LockIndicator.SetActive(false);
		//ClosedDoor.SetActive(false);
		//OpenedDoor.SetActive(false);
		//BrokenDoor.SetActive(true);
		DoorBroken = true;
		DoorAnimator.SetBool("DoorBroken", DoorBroken);
		//DoorOpenUi.isOn = true;

		Debug.Log("Door: BargeDoor");
		yield return new WaitForSeconds(0.5f);
		DoorOpen = true;
		DoorAnimator.SetBool("DoorOpen", DoorOpen);
		yield break;
	}


	public IEnumerator UnlockDoor()
	{
		LockIndicator.SetActive(false);
		DoorLocked = false;
		DoorAnimator.SetBool("DoorLocked", DoorLocked);
		//DoorLockedUi.isOn = false;

		Debug.Log("Door: UnlockDoor");
		yield return new WaitForSeconds(0.2f);
		yield break;
	}

	public IEnumerator LockDoor()
	{
		if (DoorOpen) yield break;
		LockIndicator.SetActive(true);
		DoorLocked = true;
		DoorAnimator.SetBool("DoorLocked", DoorLocked);
		//DoorLockedUi.isOn = true;

		Debug.Log("Door: LockDoor");
		yield return new WaitForSeconds(0.2f);
		yield break;
	}
}
