using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCollider : MonoBehaviour
{
    private Collider2D coll; //collider gameobjectnya

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Function agar setiap enemy bisa meng ignore collision enemy lainnya
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
            Physics2D.IgnoreCollision(coll, collision.gameObject.GetComponent<Collider2D>());
    }
}
