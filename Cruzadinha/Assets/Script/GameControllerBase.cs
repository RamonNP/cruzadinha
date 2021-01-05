using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract  class GameControllerBase : MonoBehaviour 
{
    public Text txtPontos;

    public abstract AudioClip GetAudioSelecionado();
    public abstract int lockKK { get; set; }
    public abstract void addRight();
    public abstract void addError();
    public abstract void playFx(AudioClip fxAudio);

    public void resizeColiderMin(BoxCollider2D bc2d)
    {
        bc2d.size = new Vector2(0.00001f, 0.00001f);
    }

    public void resizeColiderMax(BoxCollider2D bc2d, SpriteRenderer spriteRenderer)
    {
        bc2d.size = new Vector2(2f, 3f);
        spriteRenderer.sortingOrder = 5;
    }
    public void gravarConquista(string achievement_uma_linda_historia, string palavra, int pontos)
    {
        throw new NotImplementedException();
    }

    public void atualizarPontos(bool incremento)
    {
        if (incremento)
        {
            //BancoPlayerprefs.instance.gravarPontos();
        }
        //txtPontos.text = //BancoPlayerprefs.instance.intPontos.ToString().PadLeft(5, '0');
    }
    public void atualizarConquistaPontos()
    {
/*
        if(BancoPlayerprefs.instance.intPontos < 11)
        {
            PlayServices.IncrementAchievment(GooglePlayServiceConquistas.achievement_alcance_10_pontos, 1);
            PlayServices.IncrementAchievment(GooglePlayServiceConquistas.achievement_alcance__100_pontos, 1);
            PlayServices.IncrementAchievment(GooglePlayServiceConquistas.achievement_alcance__1000_pontos, 1);

        } else if(BancoPlayerprefs.instance.intPontos > 101)
        {
            PlayServices.IncrementAchievment(GooglePlayServiceConquistas.achievement_alcance__100_pontos, 1);
            PlayServices.IncrementAchievment(GooglePlayServiceConquistas.achievement_alcance__1000_pontos, 1);
        } else
        {
            PlayServices.IncrementAchievment(GooglePlayServiceConquistas.achievement_alcance__1000_pontos, 1);
        }
        PlayServices.PostScore(BancoPlayerprefs.instance.intPontos, GooglePlayServiceConquistas.leaderboard_ranking_principal);
        */
    }
    

}
