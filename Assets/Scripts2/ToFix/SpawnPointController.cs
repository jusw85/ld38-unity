using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointController : MonoBehaviour {

    public GameObject spawnType;
    public float initialDelay = 0f;
    public bool runForever = false;
    public int spawnInstances = 1;
    public int poolInstances = 1;
    public float spawnInterval = 5f;

    public int numPerSpawn = 1;

    private bool isStarted = false;

    private bool isRunning = false;
    private bool isPooled = false;

    private PoolManager poolManager;

    private void Awake() {
        PoolObject poolObject = (PoolObject)spawnType.GetComponent(typeof(PoolObject));
        if (poolObject != null) {
            isPooled = true;

            poolManager = Toolbox.GetOrAddComponent<PoolManager>();
            poolManager.CreatePool(spawnType, Mathf.Min(poolInstances, 50));
        }
    }

    private void Update() {
        if (!isStarted) {
            initialDelay -= Time.deltaTime;
            if (initialDelay <= 0)
                isStarted = true;
            return;
        }
        if (!isRunning && (runForever || spawnInstances > 0)) {
            StartCoroutine(SpawnCoroutine());
        }

    }

    private IEnumerator SpawnCoroutine() {
        isRunning = true;
        while (runForever || (spawnInstances - numPerSpawn) >= 0) {
            spawnInstances -= numPerSpawn;
            for (int i = 0; i < numPerSpawn; i++) {
                Spawn();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        isRunning = false;
    }

    public void Spawn() {
        if (isPooled) {
            poolManager.ReuseObject(spawnType, transform.position, Quaternion.identity);
        } else {
            Instantiate(spawnType, transform.position, Quaternion.identity);
        }
        //GameObject obj = Instantiate(spawnType, transform.position, Quaternion.identity);
    }

}
