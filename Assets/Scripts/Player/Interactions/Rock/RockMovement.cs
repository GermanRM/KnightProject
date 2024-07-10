using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    [Header("Rock Properties")]
    [SerializeField] private float lifeTime;
    [SerializeField] private float lifeTimeCounter;
    [SerializeField] private float damage;

    [Header("Movement Properties")]
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool startMovement;

    public void Initialize(Vector2 moveDirection, float moveSpeed)
    {
        this.moveDirection = moveDirection;
        this.moveSpeed = moveSpeed;
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
    }
}
