using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool isTrigger; //dipanggil dari trigger switch
    private bool triggered; //juka sudah tertrigger
    [SerializeField] private GameObject triggerActive;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!triggered && isTrigger)
            Triggered();
    }

    private void Triggered() {
        triggerActive.SetActive(true);
    }
}
