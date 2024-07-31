using Events;
using Godot;
using SisyphusFramework;
using SisyphusFramework.Popup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCU.Data;
using TCU.Scene;
using TCU.UI;
using TTCU;
using static Events.UIMainGameEvents;
using static TCU.TCUDefine;


namespace TCU.Manager
{
	public partial class SceneManager : BaseSingleton<SceneManager>
	{

		[Export] private SceneMain _sceneMain = null;
		[Export] private ScenePlay _scenePlay = null;
		[Export] private SceneSplash _sceneSplash = null;
		[Export] private SceneCredits _sceneCredits = null;

		[Export] private AudioStreamPlayer2D _playerBG = null;
        [Export] private AudioStreamPlayer2D _clickAudio = null;


        private bool _isInGame = false;

		public Node DialogicHelper { get { 
			if (_cachedDialogicNode != null) return _cachedDialogicNode;
				return null;
		}}
		private Node _cachedDialogicNode = null;

		private Dictionary<SceneName, ISceneController> _sceneCtrlDct = new Dictionary<SceneName, ISceneController>();
		// Called when the node enters the scene tree for the first time.
		private const string DialogicHelperNodeName = "DialogicHelper";
		public async override void _Ready()
		{
            CacheDialogicHandler();

            _sceneCtrlDct.Clear();
			_sceneCtrlDct.Add(SceneName.Main, _sceneMain);
			_sceneCtrlDct.Add(SceneName.Play, _scenePlay);
			_sceneCtrlDct.Add(SceneName.SplashScreen, _sceneSplash);
			_sceneCtrlDct.Add(SceneName.Credits, _sceneCredits);
			
			if (!TCUGameDefine.CheatSkipSplashScreen)
				await RunSplashScreen();
			await StartMainScene();
			UIMainGameEvents.Instance.Subscribe(UIGameEvent.OnNewGameEnter, OnNewGameEntered);
			UIMainGameEvents.Instance.Subscribe(UIGameEvent.OnNewGameStarted, OnNewGameStarted);
			UIMainGameEvents.Instance.Subscribe(UIGameEvent.BackToMainMenu, OnBackToMainMenu);
            UIMainGameEvents.Instance.Subscribe(UIGameEvent.OnGameLoaded, OnGameLoaded);


			UIMainGameEvents.Instance.Subscribe(UIGameEvent.OnCreditsGameClick, OnGameCreditsClick);
        }
		private async Task SetupMusicVolume()
		{

			var volumeData = PlayerPreferencesManager.Instance.LoadMasterVolume();
			var dbVolume = Mathf.LinearToDb(volumeData);
            _playerBG.VolumeDb = dbVolume;
			_playerBG.Play();
            
            await TweenAudioUtils.TweenSound(_playerBG, dbVolume, 1.5f);
            AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), dbVolume);


        }
        private void OnGameCreditsClick(object sender, EventArgs e)
        {

            var sceneMain = _sceneCtrlDct[SceneName.Main];
            sceneMain.Deactivate();
            var sceneCredits = _sceneCtrlDct[SceneName.Credits];
            sceneCredits.Activate();

        }

        private async void OnBackToMainMenu(object sender, EventArgs e)
        {

            await StartMainScene();
        }

		private void OnGameLoaded(object sender, EventArgs e) {

			
            var customArgs = e as LoadGameArgs;
            if (customArgs == null)
            {
				SmartLog.Print("Not valid LoadGameArgs", SmartLog.LogLevel.Error);
				return;
            }
            OnNewGameEntered(sender, customArgs);
            OnNewGameStarted(sender, customArgs);


        }
        private void CacheDialogicHandler()
		{
            Node rootNode = GetTree().Root;
			Node dialogicNode = rootNode.GetNode(DialogicHelperNodeName);
			if (dialogicNode != null)
			{
				_cachedDialogicNode = dialogicNode;
			}
        }

        public override void _ExitTree()
        {
            base._ExitTree();
            UIMainGameEvents.Instance.Unsubscribe(UIGameEvent.OnNewGameEnter, OnNewGameEntered);
            UIMainGameEvents.Instance.Unsubscribe(UIGameEvent.OnNewGameStarted, OnNewGameStarted);
            UIMainGameEvents.Instance.Unsubscribe(UIGameEvent.BackToMainMenu, OnBackToMainMenu);
            UIMainGameEvents.Instance.Unsubscribe(UIGameEvent.OnGameLoaded, OnGameLoaded);


			UIMainGameEvents.Instance.Unsubscribe(UIGameEvent.OnCreditsGameClick, OnGameCreditsClick);

        }


        public override void _Process(double delta)
		{
			base._Process(delta);
			HandleQuitButton();
		}

		private void HandleQuitButton()
		{
			if (Input.IsActionJustReleased(TCUAction.Escape))
			{
				if (UIInfoAlert.Instance.IsConfirmationOn)
				{
					UIInfoAlert.Instance.CloseConfirmationBox();
					return;
				}
				if (PopupManager.Instance.IsAnyPopupOpened) { 
					PopupManager.Instance.ClosePopup(); 
					return; 
				}
				if (_isInGame)
				{
                    PopupManager.Instance.OpenPopup(TCUPopupDefine.Setting, new UI.SettingPopup.Param { Placement= UI.SettingPopup.Placement.InGame});
					return;
                }
			}
        }

        private async Task RunSplashScreen(){
			await Task.Delay(1000);
			var sceneSplash = _sceneCtrlDct[SceneName.SplashScreen];
			await sceneSplash.Activate();
		}
		private async Task StartMainScene(){
            PopupManager.Instance.ToggleLocked(false);

            SetupMusicVolume();
            var sceneMain = _sceneCtrlDct[SceneName.Main];
			await sceneMain.Activate();
			sceneMain.Setup();
		}

		
		#region  Event
        private void OnNewGameEntered(object __, EventArgs args)
        {
            TweenAudioUtils.TweenSound(_playerBG, -30, 5.0f, TweenAudioUtils.TweenType.FadeOut);
            var customArgs = args as LoadGameArgs;
            if (customArgs == null)
			{
                // new game
                var scenePlay = _sceneCtrlDct[SceneName.Play];
				scenePlay.Activate(new ScenePlay.Param
				{
					IsNewGame = true
				});
				UIMainGameEvents.Instance.Publish(UIGameEvent.LockUIButton, new LockUIToggleArgs
				{
					IsLocked = true
				});
				return;
            }
			else
			{
                var scenePlay = _sceneCtrlDct[SceneName.Play];
				scenePlay.Activate(new ScenePlay.Param
				{
					IsNewGame = false,
					Slot = customArgs.Slot,
				});
            }

        }
		private void OnNewGameStarted(object sender, EventArgs e)
		{
            _playerBG.Stop();
            _isInGame = true;
            var sceneMain = _sceneCtrlDct[SceneName.Main];
            sceneMain.Deactivate();
        }

        #endregion


        #region Effect SFX
		public void PlayUIClickSound()
		{
			_clickAudio.Play();
		}
        #endregion
    }
}

