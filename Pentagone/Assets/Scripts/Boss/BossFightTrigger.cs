using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayMaker;

public class BossFightTrigger : MonoBehaviour
{
    [SerializeField] private PlayMakerFSM bossPatternFSM;

    [SerializeField] private bool right; //posisi player di kanan

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (right)
            {
                if (collision.transform.position.x > transform.position.x)
                    bossPatternFSM.FsmVariables.FindFsmBool("startFight").Value = true;
            }
            else
            {
                if (collision.transform.position.x < transform.position.x)
                    bossPatternFSM.FsmVariables.FindFsmBool("startFight").Value = true;
            }
        }
    }
}
