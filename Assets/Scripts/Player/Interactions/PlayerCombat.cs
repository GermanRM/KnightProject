using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combat Properties")]
    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackCooldownTimer;
    [SerializeField] private GameObject attackArea;

    [Header("Player Properties")]
    [SerializeField] private float health;
    public bool isDamaged;
    [SerializeField] private float damagedCooldown;
    [SerializeField] private float damagedCooldownTimer;

    private PlayerInputs playerInputs;

    #region Events

    public static event Action OnPlayerAttack;
    public static event Action<float> OnPlayerDamaged;
    public static event Action OnPlayerHealed;

    #endregion

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Combat.Enable();

        attackCooldownTimer = attackCooldown;
    }
    
    void Update()
    {
        AttackCooldown();
        Attack();

        DamageCooldown();

        if (Input.GetKeyDown(KeyCode.R)) DamagePlayer(1);
    }

    private void Attack()
    {
        if (playerInputs.Combat.Attack.WasPerformedThisFrame())
        {
            if (attackCooldownTimer > 0) return;

            attackCooldownTimer = attackCooldown;
            attackArea.SetActive(true);
            Invoke(nameof(DisableAttackArea), attackCooldown);

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

    private void DisableAttackArea()
    {
        attackArea.SetActive(false);
    }
}
