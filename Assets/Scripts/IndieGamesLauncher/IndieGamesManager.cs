using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.IndieGamesLauncher
{
    //Add new games to this enum
    public enum IndieGameType { TowerOfElements }

    [Serializable]
    public class IndieGame
    {
        public string name;
        public IndieGameType type;
        //path should look like this "IndieGames\GameName\GameName.exe"
        public string path;
    }


    //Place IndieGamesManager on Scene
    //Start Games using IndieGamesManager.instance.StartGame(IndieGameType.GameName);
    //The path to the game needs to be in the same folder as the .exe of the game
    //
    //Example:
    //
    //GameFolder
    //-Data_GI/....
    //-GI.exe
    //-IndieGames/TowerOfElements/TowerOfElements.exe
    public class IndieGamesManager : MonoBehaviour
    {
        public static IndieGamesManager instance;
        
        [SerializeField]
        private List<IndieGame> indieGames;

        void Awake()
        {
            instance = this;
        }

        public void StartGame(IndieGameType gameType)
        {
            foreach (var game in indieGames)
            {
                if (game.type == gameType)
                {
                    LaunchGame(game);
                    return;
                }
            }
        }

        public void StartGame(string gameName)
        {
            foreach (var game in indieGames)
            {
                if (game.name == gameName)
                {
                    LaunchGame(game);
                    return;
                }
            }
        }

        private void LaunchGame(IndieGame game)
        {
            Application.OpenURL(game.path);
        }
    }
}
