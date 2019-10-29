using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitMonsterAttack : MonoBehaviour
{
    [SerializeField] private EnemySight enemySight; //untuk data sight posisi player
    [SerializeField] private Transform projectilePrefab; //prefab projectilenya
    [SerializeField] private Transform projectileSpawnPosition; //posisi projectile spawnnya

    [Header("With Force Input")]
    [SerializeField] private float[] farXForce; //rigidbody velocity x for far shot
    [SerializeField] private float[] farYForce; //rigidbody velocity y for far shot
    [SerializeField] private float[] nearXforce; //rigidbody velocity x for near shot
    [SerializeField] private float[] nearYforce; //rigidbody velocity x for near shot

    [Header("Scale Controller")]
    private Vector3 defaultScale; //default scalenya
    private Vector3 currentScale; //scale yang dipakai untuk mereset/melanjutkan pattern

    private void Start()
    {
        defaultScale = transform.localScale;
    }

    /// <summary>
    /// Function untuk memulai serangan spit monster
    /// </summary>
    public void StartAttack() {
        currentScale = transform.localScale;
        if (enemySight.target.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
        else
            transform.localScale = defaultScale;
    }

    /// <summary>
    /// Function serangan spit monster
    /// Digunakan pada event di animation attack
    /// </summary>
    public void LaunchAttack()
    {
        if (enemySight.target.transform.position.x > transform.position.x)
            transform.localScale = new Vector3(-defaultScale.x, defaultScale.y, defaultScale.z);
        else
            transform.localScale = defaultScale;

        if (Vector2.Distance(transform.position, enemySight.target.position) <= 2)
            Shot(nearXforce, nearYforce);
        else
            Shot(farXForce, farYForce);
    }

    private void Shot(float[] xForce, float[] yForce) {

        for (int i = 0; i < farXForce.Length; i++)
        {

            float x = xForce[i];
            float y = yForce[i];
            if (enemySight.target.transform.position.x < transform.position.x)
                x *= -1;

            Rigidbody2D projectile = Instantiate(projectilePrefab, projectileSpawnPosition.position, Quaternion.identity).GetComponent<Rigidbody2D>();
            projectile.velocity = new Vector2(x, y);
        }
    }

    /// <summary>
    /// Function setelah monster menyerang
    /// </summary>
    public void ResetScale() {
        transform.localScale = currentScale;
    }
}
