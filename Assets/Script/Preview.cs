using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Preview : MonoBehaviour
{
    //para debuggear nodos de los niveles
    XmlDocument xmlDoc;
    XmlNode cursor;
    public GameObject meh;

    // Update is called once per frame
    private void Awake()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Resources.Load<TextAsset>("Levels/Level2").text);
        int i = 0;
        foreach (XmlNode row in xmlDoc.SelectNodes("//level/NPCRun/point"))
        {
            Instantiate(meh, new Vector3(
                Single.Parse(row.Attributes["x"].Value),
                Single.Parse(row.Attributes["y"].Value)), Quaternion.identity).transform.name = i.ToString();
            i++;
        }
        
    }
}
