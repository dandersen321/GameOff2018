using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnerManager : MonoBehaviour {

    float interval = 60f;
    public float localSpawnRadius;

    public List<GameObject> spawnPoints;

    Timer intervalTimer = new Timer();

    public List<GameObject> enemyPrefabs;

    int waveNumber = 1;
    int lastWaveCount = 0;
    bool waveRunning = false;

    int enemyTotalSpawned = 0;

    //public Dictionary<string, List<GameObject>> enemyObjectPool = new Dictionary<string, List<GameObject>>();

    GameObject CreateEnemy(GameObject enemyPrefab, Vector3 position)
    {
        enemyTotalSpawned += 1;
        GameObject enemy;
        NavMeshHit hit = new NavMeshHit();
        if (NavMesh.SamplePosition(position, out hit, 10f, NavMesh.AllAreas))
        {
            position = hit.position;
        }
        else
        {
            return null;
        }
        string name = enemyPrefab.GetComponent<Enemy>().name;
        //if (enemyObjectPool[name].Count > 0)
        //{
        //    enemy = enemyObjectPool[name][0];
        //    enemyObjectPool[name].RemoveAt(0);
        //}
        //else
        //{
            enemy = GameObject.Instantiate(enemyPrefab, position, Quaternion.identity);
        //}
        enemy.SetActive(true);
        enemy.transform.position = position;

        return enemy;
    }

    // Use this for initialization
    void Start()
    {
        spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemySpawner"));
        //enemyObjectPool["MaleZombiePrefab"] = new List<GameObject>();
        SpawnWave();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Wave: " + (waveNumber-1));
        if (intervalTimer.Expired() && !waveRunning)
        {
            SpawnWave();
        }

    }

    void SpawnWave()
    {
        waveRunning = true;

        List<int> rangeValues = new List<int>();
        for (int i = 0; i < spawnPoints.Count; ++i)
        {
            rangeValues.Add(i);
        }

        int numberOfSpawnLocations = spawnPoints.Count;
        List<int> randomIndexes = new List<int>();
        for (int i = 0; i < numberOfSpawnLocations; i++)
        {
            int index = Random.Range(0, rangeValues.Count);
            randomIndexes.Add(rangeValues[index]);
            rangeValues.RemoveAt(index);
        }

        int numberOfEnemiesToSpawn = lastWaveCount + waveNumber * 2;
        if (waveNumber == 1)
        {
            numberOfEnemiesToSpawn = 15;
        }
        else if (waveNumber == 2)
        {
            numberOfEnemiesToSpawn = 20;
        }
        else if (waveNumber == 3)
        {
            numberOfEnemiesToSpawn = 30;
        }
        else if (waveNumber == 5)
        {
            numberOfEnemiesToSpawn = 60;
        }
        else if (waveNumber == 6)
        {
            numberOfEnemiesToSpawn = 60;
        }
        else if (waveNumber == 7)
        {
            numberOfEnemiesToSpawn = 60;
        }
        else if (waveNumber >= 8)
        {
            numberOfEnemiesToSpawn = 0;
        }
        lastWaveCount = numberOfEnemiesToSpawn;
        int numPerWave = numberOfEnemiesToSpawn / numberOfSpawnLocations;
        numPerWave += waveNumber; //Add some addtiona scaling
        numPerWave += waveNumber / 10 * 10;
        int enemiesPerWave = numberOfEnemiesToSpawn / numberOfSpawnLocations;
        List<GameObject> selectedSpawnPoints = new List<GameObject>();
        for (int i = 0; i < randomIndexes.Count; ++i)
        {
            var spawnP = spawnPoints[randomIndexes[i]];
            selectedSpawnPoints.Add(spawnP);
            StartCoroutine(SpawnEnemiesAtPosition(i, enemiesPerWave));

        }

        intervalTimer.Start(interval);
        waveNumber += 1;
    }

    IEnumerator SpawnEnemiesAtPosition(int spawnIndex, int numberOfEnemiesToSpawn)
    {
        GameObject spawnObj = spawnPoints[spawnIndex];
        Vector3 spawnPoint = spawnObj.transform.position;
        Vector2 localSpawnPoint = new Vector2(spawnPoint.x, spawnPoint.z);

        float enemiesToSpawnPerSecond = Mathf.Max(interval / numberOfEnemiesToSpawn, 0.5f);

        for (int i = 0; i < numberOfEnemiesToSpawn; ++i)
        {
            yield return new WaitForSeconds(enemiesToSpawnPerSecond);
            

            GameObject newEnemyPrefab = enemyPrefabs[0];
            //if (Random.Range(0, 3) == 1 && waveNumber > 2)
            //{
            //    newEnemyPrefab = enemyPrefabs[1];
            //}
            //else
            //{
            //    newEnemyPrefab = enemyPrefabs[0];
            //}

            Vector2 enemySpawnPointV2 = (Random.insideUnitCircle.normalized * localSpawnRadius) + localSpawnPoint;
            Vector3 enemySpawnPoint = new Vector3(enemySpawnPointV2.x, spawnPoint.y, enemySpawnPointV2.y);
            this.CreateEnemy(newEnemyPrefab, enemySpawnPoint);

        }

        waveRunning = false;

    }

}
