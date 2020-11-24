using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebService : MonoBehaviour
{
    public bool yes = false;

    [Serializable]
    public class Carreras
    {
        public int id;
        public string Nombre;
        public int Nivel;
        public int Lugar;
        public float Tiempo;

        public Carreras(int id, string nombre, int nivel, int lugar, float tiempo)
        {
            this.id = id;
            this.Nombre = nombre;
            this.Nivel = nivel;
            this.Lugar = lugar;
            this.Tiempo = tiempo;
        }
    }

    private void Update()
    {
        if (yes)
        {
            yes = false;
            StartCoroutine(SendScore("yolo", 1, 1, 2.1f));
        }
    }

    private UnityWebRequest www;

    private const string webServiceURL = "http://znzn00.servegame.com:64198/api/Carreras/values";
    private const string localWebServiceUrl = "http://localhost:64198/api/Carreras/values";

    public IEnumerator SendScore(string Nombre, int nivel, int Lugar, float tiempo)
    {
        Carreras temp = new Carreras(0,Nombre, nivel, Lugar, tiempo);
        www = UnityWebRequest.Put(webServiceURL, JsonUtility.ToJson(temp));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
        www = UnityWebRequest.Put(localWebServiceUrl, JsonUtility.ToJson(temp));
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
    }
    
    public IEnumerator Get(string table, string orderby, string[] ans)
    {
        string Dirr = $"http://0.0.0.0:64198/api/{table}/showall?orderby={orderby}";
        www = UnityWebRequest.Get(Dirr);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        ans[0] = www.downloadHandler.text;
        Debug.Log(www.downloadHandler.text);
    }
    
    public IEnumerator Send(string tabla, string jsonfile)
    {
        string Dirr = $"http://0.0.0.0:64198/api/{tabla}/values";
        www = UnityWebRequest.Put(Dirr, jsonfile);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.text);
    }
}
