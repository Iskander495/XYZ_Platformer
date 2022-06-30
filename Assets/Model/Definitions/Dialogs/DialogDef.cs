using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model.Data.Dialogs
{
    [CreateAssetMenu(menuName = "Defs/Dialog", fileName = "Dialog")]
    public class DialogDef : ScriptableObject
    {
        [SerializeField] private DialogData _data;

        public DialogData Data => _data;
    }
}