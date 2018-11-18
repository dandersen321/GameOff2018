using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UFO : MonoBehaviour {

    private List<GameObject> enemyPrefabs;
    private List<Enemy> landedEnemies;
    private Vector3 landingPosition;
    private bool flying = true;
    private bool unloadingTroops = false;
    private bool waitingForTroops = false;
    private float speed = 5f;

    public void init(List<GameObject> enemyPrefabs, Vector3 landingPosition)
    {
        this.enemyPrefabs = enemyPrefabs;
        this.landingPosition = landingPosition;
    }

	// Use this for initialization
	void Start () {
		
	}

    void updateFlying()
    {
        float step = speed * Time.deltaTime;
        Vector3 targetPosition = landingPosition + new Vector3(0, 10, 0);
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
        if (Vector3.Distance(this.transform.position, targetPosition) < 1f)
        {
            flying = false;
            unloadingTroops = true;
            StartCoroutine(unloadTroops());
        }
    }

    IEnumerator unloadTroops()
    {
        Vector3 spawnPoint = landingPosition;
        Vector2 localSpawnPoint = new Vector2(spawnPoint.x, spawnPoint.z);
        float localSpawnRadius = 5f;

        float secondsPerEnemyUnload = 1;

        landedEnemies = new List<Enemy>();

        foreach (GameObject enemyPrefab in enemyPrefabs)
        {
            

            Vector2 enemySpawnPointV2 = (Random.insideUnitCircle.normalized * localSpawnRadius) + localSpawnPoint;
            Vector3 enemySpawnPoint = new Vector3(enemySpawnPointV2.x, spawnPoint.y, enemySpawnPointV2.y);
            Enemy landedEnemy = this.CreateEnemy(enemyPrefab, enemySpawnPoint).GetComponent<Enemy>();
            landedEnemies.Add(landedEnemy);

            yield return new WaitForSeconds(secondsPerEnemyUnload);
        }

        unloadingTroops = false;
        waitingForTroops = true;
    }

    GameObject CreateEnemy(GameObject enemyPrefab, Vector3 position)
    {
        GameObject enemy;
        NavMeshHit hit = new NavMeshHit();
        if (NavMesh.SamplePosition(position, out hit, 10f, NavMesh.AllAreas))
        {
            position = hit.position;
        }
        else
        {
            // TODO remove this
            throw new KeyNotFoundException("Hmm, no nav mesh hit?");
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

    void waitForTroops()
    {
        foreach(Enemy enemy in landedEnemies)
        {
            if(enemy.alive)
            {
                return;
            }
        }

        retreat();


    }

    void retreat()
    {
        Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {

        if(flying)
        {
            updateFlying();
        }
        else if (unloadingTroops)
        {
            //updateUnloadingTroops();
        }
        else
        {
            waitForTroops();
        }        
		
	}
}
