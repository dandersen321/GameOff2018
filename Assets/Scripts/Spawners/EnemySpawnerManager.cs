using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerManager : MonoBehaviour {

    float interval = 60f;
    public float localSpawnRadius;
    private Vector3 ufoStartingPosition;

    //public List<GameObject> spawnPoints;
    //public List<GameObject> enemyPrefabs;
    //public GameObject ufoPrefab;

    public List<Wave> waves;

    int nightNumber = 0;

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
        Debug.Log("Starting Night " + nightNumber);
        References.getArtifact().isInRock = true;
        References.getChickenUIManager().showNightUI();
        waves[nightNumber].StartWave(this);
    }

    public void endNightMode()
    {
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
