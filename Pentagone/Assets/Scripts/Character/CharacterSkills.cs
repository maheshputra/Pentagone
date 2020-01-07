using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSkills : MonoBehaviour
{
    public static CharacterSkills instance;
    public Color activateColor;
    public Color deactivateColor;
    public bool developerMode;

    [Space(10)]
    [Header("Skill List")]
    public bool holdJumpSkill; //untuk lompat tahan lebih tinggi
    public bool magneticSkill; //untuk membuat drop dari enemies dateng ke player
    public bool dashSkill; //dash

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void ActivateDashSkill(Image image) {
        if (dashSkill && developerMode)
        {
            dashSkill = false;
            image.color = deactivateColor;
        }
        else if (!dashSkill && PlayerStatus.instance.skillPoint > 0)
        {
            dashSkill = true;
            image.color = activateColor;
            PlayerStatus.instance.UsePoint();
        }
    }

    public void ActivateHoldJump(Image image) {
        if (holdJumpSkill && developerMode)
        {
            holdJumpSkill = false;
            image.color = deactivateColor;
        }
        else if(!holdJumpSkill && PlayerStatus.instance.skillPoint>0)
        {
            holdJumpSkill = true;
            image.color = activateColor;
            PlayerStatus.instance.UsePoint();
        }
    }

    public void ActivateRewind(Image image) {
        if (CharacterRewind.instance.canRewind && developerMode)
        {
            CharacterRewind.instance.canRewind = false;
            image.color = deactivateColor;
        }
        else if (!CharacterRewind.instance.canRewind && PlayerStatus.instance.skillPoint > 0)
        {
            CharacterRewind.instance.canRewind = true;
            image.color = activateColor;
            PlayerStatus.instance.UsePoint();
        }
    }

    public void ActivateDoubleJump(Image image)
    {
        if (CharacterMovement.instance.maxJumpCharge==2 && developerMode)
        {
            CharacterMovement.instance.maxJumpCharge = 1;
            image.color = deactivateColor;
        }
        else if(CharacterMovement.instance.maxJumpCharge ==1 && PlayerStatus.instance.skillPoint > 0)
        {
            CharacterMovement.instance.maxJumpCharge = 2;
            image.color = activateColor;
            PlayerStatus.instance.UsePoint();
        }
    }

    public void ActivateMagneticCollect(Image image) {
        if (magneticSkill && developerMode)
        {
            magneticSkill = false;
            image.color = deactivateColor;
        }
        else if (!magneticSkill && PlayerStatus.instance.skillPoint > 0)
        {
            magneticSkill = true;
            image.color = activateColor;
            PlayerStatus.instance.UsePoint();
        }
    }
}
