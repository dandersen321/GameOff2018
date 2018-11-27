using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wave : MonoBehaviour
{

    float interval = 60f;
    public float localSpawnRadius;

    private List<UFO> ufos;
    private int ufosActive = 0;
    private int ufosDead = 0;
    //private bool noUfosActive = false;

    //private Vector3 ufoStartingPosition;

    //private List<UFO> activeUfos;
    EnemySpawnerManager enemySpawnManager;

    // Use this for initialization
    void Awake()
    {
        //ufoStartingPosition = GameObject.Find("UfoSpawner").transform.position;
        ufos = new List<UFO>(GetComponentsInChildren<UFO>());
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void endWave()
    {
        this.enemySpawnManager.endNightMode();
    }

    public void StartWave(EnemySpawnerManager enemySpawnManager)
    {
        this.enemySpawnManager = enemySpawnManager;
        StartCoroutine(SpawnUfos());
    }

    public IEnumerator SpawnUfo(UFO ufo)
    {
        for(int i = 0; i < ufo.chargeTime; ++i)
        {
            if(ufosActive == 0)
            {
                break;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }

        Debug.Log("Spawn Ufo");
        ufosActive += 1;
        ufo.gameObject.SetActive(true);
        ufo.init(this);

        //UFO ufo = GameObject.Instantiate(ufoPrefab, startingPosition, Quaternion.identity).GetComponent<UFO>();
        //ufo.transform.rotation = GameObject.Find("UfoSpawner").transform.rotation; // Ufos aren't on straight
        //ufo.init(enemyPrefabs, landingPosition);
        //ufo.init();
        //activeUfos.Add(ufo);
    }

    IEnumerator SpawnUfos()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("SpawnUfos");

        foreach (UFO ufo in ufos)
        {
            Debug.Log("SpawnUfos for");
            StartCoroutine(SpawnUfo(ufo));
        }
    }

    public void ufoDied()
    {
        ufosDead += 1;
        if(ufosDead == ufos.Count)
        {
            endWave();
        }
    }

}
