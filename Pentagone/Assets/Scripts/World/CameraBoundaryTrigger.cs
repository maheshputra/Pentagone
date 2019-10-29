using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaryTrigger : MonoBehaviour
{
    public int boundaryNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            CameraBoundary.instance.SetNewBoundary(boundaryNumber);
    }
}
