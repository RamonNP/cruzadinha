using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextoMoedasAdd : MonoBehaviour
{
    public TMPro.TMP_Text m_textMeshPro;

    private const string label = "{0}";
    private float m_frame;
    private int qtdMoedaMax;

    // Start is called before the first frame update
    void Start()
    {
        m_textMeshPro = this.GetComponent<TMPro.TMP_Text>();
        qtdMoedaMax = PlayerPrefs.GetInt("MoedasBanco");
        m_frame = qtdMoedaMax;
    }

    // Update is called once per frame
    void Update()
    {
        if(qtdMoedaMax > m_frame){
            m_textMeshPro.SetText(label, (int)m_frame);
            m_frame += 1;// * Time.deltaTime;
        } else {
             m_textMeshPro.SetText(label, (int)qtdMoedaMax);
        }
    }
    public void addMoedas(int qtd){
        qtdMoedaMax  += qtd;
        PlayerPrefs.SetInt("MoedasBanco", qtdMoedaMax);
    }
}

