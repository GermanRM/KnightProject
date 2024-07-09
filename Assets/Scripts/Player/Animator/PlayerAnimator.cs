using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Animator Properties")]
    [SerializeField] Animator animator;

    [Header("Renderer Properties")]
    [SerializeField] SpriteRenderer spriteRenderer;

    #region References
    private PlayerMovement playerMovement;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        OnPlayerMoved(playerMovement.GetMovementInput());
    }

    private void OnPlayerMoved(Vector2 movementInput)
    {
        if (movementInput.x > 0) spriteRenderer.flipX = false;
        else if (movementInput.x < 0) spriteRenderer.flipX = true;
    }
}
