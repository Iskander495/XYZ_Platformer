using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Components.UI.Hud.Dialogs
{
    public class YesNoDialogController : MonoBehaviour
    {
        public void SetData(OptionDialogData data)
        {

        }
    }

    [Serializable]
    public class OptionDialogData
    {
        public string DialogText;
        public OptionData[] Options;
    }

    [Serializable]
    public class OptionData
    {
        public string Text;
        public UnityEvent OnSelect;
    }
}