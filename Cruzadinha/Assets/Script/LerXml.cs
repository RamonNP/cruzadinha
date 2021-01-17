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
            //print(item["nome"].InnerText);
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
