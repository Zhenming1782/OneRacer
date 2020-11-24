using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Entities;
using UnityEngine;

public class UpgradeBehaviour : MonoBehaviour
{
    private TextMesh _textMesh;
    private const int costo = 5;
    public void OnMouseEnter()
    {
        transform.localScale *= MenuItemController._HOVERSCALEFACTOR;
    }

    public void OnMouseExit()
    {
        transform.localScale /= MenuItemController._HOVERSCALEFACTOR;
    }

    private void OnMouseUp()
    {
        int money = 0;
        switch (transform.name)
        {
            case "Velocidad":
                if ((Game.CurrentGame.upSpeed + 1) * costo <= Game.CurrentGame.dinero && Game.CurrentGame.upSpeed<5)
                {
                    money = (Game.CurrentGame.upSpeed + 1) * costo;
                    Game.CurrentGame.upSpeed++;
                }
                break;
            case "Accel":
                if ((Game.CurrentGame.upAccel + 1) * costo <= Game.CurrentGame.dinero && Game.CurrentGame.upAccel<5)
                {
                    money = (Game.CurrentGame.upAccel + 1) * costo;
                    Game.CurrentGame.upAccel++;
                }
                break;
            case "Manejo":
                if ((Game.CurrentGame.upSteeling + 1) * costo <= Game.CurrentGame.dinero && Game.CurrentGame.upSteeling<5)
                {
                    money = (Game.CurrentGame.upSteeling + 1) * costo;
                    Game.CurrentGame.upSteeling++;
                }
                break;
            case "Freno":
                if ((Game.CurrentGame.upBrake + 1) * costo <= Game.CurrentGame.dinero && Game.CurrentGame.upBrake<5)
                {
                    money = (Game.CurrentGame.upBrake + 1) * costo;
                    Game.CurrentGame.upBrake++;
                }
                break;
        }

        Game.CurrentGame.dinero -= money;
        Game.SaveCurrentState();
    }

    private void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
    }

    private void Update()
    {
        switch (transform.name)
        {
            case "Velocidad":
                if(Game.CurrentGame.upSpeed<5)
                {
                    _textMesh.text = String.Format("Velocidad [{0}] - {1}", Game.CurrentGame.upSpeed,
                        (Game.CurrentGame.upSpeed+1)*costo);
                }
                else
                {
                    _textMesh.text = "Velocidad [MAX]";
                }
                break;
            case "Accel":
                if(Game.CurrentGame.upAccel<5)
                {
                    _textMesh.text = String.Format("Aceleración [{0}] - {1}", Game.CurrentGame.upAccel,
                        (Game.CurrentGame.upAccel+1)*costo);
                }
                else
                {
                    _textMesh.text = "Aceleración [MAX]";
                }
                break;
            case "Manejo":
                if(Game.CurrentGame.upSteeling<5)
                {
                    _textMesh.text = String.Format("Manejo [{0}] - {1}", Game.CurrentGame.upSteeling,
                        (Game.CurrentGame.upSteeling+1)*costo);
                }
                else
                {
                    _textMesh.text = "Manejo [MAX]";
                }
                
                break;
            case "Freno":
                if(Game.CurrentGame.upBrake<5)
                {
                    _textMesh.text = String.Format("Frenos [{0}] - {1}", Game.CurrentGame.upBrake,
                        (Game.CurrentGame.upBrake + 1) * costo);
                }
                else
                {
                    _textMesh.text = "Frenos [MAX]";
                }
                break;
        }
    }

    // Update is called once per frame
}
