using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events;
using Godot;
using SisyphusFramework;
using SisyphusFramework.Popup;

namespace TCU.Scene
{
    internal partial class SceneCredits : SceneBase, ISceneController
    {
        [Export] private bool _playEndGameText = true;
        [Export] private RichTextLabel _endGameText = null;

        [Export] private PackedScene _creditsScene = null;
        private Control _creditsObject = null;
        private AudioStreamPlayer _audioPlayer;


        


        private bool _didLoadCredits = false;

        private void LoadCredits()
        {
            var resourcesObject = _creditsScene;
            _creditsObject = (Control) resourcesObject.Instantiate();
            var color = _creditsObject.Modulate;
            _creditsObject.Modulate = new Godot.Color(color.R, color.G, color.B, 0);
            _creditsObject.Connect("ended", new Callable(this, "OnEndSignal"));
            _audioPlayer = _creditsObject.GetNode<AudioStreamPlayer>("musicPlayer");
            AddChild(_creditsObject);
        }
        private async void OnEndSignal()
        {
            TweenAudioUtils.TweenSound(_audioPlayer, -30f, 3.0f, TweenAudioUtils.TweenType.FadeOut);
            await TweenUtils.TweenAlphaFadeOut(this, _creditsObject, 3);
            _audioPlayer.Stop();

            if (_playEndGameText)
            {
                await TweenUtils.TweenAlphaFadeOut(this, _endGameText);
            }
            UIMainGameEvents.Instance.Publish(UIGameEvent.BackToMainMenu);
            RemoveChild(_creditsObject);
            _creditsObject.QueueFree();
            _creditsObject = null;
            Deactivate();

        }
        private void PlayCredits()
        {
            _creditsObject.Call("PlayCredits");

        }
        public async Task Activate(object param = null)
        {
            PopupManager.Instance.ToggleLocked(true);

            var color = _endGameText.Modulate;
            _endGameText.Modulate = new Godot.Color(color.R, color.G, color.B, 0);
            ControlVisible(true);
            if (_playEndGameText)
            {
                await TweenUtils.TweenAlphaFadeInAndOut(this, _endGameText);
            }

            await Task.Delay(1000);
            LoadCredits();
            PlayCredits();
            await TweenUtils.TweenAlphaFadeIn(this, _creditsObject);

            return;
        }

        public void Deactivate(object param = null)
        {
            ControlVisible(false);
            PopupManager.Instance.ToggleLocked(false);


        }

        public void Setup(object param = null)
        {
        }
    }
}
