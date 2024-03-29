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

	[SerializeField]
	private GameObject OpenedDoor;
	[SerializeField]
	private GameObject ClosedDoor;
	[SerializeField]
	private GameObject BrokenDoor;
	[SerializeField]
	private GameObject LockIndicator;

	public Toggle DoorOpenUi;
	public Toggle DoorLockedUi;


	void Start()
	{
		//DoorOpenUi.onValueChanged.AddListener(delegate {
		//	DoorOpen = DoorOpenUi.isOn;
		//	Reset();
		//});
		//DoorLockedUi.onValueChanged.AddListener(delegate {
		//	DoorLocked = DoorLockedUi.isOn;
		//	Reset();
		//});
		Reset();
	}

	public void Reset()
	{
		DoorOpen = DoorOpenUi.isOn;
		DoorLocked = DoorLockedUi.isOn;

		if (DoorBroken) {
			DoorOpen = true;
			DoorOpenUi.isOn = true;
			DoorLocked = false;
			DoorLockedUi.isOn = false;
		}
		if (DoorLocked) {
			DoorOpen = false;
			DoorOpenUi.isOn = false;
		}

		BrokenDoor.SetActive(DoorBroken);
		
		ClosedDoor.SetActive(!DoorOpen);
		OpenedDoor.SetActive(DoorOpen);

		LockIndicator.SetActive(DoorLocked);
	}

	public IEnumerator OpenDoor()
	{
		if (DoorLocked) yield break;
		ClosedDoor.SetActive(false);
		OpenedDoor.SetActive(true);
		DoorOpen = true;
		//DoorOpenUi.isOn = true;
		Debug.Log("Door: OpenDoor");
		yield break;
	}

	public IEnumerator CloseDoor()
	{
		OpenedDoor.SetActive(false);
		ClosedDoor.SetActive(true);
		DoorOpen = false;
		//DoorOpenUi.isOn = true;
		Debug.Log("Door: CloseDoor");
		yield break;
	}

	public IEnumerator BargeDoor()
	{
		DoorLocked = false;
		//DoorLockedUi.isOn = false;
		LockIndicator.SetActive(false);
		ClosedDoor.SetActive(false);
		OpenedDoor.SetActive(false);
		BrokenDoor.SetActive(true);
		DoorOpen = true;
		//DoorOpenUi.isOn = true;
		Debug.Log("Door: BargeDoor");
		yield break;
	}


	public IEnumerator UnlockDoor()
	{
		LockIndicator.SetActive(false);
		DoorLocked = false;
		//DoorLockedUi.isOn = false;
		Debug.Log("Door: UnlockDoor");
		yield break;
	}

	public IEnumerator LockDoor()
	{
		if (DoorOpen) yield break;
		LockIndicator.SetActive(true);
		DoorLocked = true;
		//DoorLockedUi.isOn = true;
		Debug.Log("Door: LockDoor");
		yield break;
	}
}
