using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Properties")]
    public float damage;
    [SerializeField] private float attackKnockback;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackCooldownTimer;

    [Header("Attack Hitbox")]
    [SerializeField] private Transform attackArea;
    [SerializeField] private Vector2 attackAreaSize;
    [SerializeField] private LayerMask enemyHitboxMask;

    [Header("Player Properties")]
    public float health;
    public bool isDamaged;
    [SerializeField] private float damagedCooldown;
    [SerializeField] private float damagedCooldownTimer;

    private PlayerInputs playerInputs;
    private PlayerMovement playerMovement;

    #region Events

    public static event Action OnPlayerAttack;
    public static event Action<float> OnPlayerDamaged;
    public static event Action OnPlayerHealed;

    public static event Action OnPlayerDeath;

    #endregion

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Combat.Enable();
        playerMovement = GetComponent<PlayerMovement>();

        attackCooldownTimer = attackCooldown;
    }

    private void OnEnable()
    {
        OnPlayerDamaged += OnPlayerGetDamaged;
    }

    private void OnDisable()
    {
        OnPlayerDamaged -= OnPlayerGetDamaged;
    }

    void Update()
    {
        AttackCooldown();
        Attack();

        DamageCooldown();

        if (Input.GetKeyDown(KeyCode.R)) DamagePlayer(1);
    }

    #region Combat

    private void Attack()
    {
        if (playerInputs.Combat.Attack.WasPerformedThisFrame())
        {
            if (attackCooldownTimer > 0 || isDamaged) return;

            attackCooldownTimer = attackCooldown;
            EnableAttackArea();

            OnPlayerAttack?.Invoke();
        }
    }

    private void AttackCooldown()
    {
        attackCooldownTimer -= Time.deltaTime;

        if (attackCooldownTimer <= 0)
        {
            attackCooldownTimer = 0;
        }
    }

    private void DamageCooldown()
    {
        damagedCooldownTimer -= Time.deltaTime;

        if (damagedCooldownTimer <= 0)
        {
            isDamaged = false;
            damagedCooldownTimer = 0;
        }
    }

    public void DamagePlayer(float damage)
    {
        health -= damage;
        isDamaged = true;
        damagedCooldownTimer = damagedCooldown;
        OnPlayerDamaged?.Invoke(damage);
    }

    public void HealPlayer(float heal)
    {
        health += heal;
    }

    private void EnableAttackArea()
    {
        RaycastHit2D hit = Physics2D.BoxCast(attackArea.position, attackAreaSize, 0, Vector2.zero, 0, enemyHitboxMask);

        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (!enemy.isHitted)
            {
                Vector2 force = playerMovement.GetLastMovDir() * attackKnockback;
                enemy.HurtEnemy(damage, force);
            }
        }
    }

    #endregion

    #region Interactions

    private void OnPlayerGetDamaged(float damage)
    {
        
        if (health <= 0)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    #endregion

    #region Debug
    private void OnDrawGizmos()
    {
        //Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackArea.position, attackAreaSize); //Draw box check
    }
    #endregion
}
