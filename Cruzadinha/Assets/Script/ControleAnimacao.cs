using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleAnimacao : MonoBehaviour
{
    private CruzadinhaControleV2 cruzadinhaControleV2;
    // Start is called before the first frame update
    void Start()
    {
        cruzadinhaControleV2 = FindObjectOfType(typeof(CruzadinhaControleV2)) as CruzadinhaControleV2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

public void embaralhar() {
    cruzadinhaControleV2.embaralhar();
}

    
}
