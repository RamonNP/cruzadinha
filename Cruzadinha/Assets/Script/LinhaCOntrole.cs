using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LinhaCOntrole : MonoBehaviour
{

    private CardCompletouController cardCompletouController;
    private AudioControllerV2 audioController;
    [SerializeField]
    public Vector2 initialPosition;
    private IEnumerator coroutine;

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    public int lengthOfLineRenderer = 2;

    Vector3 posicaoIniciao;
    Vector3 posicaoAtual;

    public int cordecadasLetras = 0;
    public int qtdLetrasEscolhidas = 1;

    //referente a palavra escritas depois de escolher as letras
    public int qtdLetrasConectadas = 1;
    public string palavraMontada;

    public List<string> letrasNaoRepetir = new List<string>();
    public List<GameObject> letrasPalavraDestroir = new List<GameObject>();
    public List<GameObject> letrasFormarPalavra = new List<GameObject>();
    CruzadinhaControleV2 cruzadinhaControleV2;

    //SHAKE CAMERA
    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    public float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    public float ShakeAmplitude = 3.2f;         // Cinemachine Noise Profile Parameter
    public float ShakeFrequency = 3.0f;         // Cinemachine Noise Profile Parameter

    public float ShakeElapsedTime = 0f;

    private GameObject cardCompletou;
    private GameObject bom;
    private GameObject otimo;
    private GameObject bravo;
    private GameObject perfeito;
    private GameObject parabens;

    private bool ternimouFase = false;
    public int _letraDica = 0;
    public int _qtdMoedasDicas = 70;
    
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

        cruzadinhaControleV2 = FindObjectOfType(typeof(CruzadinhaControleV2)) as CruzadinhaControleV2;
        cardCompletouController = FindObjectOfType(typeof(CardCompletouController)) as CardCompletouController;

        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null){
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>(); 
        }

        cardCompletou = GameObject.Find("cardCompletou");
        bom = GameObject.Find("Bom");
        otimo = GameObject.Find("Otimo");
        bravo = GameObject.Find("Bravo");
        perfeito = GameObject.Find("perfeito");
        parabens = GameObject.Find("parabens");
        bom.SetActive(false);
        otimo.SetActive(false);
        bravo.SetActive(false);
        perfeito.SetActive(false);
        parabens.SetActive(false);
        cardCompletou.SetActive(false);
        ternimouFase = false;
        audioController = FindObjectOfType(typeof(AudioControllerV2)) as AudioControllerV2;
        
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
        if(!ternimouFase){
            Touch touch = simulatess();
            if (Application.platform == RuntimePlatform.Android) {
                touch = Input.GetTouch(0);//simulatess();//Input.GetTouch(0); SEM SIMULADOR
            } else if (Application.platform == RuntimePlatform.OSXEditor) {
                Input.GetTouch(0); 
            }
            //Touch touch = Input.GetTouch(0);//simulatess();//Input.GetTouch(0); SEM SIMULADOR

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
                            montarPalavraAcertou(item.GetComponent<Place>().letraPace);
                            
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
                                montarPalavraAcertou(item.GetComponent<Place>().letraPace);
                            }
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    // para corrigir bug de refresh no teclado letras
                    if(palavraMontada != null) {
                        acertouPalavra();
                    }
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
        }

    }

    public void dicasLiberarPalavra() {
        TextoMoedasAdd tma = FindObjectOfType(typeof(TextoMoedasAdd)) as TextoMoedasAdd;
        if(tma.qtdMoedaMax < _qtdMoedasDicas) {
            audioController.playFx(audioController.fxError, 1);
            return;
        } 
        List<GameObject> palavra = null;
        int letra = 0;
        //percorre para encontrar a primeira palavra não preenchida
        foreach (List<GameObject> item in cruzadinhaControleV2.palavrasCruzadinha.Values)  {
           if(letra == 0){
               palavra = item;
           }
           letra ++;
        }
       letra = 0;
       //percorre para encontrar a primeira letra que não foi preenchida e preencher 
        foreach (var item in palavra)
        {
            if(!item.GetComponent<Place>()._preenchido){
                GameObject efeito =  Instantiate (GameObject.Find("EfeitoAmarelo"));
                efeito.gameObject.transform.localPosition = item.transform.position;
                //yield return new WaitForSeconds(0.4f);
                CameraShake._instance.ShakeCamera(5f, 0.01f);
                //toca o som de quando Aparece as Letras na cruzadinha
                audioController.playFx(audioController.fxVisualizaLetra2, 1);
                GameObject letra1 =  Instantiate (GameObject.Find(item.GetComponent<Place>().letraPace));
                letra1.gameObject.transform.localPosition = item.transform.position;
                letra1.gameObject.transform.localScale =  item.transform.localScale;
                letra1.GetComponent<SpriteRenderer>().sortingOrder = 1;
                //yield return new WaitForSeconds(0.1f);
                Destroy(efeito);
                item.GetComponent<Place>()._preenchido = true;
                tma.qtdMoedaMax = tma.qtdMoedaMax - _qtdMoedasDicas;
                return;
            } else {
                _letraDica++;
            }
            letra ++;
        }
        _letraDica=0;
    }

    public void acertouPalavra() {

        List<GameObject> palavraGameObjects = null;
         //pegar item 
        if(cruzadinhaControleV2.palavrasCruzadinha.TryGetValue(palavraMontada, out palavraGameObjects)) {
            
            //print("1");
            //chama animação de mostrar palavras prontras
            coroutine = animacaoAcertouENUM(palavraGameObjects);
            StartCoroutine(coroutine);

            //print("2");
            //apaga as linhas por que acertou a palavras
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;

            //print(palavraMontada);

            //remover palavra da lista
            cruzadinhaControleV2.palavrasCruzadinha.Remove(palavraMontada);
            //reseta a contagem das pavras para liberar as dicas    
            _letraDica = 0;
            
        }
    }
    private void montarPalavraAcertou(string letra) {
        palavraMontada  = palavraMontada + letra;
        
        switch (palavraMontada.Length)
        {
            case 1:
                audioController.playFx(audioController.fxCombo1, 1);
                break;
            case 2:
                audioController.playFx(audioController.fxCombo2, 1);
                break;
            case 3:
                audioController.playFx(audioController.fxCombo3, 1);
                break;
            case 4:
                audioController.playFx(audioController.fxCombo4, 1);
                break;
            case 5:
                audioController.playFx(audioController.fxCombo5, 1);
                break;
            case 6:
                audioController.playFx(audioController.fxCombo6, 1);
                break;
            case 7:
                audioController.playFx(audioController.fxCombo7, 1);
                break;
            default:
                break;
        }
             

    }

    IEnumerator animacaoAcertouENUM(List<GameObject> palavraGameObjects) {
        //toca o som de quando acerta a palavra
        audioController.playFx(audioController.fxAcertouPalavra, 1);

        //quando completa a palavra, apresenta bom ou otimo ou bravo ou parabens
        int rand = Random.Range(2,8);
        GameObject obj = bom;
        if(rand < 4 ){
            obj = bom;
        } else if (rand == 5){
            obj = otimo;
        } else if (rand == 6){
            obj = bravo;
        } else if (rand == 7){
            obj = perfeito;
        } else if (rand == 8){
            obj = parabens;
        }
        obj.SetActive(true);
         //percorrer places para instanciar novas letras, nas mesma posiçoes
        foreach (var item in palavraGameObjects)
        {
            if(!item.GetComponent<Place>()._preenchido){
                GameObject efeito =  Instantiate (GameObject.Find("EfeitoAmarelo"));
                efeito.gameObject.transform.localPosition = item.transform.position;
                yield return new WaitForSeconds(0.4f);
                CameraShake._instance.ShakeCamera(5f, 0.01f);
                //toca o som de quando Aparece as Letras na cruzadinha
                audioController.playFx(audioController.fxVisualizaLetra2, 1);
                GameObject letra1 =  Instantiate (GameObject.Find(item.GetComponent<Place>().letraPace));
                letra1.gameObject.transform.localPosition = item.transform.position;
                letra1.gameObject.transform.localScale =  item.transform.localScale;
                letra1.GetComponent<SpriteRenderer>().sortingOrder = 1;
                yield return new WaitForSeconds(0.1f);
                Destroy(efeito);
            }
        }

        //codigos para quando completar a fase
        if(cruzadinhaControleV2.palavrasCruzadinha.Count == 0){

            //explodindo os botoes quando termina a fase
            try
            {
                ternimouFase = true;
                CameraShake._instance.ShakeCamera(5f, 0.05f);
                GameObject go1 = GameObject.Find("letra1");
                GameObject efeito1 =  Instantiate (GameObject.Find("Explosao"));
                efeito1.gameObject.transform.position = go1.gameObject.transform.position;
                Destroy(go1);

                //desativar botão refresh, pois foi destruido os botoes no efeito
                GameObject rf = GameObject.Find("Refresh");
                GameObject efeitorf =  Instantiate (GameObject.Find("Explosao"));
                efeitorf.gameObject.transform.position = rf.gameObject.transform.position;
                Destroy(rf);

                GameObject go2 = GameObject.Find("letra2");
                GameObject efeito2 =  Instantiate (GameObject.Find("Explosao"));
                efeito2.gameObject.transform.position = go2.gameObject.transform.position;
                Destroy(go2);

                GameObject go3 = GameObject.Find("letra3");
                GameObject efeito3 =  Instantiate (GameObject.Find("Explosao"));
                efeito3.gameObject.transform.position = go3.gameObject.transform.position;
                Destroy(go3);

                GameObject go4 = GameObject.Find("letra4");
                GameObject efeito4 =  Instantiate (GameObject.Find("Explosao"));
                efeito4.gameObject.transform.position = go4.gameObject.transform.position;
                Destroy(go4);

                GameObject go5 = GameObject.Find("letra5");
                GameObject efeito5 =  Instantiate (GameObject.Find("Explosao"));
                efeito5.gameObject.transform.position = go5.gameObject.transform.position;
                Destroy(go5);

                GameObject go6 = GameObject.Find("letra6");
                GameObject efeito6 =  Instantiate (GameObject.Find("Explosao"));
                efeito6.gameObject.transform.position = go6.gameObject.transform.position;
                Destroy(go6);

                GameObject go7 = GameObject.Find("letra7");
                GameObject efeito7 =  Instantiate (GameObject.Find("Explosao"));
                efeito7.gameObject.transform.position = go7.gameObject.transform.position;
                Destroy(go7);
            }
            catch (System.Exception ex)
            {
                 // TODO
            }
            audioController.playFx(audioController.fxCompletouFase, 1);
            //abre o card completou
            cardCompletou.SetActive(true);
            //abre a proxima pedra para liberar o presente
            cardCompletouController.abrirProximaGema();

        }
        //palavraMontada = null;
        yield return new WaitForSeconds(1f);
        obj.SetActive(false);
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
                    letra1.gameObject.transform.localScale =  GameObject.Find("Letra1").transform.localScale * 0.5f;
                    letrasPalavraDestroir.Add(letra1);
                break;
            case 2:
                    GameObject letra2 =  Instantiate (GameObject.Find(letra.GetComponent<Place>().letraPace));
                    letra2.gameObject.transform.localPosition = GameObject.Find("Letra2").transform.position;
                    letra2.gameObject.transform.localScale =  GameObject.Find("Letra2").transform.localScale * 0.5f;
                    letrasPalavraDestroir.Add(letra2);
                break;
            case 3:
                    GameObject letra3 =  Instantiate (GameObject.Find(letra.GetComponent<Place>().letraPace));
                    letra3.gameObject.transform.localPosition = GameObject.Find("Letra3").transform.position;
                    letra3.gameObject.transform.localScale =  GameObject.Find("Letra3").transform.localScale * 0.5f;
                    letrasPalavraDestroir.Add(letra3);
                break;
            case 4:
                    GameObject letra4 =  Instantiate (GameObject.Find(letra.GetComponent<Place>().letraPace));
                    letra4.gameObject.transform.localPosition = GameObject.Find("Letra4").transform.position;
                    letra4.gameObject.transform.localScale =  GameObject.Find("Letra4").transform.localScale * 0.5f;
                    letrasPalavraDestroir.Add(letra4);
                break;
            case 5:
                    GameObject letra5 =  Instantiate (GameObject.Find(letra.GetComponent<Place>().letraPace));
                    letra5.gameObject.transform.localPosition = GameObject.Find("Letra5").transform.position;
                    letra5.gameObject.transform.localScale =  GameObject.Find("Letra5").transform.localScale * 0.5f;
                    letrasPalavraDestroir.Add(letra5);
                break;
            default:
                break;
        }
        
    }
    void OnTriggerEnter2D(Collider2D collision2d) {
        switch (collision2d.gameObject.tag)
        {
            case "Place":
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
                break;
            default:
            {
                break;
            }
            
        }
    }
    /*public void cameraShake() {
         // If the Cinemachine componet is not set, avoid update
        //if (VirtualCamera != null && virtualCameraNoise != null)
       // {
            // If Camera Shake effect is still playing
            if (ShakeElapsedTime > 0)
            {
                // Set Cinemachine Camera Noise parameters
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;

                // Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                virtualCameraNoise.m_AmplitudeGain = 0f;
                ShakeElapsedTime = 0f;
            }
        }
    } ;*/
}
