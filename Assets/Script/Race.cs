using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Assets.Script.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ePhase
{
    Loading,
    Playing,
    Finished
}

public class Race : MonoBehaviour
{
    public ePhase state = ePhase.Loading;
    public static string level = "Level3";
    public static float botSpeed = 4f;
    public static float penalty = 2f;
    public GameObject[] checkpoint;
    public List<Vector3> points;
    XmlDocument xmlDoc;
    public float startTime = 0f;
    private Controller player;
    public GameObject _playerGO;
    private List<KeyValuePair<Controller, float>> lugares;
    private TextMesh countdown;
    private AudioManager _audioManager;
    XmlNode cursor;
    private GameObject dialog;
    private WebService _webService;
    private bool showMenu = false;
    
    private void Awake()
    {
        _webService = GetComponent<WebService>();
        dialog = Camera.main.gameObject.transform.Find("Dialog").gameObject;
        dialog.SetActive(false);
        _playerGO = GameObject.Find("Player");
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        countdown = _playerGO.transform.Find("Countdown").GetComponent<TextMesh>();
        player = _playerGO.GetComponent<Controller>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize += Game.CurrentGame.zoom;
        lugares = new List<KeyValuePair<Controller, float>>();
        xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(Resources.Load<TextAsset>("Levels/" + level).text);
        loadMap();
    }
    
    public void volver()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if (state == ePhase.Playing)
        {
            if (Time.time - startTime > 2f)
                countdown.text = "";
            if (player.loops >= 4)
            {
                state = ePhase.Finished;
                StartCoroutine(_webService.SendScore(Game.CurrentGame.PlayerName,
                    int.Parse("" + level[level.Length - 1]), getPlayerPos(), player.currentTime));
                int playerPos = getPlayerPos();
                dialog.SetActive(true);
                dialog.transform.Find("Mensaje").GetComponent<TextMesh>().text = string.Format("¡has {0}!",(playerPos<=3? "Ganado": "Perdido"));
                int gain;
                switch (playerPos)
                {
                    case 1:
                        gain = 10;
                        break;
                    case 2:
                        gain = 8;
                        break;
                    case 3:
                        gain = 5;
                        break;
                    default:
                        gain = 0;
                        break;
                }

                if (playerPos<=3)
                {
                    switch (Race.level)
                    {
                        case "Level1":
                            if (Game.CurrentGame.niveles < 1)
                            {
                                Game.CurrentGame.niveles = 1;
                            }
                            break;
                        case "Level2":
                            if (Game.CurrentGame.niveles < 2)
                            {
                                Game.CurrentGame.niveles = 2;
                            }
                            break;
                        case "Level3":
                            if (Game.CurrentGame.niveles < 3)
                            {
                                Game.CurrentGame.niveles = 3;
                            }
                            break;
                        case "Level4":
                            if (Game.CurrentGame.niveles < 4)
                            {
                                Game.CurrentGame.niveles = 4;
                            }
                            break;
                        default:
                            break;
                    }
                }
                Game.CurrentGame.dinero += gain;
                dialog.transform.Find("Money").GetComponent<TextMesh>().text = string.Format("+{0}",gain);
                Game.SaveCurrentState();
            }
            else
            {
                for (int i = 0; i < lugares.Count; i++)
                {
                    lugares[i] = new KeyValuePair<Controller, float>(lugares[i].Key, lugares[i].Key.getPoints());
                }

                lugares.Sort((x, y) => y.Value.CompareTo(x.Value));
            }
        }
        else if (state == ePhase.Loading)
        {
            float timing = Time.time - startTime;
            if (timing > 10f)
            {
                countdown.text = "GO";
                state = ePhase.Playing;
                startTime = Time.time;
                _audioManager.play("start");
            }
            else
            {
                countdown.text = (10-(int)timing).ToString();
            }
        }
    }

    public int getPlayerPos()
    {
        int i;
        for (i = 0; i < lugares.Count; i++)
        {
            if (lugares[i].Key == player)
            {
                break;
            }
        }

        return i + 1;
    }

    void loadMap()
    {
        Transform parentG = GameObject.Find("Cells").transform;
        cursor = xmlDoc.SelectSingleNode("//level/background");
        Camera.main.backgroundColor = new Color(Single.Parse(cursor.Attributes["r"].Value) / 255f,
            Single.Parse(cursor.Attributes["g"].Value) / 255f, Single.Parse(cursor.Attributes["b"].Value) / 255f);
        cursor = xmlDoc.SelectSingleNode("//level/size");
        GameObject newG;
        int x = Int32.Parse(cursor.Attributes["x"].Value), y = Int32.Parse(cursor.Attributes["y"].Value);
        cursor = xmlDoc.SelectSingleNode("//level/mapa");
        string[] mapa = cursor.InnerText.Split(',');
        string[] fila;
        int i, j;
        for (i = 0, j = 0; i < mapa.Length; i++)
        {
            if (i % x == 0)
            {
                --j;
            }

            newG = Resources.Load<GameObject>("Tiles/" + String.Format("{0:D3}", Int32.Parse(mapa[i])));
            Instantiate(newG, new Vector3(i % x, j, 1), Quaternion.identity, parentG);
        }

        Controller auxCon;

        i = 0;
        foreach (XmlNode row in xmlDoc.SelectNodes("//level/posiciones/pos"))
        {
            if (i == 0)
            {
                newG = GameObject.Find("Player");
                newG.transform.position = new Vector3(
                    Single.Parse(row.Attributes["x"].Value),
                    Single.Parse(row.Attributes["y"].Value));
            }
            else
            {
                newG = Instantiate(Resources.Load<GameObject>("NPC"),
                    new Vector3(Single.Parse(row.Attributes["x"].Value),
                        Single.Parse(row.Attributes["y"].Value)), Quaternion.identity);
            }

            auxCon = newG.GetComponent<Controller>();
            lugares.Add(new KeyValuePair<Controller, float>(auxCon, 0f));

            i++;
        }

        foreach (XmlNode row in xmlDoc.SelectNodes("//level/NPCRun/point"))
        {
            points.Add(new Vector3(
                Single.Parse(row.Attributes["x"].Value),
                Single.Parse(row.Attributes["y"].Value)));
        }

        XmlNodeList checker = xmlDoc.SelectNodes("//level/Checkpoints/check");
        newG = Resources.Load<GameObject>("Checkpoint");
        checkpoint = new GameObject[checker.Count];
        parentG = GameObject.Find("CheckPoints").transform;
        for (i = 0; i < checker.Count; i++)
        {
            checkpoint[i] = Instantiate(newG,
                new Vector3(float.Parse(checker[i].Attributes["x"].Value),
                    float.Parse(checker[i].Attributes["y"].Value)), Quaternion.identity);
            checkpoint[i].transform.rotation = Quaternion.Euler(0f, 0f, float.Parse(checker[i].Attributes["r"].Value));
            checkpoint[i].transform.localScale = new Vector3(float.Parse(checker[i].Attributes["sx"].Value),
                float.Parse(checker[i].Attributes["sy"].Value));
            checkpoint[i].name = i.ToString();
            checkpoint[i].transform.parent = parentG;
        }

        startTime = Time.time;
    }
}