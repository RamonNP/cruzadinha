using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruzadinhaControleV2 : MonoBehaviour
{
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
        //if(xmlLerDados !=null){
            //xmlLerDados.LoadDialogoData(gameController.idiomaFolder[gameController.idioma] + "/" + gameController.nomeArquivoXml); //ler o arquivo interação com itens;
        //}
        //Debug.Log("INICIO CARREGANDO");
        xmlLerDados = LerXml.getInstance();
        //Debug.Log("INSTANCIADO");
        objetos = xmlLerDados.LoadDialogoData("fases");
        //Debug.Log("FIM CARREGANDO");
        preencherPlace();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void  preencherPlace() {
        gameController.pontos = 0;
        //variavel que guarda letras que sera apresentado na tela para usuario selecionar e montar palavras
        List<string> ListaLetrasControle = xmlLerDados.pularletrasControle;
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
            print(palavra);
            
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
                    letra1.gameObject.transform.localScale =  pp1.transform.localScale;
                    letra1.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra1.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    GameObject.Find("CL1").SetActive(false);
                    LetrasControleTransforme.Add(letra1);
                break; 
                case 2:
                    Transform pp2 = GameObject.Find("CL2").gameObject.transform;
                    GameObject letra2 =  Instantiate (GameObject.Find(item.ToUpper()));
                    letra2.gameObject.transform.localPosition = pp2.transform.position;
                    letra2.gameObject.transform.localScale =  pp2.transform.localScale;
                    letra2.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra2.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    GameObject.Find("CL2").SetActive(false);
                    LetrasControleTransforme.Add(letra2);
                break; 
                case 3:
                    Transform pp3 = GameObject.Find("CL3").gameObject.transform;
                    GameObject letra3 =  Instantiate (GameObject.Find(item.ToUpper()));
                    letra3.gameObject.transform.localPosition = pp3.transform.position;
                    letra3.gameObject.transform.localScale =  pp3.transform.localScale;
                    letra3.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra3.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    GameObject.Find("CL3").SetActive(false);
                    LetrasControleTransforme.Add(letra3);
                break; 
                case 4:
                    Transform pp4 = GameObject.Find("CL4").gameObject.transform;
                    GameObject letra4 =  Instantiate (GameObject.Find(item.ToUpper()));
                    letra4.gameObject.transform.localPosition = pp4.transform.position;
                    letra4.gameObject.transform.localScale =  pp4.transform.localScale;
                    letra4.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra4.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    GameObject.Find("CL4").SetActive(false);
                    LetrasControleTransforme.Add(letra4);
                break; 
                case 5:
                    Transform pp5 = GameObject.Find("CL5").gameObject.transform;
                    GameObject letra5 =  Instantiate (GameObject.Find(item.ToUpper()));
                    letra5.gameObject.transform.localPosition = pp5.transform.position;
                    letra5.gameObject.transform.localScale =  pp5.transform.localScale;
                    letra5.GetComponent<SpriteRenderer>().sortingLayerName = "Controle";
                    letra5.GetComponent<SpriteRenderer>().sortingOrder = 5;
                    GameObject.Find("CL5").SetActive(false);
                    LetrasControleTransforme.Add(letra5);
                break; 

             }
             i++;

        }
        //alfabeto.SetActive(false);
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
}
