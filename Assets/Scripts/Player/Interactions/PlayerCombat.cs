using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combo System Properties")]
    [SerializeField] private float comboTime;
    [SerializeField] private int comboCounter;
    bool isAttacking;

    [Header("Combo System Counter")]
    [SerializeField] private bool startCounter;
    [SerializeField] private float comboTimeCounter;

    private PlayerInputs playerInputs;

    #region Events

    public static event Action<int> OnPlayerAttack;

    #endregion

    void Start()
    {
        playerInputs = new PlayerInputs();
        playerInputs.Combat.Enable();

        comboTimeCounter = comboTime;
    }
    
    void Update()
    {
        CounterManager();
        Attack();
    }

    private void CounterManager()
    {
        if (startCounter)
            comboTimeCounter -= Time.deltaTime;

        if (!isAttacking)
            startCounter = false;

        if (comboTimeCounter <= 0)
        {
            isAttacking = false;
            startCounter = false;
            comboCounter = 0;
            comboTimeCounter = comboTime;
        }
    }

    private void Attack()
    {
        if (playerInputs.Combat.Attack.WasPerformedThisFrame())
        {
            if (!isAttacking)
            {
                isAttacking = true;
                startCounter = true;

                comboCounter++;
            }
            else
            {
                if (comboCounter >= 4)
                {
                    comboCounter = 0;
                }
                else
                {
                    comboCounter++;
                    comboTimeCounter = comboTime;
                }
            }

            OnPlayerAttack?.Invoke(comboCounter);
        }
    }
}
