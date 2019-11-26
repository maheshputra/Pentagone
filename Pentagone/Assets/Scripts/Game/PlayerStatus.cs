using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance; //singleton

    private int maxHp = 20; //maximum hp
    
    [SerializeField] [Range(1, 20)] private int currentMaxHp; //current maximum hp
    [SerializeField] private int currentHp; //current hp
    public int dropCoin; //total drop coin

    [Header("UI")]
    [SerializeField] private GameObject healthBar; //bar hp
    [SerializeField] private GameObject[] healthPoint; //gambar point hpnya

    [Header("Skills")]
    public bool autoCollect;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        healthPoint = new GameObject[(int)maxHp];
        for (int i = 0; i < maxHp; i++)
        {
            healthPoint[i] = healthBar.transform.GetChild(i).gameObject;
        }
        autoCollect = true;
        ResetHP();
    }

    /// <summary>
    /// Function untuk mereset hp
    /// </summary>
    void ResetHP() {
        currentHp = currentMaxHp;
        for (int i = 0; i < maxHp; i++)
        {
            if(i<currentMaxHp)
                healthPoint[i].SetActive(true);
            else
                healthPoint[i].SetActive(false);
        }
    }

    /// <summary>
    /// Function ketika terkena damage
    /// </summary>
    public void Damaged()
    {
        if (currentHp > 0)
        {
            currentHp -= 1;
            healthPoint[currentHp].SetActive(false);
        }
    }

    public void Collect() {
        dropCoin++;
    }
}
