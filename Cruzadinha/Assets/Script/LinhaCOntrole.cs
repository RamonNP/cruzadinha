using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinhaCOntrole : MonoBehaviour
{

    [SerializeField]
    public Vector2 initialPosition;
    private IEnumerator coroutine;

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 2;


    //efeito quando arrasta pega aumenta.
    float x;
    float y;
    //float z;
    float xN;
    float yN;

    public Vector3 posicaoIniciao;
    public Vector3 posicaoAtual;
    public Vector3 teste1;

    public int cordecadasLetras = 0;
    public int qtdLetrasEscolhidas = 1;

    //referente a palavra escritas depois de escolher as letras
    public int qtdLetrasConectadas = 1;
    public string palavraMontada;

    public List<string> letrasNaoRepetir = new List<string>();
    public List<GameObject> letrasPalavraDestroir = new List<GameObject>();
    public List<GameObject> letrasFormarPalavra = new List<GameObject>();
    CruzadinhaControleV2 cruzadinhaControleV2;
    //public GameObject colisor;
    void Start()
    {
        //colisor = GameObject.Find("colisorLetras");
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = lengthOfLineRenderer;
        lineRenderer.sortingLayerName = "Controle";
        lineRenderer.sortingOrder = 4;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        coroutine = waith();
        StartCoroutine("waith");
        x = transform.localScale.x;
        y = transform.localScale.y;
        //z = transform.localScale.z;

        xN = x * 2f;
        yN = y * 2f;

        cruzadinhaControleV2 = FindObjectOfType(typeof(CruzadinhaControleV2)) as CruzadinhaControleV2;
        
    }
    //esta se perdendo na hora de guarda posição inicial, com esse metodo aguarda definir para depois guardar.
    IEnumerator waith()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        initialPosition = transform.position;
    }
    IEnumerator waith2S()
    {
        yield return new WaitForSecondsRealtime(2f);
    }

    void Update()
    {
        //if (Input.touchCount > 0 && !locked) //SEM SIMULADOR
        //if ()
        //{
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
            } */
            //Touch touch = Input.GetTouch(0);//simulatess();//Input.GetTouch(0); SEM SIMULADOR
            Touch touch = simulatess();//Input.GetTouch(0); SEM SIMULADOR

            //Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    foreach (GameObject item in cruzadinhaControleV2.LetrasControleTransforme)
                    {
                        lineRenderer.enabled = true;
                        //caso a posição inicial for alguma letra, encaixarna letra. 
                        posicaoIniciao = GetCurrentMousePosition(touch.position).GetValueOrDefault();
                        if ((Mathf.Abs(item.transform.position.x - posicaoIniciao.x) <= 0.3f &&
                        Mathf.Abs(item.transform.position.y - posicaoIniciao.y) <= 0.3f))
                        {
                            cordecadasLetras = 0;
                            qtdLetrasEscolhidas = 1;
                            qtdLetrasConectadas = 1;
                            //Coloca a letra escolhida na palavra que estamos montando
                            montarPalavra(qtdLetrasConectadas, item);
                            lineRenderer.SetPosition(cordecadasLetras, item.transform.position);
                            lineRenderer.SetVertexCount(qtdLetrasEscolhidas);

                            cordecadasLetras++;
                            qtdLetrasEscolhidas++;
                            //preenche a primeira letra com a primeira escolha, assim a proxima letra não pode ser a mesma.
                            letrasNaoRepetir.Add(item.GetComponent<Place>().letraPace+item.transform.position.x+item.transform.position.y);
                            acertouPalavra(item.GetComponent<Place>().letraPace);
                            
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    posicaoAtual = GetCurrentMousePosition(touch.position).GetValueOrDefault();
                    lineRenderer.SetVertexCount(qtdLetrasEscolhidas);
                    lineRenderer.SetPosition(cordecadasLetras, new Vector3(posicaoAtual.x,posicaoAtual.y,posicaoAtual.z));
                    foreach (GameObject item in cruzadinhaControleV2.LetrasControleTransforme)
                    {
                        //caso a posição inicial for alguma letra, encaixarna letra. 
                        if ((Mathf.Abs(item.transform.position.x - posicaoAtual.x) <= 0.3f &&
                        Mathf.Abs(item.transform.position.y - posicaoAtual.y) <= 0.3f))
                        {
                            if(!letrasNaoRepetir.Contains( item.GetComponent<Place>().letraPace+item.transform.position.x+item.transform.position.y)) {
                                var t = Time.time;
                                this.transform.position = posicaoAtual;
                                lineRenderer.SetVertexCount(qtdLetrasEscolhidas);
                                lineRenderer.SetPosition(cordecadasLetras, item.transform.position);
                                cordecadasLetras++;
                                qtdLetrasEscolhidas++;
                                qtdLetrasConectadas++;
                                letrasNaoRepetir.Add(item.GetComponent<Place>().letraPace+item.transform.position.x+item.transform.position.y);
                                //Coloca a letra escolhida na palavra que estamos montando
                                montarPalavra(qtdLetrasConectadas, item);
                                acertouPalavra(item.GetComponent<Place>().letraPace);
                            }
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    lineRenderer.enabled = false;
                    this.transform.position = new Vector3(0,0,0);
                    var releasePosition = GetCurrentMousePosition(touch.position).GetValueOrDefault();
                    var direction = (Vector3)releasePosition - posicaoIniciao;

                    //apagar o rastro do controle
                    cordecadasLetras = 0;
                    qtdLetrasEscolhidas = 1;
                    //Limpar a palavra formada
                    limpaPalavraMontada();
                    letrasNaoRepetir.Clear();
                    //limpar palavra montada
                    palavraMontada = null;

                    break;

            }
       // } else //adicionado para sempre mater a posição inicial atualizada. 
       // {
       //     initialPosition = transform.position;
       // }
    }
    private void acertouPalavra(string letra) {
        palavraMontada  = palavraMontada + letra;
        List<GameObject> palavraGameObjects = null;

        //pegar item 
        if(cruzadinhaControleV2.palavrasCruzadinha.TryGetValue(palavraMontada, out palavraGameObjects)) {
            
            //print(palavraGameObject);
            //percorrer places para instanciar novas letras, nas mesma posiçoes
            foreach (var item in palavraGameObjects)
            {
                print(item.GetComponent<Place>().letraPace);
                GameObject letra1 =  Instantiate (GameObject.Find(item.GetComponent<Place>().letraPace));
                letra1.gameObject.transform.localPosition = item.transform.position;
                letra1.gameObject.transform.localScale =  item.transform.localScale;
            }
            //apaga as linhas por que acertou a palavras
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;

            //remover palavra da lista
            cruzadinhaControleV2.palavrasCruzadinha.Remove(palavraMontada);
            
            //limpar palavra montada
            palavraMontada = null;
        }


        /*
        foreach (var pa in cruzadinhaControleV2.palavrasCruzadinha)
        {
            //(List<Dictionary<string, GameObject>>) pa.
        }
        foreach (Objeto item in cruzadinhaControleV2.objetos)
        {
            if(palavraMontada == item.nome) {
                print("acertouPlavra");
                LineRenderer lineRenderer = GetComponent<LineRenderer>();
                lineRenderer.enabled = false;
                
            }
        }*/

    }
    public void limpaPalavraMontada() {
        foreach (var item in letrasPalavraDestroir)
        {
            Destroy(item);
        }
    }
    private Vector3? GetCurrentMousePosition(Vector3 pos)
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.forward, Vector3.zero);

        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);
            
        }

        return null;
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
    public void montarPalavra(int posicao, GameObject letra) {
        switch (posicao)
        {
            case 1:
                    GameObject letra1 =  Instantiate (GameObject.Find(letra.GetComponent<Place>().letraPace));
                    letra1.gameObject.transform.localPosition = GameObject.Find("Letra1").transform.position;
                    letra1.gameObject.transform.localScale =  GameObject.Find("Letra1").transform.localScale;
                    letrasPalavraDestroir.Add(letra1);
                break;
            case 2:
                    GameObject letra2 =  Instantiate (GameObject.Find(letra.GetComponent<Place>().letraPace));
                    letra2.gameObject.transform.localPosition = GameObject.Find("Letra2").transform.position;
                    letra2.gameObject.transform.localScale =  GameObject.Find("Letra2").transform.localScale;
                    letrasPalavraDestroir.Add(letra2);
                break;
            case 3:
                    GameObject letra3 =  Instantiate (GameObject.Find(letra.GetComponent<Place>().letraPace));
                    letra3.gameObject.transform.localPosition = GameObject.Find("Letra3").transform.position;
                    letra3.gameObject.transform.localScale =  GameObject.Find("Letra3").transform.localScale;
                    letrasPalavraDestroir.Add(letra3);
                break;
            case 4:
                    GameObject letra4 =  Instantiate (GameObject.Find(letra.GetComponent<Place>().letraPace));
                    letra4.gameObject.transform.localPosition = GameObject.Find("Letra4").transform.position;
                    letra4.gameObject.transform.localScale =  GameObject.Find("Letra4").transform.localScale;
                    letrasPalavraDestroir.Add(letra4);
                break;
            case 5:
                    GameObject letra5 =  Instantiate (GameObject.Find(letra.GetComponent<Place>().letraPace));
                    letra5.gameObject.transform.localPosition = GameObject.Find("Letra5").transform.position;
                    letra5.gameObject.transform.localScale =  GameObject.Find("Letra5").transform.localScale;
                    letrasPalavraDestroir.Add(letra5);
                break;
            default:
                break;
        }
        
    }
    void OnTriggerEnter2D(Collider2D collision2d) {
        Transform place = collision2d.gameObject.transform;
        switch (collision2d.gameObject.tag)
        {
            case "Place":

                if ((Mathf.Abs(transform.position.x - place.position.x) <= 1.0f &&
                       Mathf.Abs(transform.position.y - place.position.y) <= 1.0f))
                    {
//                        print("OPAAAAAAA");
//                        proximaLetra = true;
                        //print("ENCAIXANDOOOOO");

                        //transform.position = new Vector3(place.position.x, place.position.y, 0);

                        //transform.localScale = new Vector3(x, y, 0);

                    }
                //Debug.Log(collision2d.gameObject.GetComponent<Place>().letraPace);
                //if(collision2d.gameObject.GetComponent<Place>().letraPace == letraMove ){
                    //letraPlace = collision2d.gameObject;
                //}
                //collision2d.gameObject.SendMessage("removeInteracao", SendMessageOptions.DontRequireReceiver);
                break;
            default:
            {
                break;
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D collision2d) {
        //Debug.Log(collision2d.gameObject.tag);
         switch (collision2d.gameObject.tag)
        {
            case "Place":
                //if(collision2d.gameObject.GetComponent<Place>().letraPace == letraMove ){
                //    letraPlace = null;
               // }
                //collision2d.gameObject.SendMessage("removeInteracao", SendMessageOptions.DontRequireReceiver);
                break;
            default:
            {
                break;
            }
            
        }
    }
}
