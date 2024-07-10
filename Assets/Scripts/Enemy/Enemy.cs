using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [Header("Enemy Combat Properties")]
    public float damage;
    [SerializeField] private float firstHitSpeed;
    [SerializeField] private float attackSpeed;
    public float enemyHealth;
    [SerializeField] private float hittedCooldown;
    [SerializeField] private bool delayFirstHit;
    public bool isHitted;
    [SerializeField] private bool canAttack = true;
    [SerializeField] bool isFinalBoss;

    [Header("Enemy CombatBox Properties")]
    [SerializeField] private Transform combatBox;
    [SerializeField] private Vector2 combatBoxSize;
    [SerializeField] private LayerMask playerHitboxLayer;
    [SerializeField] private bool flipCombatBoxPos;
    Vector3 combatBoxInitialPos;

    [Header("Enemy Movement Properties")]
    public float movementSpeed;
    [SerializeField] bool hasArrived;
    private bool applyKB;
    private Vector2 kbForce;
    private Rigidbody2D rb;
    bool stopFollow = false;

    [Header("NavMesh Properties")]
    public Transform target;
    public NavMeshAgent agent;

    private SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

    #region Events
    public event Action OnEnemyHitted;
    public event Action OnEnemyEndHitted;

    public event Action OnEnemyAttack;
    public event Action OnEnemyDeath;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        InitializeNavAgent();
    }

    private void OnEnable()
    {
        OnEnemyHitted += OnEnemyGetHitted;
        OnEnemyEndHitted += OnEnemyFinishHitted;
    }

    private void OnDisable()
    {
        OnEnemyHitted -= OnEnemyGetHitted;
        OnEnemyEndHitted -= OnEnemyFinishHitted;
    }

    private void InitializeNavAgent()
    {
        if (flipCombatBoxPos) combatBoxInitialPos = combatBox.localPosition;

        //Find Player
        target = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = movementSpeed;
    }

    void Update()
    {
        FollowPlayer();

        if (flipCombatBoxPos) FlipCombatBox();

        DetectIfArrived();
        Attack();
    }

    private void FixedUpdate()
    {
        if (applyKB)
        {
            applyKB = false;
            rb.AddForce(kbForce, ForceMode2D.Impulse);
            kbForce = Vector2.zero;
        }
    }

    #region Movement

    private void FollowPlayer()
    {
        if (!target.gameObject.GetComponent<PlayerCombat>().isDead || stopFollow)
        {
            if (!isHitted)
                agent.SetDestination(target.position);
        }
    }

    private void DetectIfArrived()
    {
        if (!agent.pathPending && !isHitted)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.25f)
                {
                    hasArrived = true;
                }
                else
                {
                    hasArrived = false;
                }
            }
            else
            {
                hasArrived = false;
            }
        }
    }

    #endregion

    #region Enemy Get Hurts

    public void HurtEnemy(float damage, Vector2 forceDirection)
    {
        enemyHealth -= damage;
        ApplyImpulse(forceDirection);
        OnEnemyHitted?.Invoke();
    }

    private void OnEnemyGetHitted()
    {
        StartCoroutine(PlayerHittedCooldown());
    }

    private IEnumerator PlayerHittedCooldown()
    {
        isHitted = true;
        canAttack = false;
        yield return new WaitForSeconds(hittedCooldown);
        isHitted = false;
        OnEnemyEndHitted?.Invoke();
    }

    private void ApplyImpulse(Vector2 force)
    {
        agent.enabled = false;
        rb.isKinematic = false;

        applyKB = true;
        kbForce = force;

        StartCoroutine(ReenableNavMeshAgent());
    }

    IEnumerator ReenableNavMeshAgent()
    {
        yield return new WaitForSeconds(hittedCooldown); // Esperar al cooldown de hitted
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // Hacer el Rigidbody cinemático nuevamente
        agent.enabled = true; // Rehabilitar el NavMeshAgent
    }

    private void OnEnemyFinishHitted()
    {
        if (enemyHealth <= 0)
        {
            if (!isFinalBoss)
            {
                GameManager.instance.killCounter++;
                GameManager.instance.playerReference.GetComponent<PlayerCombat>().damage = GameManager.instance.playerReference.GetComponent<PlayerCombat>().damage + 0.2f;
                GameManager.instance.playerReference.GetComponent<PlayerMovement>().movementSpeed = GameManager.instance.playerReference.GetComponent<PlayerMovement>().movementSpeed + 0.2f;
                Destroy(gameObject);
            }
            else
            {
                GameManager.instance.killCounter++;
                GameManager.instance.playerReference.GetComponent<PlayerCombat>().damage = GameManager.instance.playerReference.GetComponent<PlayerCombat>().damage + 0.2f;
                GameManager.instance.playerReference.GetComponent<PlayerMovement>().movementSpeed = GameManager.instance.playerReference.GetComponent<PlayerMovement>().movementSpeed + 0.2f;
                stopFollow = true;
                animator.SetTrigger("DeathTrigger");
                GameManager.instance.OnGameWin();
            }
        }
    }

    #endregion

    #region EnemyCombat

    private void Attack()
    {
        if (target.gameObject.GetComponent<PlayerCombat>().isDead) return;

        if (hasArrived && canAttack)
        {
            if (isHitted) return;

            if (delayFirstHit)
                StartAttackDelayed();
            else
                StartAttack();

        }
    }

    private void StartAttack()
    {
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;

        //Debug.Log("Attack");
        EnableAttackArea();
        OnEnemyAttack?.Invoke();

        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

    private void StartAttackDelayed()
    {     
        StartCoroutine(AttackCooldownDelayed());
    }

    private IEnumerator AttackCooldownDelayed()
    {
        canAttack = false;
        OnEnemyAttack?.Invoke();

        yield return new WaitForSeconds(attackSpeed);
        EnableAttackArea();      

        canAttack = true;

    }

    private void EnableAttackArea()
    {
        RaycastHit2D hit = Physics2D.BoxCast(combatBox.position, combatBoxSize, 0, Vector2.zero, 0, playerHitboxLayer);

        if (hit.collider != null)
        {
            PlayerCombat combat = hit.collider.GetComponentInParent<PlayerCombat>();

            if (combat != null)
            {
                if (!combat.isDamaged)
                {
                    combat.DamagePlayer(damage);
                }
            }
        }
    }

    private void FlipCombatBox()
    {
        if (!spriteRenderer.flipX) combatBox.localPosition = combatBoxInitialPos;
        else combatBox.localPosition = -combatBoxInitialPos;
    }

    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        //Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(combatBox.position, combatBoxSize); //Draw box check
    }
    #endregion
}
