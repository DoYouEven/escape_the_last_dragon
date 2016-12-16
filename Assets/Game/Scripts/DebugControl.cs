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
        [Header("Ui")]
        [SerializeField]
        private Text displayText;

        [SerializeField]
        private Image normalBotton;
        [SerializeField]
        private Image warningBotton;
        [SerializeField]
        private Image errorBotton;

        [SerializeField]
        private Color enableColor;
        [SerializeField]
        private Color disableColor;

        private List<DebugText> textList = new List<DebugText>();

        private bool showNormalText = true;
        private bool showWarningText = true;
        private bool showErrorText = true;

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
            ChangeButtonColor(normalBotton, showNormalText);
            RefreshDisplayText();
        }
        //Show and hide warning debugs
        public void ToggleShowWarnings()
        {
            showWarningText = !showWarningText;
            ChangeButtonColor(warningBotton, showWarningText);
            RefreshDisplayText();
        }
        //Show and hide error debugs
        public void ToggleShowErrors()
        {
            showErrorText = !showErrorText;
            ChangeButtonColor(errorBotton, showErrorText);
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
        private void DisplayTextOfType(DebugText nText)
        {
            switch (nText.DebugTextType)
            {
                case DebugType.Normal:
                    if (showNormalText)
                    {
                        displayText.text += "<color=#008000ff>" + nText.TextToShow + "</color>" + "\n";
                    }
                    break;
                case DebugType.Warning:
                    if (showWarningText)
                    {
                        displayText.text += "<color=#ffa500ff>" + nText.TextToShow + "</color>" + "\n";
                    }
                    break;
                case DebugType.Error:
                    if (showErrorText)
                    {
                        displayText.text += "<color=#ff0000ff>" + nText.TextToShow + "</color>" + "\n";
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

