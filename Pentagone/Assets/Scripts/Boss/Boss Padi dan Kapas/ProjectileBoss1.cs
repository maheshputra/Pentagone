using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBoss1 : MonoBehaviour
{
    public enum ProjectileType { Fall, Targeting}
    [SerializeField] private float projectileSpeed; //projectile speed
    [SerializeField] ProjectileType projectileType;

    void Start()
    {
        if (projectileType == ProjectileType.Targeting)
        {
            Transform target = GameObject.FindGameObjectWithTag("Player").transform;
            Vector3 direction = target.position - transform.position;
            direction = direction.normalized;
            transform.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
        }
        else if (projectileType == ProjectileType.Fall)
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector3.down * projectileSpeed;
        }
    }
}
