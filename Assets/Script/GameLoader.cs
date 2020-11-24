using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Assets.Script.Entities;
using UnityEngine;

public class GamePath
{
    public string path;
    public GamePath()
    {
        path = Application.persistentDataPath;
    }
}
public class GameLoader : MonoBehaviour
{
    
    // Start is called before the first frame update

    public static GamePath theGamePath;
    private TextMesh money;

    public static void save()
    {
        string _localPath = Application.persistentDataPath + "/path.json";
        File.WriteAllText(_localPath, JsonUtility.ToJson(theGamePath));
    }

    private void Awake()
    {
        string _localPath = Application.persistentDataPath + "/path.json";
        if (File.Exists(_localPath))
        {
            theGamePath = JsonUtility.FromJson<GamePath>(File.ReadAllText(_localPath));
        }
        else
        {
            theGamePath = new GamePath();
            save();
        }
        
        Game.LoadCurrentState(GameLoader.theGamePath.path + "/lastGameState.json");
        money = Camera.main.transform.Find("Money").GetComponent<TextMesh>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        money.text = Game.CurrentGame.dinero.ToString();
    }
}
