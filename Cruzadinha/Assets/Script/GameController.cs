using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : GameControllerBase
{
    public enum CATEGORIA { ESCREVER = 0, LER = 1000, CONTAR = 2000, DINAMICO = 8, HISTORIAS = 10, CRUZADINHA = 11 };
    public enum TIPO { ANIMAIS = 0, OBJETOS = 100, FRUTAS = 200, SONS = 8, CORES = 9, HISTORIAS = 10, MISTO = 11 };



    public CATEGORIA cat;
    public TIPO tipo;
    private int fases;
    public static Dictionary <int, float> itensPosition;
    public int pontos;
    public Slider slider;
    public GameObject hudGameOver;
    private AudioControllerV2 audioController;
    public int right;
    public int error;
    private IEnumerator coroutine;

    public override int lockKK { get => lockKK; set => lockKK = value; }

    // Start is called before the first frame update
    void Start()
    {
        
        audioController = FindObjectOfType(typeof(AudioControllerV2)) as AudioControllerV2;
        right = 0;
        error = 0;
        hudGameOver.SetActive(false);
        fases = 4;
        atualizarPontos(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public override void playFx(AudioClip fxAudio)
    {
        audioController.playFx(fxAudio, 1);
    }

    public override void addRight()
    {
        right++;
        if (right >= pontos)
        {
            victory();
            atualizarPontos(true);
            atualizarConquistaPontos();
            coroutine = playVictoryEnum();
            StartCoroutine("playVictoryEnum");
        }
        else
        {
            // audioController.playFx(audioController.fx, 1);
        }
        //Debug.Log(" right" + right);
    }
    public override void addError()
    {
        error++;
        audioController.playFx(audioController.fxError, 1);
    }

    public void victory()
    {
        hudGameOver.SetActive(true);
        Debug.Log(" victory ");
        audioController.pauseMusic();
    }
    public void Reentry()
    {
        this.
        hudGameOver.SetActive(false);

        //audioController.changeMusic(audioController.musicFase1, "Fase_" + audioController.faseAtual, true, slider);
        SceneManager.LoadScene("Fase_"+ (cat + (int) tipo + audioController.faseAtual));
    }
    public void Next()
    {
        //Debug.Log(proximaFase);
        int proximaFase = ((audioController.faseAtual % fases)+1)+ (int) cat + (int )tipo;
        audioController.changeMusic(audioController.musicFase1, "Fase_" + proximaFase, true, slider);
        SceneManager.LoadScene("Fase_"+ proximaFase);
    }

    
    public void resizeColiderMaxPalavra(BoxCollider2D bc2d, SpriteRenderer spriteRenderer, float x, float y)
    {
        bc2d.size = new Vector2(x, y);
        spriteRenderer.sortingOrder = 5;
    }
    
    /*
    public void initList()
    {
        itensPosition = new Dictionary<int, float>();
        float position = 0f;
        int i = 0;
        //while (itensPosition.Count < 5 || i >100) retorna 

        retornar lista de posiçoes
        {
            i++;
            int number = Random.Range(0, 5);
            if(!itensPosition.ContainsKey(number))
            {
                Debug.Log(number +" Poition "+ position);
                position = position + 3.2f;
                itensPosition[1] = position;
            }
        }
    }*/
    IEnumerator playVictoryEnum()
    {
        yield return new WaitForSecondsRealtime(1f);
        //audioController.playFx(audioController.fxPalavra, 1);
        yield return new WaitForSecondsRealtime(0.5f);
        //audioController.playFx(audioController.fxVictory, 1);

    }

    public override AudioClip GetAudioSelecionado()
    {
        throw new System.NotImplementedException();
    }

}
