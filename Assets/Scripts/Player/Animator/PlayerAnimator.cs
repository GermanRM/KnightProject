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
    private PlayerCombat playerCombat;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    private void OnEnable()
    {
        PlayerCombat.OnPlayerAttack += OnPlayerCombo;
        PlayerCombat.OnPlayerDamaged += OnPlayerGetDamage;
        PlayerCombat.OnPlayerDeath += OnPlayerDeathed;
        PlayerThrowing.OnPlayerThrow += OnPlayerThrowed;
    }

    private void OnDisable()
    {
        PlayerCombat.OnPlayerAttack -= OnPlayerCombo;
        PlayerCombat.OnPlayerDamaged -= OnPlayerGetDamage;
        PlayerCombat.OnPlayerDeath -= OnPlayerDeathed;
        PlayerThrowing.OnPlayerThrow -= OnPlayerThrowed;
    }

    // Update is called once per frame
    void Update()
    {
        OnPlayerMoved(playerMovement.GetMovementInput());
    }

    #region Player Movement Animations

    private void OnPlayerMoved(Vector2 movementInput)
    {
        if (movementInput.x > 0) spriteRenderer.flipX = false;
        else if (movementInput.x < 0) spriteRenderer.flipX = true;

        if (movementInput != Vector2.zero) animator.SetBool("IsMoving", true);
        else animator.SetBool("IsMoving", false);
    }

    #endregion

    #region Player Combat Animations

    private void OnPlayerCombo()
    {
        animator.SetTrigger($"AttackAnim");
    }

    private void OnPlayerThrowed()
    {
        animator.SetTrigger("ThrowAnim");
    }

    private void OnPlayerGetDamage(float damage)
    {
        animator.SetTrigger("HurtAnim");
    }

    private void OnPlayerDeathed()
    {
        animator.SetTrigger("DeathTrigger");
    }

    #endregion
}
