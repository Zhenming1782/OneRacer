using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Script.Entities;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    private GameObject canvas;

    private AudioManager _audioManager;

    // directorio
    public InputField directory;

    // public string PlayerName;
    public InputField playerName;

    // public float MusicVolume;
    public Slider music;

    // public float EffectsVolume;
    public Slider effect;

    // public eDifficulty Difficulty;
    public Dropdown DifficultyDropdown;

    // public int car;
    public Dropdown carro;

    // public int color;
    public Dropdown carColor;

    // public bool camaraRotation;
    public Toggle camaraRotation;

    // public float zoom;
    public Slider zoom;

    private void updateGUIData()
    {
        directory.text = GameLoader.theGamePath.path;
        playerName.text = Game.CurrentGame.PlayerName;
        music.value = Game.CurrentGame.MusicVolume;
        effect.value = Game.CurrentGame.EffectsVolume;
        DifficultyDropdown.value = (int) Game.CurrentGame.Difficulty;
        carro.value = Game.CurrentGame.car;
        carColor.value = Game.CurrentGame.color;
        camaraRotation.isOn = Game.CurrentGame.camaraRotation;
        zoom.value = Game.CurrentGame.zoom;
    }

    private void updateData()
    {
        if(Directory.Exists(directory.text))
        {
            GameLoader.theGamePath.path = directory.text;
            GameLoader.save();
        }
        Game.CurrentGame.PlayerName = playerName.text;
        Game.CurrentGame.MusicVolume = music.value;
        Game.CurrentGame.EffectsVolume = effect.value;
        Game.CurrentGame.Difficulty = (Game.eDifficulty) DifficultyDropdown.value;
        Game.CurrentGame.car = carro.value;
        Game.CurrentGame.color = carColor.value;
        Game.CurrentGame.camaraRotation = camaraRotation.isOn;
        Game.CurrentGame.zoom = zoom.value;
        Game.SaveCurrentState();
    }

    private void Awake()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
    }

    public bool isCanvas()
    {
        return canvas.active;
    }

    public void openOption()
    {
        updateGUIData();
        canvas.SetActive(true);
    }

    public void saveOption()
    {
        updateData();
        _audioManager.reload();
        closeOption();
    }

    public void closeOption()
    {
        canvas.SetActive(false);
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