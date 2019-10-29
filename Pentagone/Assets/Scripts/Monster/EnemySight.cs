using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayMaker;

public class EnemySight : MonoBehaviour
{
    [SerializeField] private PlayMakerFSM sightFSM; //fsm untuk sight
    public Transform target; //target serang monster
    public bool onRange; //jika dalam range sight
    public bool onSight; //ketika enemy raycast langsung ke player tanpa adanya penghalang
    [SerializeField] private bool showSightGizmos; //untuk melihat size sight
    [SerializeField] private float sightRadius; //jarak sight musuh
    [SerializeField] private Vector2 sightPivot; //sight pivot
    [SerializeField] private LayerMask playerLayer; //layer khusus player (ketika masuk ke range sight)
    [SerializeField] private LayerMask sightLayer; //layer yang dapat terkena raycast hit sight (ground dan player)
    [SerializeField] private float direction;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        sightFSM.FsmVariables.FindFsmBool("onSight").Value = onSight;
    }

    private void FixedUpdate()
    {
        onRange = Physics2D.OverlapCircle((Vector2)transform.position + sightPivot, sightRadius, playerLayer);
        if (onRange)
        {
            Vector2 direction = target.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + sightPivot, direction.normalized, sightRadius, sightLayer);
            try
            {
                if (hit.collider.CompareTag("Player"))
                    onSight = true;
                else
                    onSight = false;
            } catch { }
        }
        else
            onSight = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (showSightGizmos)
            Gizmos.DrawSphere((Vector2)transform.position + sightPivot, sightRadius);
        if (onRange)
            Gizmos.DrawRay(transform.position, target.position - transform.position);
    }
}
