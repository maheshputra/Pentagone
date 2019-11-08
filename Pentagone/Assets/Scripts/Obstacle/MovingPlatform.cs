using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum MovingPlatformType { TriggerMove, AlwaysMove }

    [SerializeField] private MovingPlatformType movingPlatformType;
    [SerializeField] private List<Transform> path = new List<Transform>(); //list path yang dituju
    [SerializeField] private float speed; //speed platformnya

    [Header("Is Already Triggered or No")]
    public bool isTrigger; //dipanggil dari trigger switch jika ingin ada triggernya
    private bool triggered; //juka sudah tertrigger
    private bool moving; //jika player sedang diatas platform
    private int currentPath; //path yang sedang dituju

    void Start()
    {
        triggered = false;
        currentPath = 0;
        moving = false;
    }

    private void Update()
    {
        if (movingPlatformType == MovingPlatformType.TriggerMove)
        {
            if (!triggered && isTrigger)
                Triggered();
            if (triggered)
            {
                if (!moving)
                {
                    ReverseMove();
                }
            }
        }
    }

    /// <summary>
    /// Function untuk menjalankan platform jika triggernya sudah dipicu
    /// </summary>
    public void Triggered() {
        triggered = true;
        transform.position = path[0].position;
        Debug.Log(gameObject.name);
    }

    /// <summary>
    /// Platform maju sesuai path
    /// </summary>
    private void TriggerMove()
    {
        if (currentPath < path.Count - 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[currentPath + 1].position, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, path[currentPath+1].position) < 0.1)
            {
                currentPath++;
            }
        }
    }

    /// <summary>
    /// Platform mundur melawan path
    /// </summary>
    private void ReverseMove()
    {
        if(Vector2.Distance(transform.position, path[currentPath].position) > 0.1)
            transform.position = Vector3.MoveTowards(transform.position, path[currentPath].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, path[currentPath].position) < 0.1)
        {
            if (currentPath > 0)
            {
                currentPath--;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (movingPlatformType == MovingPlatformType.TriggerMove)
        {
            if (triggered)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.transform.parent = this.transform;
                    TriggerMove();
                    moving = true;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (movingPlatformType == MovingPlatformType.TriggerMove)
        {
            if (triggered)
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.transform.parent = null;
                    moving = false;
                }
            }
        }
    }
}
