using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private Transform player; //target 
    [SerializeField] private float speed; //units moved per second
    [SerializeField] private float speedMultiplier; //speed tambah cepat perwaktu
    [SerializeField] private float waitTime; //waktu ketika baru drop
    [SerializeField] private Vector3 playerOffset;
    private bool moveTowards; //jika sudah selesai menjalakan coroutine
    private Rigidbody2D rigid;
    private Collider2D coll;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveTowards = false;
        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStatus.instance.autoCollect && moveTowards)
        {
            transform.position = Vector3.MoveTowards(transform.position , player.position + playerOffset, speed * Time.deltaTime);
            speed += Time.deltaTime * speedMultiplier;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStatus.instance.Collect();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Function untuk membuat delay antara collectable muncul dan menuju ke player jika player sudah memiliki autocollect skill
    /// </summary>
    /// <returns></returns>
    IEnumerator Wait() {
        yield return new WaitForSeconds(waitTime);
        coll.isTrigger = true;
        rigid.bodyType = RigidbodyType2D.Static;
        moveTowards = true;
    }
}
