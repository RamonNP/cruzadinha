using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LerXml : MonoBehaviour
{
    public Dictionary<string, string> mensagemInteracao;
    // Start is called befo
    public static LerXml instance;
    
    public List<String> pularletrasControle = new List<string>();

    public static LerXml getInstance() {
        if(instance == null)
        {
            instance = GameObject.FindObjectOfType<LerXml>();
            instance.mensagemInteracao = new Dictionary<string, string>();
        }
        return instance;
    }
    void Start()
    {

       // mensagemInteracao = new Dictionary<string, string>();
       // print("mensagemInteracao"+ mensagemInteracao);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Objeto> LoadDialogoData(string caminho)
    {
        List<Objeto> list = new List<Objeto>();
        TextAsset xmlData = Resources.Load<TextAsset>(caminho);
        print(caminho);
        XmlDocument XmlDocument = new XmlDocument();
        XmlDocument.LoadXml(xmlData.text);
        foreach(XmlNode item in XmlDocument["palavras"].ChildNodes)
        {
            Objeto obj = new Objeto();
            obj.nome = item["nome"].InnerText;
            obj.pontos = item["pontos"].InnerText;
            //obj.dificuldade = item["dificuldade"].InnerText;
            obj.vertical = item["vertical"].InnerText;
            obj.inicio = item["inicio"].InnerText;
            //print( obj.inicio + "-"+  obj.pontos);
            if(item["pular"].ChildNodes != null) {
                foreach(XmlNode pular in item["pular"].ChildNodes) {
                    obj.pular.Add(pular.InnerText);
                }
            }
            //print(item["letra"].InnerText);
            if(item["letra"] != null) {
                foreach(XmlNode letra in item["letra"].ChildNodes) {
                    pularletrasControle.Add(letra.InnerText);
                    obj.pular.Add(letra.InnerText);
                }
            }
            list.Add(obj);
        }
        return list;
    }

    public string TextoFormatado (string frase)
	{
		string temp = frase;

		// Subtitui palavras especificas
		temp = temp.Replace ("**cor=yellow", "<color=#FFFF00FF>");
		temp = temp.Replace ("**cor=green", "<color=#008A22>");
		temp = temp.Replace ("**cor=red", "<color=#ff0000ff>");
		temp = temp.Replace ("**cor=orange", "<color=#ffa500ff>");
		temp = temp.Replace ("**cor=prata", "<color=#9F9999>");
		temp = temp.Replace ("fimnegrito**", "</b>");
		temp = temp.Replace ("**negrito", "<b>");
		temp = temp.Replace ("fimcor**", "</color>");

		return temp;
	}
}

public class Objeto {
    public String nome;
    public String dificuldade;
    public String pontos;
    public String ordem;
    public String vertical;
    public String inicio;
    public List<String> pular = new List<string>();
}
