using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRewind : MonoBehaviour
{
    public static CharacterRewind instance;//singleton

    public bool canRewind;//untuk activate
    public bool isRewinding;//public untuk dipanggil di character movement

    private List<RewindData> rewindData = new List<RewindData>();

    private float recordTime; //waktu record movement playernya

    [SerializeField] private float maxRewindTime;
    [SerializeField] private float rewindTime;
    [SerializeField] private bool holdRewindTime;//delay untuk rewindtime nya
    [SerializeField] private bool increaseRewindTime;//sedang ditambah rewind timenya

    [SerializeField] private Transform character;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Image rewindBar;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        recordTime = 5f;
        maxRewindTime = .5f;
        rewindTime = maxRewindTime;
    }

    void Update()
    {
        if(canRewind && !GameStatus.instance.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.R))
                if(rewindTime>0)
                    StartRewind();
            if (Input.GetKeyUp(KeyCode.R) && (!increaseRewindTime || isRewinding))
                StopRewind();

            if (!holdRewindTime)
            {
                if (rewindTime == 0 && !increaseRewindTime)
                {
                    StopRewind();
                }
                else if (!isRewinding && rewindTime < maxRewindTime && increaseRewindTime)
                {
                    rewindTime += .1f * Time.deltaTime;
                    rewindBar.fillAmount = rewindTime / maxRewindTime;
                }
            }

            if (rewindTime >= maxRewindTime && increaseRewindTime)
            {
                increaseRewindTime = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (canRewind && !GameStatus.instance.isPaused)
        {
            if (isRewinding)
                Rewind();
            else
                Record();
        }
    }

    /// <summary>
    /// Rewind
    /// </summary>
    private void Rewind() {
        if (rewindData.Count > 0 && rewindTime>0)
        {
            rewindTime -= Time.deltaTime;
            rewindBar.fillAmount = rewindTime / maxRewindTime;
            RewindData rewindDatum = rewindData[0];
            character.position = rewindDatum.position;
            character.rotation = rewindDatum.rotation;
            spriteRenderer.transform.localScale = rewindDatum.scale;
            spriteRenderer.sprite = rewindDatum.sprite;
            rewindData.RemoveAt(0);
            CharacterMovement.instance.SpawnEcho();
        }
        else
        {
            StopRewind();
        }
    }

    /// <summary>
    /// Merecord hal yang terjadi selama recordtime
    /// </summary>
    private void Record() {
        if (rewindData.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            rewindData.RemoveAt(rewindData.Count - 1);
        }
        rewindData.Insert(0, new RewindData(spriteRenderer.sprite, character.rotation, character.position, spriteRenderer.transform.localScale));

    } 

    /// <summary>
    /// Memulai rewind
    /// </summary>
    public void StartRewind()
    {
        rigid.isKinematic = true;
        rigid.velocity = Vector2.zero;
        spriteRenderer.GetComponent<Animator>().enabled = false;
        isRewinding = true;
        holdRewindTime = false;
    }

    /// <summary>
    /// Stop rewind
    /// </summary>
    public void StopRewind()
    {
        holdRewindTime = true;
        rigid.isKinematic = false;
        rigid.velocity = Vector2.zero;
        spriteRenderer.GetComponent<Animator>().enabled = true;
        isRewinding = false;
        StopCoroutine(IncreaseRewindTime());
        StartCoroutine(IncreaseRewindTime());
    }

    IEnumerator IncreaseRewindTime() {
        yield return new WaitForSeconds(2f);
        holdRewindTime = false;
        increaseRewindTime = true;
    }
}
