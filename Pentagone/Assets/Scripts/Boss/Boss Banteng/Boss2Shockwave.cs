using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Shockwave : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] shockwave; //shockwave
    [SerializeField] private GameObject platform; //platform untuk dodge shockwave

    public void Shockwave() {
        foreach (ParticleSystem shock in shockwave)
        {
            shock.Play();
        }
    }

    public void Platform() {
        StartCoroutine(SpawnPlatform());
    }

    IEnumerator SpawnPlatform() {
        yield return new WaitForSeconds(0.5f);
        platform.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        platform.SetActive(false);
    }
}
