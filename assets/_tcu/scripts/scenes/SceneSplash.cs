using Godot;
using SisyphusFramework;
using System;
using System.Threading.Tasks;
using TCU;
using TCU.Scene;

namespace TCU.Scene
{

    public partial class SceneSplash : SceneBase, ISceneController
    {
        [Export] private RichTextLabel _studioNameLabelTxt = null;
        [Export] private Control _godotThumbnailImg = null;

        public void Setup(object param)
        {

        }
        public void Deactivate(object param = null)
        {
            ControlVisible(false);
        }

        async Task ISceneController.Activate(object param)
        {
            var color = _studioNameLabelTxt.Modulate;
            _studioNameLabelTxt.Modulate = new Color(color.R, color.G, color.B, 0);
            _godotThumbnailImg.Modulate = new Color(color.R, color.G, color.B, 0);
            ControlVisible(true);
            await RunTweenSplash();

        }

        private async Task RunTweenSplash()
        {
            await Task.Delay(1000);
            await TweenUtils.TweenAlphaFadeInAndOut(this, _studioNameLabelTxt);
            await Task.Delay(1000);
            await TweenUtils.TweenAlphaFadeInAndOut(this, _godotThumbnailImg);
            Deactivate();
        }

    }


}