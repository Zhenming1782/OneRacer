using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Entities
{
    public class Game
    {
        public enum eDifficulty
        {
            VeryEasy,
            Easy,
            Moderate,
            Hard
        }

        #region "Singleton"

        private static Game _currentGame;

        public static Game CurrentGame
        {
            get
            {
                if (_currentGame == null)
                {
                    _currentGame = new Game();
                    _currentGame.PlayerName = "Fulano";
                    _currentGame.MusicVolume = 100f;
                    _currentGame.EffectsVolume = 100f;
                    _currentGame.Difficulty = eDifficulty.Moderate;
                    _currentGame.niveles = 0;
                    _currentGame.dinero = 0;
                    _currentGame.car = 0;
                    _currentGame.color = 0;
                    _currentGame.upSpeed = 0;
                    _currentGame.upBrake = 0;
                    _currentGame.upAccel = 0;
                    _currentGame.upBrake = 0;
                    _currentGame.camaraRotation = true;
                    _currentGame.zoom = 0;
                }

                return _currentGame;
            }
            set { _currentGame = value; }
        }

        #endregion

        #region "Attributes"
        
        public int niveles;
        public int dinero;
        public int upSpeed;
        public int upSteeling;
        public int upAccel;
        public int upBrake;
        //opciones
        public string PlayerName;
        public float MusicVolume;
        public float EffectsVolume;
        public eDifficulty Difficulty;
        public int car;
        public int color;
        public bool camaraRotation;
        public float zoom;
        // string _localPath = Application.persistentDataPath + "/lastGameState.json";

        #endregion

        #region "Behaviours"

        public static void SaveCurrentState()
        {
            File.WriteAllText(GameLoader.theGamePath.path + "/lastGameState.json", JsonUtility.ToJson(CurrentGame));
        }

        public static void LoadCurrentState(String path)
        {
            if (File.Exists(path))
            {
                CurrentGame = JsonUtility.FromJson<Game>(File.ReadAllText(path));
            }
        }

        #endregion
    }
}