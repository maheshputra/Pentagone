using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayMaker;

public class Boss1TopMiddleAttack : MonoBehaviour
{
    [SerializeField] private PlayMakerFSM attackTopMiddleFSM;
    [SerializeField] private Transform projectilePosParent;
    [SerializeField] private List<Transform> projectilePos = new List<Transform>();
    [SerializeField] private GameObject projectilePrefab;
    private int[] pattern1 = new int[9] {0,1,2,5,6,7,10,11,12};
    private int[] pattern2 = new int[6] {1,3,5,7,9,11 };
    private int[] pattern3 = new int[7] {0,2,4,6,8,10,12 };
    private int[] pattern4 = new int[7] { 0, 1, 4, 5, 8, 9, 12 };
    private int[] pattern5 = new int[6] { 2, 3, 6, 7, 10, 11 };
    private int[] pattern6 = new int[9] { 0, 2, 3, 5, 6, 8, 9, 11, 12 };
    private int[] pattern7 = new int[9] { 0, 1, 3, 4, 6, 7, 9, 10, 12 };
    private bool isAttacking;
    private int previousPattern;
    private int rand;

    private void Start()
    {
        for (int i = 0; i < projectilePosParent.childCount; i++)
        {
            projectilePos.Add(projectilePosParent.GetChild(i));
        }
        previousPattern = 0;
    }

    private void Update()
    {
        if (attackTopMiddleFSM.FsmVariables.FindFsmBool("attack").Value == true && !isAttacking)
            Attack();
    }

    private void Attack() {
        isAttacking = true;

        while (rand == previousPattern)
        {
            rand = Random.Range(1, 8);
        }
        previousPattern = rand;

        if (rand == 1)
            LaunchAttack(pattern1);
        else if (rand == 2)
            LaunchAttack(pattern2);
        else if (rand == 3)
            LaunchAttack(pattern3);
        else if (rand == 4)
            LaunchAttack(pattern4);
        else if (rand == 5)
            LaunchAttack(pattern5);
        else if (rand == 6)
            LaunchAttack(pattern6);
        else if (rand == 7)
            LaunchAttack(pattern7);
    }

    void LaunchAttack(int[] posList) {
        for (int i = 0; i < projectilePos.Count; i++)
        {
            for (int j = 0; j < posList.Length; j++)
            {
                if (i == posList[j])
                {
                    Instantiate(projectilePrefab, projectilePos[posList[j]].position, Quaternion.identity, null);
                    break;
                }
            }
        }
        attackTopMiddleFSM.FsmVariables.FindFsmBool("attack").Value = false;
        isAttacking = false;
    }
}
