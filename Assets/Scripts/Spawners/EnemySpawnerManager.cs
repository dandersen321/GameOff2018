using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemySpawnerManager : MonoBehaviour {

    float interval = 60f;
    public float localSpawnRadius;
    public Animator transitionAnimation;
    public int winSceneIndex = 3;

    private Vector3 ufoStartingPosition;

    private int prePlayerMoney;
    private List<int> preChickenCounts;

    //public List<GameObject> spawnPoints;
    //public List<GameObject> enemyPrefabs;
    //public GameObject ufoPrefab;

    public List<Wave> waves;

    public int nightNumber = 0;

    public bool inSpawnUfoMode = false;
    public bool inNightMode = false;

    // Use this for initialization
    void Start()
    {
        //spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemySpawner"));
        //waves = new List<GameObject>(GameObject.FindGameObjectsWithTag("Wave"));
        waves = new List<Wave>(GetComponentsInChildren<Wave>());
        //ufoStartingPosition = GameObject.Find("UfoSpawner").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StartNight()
    {
        prePlayerMoney = References.getChickenUIManager().playerMoney;
        preChickenCounts = new List<int>();
        foreach(ChickenType chickenType in References.getInventoryManager().chickenInventories)
        {
            preChickenCounts.Add(chickenType.chickenCount);
        }

        Debug.Log("Starting Night " + nightNumber);
        References.getArtifact().isInRock = true;
        References.getChickenUIManager().showNightUI();
        References.GetPlayer().GetComponent<Health>().resetHealth();
        waves[nightNumber].StartWave(this);
    }

    public void endNightMode()
    {
        if (nightNumber == waves.Count - 1)
        {
            //Victory
            Debug.Log("Game won");
            StartCoroutine(LoadScene());
        }
        else
        {
            nightNumber += 1;
            inNightMode = false;
            foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("DirtPatch"))
            {
                gameObject.GetComponent<DirtPatch>().endNight();
            }
            References.getChickenUIManager().showDayUI();
            References.getArtifact().resetPosition();
            StartCoroutine(References.GetPlayerMovementController().endTurrentMode());
        }
    }

    IEnumerator LoadScene()
    {
        References.GetPlayerMovementController().beginMenu();
        transitionAnimation.SetTrigger("FadeToGrey");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(winSceneIndex);
    }

    public void resetNight()
    {
        References.getChickenUIManager().playerMoney = prePlayerMoney;
        for (int i = 0; i < preChickenCounts.Count; ++i)
        {
            References.getInventoryManager().chickenInventories[i].chickenCount = preChickenCounts[i];
            //preChickenCounts.Add(chickenType.chickenCount);
        }

        References.getArtifact().resetPosition();
        References.GetPlayer().GetComponent<Health>().resetHealth();

        waves[nightNumber].resetWave();

        References.getChickenUIManager().showNightUI();


    }
}
