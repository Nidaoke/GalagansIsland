using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class MultiplayerManager : MonoBehaviour
    {
        public static MultiplayerManager instance;

        [Header("Mages Prefabs")]
        public List<GameObject> MagesPrefabs;
        public int MagesCount;

        [Header("Players info")]
        public GameObject Player1Prefab;
        public GameObject Player2Prefab;
        public int Player1MageId = 1;
        public int Player2MageId = 1;

        [Header("UI Info")]
        public Button Player1LeftButton;
        public Button Player1RightButton;
        public Button Player2LeftButton;
        public Button Player2RightButton;
        public Image Player1Image;
        public Image Player2Image;

        void Awake()
        {
            instance = this;
        }

        public void ChangeMageForPlayerRight(int player)
        {
            switch (player)
            {
                case 1:
                    Player1MageId++;
                    Player1Prefab = MagesPrefabs[Player1MageId - 1];

                    if (Player1MageId == MagesCount)
                    {
                        Player1RightButton.interactable = false;
                    }
                    Player1LeftButton.interactable = true;
                    break;
                case 2:
                    Player2MageId++;
                    Player2Prefab = MagesPrefabs[Player2MageId - 1];

                    if (Player2MageId == MagesCount)
                    {
                        Player2RightButton.interactable = false;
                    }
                    Player2LeftButton.interactable = true;
                    break;
            }
        }

        public void ChangeMageForPlayerLeft(int player)
        {
            switch (player)
            {
                case 1:
                    Player1MageId--;
                    Player1Prefab = MagesPrefabs[Player1MageId - 1];

                    if (Player1MageId == MagesCount)
                    {
                        Player1LeftButton.interactable = false;
                    }
                    Player1RightButton.interactable = true;
                    break;
                case 2:
                    Player2MageId--;
                    Player2Prefab = MagesPrefabs[Player2MageId - 1];

                    if (Player2MageId == MagesCount)
                    {
                        Player2LeftButton.interactable = false;
                    }
                    Player2RightButton.interactable = true;
                    break;
            }
        }

        private void ChangeMageImageForPlayer(int player)
        {

        }

        
    }
}
