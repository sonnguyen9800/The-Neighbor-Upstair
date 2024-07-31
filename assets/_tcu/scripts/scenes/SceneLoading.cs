using Godot;
using SisyphusFramework;
using SisyphusFramework.Popup;
using System;
using System.Threading;
using System.Threading.Tasks;
using TCU;

public partial class SceneLoading : BaseSingleton<SceneLoading>
{
	[Export] private Control _loadingUI = null;
	[Export] private RichTextLabel _loadingTextTitle = null;
	private int _currentStateIndex = 0;
	private const float _timeDelayAnim = 0.8f;
	private bool _isRunning = false;

	private readonly string[] _loadingTextStates = { 
		"Loading", "Loading.", "Loading..", "Loading..." 
	};
	private CancellationTokenSource _cancellationTokenSource;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UIGameUtils.ControlVisible(_loadingUI, false);
		SetupLoading();
		_isRunning = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void SetupLoading()
	{
		_cancellationTokenSource = new CancellationTokenSource();
	}

	#region Loading Game
	public void StopAnimation()
	{
		if (!_isRunning)
			return;
		_cancellationTokenSource.Cancel();
		UIGameUtils.ControlVisible(_loadingUI, false);
		_isRunning = false;
        PopupManager.Instance.ToggleLocked(false);

    }

    public void StartAnimation()
	{
		if (_isRunning)
			return;
        DialogicUtils.PauseDialogic();
        _cancellationTokenSource = new CancellationTokenSource();
		_isRunning = true;
		UIGameUtils.ControlVisible(_loadingUI, true);
		PopupManager.Instance.ToggleLocked(true);
		AnimateLoadingText(_cancellationTokenSource.Token);

	}

	private async Task AnimateLoadingText(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			_loadingTextTitle.Text = _loadingTextStates[_currentStateIndex];
			_currentStateIndex = (_currentStateIndex + 1) % _loadingTextStates.Length;
			await Task.Delay(TimeSpan.FromSeconds(_timeDelayAnim), cancellationToken);
		}
	}

	#endregion
}
