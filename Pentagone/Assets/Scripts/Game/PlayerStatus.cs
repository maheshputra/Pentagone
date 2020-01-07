using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using TMPro;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance; //singleton

    private int maxHp = 20; //maximum hp
    
    [SerializeField] [Range(1, 20)] private int currentMaxHp; //current maximum hp
    [SerializeField] private int currentHp; //current hp
    [SerializeField] private int dropCoin; //total drop coin
    private int targetCoin;
    public int skillPoint;
    private float pointPercentage;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private GameObject healthBar; //bar hp
    [SerializeField] private GameObject[] healthPoint; //gambar point hpnya
    [SerializeField] private TextMeshProUGUI menuSkillPoint;
    [SerializeField] private Image menuSkillPointCoin;

    [Space(10)]
    [Header("PopUp")]
    [SerializeField] private UIView popUpCoinView;
    [SerializeField] private TextMeshProUGUI popUpSkillPoint;
    [SerializeField] private Image popUpSkillPointCoin;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    void Start()
    {
        targetCoin = 10;

        healthPoint = new GameObject[(int)maxHp];
        for (int i = 0; i < maxHp; i++)
        {
            healthPoint[i] = healthBar.transform.GetChild(i).gameObject;
        }
        ResetHP();
        pointPercentage = (float)dropCoin / (float)targetCoin;
        menuSkillPointCoin.fillAmount = pointPercentage;
        popUpSkillPointCoin.fillAmount = pointPercentage;
        popUpSkillPoint.text = skillPoint.ToString();
        menuSkillPoint.text = skillPoint.ToString();
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
        pointPercentage = (float)dropCoin / (float)targetCoin;
        menuSkillPointCoin.fillAmount = pointPercentage;
        popUpSkillPointCoin.fillAmount = pointPercentage;
        if (pointPercentage == 1)
        {
            pointPercentage = 0;
            dropCoin = 0;
            targetCoin += 10;
            skillPoint++;
            menuSkillPointCoin.fillAmount = pointPercentage;
            popUpSkillPointCoin.fillAmount = pointPercentage;
            popUpSkillPoint.text = skillPoint.ToString();
            menuSkillPoint.text = skillPoint.ToString();
        }

        StopCoroutine(ShowCoin());
        StartCoroutine(ShowCoin());
    }

    public void UsePoint() {
        skillPoint--;
        popUpSkillPoint.text = skillPoint.ToString();
        menuSkillPoint.text = skillPoint.ToString();
    }

    IEnumerator ShowCoin()
    {
        popUpCoinView.Show();
        yield return new WaitForSeconds(3f);
        popUpCoinView.Hide();
    }
}
