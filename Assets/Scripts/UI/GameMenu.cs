using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

    public GameObject gameMenu;

	// Use this for initialization
	void Start () {
        gameMenu.SetActive(false);
	}

    public void ToggleGameMenu()
    {
        gameMenu.SetActive(!gameMenu.activeSelf);
        if (gameMenu.activeSelf)
            References.GetPlayerMovementController().beginMenu();
        else
            References.GetPlayerMovementController().closeMenu();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleGameMenu();
        }
	}
}
