using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenerateTiles : MonoBehaviour
{
    //Solo para build mode
    [MenuItem("AssetDatabase/Back To Menu")]
    static void MainTo()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    [MenuItem("AssetDatabase/GenerateTiles")]
    static void ImportExample()
    {
        GameObject nT;
        SpriteRenderer _spriteRenderer;
        string Name;
        for (int i = 1; i <= 312; i++)
        {
            Name = string.Format("{0:D3}", i);
            nT = new GameObject(Name);
            _spriteRenderer = nT.AddComponent<SpriteRenderer>();
            _spriteRenderer.sprite =
                (Sprite) AssetDatabase.LoadAssetAtPath($"Assets/Sprites/Tiles/{Name}.png", typeof(Sprite));
            nT.AddComponent<PolygonCollider2D>().usedByEffector = true;
        }
    }

    public void Update()
    {
    }
}