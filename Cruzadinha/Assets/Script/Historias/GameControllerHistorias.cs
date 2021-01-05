using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerHistorias : MonoBehaviour
{

    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //animator.SetInteger("cena", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void proximaCena(string cena, int valor)
    {
        animator.SetInteger(cena, valor);
    }
}
