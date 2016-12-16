using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PK.InfinuteRunner.Game
{
    public class InputUIDebug : MonoBehaviour
    {

        private static InputUIDebug instance;

        public static InputUIDebug Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<InputUIDebug>() as InputUIDebug;
                }
                else
                {
                    Debug.Log("Add a InputUIDebug to the scene");
                }
                return instance;
            }
        }



        [SerializeField] private Text textToShow;

        [SerializeField] private Text tiltText;

        public void ShowText(string newText)
        {
            textToShow.text = newText;
        }

        public void ShowTiltText(string tiltDir)
        {
            tiltText.text = "Tilting " + tiltDir;
        }
    }
}

