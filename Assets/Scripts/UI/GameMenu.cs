using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

    public GameObject gameMenu;
    public GameObject hudToDisable;

	// Use this for initialization
	void Start () {
        gameMenu.SetActive(false);
	}

    public void ToggleGameMenu()
    {
        gameMenu.SetActive(!gameMenu.activeSelf);
        hudToDisable.SetActive(!hudToDisable.activeSelf);
        if (gameMenu.activeSelf)
        {
            References.GetPlayerMovementController().beginMenu();
            Time.timeScale = 0;
        }
        else
        {
            References.GetPlayerMovementController().closeMenu();
            Time.timeScale = 1;
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape) && !References.GetPlayerMovementController().inShopMenu && !References.GetPlayerMovementController().lost)
        {
            ToggleGameMenu();
        }
	}

    public bool isDisplayed()
    {
        return gameMenu.activeSelf;
    }
}
