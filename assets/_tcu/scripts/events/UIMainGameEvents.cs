using SisyphusFramework;
using System;
using static TCU.TCUGameDefine;

namespace Events {
    public enum UIGameEvent {
        OnNewGameEnter,
        OnNewGameStarted,
        OnGameLoaded,
        OnSettingGameClick,
        OnCreditsGameClick,
        OnQuitGameClick,
        OnExitFromInGame,
        BackToMainMenu,


        LockUIButton,
        UnlockUIButton,
    }

    public partial class UIMainGameEvents : SingletonEventHub<UIGameEvent>{

        public class LoadGameArgs : EventArgs
        {
            public SaveSlot Slot { get; set; }
            public DateTime SaveTime { get; set; }
        }

        public class LockUIToggleArgs : EventArgs
        {
            public bool IsLocked { get; set; }
        }
    }

}