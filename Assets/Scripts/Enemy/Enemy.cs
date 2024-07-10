using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [Header("Enemy Combat Properties")]
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float enemyHealth;
    [SerializeField] private float hittedCooldown;
    public bool isHitted;

    [Header("Enemy Movement Properties")]
    [SerializeField] private float movementSpeed;
    private bool applyKB;
    private Vector2 kbForce;
    private Rigidbody2D rb;

    [Header("NavMesh Properties")]
    public Transform target;
    NavMeshAgent agent;

    #region Events
    public event Action OnPlayerHitted;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        InitializeNavAgent();
    }

    private void OnEnable()
    {
        OnPlayerHitted += OnPlayerGetHitted;
    }

    private void OnDisable()
    {
        OnPlayerHitted -= OnPlayerGetHitted;
    }

    private void InitializeNavAgent()
    {
        //Find Player
        target = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = movementSpeed;
    }

    void Update()
    {
        FollowPlayer();
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
        if (!isHitted)
            agent.SetDestination(target.position);
    }

    #endregion

    #region Enemy Get Hurts

    public void HurtEnemy(float damage, Vector2 forceDirection)
    {
        enemyHealth -= damage;
        ApplyImpulse(forceDirection);
        OnPlayerHitted?.Invoke();
    }

    private void OnPlayerGetHitted()
    {
        StartCoroutine(PlayerHittedCooldown());        
    }

    private IEnumerator PlayerHittedCooldown()
    {
        isHitted = true;
        yield return new WaitForSeconds(hittedCooldown);
        isHitted = false;
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

    #endregion
}
