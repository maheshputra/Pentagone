using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1MeleeAttack : MonoBehaviour
{
    [Header("Attack")]
    public int attackDamage;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask playerLayer;

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    private void Attack()
    {
        try
        {
            Collider2D playerHit = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);
            playerHit.GetComponent<CharacterMovement>().Damaged(transform.position);
        } catch { 
            // didnt hit
        }
    }
}
