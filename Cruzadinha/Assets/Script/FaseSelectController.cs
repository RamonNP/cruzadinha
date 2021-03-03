using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FaseSelectController : MonoBehaviour
{
    public List<GameObject> paginas;
    public int paginaAtual;
    private const string FASE_DAO = "Fase";
    private const string BTN_ABERTO = "BtnAberto";
    private const string BTN_ABERTO_1 = "BtnAbertto1";
    private const string BTN_ABERTO_2 = "BtnAbertto2";
    private const string BTN_ABERTO_3 = "BtnAbertto3";
    public List <GameObject> listaFases;
    // Start is called before the first frame update
    void Start()
    {
        paginaAtual = AppDao.getInstance().loadInt(AppDao.PAGINA);
        //inicializa a pagina que esta no banco, depende da variavel pagina atual estar iniciada
        inicializaPagina();
        AppDao.getInstance().saveInt(FASE_DAO+1,1);
        // muda para retrato para escolher as fases
        //Screen.orientation = ScreenOrientation.Portrait;
        int indice = 1;
        //percorre as fases para abrir 
        foreach (var item in listaFases)
        {
            AbrirFase(item, indice);
            indice++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AbrirFase(GameObject obj, int indice) {
        //verifica se a fase ja foi aberta: 1 aberto, 0 fechado
        int fase = AppDao.getInstance().loadInt(FASE_DAO+indice);
        //print(fase);
        if(fase == 1){
            //abre a imagem com a quantidade de estrelas correspondentes
            int QtdEstrelas = AppDao.getInstance().loadInt(AppDao.ESTRLA_FASES+indice);
            for (int i = 0; i < obj.transform.childCount; i++){
                GameObject child = obj.transform.GetChild(i).gameObject;

                if(QtdEstrelas == 0){
                    if(child.name == BTN_ABERTO) {
                        child.SetActive(true);
                    } else {
                        child.SetActive(false);
                    }
                }
                if(QtdEstrelas == 1){
                    if(child.name == BTN_ABERTO_1) {
                        child.SetActive(true);
                    } else {
                        child.SetActive(false);
                    }
                }
                if(QtdEstrelas == 2){
                    if(child.name == BTN_ABERTO_2) {
                        child.SetActive(true);
                    } else {
                        child.SetActive(false);
                    }
                }
                if(QtdEstrelas == 3){
                    if(child.name == BTN_ABERTO_3) {
                        child.SetActive(true);
                    } else {
                        child.SetActive(false);
                    }
                }
                if(child.name == "Text") {
                    child.SetActive(true);
                }
                
            }
        }

    }

    public void btnAbrirFase(int fase) {
         //salva no formato para abrir a proxima fase
        AppDao.getInstance().saveInt(AppDao.FASE,fase);
        SceneManager.LoadScene("Fase1");
    }

    private void inicializaPagina() {
        GameObject pagina = paginas[paginaAtual];
        foreach (var item in paginas)
        {
            item.SetActive(false);
        }
        pagina.SetActive(true);
    }
    public void proximaPagina(){
        if(paginaAtual < (paginas.Count-1)){
            paginaAtual +=1;
            AppDao.getInstance().saveInt(AppDao.PAGINA,paginaAtual);
            GameObject pagina = paginas[paginaAtual];
            foreach (var item in paginas)
            {
                item.SetActive(false);
            }
            pagina.SetActive(true);
        }
    }
    public void anteriorPagina(){
        if(paginaAtual > 0){
            paginaAtual -=1;
            AppDao.getInstance().saveInt(AppDao.PAGINA,paginaAtual);
            GameObject pagina = paginas[paginaAtual];
            foreach (var item in paginas)
            {
                item.SetActive(false);
            }
            pagina.SetActive(true);
        }
    }
}
