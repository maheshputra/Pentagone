using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaryTrigger : MonoBehaviour
{
    public int boundaryNumber; //public karena akan di set dari instance camera boundary

    /// <summary>
    /// Function untuk mengubah camera boundary jika terkena trigger tersebut
    /// </summary>
    /// <param name="collision">player</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            CameraBoundary.instance.SetNewBoundary(boundaryNumber);
    }
}
