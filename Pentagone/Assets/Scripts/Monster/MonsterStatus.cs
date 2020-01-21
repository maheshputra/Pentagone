using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStatus : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Animator anim;

    [SerializeField] private int maxHp; //max hp yang ada di monster
    public int currentHp; //current hp monster

    [Header("Dead Settings")]
    [SerializeField] private GameObject[] destroy; //gameobject yang akan di destroy ketika mati
    [SerializeField] private float destroyTime; //waktu gameobject mati

    [Space(10), Header("Drop Coin")]
    [SerializeField] private int totalCoin;
    [SerializeField] private GameObject coin;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = transform.Find("Sprite").GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
    }

    public void Damaged(int damage) {
        if (currentHp > 0)
        {
            currentHp -= damage;
            anim.SetTrigger("damaged");
            if (currentHp <= 0)
            {
                StartCoroutine(Dead());
            }
        }
    }

    public int GetCurrentHp() {
        return currentHp;
    }

    IEnumerator Dead()
    {
        rigid.velocity = Vector2.zero;

        if (totalCoin > 0)
            SpawnCoin();

        anim.SetTrigger("dead");

        foreach (GameObject g in destroy)
        {
            Destroy(g);
        }
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    public void SpawnCoin()
    {
        for (int i = 0; i < totalCoin; i++)
        {
            Instantiate(coin, transform.position, Quaternion.identity, null);
        }
    }
}
