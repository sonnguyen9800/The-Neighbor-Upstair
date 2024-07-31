using Events;
using Godot;
using SisyphusFramework.Popup;
using System;
using TCU.Data;
using TCU.Manager;

namespace TCU.UI
{
    public partial class SettingPopup : PopupBase
    {
        public enum Placement
        {
            MainMenu,
            InGame,
        }

        public struct Param
        {
            public Placement Placement;
        }
        [Export]
        Control _tglMainMenu = null;
        [Export]
        Control _tglInGame = null;

        [Export] Slider _sliderInGame = null;
        [Export] Slider _sliderMenu = null;


        Slider _activeSlider = null;
        //Data
        private Param _param;
        private Placement _placement = Placement.MainMenu;
        private bool _resumeDiaOnClosed = true;
        
        private float _volume = 0;

        public override void OnShowing(object param = null)
        {
            base.OnShowing(param);
            _param = (Param)param;
            _placement = _param.Placement;

            HandlePlacement();
            _resumeDiaOnClosed = true;
            if (_placement == Placement.InGame)
                DialogicUtils.PauseDialogic();
            SetupMasterVolume();
        }
        public void SetMasterVolumeDb(float sliderValue)
        {
            var dbVolume = Mathf.LinearToDb(sliderValue);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), dbVolume);
            GD.Print(dbVolume);
        }
        public void OnMasterVolumeSetEnded(bool _ended)
        {
            // Save
            if (_ended == false) return;

            var sliderValue = _activeSlider.Value;
            var dbVolume = Mathf.LinearToDb(sliderValue);
            PlayerPreferencesManager.Instance.SaveMasterVolume(sliderValue);
        }
        private void SetupMasterVolume()
        {
            var dataVolume = PlayerPreferencesManager.Instance.LoadMasterVolume();
            _activeSlider.Value = dataVolume;
        }
        public override void OnHidden()
        {
            base.OnHidden();
            if (_placement == Placement.InGame && _resumeDiaOnClosed)
                DialogicUtils.ResumeDialogic();

        }
        private void HandlePlacement()
        {
            switch(_placement)
            {
                case Placement.MainMenu:
                    UIGameUtils.ControlVisible(_tglMainMenu, true);
                    UIGameUtils.ControlVisible(_tglInGame, false);
                    _activeSlider = _sliderMenu;
                    break;
                case Placement.InGame:
                    UIGameUtils.ControlVisible(_tglMainMenu, false);
                    UIGameUtils.ControlVisible(_tglInGame, true);
                    _activeSlider = _sliderInGame;
                    break;
            }
        }

        #region Mode: In-Game
        public void OnSaveButtonClicked()
        {
            SceneManager.Instance.PlayUIClickSound();

            PopupManager.Instance.OpenPopup(TCUPopupDefine.Save);

        }

        public void OnLoadButtonClicked()
        {
            SceneManager.Instance.PlayUIClickSound();

            PopupManager.Instance.OpenPopup(TCUPopupDefine.Load);

        }

        public async void OnQuitToMainMenuClicked()
        {
            SceneManager.Instance.PlayUIClickSound();
            string titleFormat = string.Format("Do you want to quit?");
            await UIInfoAlert.Instance.OpenConfirmationBox(
            title: string.Format("[center]{0}[/center]", titleFormat),
            onCancel: () =>
            {

            },
            onConfirm: () =>
            {
                if (_placement == Placement.InGame)
                {
                    UIMainGameEvents.Instance.Publish(UIGameEvent.OnExitFromInGame);
                    _resumeDiaOnClosed = false;
                }
            });



            ClosePopup();
        }


        #endregion

    }

}
