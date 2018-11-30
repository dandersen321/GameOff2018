using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UFO : MonoBehaviour {

    public List<EnemyType> enemyTypes;

    //private List<GameObject> enemyPrefabs;
    private List<Enemy> landedEnemies;
    private Vector3 startingPosition;
    private Vector3 landingPosition;
    private bool started = false;
    private bool flying = true;
    private bool unloadingTroops = false;
    private bool waitingForTroops = false;
    private bool retreating = false;
    private float speed = 5f;
    public float chargeTime;
    public float intervalTime = 1f;
    private int failedToSpawn;

    private Wave wave;

    //public void init(List<GameObject> enemyPrefabs, Vector3 landingPosition)
    public void init(Wave wave)
    {
        //this.enemyPrefabs = enemyPrefabs;
        //this.landingPosition = landingPosition;
        this.wave = wave;
        started = true;
        flying = true;
        unloadingTroops = false;
        waitingForTroops = false;
        retreating = false;
        landedEnemies = new List<Enemy>();
        //this.startingPosition = this.transform.position;
    }

	// Use this for initialization
	void Start () {
        // TODO might need to do this on awake and disable these initially?
        this.landingPosition = this.transform.position;
        //this.startingPosition = this.transform.parent.position;
        float distanceAway = 20f;
        Vector2 offsetV2 = (Random.insideUnitCircle.normalized * distanceAway);
        Vector3 ufoOffset = new Vector3(offsetV2.x, landingPosition.y + distanceAway, offsetV2.y);

        this.startingPosition = landingPosition + ufoOffset;
        this.transform.position = this.startingPosition;
        this.gameObject.SetActive(false);
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

        
        failedToSpawn = 0;
        foreach (EnemyType enemyType in enemyTypes)
        {
            

            Vector2 enemySpawnPointV2 = (Random.insideUnitCircle.normalized * localSpawnRadius) + localSpawnPoint;
            Vector3 enemySpawnPoint = new Vector3(enemySpawnPointV2.x, spawnPoint.y, enemySpawnPointV2.y);
            Enemy landedEnemy = this.CreateEnemy(enemyType.enemyPrefab, enemySpawnPoint).GetComponent<Enemy>();
            landedEnemy.GetComponent<AgentMovementController>().init();
            
            landedEnemies.Add(landedEnemy);

            yield return new WaitForSeconds(intervalTime);
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
            failedToSpawn += 1;
            if(failedToSpawn >= 5)
            {
                retreat();
            }
            //throw new KeyNotFoundException("Hmm, no nav mesh hit?");
            //return null;
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
        waitingForTroops = false;
        retreating = true;

        retreat();


    }

    void retreat()
    {
        float step = speed * Time.deltaTime;
        Vector3 targetPosition = startingPosition;
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosition, step);
        if (Vector3.Distance(this.transform.position, targetPosition) < 1f)
        {
            foreach (Enemy enemy in landedEnemies)
            {
                Destroy(enemy.gameObject);
            }

            wave.ufoDied();
            GetComponent<AudioSource>().Stop();
            Destroy(this.gameObject);
        }

        
    }
	
	// Update is called once per frame
	void Update () {
        if (!started)
            return;

        if(flying)
        {
            updateFlying();
        }
        else if (unloadingTroops)
        {
            //updateUnloadingTroops();
        }
        else if(waitingForTroops)
        {
            waitForTroops();
        }        
        else
        {
            retreat();
        }
		
	}

    public void resetUfo()
    {
        StopAllCoroutines();
        if (landedEnemies != null && landedEnemies.Count > 0)
        {
            foreach (Enemy enemy in landedEnemies)
            {
                if (enemy.gameObject)
                {
                    Destroy(enemy.gameObject);
                }
            }
        }
        this.transform.position = this.startingPosition;
        this.gameObject.SetActive(false);
    }
}
