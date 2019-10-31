using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deadzone : MonoBehaviour
{
    [SerializeField] private Transform respawnPosition; //posisi respawn
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(Respawn(collision.gameObject));
        }
    }

    /// <summary>
    /// Function untuk respawn sesuai dengan posisi
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private IEnumerator Respawn(GameObject player) {
        yield return new WaitForSeconds(0.5f);
        player.transform.position = respawnPosition.position;
    }
}
