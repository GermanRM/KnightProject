using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    [Header("Rock Properties")]
    [SerializeField] private float lifeTime;
    [SerializeField] private float lifeTimeCounter;
    [SerializeField] private float damage;
    [SerializeField] private float rockKnockback;

    [Header("Movement Properties")]
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool startMovement;

    [Header("Rock Hitbox Properties")]
    [SerializeField] private Vector2 rockHitboxSize;
    [SerializeField] private LayerMask enemyHitboxMask;

    public void Initialize(Vector2 moveDirection, float moveSpeed, float rockKnockback)
    {
        this.moveDirection = moveDirection;
        this.moveSpeed = moveSpeed;
        this.rockKnockback = rockKnockback;

        lifeTimeCounter = lifeTime;

        startMovement = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startMovement)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            lifeTimeCounter -= Time.deltaTime;

            if (lifeTimeCounter <= 0 )
            {
                Destroy(gameObject);
            }
        }

        RockBoxCast();
    }

    private void RockBoxCast()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, rockHitboxSize, 0, Vector2.zero, 0, enemyHitboxMask);

        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();

            if (!enemy.isHitted)
            {
                Vector2 force = moveDirection * rockKnockback;
                enemy.HurtEnemy(damage, force);
            }

            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, rockHitboxSize); //Draw box check
    }
}
