using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayMaker;

public class BossFightTrigger : MonoBehaviour
{
    [SerializeField] private PlayMakerFSM bossPatternFSM;

    [SerializeField] private bool right; //posisi player di kanan
    [SerializeField] private GameObject door;
    // Start is called before the first frame update
    void Start()
    {
        door.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("entering");
            if (right)
            {
                if (collision.transform.position.x > transform.position.x)
                {
                    Debug.Log("right");
                    bossPatternFSM.FsmVariables.FindFsmBool("startFight").Value = true;
                    door.SetActive(true);
                }
            }
            else
            {
                if (collision.transform.position.x < transform.position.x)
                {
                    Debug.Log("left");
                    bossPatternFSM.FsmVariables.FindFsmBool("startFight").Value = true;
                    door.SetActive(true);
                }
            }
        }
    }
}
