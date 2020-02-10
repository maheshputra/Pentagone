using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour
{

    public bool isAttacking; //kondisi ketika sedang menyerang //public karena akan dipakai oleh charmovement
    public bool isDashing; //kondisi ketika sedang dash  //public karena akan dipakai oleh charmovement
    private Animator anim;

    [Header("Attack")]

    public int attackDamage;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask breakableLayer;
    [SerializeField] private LayerMask triggerLayer;
    private void Start()
    {
        anim = GetComponent<Animator>();
        isAttacking = false;
        isDashing = false;
    }
    private void ResetAnimTrigger()
    {
        anim.ResetTrigger("attack");
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    private void Attack() {
        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);
        foreach (Collider2D enemy in enemyHit)
        {
            MonsterStatus monsterStatus = enemy.GetComponent<MonsterStatus>();
            monsterStatus.Damaged(attackDamage);
            Debug.Log(monsterStatus.gameObject.name + " " + monsterStatus.GetCurrentHp());
        }

        Collider2D[] breakableHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, breakableLayer);
        foreach (Collider2D temp in breakableHit)
        {
            Breakable breakable = temp.GetComponent<Breakable>();
            breakable.BreakObject();
        }

        Collider2D[] triggerHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, triggerLayer);
        foreach (Collider2D temp in triggerHit)
        {
            TriggerSwitch trig = temp.GetComponent<TriggerSwitch>();
            trig.Triggered();
        }
    }
}
