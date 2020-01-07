using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCoin : MonoBehaviour
{
    [SerializeField] private int totalCoin;
    [SerializeField] private GameObject coin;

    public void SpawnCoin() {
        for (int i = 0; i < totalCoin; i++)
        {
            Instantiate(coin, transform.position, Quaternion.identity, null);
        }
    }
}
