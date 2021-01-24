using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCompletouController : MonoBehaviour
{
    private CruzadinhaControleV2 _cruzadinhaControleV2;
    public GameObject presente;
    public GameObject moedaRecompensa;
    public GameObject grupoGemas1;
    public GameObject grupoGemas2;
    public GameObject grupoGemas3;
    public GameObject grupoGemas4;
    public GameObject gemas;
    public GameObject gema1;
    public GameObject gema2;
    public GameObject gema3;
    public GameObject gema4;
    public GameObject gema5;

    // Start is called before the first frame update
    void Start()
    {
        _cruzadinhaControleV2 = FindObjectOfType(typeof(CruzadinhaControleV2)) as CruzadinhaControleV2;
        string gemaString = PlayerPrefs.GetString("gemasSequencia");
        gemas = GameObject.Find(gemaString);
        grupoGemas1 = GameObject.Find("Gemas1");
        grupoGemas2 = GameObject.Find("Gemas2");
        grupoGemas3 = GameObject.Find("Gemas3");
        grupoGemas4 = GameObject.Find("Gemas4");
        grupoGemas1.SetActive(false);
        grupoGemas2.SetActive(false);
        grupoGemas3.SetActive(false);
        grupoGemas4.SetActive(false);
        presente =  GameObject.Find("Presente");
        moedaRecompensa =  GameObject.Find("MoedasRecompensa");
        moedaRecompensa.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void abrirProximaGema(){
        //print("CHAMOU ABRIR");
        inicializarGemas();
        int gema = PlayerPrefs.GetInt("gema");
        gema++;
        if(gema > 5){
            gema = 5;
        }
        //Gema fixa com 5 para teste completou card
        //print(gema);
        //gema = 5;
        switch (gema)
        {
            case 1:
                gema1.SetActive(true);
            break;
            case 2:
                gema1.SetActive(true);
                gema2.SetActive(true);
            break;
            case 3:
                gema1.SetActive(true);
                gema2.SetActive(true);
                gema3.SetActive(true);
            break;
            case 4:
                gema1.SetActive(true);
                gema2.SetActive(true);
                gema3.SetActive(true);
                gema4.SetActive(true);
            break;
            case 5:
                gema1.SetActive(true);
                gema2.SetActive(true);
                gema3.SetActive(true);
                gema4.SetActive(true);
                gema5.SetActive(true);
                //PlayerPrefs.SetInt("gema",0);
                gema = 0;
                moedaRecompensa.SetActive(true);
                presente.SetActive(false);
                //busca o Script na arvore de objetos pelo tipo
                AbrirBauMoedasAleatorias aba = FindObjectOfType(typeof(AbrirBauMoedasAleatorias)) as AbrirBauMoedasAleatorias;
                //Gera moedas aleatorias para incrementar no banco.
                aba.GerarMoedasRandom();
                //indica que gerou moedas, para posteriomente gravalas no banco
                _cruzadinhaControleV2._completouBau = true;

                
            break;
            default:
            break;
        }
        PlayerPrefs.SetInt("gema",gema);
    }

    string sortearGemas() {
        string retorno = null;
        int r = Random.Range(1,4);
        //print("NOVOOOOOOOOO"+r);
        switch (r)
        {
            case 1:
                retorno = "Gemas1";
            break;
            case 2:
                retorno = "Gemas2";
            break;
            case 3:
                retorno = "Gemas3";
            break;
            case 4:
                retorno = "Gemas4";
            break;
            default:
            break;
        }
        //print("NOVOOOOOOOOO"+retorno);
        return retorno;
    }
    private void recuperaGema(){
        //recupera o grupo do banco
        string gemaString = PlayerPrefs.GetString("gemasSequencia");
        //print(gemaString);
        
        //ativa todos os grupos para pesquisar a sorteada
        grupoGemas1.SetActive(true);
        grupoGemas2.SetActive(true);
        grupoGemas3.SetActive(true);
        grupoGemas4.SetActive(true);
        
        // encontra o grupo sorteado
        gemas = GameObject.Find(gemaString);
        //ativa o grupo escolhido
        gemas.SetActive(true);

        //desabilita todos os grupos
        grupoGemas1.SetActive(false);
        grupoGemas2.SetActive(false);
        grupoGemas3.SetActive(false);
        grupoGemas4.SetActive(false);
        gemas.SetActive(true);


    }
    public void mudarCOr() {
        inicializarGemas();
        Color c1 = gema1.GetComponent<Image>().color;
        c1.a = 40f;
        gema1.GetComponent<Image>().color = c1;

        c1 = gema2.GetComponent<Image>().color;
        c1.a = 40f;
        gema2.GetComponent<Image>().color = c1;
    }
    public void inicializarGemas() {
         //recuperar gema do banco, caso não tenha sortear uma
        string gemaString = PlayerPrefs.GetString("gemasSequencia");
        if(PlayerPrefs.GetInt("gema") == 0) {
            gemaString = sortearGemas();
            print(gemaString);
            PlayerPrefs.SetString("gemasSequencia", gemaString);
        } 
        try
        {
            recuperaGema();
            gema1 = gemas.transform.GetChild(0).gameObject;
            gema1.SetActive(false);
            gema2 = gemas.transform.GetChild(1).gameObject;
            gema2.SetActive(false);
            gema3 = gemas.transform.GetChild(2).gameObject;
            gema3.SetActive(false);
            gema4 = gemas.transform.GetChild(3).gameObject;
            gema4.SetActive(false);
            gema5 = gemas.transform.GetChild(4).gameObject;
            gema5.SetActive(false);
        }
        catch (System.Exception)
        {
            PlayerPrefs.SetInt("gema",0);
            //inicializarGemas();
            throw;
        }



        

    }
}
