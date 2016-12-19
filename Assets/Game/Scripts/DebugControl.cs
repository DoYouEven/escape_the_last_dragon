using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PK.InfiniteRunner.Debuging
{
    public enum DebugType
    {
        Normal,
        Warning,
        Error
    }
    public class DebugControl : Singelton<DebugControl>
    {  
        private GameObject uiPrefab;
        private Text displayText;  
        private Image normalButton;
        private Image warningButton;
        private Image errorButton;
        private Button clearButton;
        //Colors For Buttons
        private Color32 enableColor = new Color32(4, 97, 182, 255);
        private Color32 disableColor = new Color32(139, 139, 139, 255);
        //Keeps track of all text
        private List<DebugText> textList = new List<DebugText>();
        //To keep track of what to show
        private bool showNormalText = true;
        private bool showWarningText = true;
        private bool showErrorText = true;

        void Awake()
        {
            //Spawn CanvasPrefab
            uiPrefab = (GameObject)Instantiate(Resources.Load("UI/DebugCanvas"));   
            //SetNormalButton and add the listener
            normalButton = GameObject.Find("Normal").GetComponent<Image>();
            if (normalButton != null)
            {
                normalButton.GetComponent<Button>().onClick.AddListener(ToggleShowNormals);
            } 
            //Set Warning button and add listener
            warningButton = GameObject.Find("Warning").GetComponent<Image>();
            if (warningButton != null)
            {
                warningButton.GetComponent<Button>().onClick.AddListener(ToggleShowWarnings);
            } 
            //Set errorButton and set listener
            errorButton = GameObject.Find("Error").GetComponent<Image>();
            if (errorButton != null)
            {
                errorButton.GetComponent<Button>().onClick.AddListener(ToggleShowErrors);
            } 
            //Set DisplayText
            displayText = GameObject.Find("TextDisplay").GetComponent<Text>(); 
            //Set Clear Button and set listener
            clearButton = GameObject.Find("Clear").GetComponent<Button>();
            if (clearButton != null)
            {
                clearButton.onClick.AddListener(Clear);
            } 
        } 
        //Recive Text To Display
        public void ShowText(string textToShow, DebugType debugType = DebugType.Normal)
        {
            DebugText nText = new DebugText(textToShow, debugType);
            textList.Add(nText);
            DisplayTextOfType(nText);
        }
        //Show and hide normal debugs
        public void ToggleShowNormals()
        {
            showNormalText = !showNormalText;
            ChangeButtonColor(normalButton, showNormalText);
            RefreshDisplayText();
        }
        //Show and hide warning debugs
        public void ToggleShowWarnings()
        {
            showWarningText = !showWarningText;
            ChangeButtonColor(warningButton, showWarningText);
            RefreshDisplayText();
        }
        //Show and hide error debugs
        public void ToggleShowErrors()
        {
            showErrorText = !showErrorText;
            ChangeButtonColor(errorButton, showErrorText);
            RefreshDisplayText();
        }
        //Change the color of the selected button
        private void ChangeButtonColor(Image image, bool enable)
        {
            if (enable)
            {
                image.color = enableColor;
            }
            else
            {
                image.color = disableColor;
            }
        }
        //Remove all debugs
        public void Clear()
        {
            textList.Clear();
            displayText.text = "";
        }
        //Update what debugs types to show
        private void RefreshDisplayText()
        {
            displayText.text = "";
            for (int i = 0; i < textList.Count; i++)
            {
                DisplayTextOfType(textList[i]);
            }
        }
        //Add a debug if its enable and asign color to the text
        private void DisplayTextOfType(DebugText debugText)
        {
            switch (debugText.DebugTextType)
            {
                case DebugType.Normal:
                    if (showNormalText)
                    {
                        displayText.text += "\n" + "<color=#008000ff>" + debugText.TextToShow + "</color>";
                    }
                    break;
                case DebugType.Warning:
                    if (showWarningText)
                    {
                        displayText.text += "\n" + "<color=#ffa500ff>" + debugText.TextToShow + "</color>";
                    }
                    break;
                case DebugType.Error:
                    if (showErrorText)
                    {
                        displayText.text += "\n" + "<color=#ff0000ff>" + debugText.TextToShow + "</color>";
                    }
                    break;
            }
        }
    }


    public class DebugText
    {
        public string TextToShow;
        public DebugType DebugTextType;

        public DebugText(string text, DebugType debugType)
        {
            TextToShow = text;
            DebugTextType = debugType;
        }
    }
}

