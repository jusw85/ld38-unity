using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

    public float projectileForce = 50f;
    public string targetTag;

    private void OnTriggerEnter2D(Collider2D other) {
        string tag = other.gameObject.tag;
        if (tag.Equals(targetTag)) {
            MoverController movable = other.transform.parent.gameObject.GetComponent<MoverController>();
            if (movable != null) {
                Transform otherTransform = other.transform.parent.transform;
                //Vector2 dir = transform.up.normalized;
                Vector3 dir = (otherTransform.position - transform.parent.position).normalized;
                movable.ExternalForce = dir * projectileForce;
            }
            IDamageable damageable = (IDamageable)other.transform.parent.gameObject.GetComponent(typeof(IDamageable));
            if (damageable != null) {
                DamageInfo damageInfo = new DamageInfo();
                damageInfo.damage = 10;
                damageable.Damage(damageInfo);
            }
            if (gameObject.tag.Equals("Player")) {
                Enemy ec = other.transform.parent.GetComponent<Enemy>();
                EnemyHpBarController.Instance.SetValue(ec.currentHp, ec.maxHp);
            }
        }


        //string tag = other.gameObject.tag;
        //if (tag == "enemy") {
        //    PoolObject poolObject = other.gameObject.GetComponent<PoolObject>();
        //    if (poolObject != null) {
        //        poolObject.Destroy();
        //    } else {
        //        Destroy(other.gameObject);
        //    }
        //}
    }
}
