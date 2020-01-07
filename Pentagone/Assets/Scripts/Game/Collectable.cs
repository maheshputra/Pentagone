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
    [SerializeField] private bool moveTowards; //jika sudah selesai menjalakan coroutine
    private Rigidbody2D rigid;
    private Collider2D coll;
    private bool canBeCollected; //jika bisa di collect
    [SerializeField] private bool isGrounded; //jika terkena ground

    [SerializeField] private bool isPlaced; //jika tidak drop dari monster

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        isGrounded = false;

        if (!isPlaced)
        {
            Jump();
            canBeCollected = false;
            moveTowards = false;
            StartCoroutine(Wait());
        }
        else
        {
            canBeCollected = true;
            rigid.bodyType = RigidbodyType2D.Static;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterSkills.instance.magneticSkill && moveTowards)
        {
            transform.position = Vector3.MoveTowards(transform.position , player.position + playerOffset, speed * Time.deltaTime);
            speed += Time.deltaTime * speedMultiplier;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canBeCollected)
        {
            PlayerStatus.instance.Collect();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isGrounded)
        {
            isGrounded = true;
            coll.isTrigger = true;
            rigid.velocity = Vector2.zero;
            rigid.bodyType = RigidbodyType2D.Static;
            moveTowards = true;
        }
    }

    private void Jump() {
        float rand = Random.Range(-2, 2);
        float rand2 = Random.Range(3, 5);

        rigid.velocity = new Vector2(rand, rand2);
    }

    /// <summary>
    /// Function untuk membuat delay antara collectable muncul dan menuju ke player jika player sudah memiliki autocollect skill
    /// </summary>
    /// <returns></returns>
    IEnumerator Wait() {
        yield return new WaitForSeconds(0.15f);
        canBeCollected = true;
        if (CharacterSkills.instance.magneticSkill)
        {
            yield return new WaitForSeconds(waitTime);
            coll.isTrigger = true;
            rigid.bodyType = RigidbodyType2D.Static;
            moveTowards = true;
        }
    }
}
