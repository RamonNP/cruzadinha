using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CruzadinhaControleV2 : MonoBehaviour
{
    public bool _completouBau = false;
    private AudioControllerV2 _audioControllerV2;
    private GameController gameController;
    public GameObject alfabeto;
    private LerXml xmlLerDados;
    public List<GameObject> LetrasControleTransforme = new List<GameObject>() ;
    public List<Objeto> objetos;
    public Dictionary<string,List<GameObject>> palavrasCruzadinha = new Dictionary<string,List<GameObject>>();


    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType(typeof(GameController)) as GameController;
        _audioControllerV2 = FindObjectOfType(typeof(AudioControllerV2)) as AudioControllerV2;
        //if(xmlLerDados !=null){
            //xmlLerDados.LoadDialogoData(gameController.idiomaFolder[gameController.idioma] + "/" + gameController.nomeArquivoXml); //ler o arquivo interação com itens;
        //}
        //Debug.Log("INICIO CARREGANDO");
        xmlLerDados = LerXml.getInstance();
        //PlayerPrefs.SetInt("faseAtual",0);
        
        if(PlayerPrefs.GetInt("faseAtual") == 0) {
            objetos = xmlLerDados.LoadDialogoData(_audioControllerV2.idioma+"/fase_1");
            PlayerPrefs.SetInt("faseAtual",1);
        } else {
            //caso ocorra erro por chegar ao final, inicia novamente
            try
            {
                string fase = "/fase_"+PlayerPrefs.GetInt("faseAtual").ToString();
                objetos = xmlLerDados.LoadDialogoData(_audioControllerV2.idioma+fase);
                
            }
            catch (System.Exception)
            {
                objetos = xmlLerDados.LoadDialogoData(_audioControllerV2.idioma+"/fase_1");
                PlayerPrefs.SetInt("faseAtual",1);
                throw;
            }
        }
        preencherPlace();
    }

    // Update is called once per frame
    void Update()       
    {
        
    }
List<string> ListaLetrasControle = null;
    public void  preencherPlace() {
        gameController.pontos = 0;
        //variavel que guarda letras que sera apresentado na tela para usuario selecionar e montar palavras
        ListaLetrasControle = xmlLerDados.pularletrasControle;
        int y = 2;
        int x = -7;
        foreach (var item in objetos)
        {
            string palavra = item.nome;
            //calcula o destino do x do proximo quadrado, problema com numero quebrado, por isso metodo criado
            //float proximaCasa = float.Parse(item.pontos);
            float proximaCasa;
            float conv = convertStringParaFLoat(item.pontos);
            if(conv!=0) {
                proximaCasa = conv;
            } else {
                float.TryParse(item.pontos, out proximaCasa);
            }

            List<GameObject> palavraGameObject = new List<GameObject>();

            foreach (var letra in palavra)
            {
                float inicio;
                float conv2 = convertStringParaFLoat(item.inicio);
                if(conv2!=0) {
                    inicio = conv2;
                } else {
                    float.TryParse(item.inicio, out inicio);
                }



                //casas para pular linha, pois a coluna horizontal vai preencher,Definido no XML.
                if(!item.pular.Contains((proximaCasa.ToString()))) {
                    string  letraString = letra.ToString();
                //caso a palavra seja vertical tratamento especial para pular as casas
                    //print( item.inicio + "-"+  proximaCasa);
                    if(item.vertical == "S"){
                        GameObject place = Instantiate (GameObject.Find("O1l1"), new Vector3(inicio, proximaCasa, 0), this.transform.localRotation);
                        place.gameObject.GetComponent<Place>().letraPace = letraString.ToUpper();
                    //caso a palavra seja horizontal, apenas adiciona no lugar certo com a letra no place.
                        palavraGameObject.Add(place);
                    } else {
                        GameObject place = Instantiate (GameObject.Find("O1l1"), new Vector3(proximaCasa, inicio, 0), this.transform.localRotation);
                        place.gameObject.GetComponent<Place>().letraPace = letraString.ToUpper();
                        palavraGameObject.Add(place);
                    }
                    //adiciona pontos para ser comparado quando terminar a cruzadinha, pontosa x acertos
                    gameController.pontos++;
                    if(x == -7) {
                        x = -6;
                    } else if(x == -6){
                        x = -7;
                    } else if(x == -5){
                        x = 5;
                    } else if(x == 5){
                        x = -5;
                    } else if(x == 6){
                        x = 7;
                    } else if(x == 7){
                        x = 6;
                    }
                    y --;
                    if(y < -3) {
                        y = 2;
                        if(x == -7 || x == -6) {
                            x = -5;
                        } else  if(x == -5 || x == 4) {
                            x = 6;
                        }
                    }
                }
                 if(item.vertical == "S"){
                    proximaCasa = proximaCasa - 0.5f;
                 } else {
                    proximaCasa= proximaCasa + 0.5f;
                 }
            }
            palavrasCruzadinha.Add(palavra, palavraGameObject);
//            print(palavra);
            
        }

        //print(ListaLetrasControle.Count);
        switch (ListaLetrasControle.Count)
        {
            case 3:
                GameObject.Find("3Letras").SetActive(true);
                
                GameObject.Find("4Letras").SetActive(false);
                GameObject.Find("5Letras").SetActive(false);
               

            break;
            case 4:
                GameObject.Find("4Letras").SetActive(true);

                GameObject.Find("3Letras").SetActive(false);
                GameObject.Find("5Letras").SetActive(false);
            break;
            case 5:
                GameObject.Find("5Letras").SetActive(true);

                GameObject.Find("3Letras").SetActive(false);
                GameObject.Find("4Letras").SetActive(false);
            break;
        }
        int i = 1;

        foreach (string item in ListaLetrasControle)
        {
            //print(item.ToUpper());
             switch(i)
             {
                case 1:
                    Transform pp1 = GameObject.Find("CL1").gameObject.transform;
                    GameObject letra1 =  Instantiate (GameObject.Find(item.ToUpper()));
                    letra1.gameObject.transform.localPosition = pp1.transform.position;
                    letra1.gameObject.transform.localScale =  pp1.transform.localScale * 1.5f;
                    letra1.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra1.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    letra1.gameObject.name = "letra1";
                    GameObject.Find("CL1").SetActive(false);
                    LetrasControleTransforme.Add(letra1);
                break; 
                case 2:
                    Transform pp2 = GameObject.Find("CL2").gameObject.transform;
                    GameObject letra2 =  Instantiate (GameObject.Find(item.ToUpper()));
                    letra2.gameObject.transform.localPosition = pp2.transform.position;
                    letra2.gameObject.transform.localScale =  pp2.transform.localScale * 1.5f;
                    letra2.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra2.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    letra2.gameObject.name = "letra2";
                    GameObject.Find("CL2").SetActive(false);
                    LetrasControleTransforme.Add(letra2);
                break; 
                case 3:
                    Transform pp3 = GameObject.Find("CL3").gameObject.transform;
                    GameObject letra3 =  Instantiate (GameObject.Find(item.ToUpper()));
                    letra3.gameObject.transform.localPosition = pp3.transform.position;
                    letra3.gameObject.transform.localScale =  pp3.transform.localScale * 1.5f;
                    letra3.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra3.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    letra3.gameObject.name = "letra3";
                    GameObject.Find("CL3").SetActive(false);
                    LetrasControleTransforme.Add(letra3);
                break; 
                case 4:
                    Transform pp4 = GameObject.Find("CL4").gameObject.transform;
                    GameObject letra4 =  Instantiate (GameObject.Find(item.ToUpper()));
                    letra4.gameObject.transform.localPosition = pp4.transform.position;
                    letra4.gameObject.transform.localScale =  pp4.transform.localScale * 1.5f;
                    letra4.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra4.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    letra4.gameObject.name = "letra4";
                    GameObject.Find("CL4").SetActive(false);
                    LetrasControleTransforme.Add(letra4);
                break; 
                case 5:
                    Transform pp5 = GameObject.Find("CL5").gameObject.transform;
                    GameObject letra5 =  Instantiate (GameObject.Find(item.ToUpper()));
                    letra5.gameObject.transform.localPosition = pp5.transform.position;
                    letra5.gameObject.transform.localScale =  pp5.transform.localScale * 1.5f;
                    letra5.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra5.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    letra5.gameObject.name = "letra5";
                    GameObject.Find("CL5").SetActive(false);
                    LetrasControleTransforme.Add(letra5);
                break; 

             }
             i++;

        }
        //alfabeto.SetActive(false);
    }

        public List<Vector3> lista = new List<Vector3>();
        public List<int> listaAux = new List<int>();
    public void embaralhar() {
        lista = new List<Vector3>();
        listaAux = new List<int>();
        for (var i = 0; i < LetrasControleTransforme.Count; i++)
        {
            lista.Add(LetrasControleTransforme[i].transform.position);
            listaAux.Add(i);

        }

        Shuffle(listaAux);
        int linha = 0;
        foreach (var item in listaAux)
        {
            //LetrasControleTransforme[linha] =  item;
            print("Linha " + linha +" - " + LetrasControleTransforme[item].transform.position.x +" "+LetrasControleTransforme[item].transform.position.y );
            //lista.Add(LetrasControleTransforme[item]);
            LetrasControleTransforme[linha].transform.position = lista[item];
            linha++;
        }

    }

    public float convertStringParaFLoat(string numero) {
        float numeroRetorno  = 0f;
        switch (numero)
        {
            case "0.5":
                numeroRetorno =  0.5f;
                break;
            case "1.5":
                numeroRetorno =  1.5f;
                break;
            case "2.5":
                numeroRetorno =  2.5f;
                break;
            case "3.5":
                numeroRetorno =  3.5f;
                break;
            case "4.5":
                numeroRetorno =  4.5f;
                break;
            case "-0.5":
                numeroRetorno =  -0.5f;
                break;
            case "-1.5":
                numeroRetorno =  -1.5f;
                break;
            case "-2.5":
                numeroRetorno =  -2.5f;
                break;
            case "-3.5":
                numeroRetorno =  -3.5f;
                break;
            case "-4.5":
                numeroRetorno =  -4.5f;
                break;
            default:
                break;
        }
        return numeroRetorno;
    } 

    private static System.Random rnd = new System.Random();  

    public static void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rnd.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    public void animacaoEMbaralhar() {
        //CameraShake._instance.ShakeCamera(5f, 0.05f);
        StartCoroutine("animacaoEMbaralharENUM");
    }

    IEnumerator animacaoEMbaralharENUM() {

        //for PARA DIMINUIR AS LETRAS QUANDO FOR EMBARARLHAR
        float init = 0.75f;
        for (var i = 0; i < 10; i++)
        {
            init = init-0.05f;
            try
            {
                LetrasControleTransforme[0].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[1].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[2].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[3].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[4].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[5].gameObject.transform.localScale = new Vector3(init,init,0);
            }
            catch (System.Exception ex)
            {
                 // TODO
            }
            yield return new WaitForSeconds(0.00000001f);

        }

        //for PARA EMBARARLHAR
        lista = new List<Vector3>();
        listaAux = new List<int>();
        for (var i = 0; i < LetrasControleTransforme.Count; i++)
        {
            lista.Add(LetrasControleTransforme[i].transform.position);
            listaAux.Add(i);

        }

        Shuffle(listaAux);
        int linha = 0;
        //for PARA TROCAR AS LETRAS DE LUGAR QUANDO FOR EMBARARLHAR
        foreach (var item in listaAux)
        {
            LetrasControleTransforme[linha].transform.position = lista[item];
            linha++;
        }

        //for PARA aumentar AS LETRAS QUANDO FOR EMBARARLHAR
        init = 0.0f;
        for (var i = 0; i < 10; i++)
        {
            init = init+0.075f;
            try
            {
                LetrasControleTransforme[0].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[1].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[2].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[3].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[4].gameObject.transform.localScale = new Vector3(init,init,0);
                LetrasControleTransforme[5].gameObject.transform.localScale = new Vector3(init,init,0);
            }
            catch (System.Exception ex)
            {
                 // TODO
            }
            yield return new WaitForSeconds(0.00000001f);

        }
    }

    public void proximaFase() {
        if(_completouBau) {
            //Chama função que guarda as moedas geradas
            AbrirBauMoedasAleatorias aba = FindObjectOfType(typeof(AbrirBauMoedasAleatorias)) as AbrirBauMoedasAleatorias;
            TextoMoedasAdd tma = FindObjectOfType(typeof(TextoMoedasAdd)) as TextoMoedasAdd;
            tma.addMoedas(aba._qtdMoedaMax);
            _completouBau = false;
            //Chama Proxima cena depois de 3 segundos
            StartCoroutine("AgruparMoedasENUM");
        } else {
            PlayerPrefs.SetInt("faseAtual",PlayerPrefs.GetInt("faseAtual")+1);
            SceneManager.LoadScene("Fase1");
        }
        //GameObject.Find("Refresh").SetActive(false);
    }

    IEnumerator AgruparMoedasENUM() {
        yield return new WaitForSeconds(2f);
        PlayerPrefs.SetInt("faseAtual",PlayerPrefs.GetInt("faseAtual")+1);
        SceneManager.LoadScene("Fase1");

    }
}

