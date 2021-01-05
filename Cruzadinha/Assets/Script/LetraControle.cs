using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLetras1 : MonoBehaviour
{
    public AudioClip fxLetra;
    [SerializeField]
    private IEnumerator coroutine;
    public GameControllerBase gameController;
    private float deltaX, deltaY;
    public string tipoDinamico;

    public bool locked;

    public Vector3 tamanhoOriginal;
    //efeito quando arrasta pega aumenta.
    float x;
    float y;
    //float z;
    float xN;
    float yN;

    public GameObject colisor;
    public LinhaCOntrole linhaCOntrole;
    void Start()
    {
        tamanhoOriginal = GetComponent<Transform>().localScale;
        locked = false;
    }

    void Update()
    {
        //if (Input.touchCount > 0 && !locked) //SEM SIMULADOR
        if (!locked)
        {
        //    return;
        //Debug.Log("Opaaa");
            /*
            if (transform.position.x != initialPosition.x && initialPosition.y != transform.position.y)
            {
                transform.localScale = new Vector3(xN, yN);
                this.GetComponent<SpriteRenderer>().sortingOrder = 11;
            } else
            {
                this.GetComponent<Renderer>().sortingOrder = 10;
                transform.localScale = new Vector2(x, y);
            }  */
            //Touch touch = Input.GetTouch(0);//simulatess();//Input.GetTouch(0); SEM SIMULADOR
            Touch touch = simulatess();//Input.GetTouch(0); SEM SIMULADOR

            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    //GetComponent<Transform>().localScale = tamanhoOriginal + new Vector3(0.2f,0.2f,0);
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        deltaX = touchPos.x - transform.position.x;
                        deltaY = touchPos.y - transform.position.y;
                    }
                    break;

                case TouchPhase.Moved:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        //transform.position = new Vector2(touchPos.x - deltaX, touchPos.y - deltaY);
                    } 
                    break;

                case TouchPhase.Ended:
                    GetComponent<Transform>().localScale = tamanhoOriginal;
                    break;

            }
        } else //adicionado para sempre mater a posição inicial atualizada. 
        {
            //initialPosition = transform.position;
        }
    }
    private Touch simulatess()
    {
        Touch touch = new Touch();
        if (Input.GetMouseButtonDown(0))
        {
            touch = new Touch();
            touch.phase = TouchPhase.Began;
            touch.position = Input.mousePosition;
        } else if (Input.GetMouseButton(0))
        {
            touch = new Touch();
            touch.phase = TouchPhase.Moved;
            touch.position = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(0))
        {
            touch = new Touch();
            touch.phase = TouchPhase.Ended;
            touch.position = Input.mousePosition;
        } else {
            touch = new Touch();
            touch.phase = TouchPhase.Canceled;
        }
        return touch;
    }
    void OnTriggerEnter2D(Collider2D collision2d) {
        //Debug.Log(collision2d.gameObject.tag);
        switch (collision2d.gameObject.tag)
        {
            case "Place":
                GetComponent<Transform>().localScale = tamanhoOriginal + new Vector3(0.2f,0.2f,0);
                linhaCOntrole = collision2d.gameObject.GetComponent<LinhaCOntrole>();
                string letra = this.gameObject.GetComponent<Place>().letraPace.ToUpper();
                print(letra);
                GameObject letra1 =  Instantiate (GameObject.Find(letra));
                //letra1.gameObject.transform.localPosition = pp1.transform.position;
                //letra1.gameObject.transform.localScale =  pp1.transform.localScale;
                if(linhaCOntrole == null){
                    linhaCOntrole = collision2d.gameObject.GetComponent<LinhaCOntrole>();
                    linhaCOntrole.letrasFormarPalavra.Add(letra1);
                } else {
                    linhaCOntrole.letrasFormarPalavra.Add(letra1);
                }
                createPalavraScree();
                break;
            default:
            {
                break;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D collision2d) {
        switch (collision2d.gameObject.tag)
        {
            case "Place":
                GetComponent<Transform>().localScale = tamanhoOriginal;
                break;
            default:
            {
                break;
            }
            
        }
    }

    public void createPalavraScree(){

        switch (linhaCOntrole.letrasFormarPalavra.Count)
        {
            case 1:
                linhaCOntrole.letrasFormarPalavra[0].transform.position = GameObject.Find("Letra4").gameObject.transform.position;
                linhaCOntrole.letrasFormarPalavra[0].transform.position = GameObject.Find("Letra4").gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                break;
            case 2:
                linhaCOntrole.letrasFormarPalavra[0].transform.position = GameObject.Find("Letra4").gameObject.transform.position;
                linhaCOntrole.letrasFormarPalavra[0].transform.localScale = GameObject.Find("Letra4").gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                linhaCOntrole.letrasFormarPalavra[1].transform.position = GameObject.Find("Letra5").gameObject.transform.position;
                linhaCOntrole.letrasFormarPalavra[1].transform.localScale = GameObject.Find("Letra5").gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                break;
            default:
                break;
        }
        
    }
}
