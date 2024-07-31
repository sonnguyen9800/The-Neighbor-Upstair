#define SKIP_SPLASH_SCREEN1
#define FULL_SETTING_POPUP_OPENED_FROM_MAIN1
#define ALLOW_SEE_CREDITS_FROM_MAIN1
#define LOGGER_ENABLE

namespace TCU
{

    public static class TCUGameNodeDefine
    {
        public const string PlayNode = "res://assets/_tcu/resources/scenes/play_scene.tscn";
    }

    public static class TCUGameDefine
    {
#if LOGGER_ENABLE
        public const bool LogEnable = true;
#else
        public const bool LogEnable = false;

#endif
#if SKIP_SPLASH_SCREEN
        public const bool CheatSkipSplashScreen = true;
#else
        public const bool CheatSkipSplashScreen = false;

#endif
#if ALLOW_SEE_CREDITS_FROM_MAIN
        public const bool CheatAllowSeeCreditsInMenu = true;
#else
        public const bool CheatAllowSeeCreditsInMenu = false;

#endif
#if FULL_SETTING_POPUP_OPENED_FROM_MAIN
        public const bool CheatDisplaySettingFromMainMenu = true;
#else
        public const bool CheatDisplaySettingFromMainMenu = false;

#endif

        public enum GameState
        {
            Splash,
            MainMenu,
            Play,

        }
        public enum SaveSlot {
            Slot1,
            Slot2,
            Slot3,
        }
    }

    public static class TCUPopupDefine {
        public const string Setting = "Setting";
        public const string Save = "Save";
        public const string Load = "Load";
        public const string Confirm = "Confirm";
    }
    public static class TCUAction
    {
        public const string Escape = "Escape";
    }

    public static class TCUSignal {
        public const string EndGame = "endgame"; // copied from define.gd
        public const string ExitGame = "exit_game"; // copied from define.gd

    }
}