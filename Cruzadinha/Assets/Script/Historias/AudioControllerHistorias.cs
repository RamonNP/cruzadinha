using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioControllerHistorias : MonoBehaviour
{

    [Header("Cenas")]
    public AudioClip[] cenas = new AudioClip[3];
    private int faseAtual = 0;
    private AudioClip cena;
    public AudioSource sFX;
    public float maxVol;
    public float minVol;
    private GameControllerHistorias _GC;
    // Start is called before the first frame update
    void Start()
    {
        _GC = FindObjectOfType(typeof(GameControllerHistorias)) as GameControllerHistorias;
        playFx(cenas[0], 4);
        _GC.proximaCena("Cena", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!sFX.isPlaying)
        {
            proximaCena();
        }
    }

    public void proximaCena()
    {
        //PlayServices.UnlockAnchievment(GooglePlayServiceConquistas.achievement_uma_linda_historia);
        faseAtual++;
        faseAtual = faseAtual % cenas.Length;
        cena = cenas[faseAtual];
        playFx(cena, 2);
        Debug.Log(faseAtual);
        _GC.proximaCena("Cena", faseAtual);
    }
    public void anteriorCena()
    {
        Debug.Log(faseAtual);
        faseAtual--;
        if (faseAtual <= 0)
        {
            faseAtual=0;
        }
        faseAtual = faseAtual % cenas.Length;
        cena = cenas[faseAtual];
        playFx(cena, 2);
        _GC.proximaCena("Cena", faseAtual);
    }
    public void playFx(AudioClip fx, float volume)
    {
        sFX.Stop();
        float tempVolume = volume;
        if (volume > maxVol)
        {
            tempVolume = maxVol;
        }
        sFX.volume = tempVolume;
        if (fx != null)
        {
            sFX.PlayOneShot(fx);
        }
    }
    public void MenuFaseSelect()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}
