using Godot;
using System;
using TCU.Manager;
using TCU.Models;
using static TCU.TCUGameDefine;

namespace TCU
{

    public static class DialogicUtils
    {
        private static Node _sceneUtilsNode = null;

        private static Node SceneUtilsNode
        {
            get
            {
                if (_sceneUtilsNode == null)
                {
                    _sceneUtilsNode = SceneManager.Instance.DialogicHelper;
                    return _sceneUtilsNode;
                }else
                { return _sceneUtilsNode; } 
            }
        }
        public static void PauseDialogic()
        {
            if (TCUGameDefine.LogEnable)
                GD.PrintRich("[color=yellow]Dialogic Pause[/color]");
            SceneUtilsNode.Call("DialogicPause");
        }

        internal static void ResumeDialogic()
        {
            if (TCUGameDefine.LogEnable)
                GD.PrintRich("[color=yellow]Dialogic Resume[/color]");
            SceneUtilsNode.Call("DialogicResume");
        }

        public static void QuickSave()
        {
            SceneUtilsNode.Call("QuickSave");

        }
        public static void QuickLoad()
        {
            SceneUtilsNode.Call("QuickSave");

        }

        public static void ShowDialogicLayout()
        {
            SceneUtilsNode.Call("ShowDialogicNode");
        }
        public static void HideDialogicLayout()
        {
            SceneUtilsNode.Call("HideDialogicNode");
        }

        public static string SaveDialogic(SaveSlot slot)
        {
            var stringResult = (string) SceneUtilsNode.Call("SaveDialogic", slot.ToString());
            return stringResult;
        }

        public static string GetInfoDialogicSaveSlot(SaveSlot slot)
        {
            var stringResult = (string) SceneUtilsNode.Call("GetInfoDialogicSaveSlot", slot.ToString());
            return stringResult;
        }

    }
}