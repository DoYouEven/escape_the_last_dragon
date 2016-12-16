using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PK.InfinuteRunner.Game
{
    //Change to correct SceneNames
    public enum SceneName
    {
        MainLobby,
        Scores,
        Shop,
        GameScene
    }
    public class GameManager : MonoBehaviour
    {

        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>() as GameManager;   
                }
                else
                {
                    Debug.Log("Add a GameManager to the scene");
                }
                return instance;
            }
        }
        void Start()
        {

        }


        public void GetPlayerData()
        {
            
        }

        public void UpdatePlayerData()
        {
             
        }

        public void LoadPlayerSettings()
        {
            
        }
        public void LoadScene(SceneName sceneName)
        {
            SceneManager.LoadScene(sceneName.ToString());
        }  
    }
}

