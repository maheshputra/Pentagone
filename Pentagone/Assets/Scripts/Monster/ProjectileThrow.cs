using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrow : MonoBehaviour
{
    private EnemySight enemySight;

    [SerializeField] private Transform projectilePrefab; //prefab projectilenya

    [Header("With Force Input")]
    [SerializeField] private float[] xForce; //rigidbody velocity x
    [SerializeField] private float[] yForce; //rigidbody velocity y
    [Range(1,10)] [SerializeField] private float attackInterval; //setiap berapa detik dia menyerang
    [SerializeField] private float countDown; //untuk menghitung attack interval

    [Header("Targeted / Parabola Physics")]
    [SerializeField] private float targetOffsetX; //offset target
    [SerializeField] private float gravity; //gravity
    [SerializeField] private float angle; //angle of shot

    private void Awake()
    {
        enemySight = GetComponent<EnemySight>();
    }

    private void Start()
    {
        countDown = 0;
    }

    private void Update()
    {
        if (enemySight.onSight)
        {
            countDown += Time.deltaTime;
            if (countDown >= attackInterval)
            {
                countDown = 0;
                SimulateProjectile();
            }
        }
    }

    /// <summary>
    /// Function mensimulasikan projectile berdasarkan input force yang dikeluarkan
    /// </summary>
    private void SimulateProjectile()
    {
        for (int i = 0; i < xForce.Length; i++)
        {

            float x = xForce[i];
            float y = yForce[i];
            if (enemySight.target.transform.position.x < transform.position.x)
                x *= -1;

            Rigidbody2D projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            projectile.velocity = new Vector2(x, y);
        }
    }

    /// <summary>
    /// Function untuk mensimulasikan projectile berdasarkan rumus parabola
    /// </summary>
    private void SimulateProjectileTargeted()
    {
        float xDistance;
        xDistance = Mathf.Abs(transform.position.x - enemySight.target.position.x);

        float Vo;
        Vo = (xDistance * gravity) / (2 * Mathf.Sin(angle) * Mathf.Cos(angle));
        Debug.Log(Vo);
        Vo = Mathf.Sqrt(Vo);
        Debug.Log(Vo);

        float Vox;
        Vox = Vo * Mathf.Cos(angle);
        float Voy;
        Voy = Vo * Mathf.Sin(angle);

        if (transform.position.x > enemySight.target.position.x)
            //Vox *= -1f;

            Debug.Log(Vox + " " + Voy);

        Transform projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Rigidbody2D projectileRigid = projectile.GetComponent<Rigidbody2D>();
        projectileRigid.velocity = new Vector2(Vox, Voy);
    }
}
