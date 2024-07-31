using Events;
using Godot;
using System;
using System.Threading.Tasks;
using TTCU;
using static Godot.ResourceLoader;
using static TCU.TCUGameDefine;
namespace TCU.Scene
{
    public partial class ScenePlay : SceneBase, ISceneController
    {
        private const string PlayNodePath = TCUGameNodeDefine.PlayNode;
        private ThreadLoadStatus status = ThreadLoadStatus.InProgress;
        private bool _isLoading = false;
        private Node _cachedPlayNode = null;

        public struct Param
        {
            public SaveSlot Slot;
            public bool IsNewGame;
        }

        private const byte _timeDelayFake = 3;
        private Param _param;
        private bool _isNewGame;
        private SaveSlot _slotLoaded;
        Callable _onGameEnd;

        // Called when the node enters the scene tree for the first time.
        public void Setup(object param = null)
        {

        }

        public override void _Ready()
        {
            base._Ready();
            _onGameEnd = new Callable(this, "OnGameEnd");
        }
        public async Task Activate(object param = null)
        {
            _param = (Param)param;
            _isNewGame = _param.IsNewGame;
            
            ControlVisible(true);
            SceneLoading.Instance.StartAnimation();
            LoadScene(PlayNodePath);
            UIMainGameEvents.Instance.Subscribe(UIGameEvent.OnExitFromInGame, OnExitFromInGame);
            await Task.CompletedTask;
        }

        private async void OnExitFromInGame(object sender, EventArgs e)
        {
            ExitGame();
            SceneLoading.Instance.StartAnimation();
            await Task.Delay(TimeSpan.FromSeconds(_timeDelayFake));
            Deactivate();
            UIMainGameEvents.Instance.Publish(UIGameEvent.BackToMainMenu);
            SceneLoading.Instance.StopAnimation();


        }

        #region Flow
        public override void _Process(double delta)
        {
            if (_isLoading == false)
                return;
            HandleLoadingStatus();
        }
        #endregion

        #region Play Node
        private void LoadScene(string path)
        {
            _isLoading = true;

            if (_cachedPlayNode != null)
            {
                HandleLoadingFinishedCached();
                return;
            }
            // Start loading the scene asynchronously
            _ = LoadThreadedRequest(path, cacheMode: CacheMode.Reuse);
        }
        private void HandleLoadingStatus()
        {
            status = LoadThreadedGetStatus(PlayNodePath);

            switch (status)
            {
                case ThreadLoadStatus.Loaded:
                    HandleLoadingFinished();
                    break;
                case ThreadLoadStatus.InProgress:
                case ThreadLoadStatus.Failed:
                default:
                    break;
            }
        }
        private async void HandleLoadingFinished()
        {
            CachingMode();
            await Task.Delay(TimeSpan.FromSeconds(_timeDelayFake));
            _isLoading = false;
            SceneLoading.Instance.StopAnimation();
            PlayGame();
        }
        private void CachingMode()
        {
            var playScenePrefab = LoadThreadedGet(path: PlayNodePath) as PackedScene;
            _cachedPlayNode = playScenePrefab.Instantiate();
            try
            {
                _cachedPlayNode.Connect(TCUSignal.EndGame, _onGameEnd);

            }
            catch (Exception ex)
            {
                GD.PrintErr(ex);
            }
            AddChild(_cachedPlayNode);
        }
        private async void HandleLoadingFinishedCached()
        {
            await Task.Delay(TimeSpan.FromSeconds(_timeDelayFake));
            _isLoading = false;
            SceneLoading.Instance.StopAnimation();
            PlayGame();
        }

        private void PlayGame()
        {
            UIMainGameEvents.Instance.Publish(UIGameEvent.OnNewGameStarted);
            if (_isNewGame)
            {
                _cachedPlayNode.Call("start_game");

            }
            else
            {
                _cachedPlayNode.Call("start_game_from_slot", _param.Slot.ToString());
            }

            DialogicUtils.ResumeDialogic();
        }

        private void OnGameEnd()
        {
            GD.Print("C#: ScenePlay: On Game End");
            Deactivate();
            UIMainGameEvents.Instance.Publish(UIGameEvent.OnCreditsGameClick);
            //OnExitFromInGame(null, null);
        }

        private void ExitGame()
        {
            DialogicUtils.HideDialogicLayout();
            _cachedPlayNode.Call(TCUSignal.ExitGame);
        }

        public void Deactivate(object param = null)
        {
            ControlVisible(false);
            UIMainGameEvents.Instance.Unsubscribe(UIGameEvent.OnExitFromInGame, OnExitFromInGame);

        }
        #endregion
    }
}