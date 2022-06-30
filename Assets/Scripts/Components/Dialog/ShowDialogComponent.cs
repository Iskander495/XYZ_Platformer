using Components.UI.Hud.Dialogs;
using Model.Data;
using Model.Data.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        private DialogBoxController _dialogBox;

        public DialogData Data
        {
            get
            {
                switch(_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void Show()
        {
            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<DialogBoxController>();

            _dialogBox.ShowDialog(Data);
        }

        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }

        public enum Mode
        {
            Bound,
            External
        }
    }
}