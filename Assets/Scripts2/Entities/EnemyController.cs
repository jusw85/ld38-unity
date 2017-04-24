using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyAnimator))]
[RequireComponent(typeof(EnemyAudio))]
public class EnemyController : PoolObject, IDamageable {

    public float pushbackForce = 40f;
    public GameObject followTarget;
    [System.NonSerialized]
    public EnemyController followTargetController;

    private MoverController moverController;
    private Animator fsm;

    public GameObject bloodSplatter;
    private PoolManager poolManager;


    private BoxCollider2D hitbox;
    private BoxCollider2D movement;
    private Rigidbody2D rb2d;
    private GameObject floatingText;

    private Enemy enemy;
    private EnemyAnimator enemyAnimator;
    private EnemyAudio enemyAudio;

    private EnemyFrameInfo frameInfo;
    private SpriteRenderer spriteRenderer;

    private EnemyHpBarControllerSingle hpbar;
    private void Awake() {
        moverController = GetComponent<MoverController>();

        hitbox = transform.Find("Hitbox").GetComponent<BoxCollider2D>();
        movement = transform.Find("Movement").GetComponent<BoxCollider2D>();
        floatingText = transform.Find("FloatingText").gameObject;
        hpbar = transform.Find("Canvas/HpBar").GetComponent<EnemyHpBarControllerSingle>();
        rb2d = GetComponent<Rigidbody2D>();

        poolManager = Toolbox.GetOrAddComponent<PoolManager>();
        poolManager.CreatePool(bloodSplatter, 150);

        damageInfo = new DamageInfo();
        damageInfo.damage = 10;


        fsm = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyAudio = GetComponent<EnemyAudio>();

        frameInfo = new EnemyFrameInfo();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        OnObjectReuse();
        //followTarget = Player.Instance.gameObject;
    }

    public bool trackTarget = false;

    private static readonly Vector2 FACE_DOWN = new Vector2(0f, -1f);
    private static readonly Vector2 FACE_RIGHT = new Vector2(1f, 0f);
    private static readonly Vector2 FACE_UP = new Vector2(0f, 1f);
    private static readonly Vector2 FACE_LEFT = new Vector2(-1f, 0f);

    [System.NonSerialized]
    public Vector2 faceDir = FACE_DOWN;
    public float moveSpeed = 12f;

    //public void Move(Vector2 moveInput) {
    //    moverController.Speed = moveSpeed;
    //    moverController.Direction = moveInput;
    //    Face(moveInput);
    //}

    public void Face(Vector2 moveDir) {
        Vector2 v = moveDir.normalized;

        if (Mathf.Abs(v.x) >= Mathf.Abs(v.y)) {
            if (v.x < 0) {
                faceDir = FACE_LEFT;
            } else {
                faceDir = FACE_RIGHT;
            }
        } else {
            if (v.y < 0) {
                faceDir = FACE_DOWN;
            } else {
                faceDir = FACE_UP;
            }
        }
    }

    public float proximityStop = 30f;

    //public void DoUpdate() 
    // decide what to do based on state and vars
    // update fsm state
    // 
    //}

    public static int ATTACKING = Animator.StringToHash("Base.Attack");
    private bool isDying = false;

    private IEnumerator DeathWait() {
        yield return new WaitForSeconds(3f);
        DOTween
            .To(() => spriteRenderer.color, x => spriteRenderer.color = x, Color.clear, 1.5f)
            .OnComplete(Destroy)
            .Play();
    }

    private bool isFollowTargetActive() {
        if (followTarget != null && followTarget.activeSelf) {
            if (followTargetController == null || (followTargetController != null && !followTargetController.isDying)) {
                return true;
            }
        }
        return false;
    }

    private bool canAttack = true;
    public float attackDelay = 1f;
    private IEnumerator AttackCooldown() {
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    private void Update() {
        //if (stopFrames-- > 0) {
        //    moverController.MoveSpeed = 0;
        //}
        if (isDying) {
            return;
        }
        if (enemy.currentHp <= 0) {
            fsm.SetTrigger("triggerDeath");
            if (faceDir.x == -1) {
                spriteRenderer.flipX = true;
            }
            if (tag.Equals("Enemy")) {
                TargetControl.Instance.Enemies.Remove(this);
            } else {
                TargetControl.Instance.Allies.Remove(this);
            }

            isDying = true;
            movement.gameObject.SetActive(false);
            hitbox.gameObject.SetActive(false);
            floatingText.SetActive(false);
            hpbar.gameObject.SetActive(false);
            rb2d.isKinematic = true;
            StartCoroutine(DeathWait());
            return;
        }

        if (trackTarget && Mathf.Clamp(stopFrames--, 0, stopFrames) <= 0) {
            if (isFollowTargetActive()) {
                moverController.Speed = moveSpeed;
                moverController.Direction = Vector2.zero;
                var followVector = (followTarget.transform.position - transform.position);
                if (followVector.magnitude <= proximityStop) {
                    moverController.Speed = 0f;
                    //Face(moverController.MoveDirection);
                    //moverController.MoveDirection = Vector2.zero;
                    moverController.Direction = followVector;

                    if (fsm.GetCurrentAnimatorStateInfo(0).fullPathHash != ATTACKING && canAttack) {
                        canAttack = false;
                        fsm.SetTrigger("triggerAttack");
                        StartCoroutine(AttackCooldown());
                    }
                } else {
                    moverController.Direction = followVector;
                }

            } else {
                moverController.Speed = 0f;
                //moverController.Direction = FACE_DOWN;
            }
            Face(moverController.Direction);
        } else {
            moverController.Speed = 0;
            moverController.Direction = FACE_DOWN;
        }
        moverController.UpdateVelocity();

        if (moverController.Speed > 0) {
            fsm.SetBool(AnimParams.ISMOVING, true);
        } else {
            fsm.SetBool(AnimParams.ISMOVING, false);
        }
        fsm.SetFloat(AnimParams.FACEDIRX, faceDir.x);
        fsm.SetFloat(AnimParams.FACEDIRY, faceDir.y);

        FsmFrameInfo state = null;
        enemy.DoUpdate(state, ref frameInfo);
        enemyAnimator.DoUpdate(enemy, ref frameInfo);
        enemyAudio.DoUpdate(enemy, ref frameInfo);

        frameInfo.Reset();
    }

    //private void OnCollisionEnter2D(Collision2D other) {
    //    string tag = other.gameObject.tag;
    //    if (tag == "Player") {
    //        MoverController movable = other.gameObject.GetComponent<MoverController>();
    //        if (movable != null) {
    //            movable.ExternalForce = moverController.Direction * pushbackForce;
    //        }
    //        IDamageable damageable = (IDamageable)other.gameObject.GetComponent(typeof(IDamageable));
    //        if (damageable != null) {
    //            damageable.Damage(damageInfo);
    //        }
    //    }
    //}

    private DamageInfo damageInfo;

    public void Damage(DamageInfo damageInfo) {
        //Vector3 inRot = damager.transform.eulerAngles;
        //Vector3 outRot = new Vector3(-inRot.z - 90f, 0f, 0f);

        //Quaternion rot = Quaternion.FromToRotation(Vector3.forward, Vector3.right);
        //rot *= Quaternion.Euler(outRot);

        //poolManager.ReuseObject(bloodSplatter, transform.position, rot);
        //if (currentHp <= 0) {
        //    Destroy();
        //}

        frameInfo.damageInfo = damageInfo;

        stopFrames = 16; // should use time-based instead

        hpbar.SetValue(enemy.currentHp - damageInfo.damage, enemy.maxHp);
    }

    public override void OnObjectReuse() {
        isDying = false;
        movement.gameObject.SetActive(true);
        hitbox.gameObject.SetActive(true);
        floatingText.SetActive(true);
        hpbar.gameObject.SetActive(true);
        rb2d.isKinematic = false;
        spriteRenderer.flipX = false;
        spriteRenderer.color = Color.white;
        faceDir = FACE_DOWN;

        if (tag.Equals("Enemy")) {
            TargetControl.Instance.Enemies.Add(this);
            EnemyController o = TargetControl.Instance.ClosestAlly(this);
            if (o == null) {
                followTarget = Player.Instance.gameObject;
            } else {
                followTarget = o.gameObject;
            }
        } else {
            TargetControl.Instance.Allies.Add(this);
            EnemyController o = TargetControl.Instance.ClosestEnemy(this);
            if (o != null) {
                followTarget = o.gameObject;
            }
        }

        if (followTarget != null) {
            followTargetController = followTarget.GetComponent<EnemyController>();
        }
        enemy.Reset();
    }


    private int stopFrames = 0;

    public IEnumerator Invuln(int numFrames) {
        hitbox.enabled = false;
        for (int i = 0; i < numFrames; i++) {
            yield return null;
        }
        hitbox.enabled = true;
    }

}

public class EnemyFrameInfo {
    public bool isDamaged;

    public bool isCharging;
    public bool isFullyCharged;
    public bool hasStoppedCharging;

    public bool toAttack;
    public bool toChargeAttack;

    public bool isAttacking;
    public bool isChargeAttacking;

    public DamageInfo damageInfo;

    public void Reset() {
        isDamaged = false;
        isCharging = false;
        isFullyCharged = false;
        hasStoppedCharging = false;
        toAttack = false;
        toChargeAttack = false;
        isAttacking = false;
        isChargeAttacking = false;
        damageInfo = null;
    }
}
