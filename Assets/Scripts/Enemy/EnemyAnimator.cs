using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [Header("Visual References")]
    private SpriteRenderer spriteRenderer;

    [Header("Animator References")]
    private Animator animator;
    [SerializeField] string EnemyAttackTrigger;

    private Enemy enemy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemy = GetComponentInParent<Enemy>();

        enemy.OnEnemyHitted += OnGetHitted;
        enemy.OnEnemyEndHitted += OnEndHitted;

        enemy.OnEnemyAttack += OnAttack;
    }

    private void OnDisable()
    {
        enemy.OnEnemyHitted -= OnGetHitted;
        enemy.OnEnemyEndHitted -= OnEndHitted;

        enemy.OnEnemyAttack -= OnAttack;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        EnemyFlip(enemy.target);
    }

    #region Movement

    private void EnemyMovement()
    {
        if (enemy.agent.velocity.magnitude > 0.1f) animator.SetBool("IsMoving", true);
        else animator.SetBool("IsMoving", false);
    }

    private void EnemyFlip(Transform target)
    {
        // Comprobar si el objeto objetivo está a la izquierda o a la derecha
        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;

            if (directionToTarget.x > 0)
            {
                //Debug.Log("El objeto está a la derecha.");
                spriteRenderer.flipX = false;

            }
            else if (directionToTarget.x < 0)
            {
                //Debug.Log("El objeto está a la izquierda.");
                spriteRenderer.flipX = true;
            }
            else
            {
                //Debug.Log("El objeto está directamente arriba o abajo.");
            }
        }
    }

    #endregion

    #region Enemy Hitted

    private void OnGetHitted()
    {
        animator.SetTrigger("HurtTrigger");
    }

    private void OnEndHitted()
    {
        animator.SetTrigger("StopHurtTrigger");
    }

    #endregion

    #region Enemy Combat

    private void OnAttack()
    {
        animator.SetTrigger(EnemyAttackTrigger);
    }

    #endregion
}
