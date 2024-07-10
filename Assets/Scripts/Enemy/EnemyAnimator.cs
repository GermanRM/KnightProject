using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    private Enemy enemy;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyFlip(enemy.target);
        EnemyHittedAnim();
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

    private void EnemyHittedAnim()
    {
        if (enemy.isHitted)
        {
            if (UnityEngine.ColorUtility.TryParseHtmlString("#FF4545", out Color newColor))
            {            
                // Si la conversión fue exitosa, asignar el color al SpriteRenderer
                spriteRenderer.color = newColor;
            }
        }
        else
        {
            if (UnityEngine.ColorUtility.TryParseHtmlString("#FFFFFF", out Color newColor))
            {
                
                // Si la conversión fue exitosa, asignar el color al SpriteRenderer
                spriteRenderer.color = newColor;
            }
        }
    }
}
