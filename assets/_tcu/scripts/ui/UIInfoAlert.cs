using Godot;
using SisyphusFramework;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using TCU.Manager;
using static SisyphusFramework.TweenUtils;
namespace TCU.UI
{
    public partial class UIInfoAlert : BaseSingleton<UIInfoAlert>
    {
        public bool IsConfirmationOn { get => _isConfirmationOn; }

        [Export] private RichTextLabel _textAlertPrefab = null;

        // Yes, No
        [Export] private CanvasLayer _confirmBoxNode = null;
        [Export] private CanvasModulate _canvasModule = null;
        [Export] private RichTextLabel _titleText = null;
        private TaskCompletionSource<bool> _taskCompletionSource;
        private Action _onYes;
        private Action _onNo;

        private bool _isConfirmationOn = false;

        // Called when the node enters the scene tree for the first time.
        
        public override void _Ready()
        {
            _textAlertPrefab.Hide();
            HideConfirmationBox();

        }
        public async void PlayText(string content, AlertType type = AlertType.Normal)
        {
            var newText = _textAlertPrefab.Duplicate() as RichTextLabel;
            AddChild(newText);
            newText.Text = string.Format("[center]{0}[/center]", content);

            if (type == AlertType.Normal)
            {
                newText.Modulate = new Color(1,1, 1,0); // alpha set to 0
            }else
                newText.Modulate = new Color(1,0,0,0); // alpha set to 0
            newText.Show();
            await TweenAlphaFadeIn(this, newText, durationFadeIn: 0.1f);
            await TweenMoveUpAndFadeOut(this, newText, upLengh: 50.0f, duration: 2.0f);
            newText.QueueFree();

        }

        private void HideConfirmationBox()
        {
            _confirmBoxNode.Hide();
            _canvasModule.Color = new Color(1, 1, 1, 0);
            _isConfirmationOn = false;
            _onYes = null;
            _onNo = null;

        }

        private async Task ShowConfirmationBox()
        {
            _canvasModule.Color = new Color(1, 1, 1, 1);

            _confirmBoxNode.Show();
            await TweenUtils.TweenAlphaFadeIn(this, _canvasModule, durationFadeIn: 0.1f);
        }

        public async Task<bool> OpenConfirmationBox(
            string title,
            Action onConfirm = null, Action onCancel = null)
        {
            if (_isConfirmationOn)
                return false;
            _titleText.Text = string.Format("[center]{0}[/center]", title);
            _onYes = onConfirm;
            _onNo = onCancel;
            _isConfirmationOn = true;
            await ShowConfirmationBox();
            _taskCompletionSource = new TaskCompletionSource<bool>();

            return await _taskCompletionSource.Task;
        }

        private void OnYesButtonPressed()
        {
            _taskCompletionSource?.SetResult(true);
            _onYes?.Invoke();
            SceneManager.Instance.PlayUIClickSound();
            HideConfirmationBox();
        }

        private void OnNoButtonPressed()
        {
            SceneManager.Instance.PlayUIClickSound();

            _taskCompletionSource?.SetResult(false);
            _onNo?.Invoke();
            HideConfirmationBox();

        }

        public void CloseConfirmationBox()
        {
            _taskCompletionSource?.SetResult(false);
            _onNo?.Invoke();
            HideConfirmationBox();
        }
    }

}
