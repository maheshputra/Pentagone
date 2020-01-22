using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaryTrigger : MonoBehaviour
{
    public int boundaryNumber; //set sesuai nomor boundary

    /// <summary>
    /// Function untuk mengubah camera boundary jika terkena trigger tersebut
    /// </summary>
    /// <param name="collision">player</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger)
        {
            CameraBoundary.instance.SetNewBoundary(boundaryNumber);
            //Debug.Log(boundaryNumber);
        }
    }
}
