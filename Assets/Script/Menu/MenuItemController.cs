using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuItemController : MonoBehaviour
{
    public const float _HOVERSCALEFACTOR = 1.4f;
    private GameObject temp;
    AudioManager _audioManager;
    private OptionMenu _optionMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _optionMenu = GameObject.Find("GlobalScript").GetComponent<OptionMenu>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        if (gameObject.name.Contains("Level"))
        {
            string meh = "" + gameObject.name[gameObject.name.Length - 1];
            if (int.Parse(meh) == Game.CurrentGame.niveles + 1)
            {
                GetComponent<TextMesh>().color = Color.white;
            }
            else if (int.Parse(meh) < Game.CurrentGame.niveles + 1)
            {
                GetComponent<TextMesh>().color = Color.yellow;
            }
        }
        temp = Camera.main.transform.Find("Money").gameObject;
    }

    public void OnMouseEnter()
    {
        transform.localScale *= _HOVERSCALEFACTOR;
    }

    public void OnMouseExit()
    {
        transform.localScale /= _HOVERSCALEFACTOR;
    }

    private void OnMouseUp()
    {
        if(_optionMenu.isCanvas())
            return;
        _audioManager.play("btnPress");
        if (gameObject.name.Contains("Level"))
        {
            string meh = "" + gameObject.name[gameObject.name.Length - 1];
            if (int.Parse(meh) <= Game.CurrentGame.niveles + 1)
            {
                Race.level = gameObject.name;
                SceneManager.LoadScene("InGame");
            }
        }

        switch (gameObject.name)
        {
            case "Play":
                Camera.main.transform.position = new Vector3(30f, 0f, -10f);
                // SceneManager.LoadScene("InGame");
                break;
            case "Opcion":
                _optionMenu.openOption();
                // _menuController.UpdateOptionsGUI();
                // _menuController.switchCanvas();
                break;
            case "Credit":
                Camera.main.transform.position = new Vector3(0f, -30f, -10f);
                temp.SetActive(false);
                break;
            case "Volver":
                Camera.main.transform.position = new Vector3(0f, 0f, -10f);
                temp.SetActive(true);
                break;
            case "Tienda":
                Camera.main.transform.position = new Vector3(50f, 0f, -10f);
                temp.SetActive(true);
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}