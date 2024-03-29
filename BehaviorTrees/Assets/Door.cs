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

	public bool OpenDoor()
	{
		if (DoorLocked) return false;
		ClosedDoor.SetActive(false);
		OpenedDoor.SetActive(true);
		DoorOpen = true;
		//DoorOpenUi.isOn = true;
		Debug.Log("Door: OpenDoor");
		return true;
	}

	public bool CloseDoor()
	{
		OpenedDoor.SetActive(false);
		ClosedDoor.SetActive(true);
		DoorOpen = false;
		//DoorOpenUi.isOn = true;
		Debug.Log("Door: CloseDoor");
		return true;
	}

	public bool BargeDoor()
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
		return true;
	}


	public bool UnlockDoor()
	{
		LockIndicator.SetActive(false);
		DoorLocked = false;
		//DoorLockedUi.isOn = false;
		Debug.Log("Door: UnlockDoor");
		return true;
	}

	public bool LockDoor()
	{
		if (DoorOpen) return false;
		LockIndicator.SetActive(true);
		DoorLocked = true;
		//DoorLockedUi.isOn = true;
		Debug.Log("Door: LockDoor");
		return true;
	}
}
