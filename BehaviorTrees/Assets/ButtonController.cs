using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	public Character character;
	public Door door;

	public Button ButtonPlay;
	public Button ButtonReset;

	public Toggle ToogleDoorOpen;
	public Toggle ToogleDoorLocked;
	public Toggle ToogleCharacterKey;

	public bool HasKey = false;
	public bool DoorOpen = false;
	public bool DoorLocked = false;
	public bool DoorBroken = false;
	
	void Start()
	{
		HasKey = ToogleCharacterKey.isOn;
		DoorOpen = ToogleDoorOpen.isOn;
		DoorLocked = ToogleDoorLocked.isOn;
		DoorBroken = false;

		character.HasKey = HasKey;
		door.DoorOpen = DoorOpen;
		door.DoorLocked = DoorLocked;
		door.DoorBroken = DoorBroken;

		character.CharacterReset();
		door.Reset();


		ToogleCharacterKey.onValueChanged.AddListener(delegate {
			HasKey = ToogleCharacterKey.isOn;
			if (!character.running) {
				character.HasKey = HasKey;
				character.CharacterReset();
			}
		});

		ToogleDoorOpen.onValueChanged.AddListener(delegate {
			DoorOpen = ToogleDoorOpen.isOn;
			if (!character.running) {
				door.DoorOpen = DoorOpen;
				door.Reset();
			}
		});

		ToogleDoorLocked.onValueChanged.AddListener(delegate {
			DoorLocked = ToogleDoorLocked.isOn;
			if (!character.running) {
				door.DoorLocked = DoorLocked;
				door.Reset();
			}
		});
	}


	public void ButtonPlayPress()
	{
		ButtonPlay.gameObject.SetActive(false);
		ButtonReset.gameObject.SetActive(true);

		character.CharacterRun();
		//StartCoroutine(CharacterPlaying());
	}
	private IEnumerator CharacterPlaying()
	{
		yield return null;
		while (character.running) yield return null;
		ButtonReset.gameObject.SetActive(true);
		yield break;
	}
	public void ButtonResetPress()
	{
		if (character.running) {
			StopAllCoroutines();
			character.StopAllCoroutines();
			character.running = false;
		}

		character.HasKey = HasKey;
		door.DoorOpen = DoorOpen;
		door.DoorLocked = DoorLocked;
		door.DoorBroken = DoorBroken;

		character.CharacterReset();
		door.Reset();

		ButtonReset.gameObject.SetActive(false);
		ButtonPlay.gameObject.SetActive(true);
	}
}
