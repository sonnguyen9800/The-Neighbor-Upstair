using Events;
using Godot;
using SisyphusFramework;
using SisyphusFramework.Popup;
using System;
using System.Threading.Tasks;
using TCU.Manager;
using TCU.UI;
using static Events.UIMainGameEvents;

namespace TCU.Scene
{
    public partial class SceneMain : SceneBase, ISceneController
    {
        private enum Panel
        {
            Control,
        }
        [Export] private Control _uiMainControl = null;
        [Export] private Control _uiCreditsButton = null;

        private bool _lockButtonInput = false;

        public override void _EnterTree()
        {
            base._EnterTree();
            UIGameUtils.ControlVisible(_uiCreditsButton, TCUGameDefine.CheatAllowSeeCreditsInMenu);
        }
        public void Setup(object param = null)
        {

        }


        public async Task Activate(object param = null)
        {
            ControlVisible(true);
            _uiMainControl.Modulate = new Color(1, 1, 1, 0);
            UIGameUtils.ControlVisible(_uiMainControl, true);

            await Task.Delay(1000);
            await TweenUtils.TweenAlphaFadeIn(parentNode: this, tweenTarget: _uiMainControl);
            _lockButtonInput = false;
         
            UIMainGameEvents.Instance.Subscribe(UIGameEvent.LockUIButton, OnUILock);
        }

        private void OnUILock(object sender, EventArgs e)
        {
            LockUIToggleArgs args = e as LockUIToggleArgs;
            if (args == null) return;
            _lockButtonInput = args.IsLocked;
        }

        public void Deactivate(object param)
        {
            ControlVisible(false);
            UIMainGameEvents.Instance.Unsubscribe(UIGameEvent.LockUIButton, OnUILock);
        }

        #region Button
        public async void OnNewGameStart()
        {
            if (_lockButtonInput)
                return;
            _lockButtonInput = true;
            SceneManager.Instance.PlayUIClickSound();

            UIMainGameEvents.Instance.Publish(UIGameEvent.LockUIButton, new LockUIToggleArgs { IsLocked = true });
            await Task.Delay(1000);
            await TweenUtils.TweenAlphaFadeOut(parentNode: this, tweenTarget: _uiMainControl);

            UIMainGameEvents.Instance.Publish(UIGameEvent.OnNewGameEnter);
  
            return;
        }
        public void OnContinueGameClick()
        {
            if (_lockButtonInput)
                return;
            SceneManager.Instance.PlayUIClickSound();

            PopupManager.Instance.OpenPopup(TCUPopupDefine.Load);
        }
        public void OnSettingGameClick()
        {
            if (_lockButtonInput)
                return;
            SceneManager.Instance.PlayUIClickSound();

            if (TCUGameDefine.CheatDisplaySettingFromMainMenu)

                PopupManager.Instance.OpenPopup(TCUPopupDefine.Setting, new SettingPopup.Param { Placement = SettingPopup.Placement.InGame });
            else
                PopupManager.Instance.OpenPopup(TCUPopupDefine.Setting, new SettingPopup.Param { Placement = SettingPopup.Placement.MainMenu });

        }
        public async void OnQuitGameClick()
        {
            if (_lockButtonInput)
                return;
            SceneManager.Instance.PlayUIClickSound();
            await UIInfoAlert.Instance.OpenConfirmationBox(
              title: "Are you sure you want to quit?",
              onCancel: () =>
              {
                  GD.Print("Do nothing");
              },
              onConfirm: () =>
              {
                  GetTree().Quit();
              });
        }

        public void OnViewCreditClick()
        {
            if (TCUGameDefine.CheatAllowSeeCreditsInMenu == false)
                return;
            if (_lockButtonInput)
                return;
            UIMainGameEvents.Instance.Publish(UIGameEvent.OnCreditsGameClick);
            UIMainGameEvents.Instance.Publish(UIGameEvent.LockUIButton, new LockUIToggleArgs { IsLocked = true });


        }
        #endregion


    }
}