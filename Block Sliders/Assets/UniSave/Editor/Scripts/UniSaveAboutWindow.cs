using System;
using UnityEngine;
using UnityEditor;

namespace UniSave.Editor
{
    public class UniSaveAboutWindow : EditorWindow
    {
        private Texture2D _unisaveLogo;

        private readonly string _text =

            "Version 2.1.1" + Environment.NewLine + "Copyright (c) 2014 MajinMHT"
            + Environment.NewLine + Environment.NewLine + Environment.NewLine +
            "Third Party Libs:"
            + Environment.NewLine + Environment.NewLine +
            "protobuf-net (v2 r668)" + Environment.NewLine +
            "by Marc Gravell."
            + Environment.NewLine + Environment.NewLine +
            "FileDB (v1.0.1)" + Environment.NewLine +
            "Copyright (c) 2011 Numeria Informática."
            + Environment.NewLine + Environment.NewLine +
            "SharpZipLib (v0.86.0)" + Environment.NewLine +
            "Copyright (c) 2010 Mike Krueger.";

        [MenuItem("Help/UniSave/About UniSave", false, 2)]
        private static void Init()
        {
            var window = CreateInstance<UniSaveAboutWindow>();
            window.title = "About UniSave";
            window.minSize = new Vector2(480, 360);
            window.maxSize = new Vector2(480, 360);
            window.ShowUtility();
        }

        private void OnEnable()
        {
            _unisaveLogo = (Texture2D) Resources.Load("UniSave_Logo", typeof (Texture2D));
            _unisaveLogo.hideFlags = HideFlags.DontSave;
        }

        private void OnGUI()
        {
            if (_unisaveLogo != null)
            {
                GUI.DrawTexture(
                    new Rect(Screen.width/2 - _unisaveLogo.width/2, 20, _unisaveLogo.width, _unisaveLogo.height),
                    _unisaveLogo);

                GUI.Label(new Rect(Screen.width/2 - _unisaveLogo.width/2, 120, _unisaveLogo.width, Screen.height), _text);
            }
        }
    }
}