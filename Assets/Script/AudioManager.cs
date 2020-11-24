using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Entities;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private Dictionary<string, AudioSource> audios;

    public void play(string id)
    {
        audios[id].Play();
    }

    private void Awake()
    {
        reload();
    }

    public void reload()
    {
        audios = new Dictionary<string, AudioSource>();
        Transform audiosGO = transform.Find("Audios");
        Transform aux;
        for (int i = 0; i < audiosGO.childCount; i++)
        {
            aux = audiosGO.GetChild(i);
            audios.Add(aux.name,aux.gameObject.GetComponent<AudioSource>());
            
            if (aux.name == "speed")
            {
                audios[aux.name].volume = Game.CurrentGame.EffectsVolume / 200f;
            }
            else
            {
                audios[aux.name].volume = Game.CurrentGame.EffectsVolume / 100f;
            }
        }
        
        audiosGO = transform.Find("Loops");
        for (int i = 0; i < audiosGO.childCount; i++)
        {
            audiosGO.GetChild(i).gameObject.GetComponent<AudioSource>().volume = Game.CurrentGame.MusicVolume / 100f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
