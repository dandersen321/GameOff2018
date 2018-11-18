using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerManager : MonoBehaviour {

    float interval = 60f;
    public float localSpawnRadius;
    private Vector3 ufoStartingPosition;

    public List<GameObject> spawnPoints;
    public List<GameObject> enemyPrefabs;
    public GameObject ufoPrefab;

    int waveNumber = 1;
    int lastWaveCount = 0;
    //bool waveRunning = false;

    int enemyTotalSpawned = 0;

    //public Dictionary<string, List<GameObject>> enemyObjectPool = new Dictionary<string, List<GameObject>>();

    //GameObject CreateEnemy(GameObject enemyPrefab, Vector3 position)
    //{
    //    enemyTotalSpawned += 1;
    //    GameObject enemy;
    //    NavMeshHit hit = new NavMeshHit();
    //    if (NavMesh.SamplePosition(position, out hit, 10f, NavMesh.AllAreas))
    //    {
    //        position = hit.position;
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //    string name = enemyPrefab.GetComponent<Enemy>().name;
    //    //if (enemyObjectPool[name].Count > 0)
    //    //{
    //    //    enemy = enemyObjectPool[name][0];
    //    //    enemyObjectPool[name].RemoveAt(0);
    //    //}
    //    //else
    //    //{
    //        enemy = GameObject.Instantiate(enemyPrefab, position, Quaternion.identity);
    //    //}
    //    enemy.SetActive(true);
    //    enemy.transform.position = position;

    //    return enemy;
    //}

    // Use this for initialization
    void Start()
    {
        spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemySpawner"));
        ufoStartingPosition = GameObject.Find("UfoSpawner").transform.position;
        //enemyObjectPool["MaleZombiePrefab"] = new List<GameObject>();
        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnWave()
    {
        //waveRunning = true;

        //List<int> rangeValues = new List<int>();
        //for (int i = 0; i < spawnPoints.Count; ++i)
        //{
        //    rangeValues.Add(i);
        //}

        //int numberOfSpawnLocations = spawnPoints.Count;


        //int numberOfEnemiesToSpawn = lastWaveCount + waveNumber * 2;
        //if (waveNumber == 1)
        //{
        //    numberOfEnemiesToSpawn = 15;
        //}
        //else if (waveNumber == 2)
        //{
        //    numberOfEnemiesToSpawn = 20;
        //}
        //else if (waveNumber == 3)
        //{
        //    numberOfEnemiesToSpawn = 30;
        //}
        //else if (waveNumber == 5)
        //{
        //    numberOfEnemiesToSpawn = 60;
        //}
        //else if (waveNumber == 6)
        //{
        //    numberOfEnemiesToSpawn = 60;
        //}
        //else if (waveNumber == 7)
        //{
        //    numberOfEnemiesToSpawn = 60;
        //}
        //else if (waveNumber >= 8)
        //{
        //    numberOfEnemiesToSpawn = 0;
        //}
        //lastWaveCount = numberOfEnemiesToSpawn;
        //int numPerWave = numberOfEnemiesToSpawn / numberOfSpawnLocations;
        //numPerWave += waveNumber; //Add some addtiona scaling
        //numPerWave += waveNumber / 10 * 10;
        //int enemiesPerWave = numberOfEnemiesToSpawn / numberOfSpawnLocations;
        StartCoroutine(SpawnUfos(waveNumber+2));

        waveNumber += 1;
    }

    public void SpawnUfo(Vector3 startingPosition, Vector3 landingPosition, List<GameObject> enemyPrefabs)
    {
        UFO ufo = GameObject.Instantiate(ufoPrefab, startingPosition, Quaternion.identity).GetComponent<UFO>();
        ufo.transform.rotation = GameObject.Find("UfoSpawner").transform.rotation; // Ufos aren't on straight
        ufo.init(enemyPrefabs, landingPosition);
    }

    IEnumerator SpawnUfos(int numberOfUfos)
    {
        Debug.Log("Spawning " + numberOfUfos.ToString() + " ufos");
        Vector2 localSpawnPoint = new Vector2(ufoStartingPosition.x, ufoStartingPosition.z);
        float secondsPerEnemy = 10f;

        List<int> rangeValues = new List<int>();
        for (int i = 0; i < spawnPoints.Count; ++i)
        {
            rangeValues.Add(i);
        }

        List<int> randomIndexes = new List<int>();
        for (int i = 0; i < numberOfUfos; i++)
        {
            int index = Random.Range(0, rangeValues.Count);
            randomIndexes.Add(rangeValues[index]);
            rangeValues.RemoveAt(index);
        }

        for (int i = 0; i < randomIndexes.Count; ++i)
        {
            yield return new WaitForSeconds(secondsPerEnemy);

            Vector2 enemySpawnPointV2 = (Random.insideUnitCircle.normalized * localSpawnRadius) + localSpawnPoint;
            Vector3 enemySpawnPoint = new Vector3(enemySpawnPointV2.x, ufoStartingPosition.y, enemySpawnPointV2.y);
            List<GameObject> newEnemyPrefabs = new List<GameObject>();
            for(int j = 0; j < 5; ++j)
            {
                newEnemyPrefabs.Add(enemyPrefabs[0]);
            }
            SpawnUfo(enemySpawnPoint, spawnPoints[randomIndexes[i]].transform.position, newEnemyPrefabs);

        }

        //waveRunning = false;

    }

}
