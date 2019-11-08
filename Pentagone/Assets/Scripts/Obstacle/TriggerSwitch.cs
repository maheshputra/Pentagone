using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSwitch : MonoBehaviour
{
    public enum TriggerFor { 
        MovingPlatform, Torch, etc
    }

    /// <summary>
    /// Jika moving platform, gameobject harus mempunya moving platform script
    /// </summary>
    [SerializeField] private List<TriggerFor> triggerFor = new List<TriggerFor>();
    [SerializeField] private List<GameObject>  target; //target triggernya
    [SerializeField] private Sprite triggeredSprite;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip triggerClip;
    [SerializeField] private bool triggered; //jika sudah tretrigger

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            if (!triggered)
            {
                for (int i = 0; i < triggerFor.Count; i++)
                {
                    switch (triggerFor[i])
                    {
                        case TriggerFor.MovingPlatform:
                            target[i].GetComponent<MovingPlatform>().isTrigger = true;
                            break;
                        case TriggerFor.Torch:
                            target[i].GetComponent<Torch>().isTrigger = true;
                            break;
                    }
                }
                Debug.Log("Triggered");
                triggered = true;
                audioSource.PlayOneShot(triggerClip);
                gameObject.GetComponent<SpriteRenderer>().sprite = triggeredSprite;
            }
        }
    }
}
