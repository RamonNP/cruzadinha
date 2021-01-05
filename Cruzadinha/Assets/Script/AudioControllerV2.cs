using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class AudioControllerV2 : MonoBehaviour
{

    public AudioControllerV2 instance;
    public float tempoInciarPalavra;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this)
        {
            //Destroy(gameObject);
        }
    }
    public Slider slider;
    private IEnumerator coroutine;
    public AudioSource sMusic;
    public AudioSource sFX;

    //public Slider slider;

    AsyncOperation async;

    [Header("Music")]

    public AudioClip musicFase1;
    public AudioClip musicMenu;

    [Header("Sound")]

    public AudioClip fxClick;
    public AudioClip fxPalavra;
    public AudioClip fxError;
    public AudioClip fxVictory;


    public float maxVol;
    public float minVol;

    private AudioClip newMusic;
    public int faseAtual;
    private string tradeScene;
    private bool changeScene;

    // Start is called before the first frame update
    void Start()
    {
        maxVol = 1;
        minVol = 1;
        changeMusic(musicMenu, "Menu2", false, null);
        //coroutine = playAudioEnum();
        //StartCoroutine("playAudioEnum");
    }
    public void trocaCena(string nomeCena) {
        //StartCoroutine("changeMusic");
        changeMusic(musicFase1, nomeCena, true, null);
    }
    public void changeMusic(AudioClip clip, string newScene, bool changeScene, Slider slider2)
    {

        if (slider2 != null)
        {
            slider = slider2;
        } else
        {
            slider = (Slider) FindObjectOfType<Slider>();
        }
        this.tradeScene = newScene;
        this.newMusic = clip;
        this.changeScene = changeScene;
        coroutine = changeMusicEnum();
        StartCoroutine("changeMusicEnum");
    }
    /*
    private IEnumerator changeMusicEnum()
    {
        for (float volume = maxVol; volume >= 0; volume -= 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            sMusic.volume = volume;
        }
        sMusic.volume = 0;
        sMusic.clip = newMusic;
        sMusic.Play();

        for (float volume = 0; volume < maxVol; volume += 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            sMusic.volume = volume;
        }
        sMusic.volume = maxVol;
        if (changeScene)
        {
            SceneManager.LoadScene(newScene);
        }
    }*/


    IEnumerator changeMusicEnum()
    {
        for (float volume = maxVol; volume >= 0; volume -= 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            sMusic.volume = volume;
        } 
        sMusic.volume = 0;
        sMusic.clip = newMusic;
        sMusic.Play();

        if (changeScene)
        {
            //Debug.Log(async == null);
            if (async == null && slider != null)
            {
                try
                {
                    slider.gameObject.SetActive(true);
                    async = SceneManager.LoadSceneAsync(tradeScene);
                    async.allowSceneActivation = false;
                }
                catch (System.Exception p)
                {
                    Debug.Log("ERRRRRRRRRRROOOOOOOOOOO"+p.StackTrace);
                    slider.gameObject.SetActive(true);
                    async = SceneManager.LoadSceneAsync("Menu");
                    async.allowSceneActivation = false;
                    throw;
                }
                while (async.isDone == false)
                {
                    //Debug.Log(newScene);
                    slider.value = async.progress;
                    if (async.progress == 0.9f)
                    {
                        slider.value = 1f;
                        async.allowSceneActivation = true;
                    }
                    yield return null;
                }
            }
        }
        
        for (float volume = 0; volume < maxVol; volume += 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            sMusic.volume = volume;
        }
        sMusic.volume = maxVol;
        
       

        async = null;
    }
    public void playPalavra()
    {
        sFX.Stop();
        coroutine = playAudioEnum();
        StartCoroutine("playAudioEnum");
    }

    public void playFx(AudioClip fx, float volume)
    {
        object[] parms = new object[2]{fx, volume};
        StartCoroutine("playFxInumerator", parms);
        //aumentar volume da musica
        
    }

    public IEnumerator playFxInumerator(object[] parms) {

        AudioClip fx = (AudioClip) parms[0];
        float volumeFX = (float) parms[1];


        for (float volume = maxVol; volume >= 0; volume -= 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            sMusic.volume = volume;
        }
        float tempVolume = volumeFX;
        if (volumeFX > maxVol)
        {
            tempVolume = maxVol;
        }
        sFX.volume = tempVolume;
        if(fx != null)
        {
            
            sFX.PlayOneShot(fx);
        }
        //sMusic.volume = 0;
        //sMusic.clip = newMusic;
        sMusic.Play();

        for (float volume = 0; volume < maxVol; volume += 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            sMusic.volume = volume;
        }
        sMusic.volume = maxVol;
        async = null;
    }

    public void pauseMusic()
    {
        sMusic.Pause();
    }
    public IEnumerator playAudioEnum()
    {
        yield return new WaitForSecondsRealtime(tempoInciarPalavra);
        //playFx(fxFrase, 1);
    }

    public void Fase1()
    {
        SceneManager.LoadScene("Fase1");
    }

}
