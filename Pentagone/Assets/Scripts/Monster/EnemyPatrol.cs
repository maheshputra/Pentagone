using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private EnemySight enemySight;

    private Rigidbody2D rb; //rigidbody enemy
    [SerializeField] private float speed; //movement speed
    private float speedDirection; //arah movement

    [Header("Ground Detection")]
    [SerializeField] private Vector2 raycastGroundOffset; //offset start raycast groundnya
    [SerializeField] private Vector2[] raycastGroundTarget; //raycast yang dia lihat
    [Header("Wall Detector")]
    [SerializeField] private Vector2 raycastWallOffset; //offset start raycastnya
    [SerializeField] private Vector2[] raycastWallTarget; //target pointnya
    [Header("Condition")]
    [SerializeField] private LayerMask groundLayer; //layer ground
    [SerializeField] private bool onRightEdge; //jika sedang di ujung kanan ground
    [SerializeField] private bool onLeftEdge; //jika sedang di ujung kiri ground


    private void Awake()
    {
        enemySight = GetComponent<EnemySight>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        speedDirection = 1;
    }

    void Update()
    {
        if (onRightEdge)
            speedDirection = -1;
        else if (onLeftEdge)
            speedDirection = 1;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * speedDirection, rb.velocity.y);

        onRightEdge = !Physics2D.Linecast((Vector2)transform.position+raycastGroundOffset, (Vector2)transform.position + raycastGroundTarget[0], groundLayer) ||
            Physics2D.Linecast((Vector2)transform.position+raycastWallOffset, (Vector2)transform.position + raycastWallTarget[0], groundLayer);
        onLeftEdge = !Physics2D.Linecast((Vector2)transform.position+raycastGroundOffset, (Vector2)transform.position + raycastGroundTarget[1], groundLayer) ||
            Physics2D.Linecast((Vector2)transform.position+raycastWallOffset, (Vector2)transform.position + raycastWallTarget[1], groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        //Ground
        Gizmos.DrawLine((Vector2)transform.position+raycastGroundOffset, (Vector2)transform.position+ raycastGroundTarget[0]);
        Gizmos.DrawLine((Vector2)transform.position+raycastGroundOffset, (Vector2)transform.position + raycastGroundTarget[1]);

        //Wall
        Gizmos.DrawLine((Vector2)transform.position+raycastWallOffset, (Vector2)transform.position + raycastWallTarget[0]);
        Gizmos.DrawLine((Vector2)transform.position+raycastWallOffset, (Vector2)transform.position + raycastWallTarget[1]);
    }
}
