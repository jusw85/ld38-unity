using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetControl : MonoBehaviour {

    private static TargetControl instance;
    public static TargetControl Instance { get { return instance; } }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        BgmManager.Instance.PlayBattle();
    }

    public float delay = 1f;

    private void Start() {
        StartCoroutine(Compute());
    }

    public IEnumerator Compute() {
        while (true) {
            foreach (EnemyController o in Enemies) {
                EnemyController closestAlly = ClosestAlly(o);

                Vector3 pos = o.transform.position;
                float d = (pos - Player.Instance.transform.position).sqrMagnitude;

                if (closestAlly != null && d > minD) {
                    o.followTarget = closestAlly.gameObject;
                    o.followTargetController = closestAlly;
                } else {
                    o.followTarget = Player.Instance.gameObject;
                    o.followTargetController = null;
                }
            }

            foreach (EnemyController o in Allies) {
                EnemyController closestEnemy = ClosestEnemy(o);
                if (closestEnemy != null) {
                    o.followTarget = closestEnemy.gameObject;
                    o.followTargetController = closestEnemy;
                }
            }
            yield return new WaitForSeconds(delay);
        }
    }

    public EnemyController ClosestEnemy(EnemyController me) {
        return MinType(me, Enemies);
    }

    public EnemyController ClosestAlly(EnemyController me) {
        return MinType(me, Allies);
    }

    private float minD;

    private EnemyController MinType(EnemyController me, HashSet<EnemyController> list) {
        Vector3 pos = me.transform.position;
        float minD = float.MaxValue;
        EnemyController minO = null;
        foreach (EnemyController o in list) {
            Vector3 opos = o.transform.position;
            float d = (pos - opos).sqrMagnitude;
            if (d < minD) {
                minD = d;
                minO = o;
            }
        }
        this.minD = minD;
        return minO;
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(Enemies.Count);
    }

    [System.NonSerialized]
    public HashSet<EnemyController> Allies = new HashSet<EnemyController>();
    [System.NonSerialized]
    public HashSet<EnemyController> Enemies = new HashSet<EnemyController>();

}
