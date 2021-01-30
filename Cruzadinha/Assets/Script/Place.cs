using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
   public string letraPace;
   public bool _preenchido;

    private void OnTriggerEnter2D(Collider2D collision2d)
    {
       switch (collision2d.gameObject.tag)
        {
            case "Letras":
                _preenchido =  true;
                break;
            
        }
    }

}
