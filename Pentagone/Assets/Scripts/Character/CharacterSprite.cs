using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour
{

    public bool isAttacking; //kondisi ketika sedang menyerang //public karena akan dipakai oleh charmovement
    public bool isDashing; //kondisi ketika sedang dash  //public karena akan dipakai oleh charmovement
    private Animator anim;

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
}
