using Godot;
using System.Reflection.Emit;
using System.Threading.Tasks;
using TCU.Manager;
using static TCU.TCUGameDefine;

namespace TCU.UI
{
    public partial class UISaveItem : Control
    {
        [Export] private Button _label;
        private SaveSlot _slot = 0;
        ISavePersistent _parent = null;
        private bool _isSaved = false;
        internal void Setup(ISavePersistent popup, SaveSlot slot)
        {
            _parent = popup;
            _slot = slot;
            SetupTextTimeSave();
        }

        private void SetupTextTimeSave(string forceData = "")
        {
            if (!string.IsNullOrEmpty(forceData))
            {
                _label.Text = DateUtils.ConvertDateFromDialogicSave(forceData);
                return;
            }
    
            var data = DialogicUtils.GetInfoDialogicSaveSlot(_slot);
            _isSaved = !string.IsNullOrEmpty(data);
            if (string.IsNullOrEmpty(data))
                _label.Text = _slot.ToString();
            else
                _label.Text = DateUtils.ConvertDateFromDialogicSave(data);
        }

        public async void OnClickSave()
        {
            SceneManager.Instance.PlayUIClickSound();

            if ( !_isSaved )
            {
                var stringDateSaved = _parent.OnInteraction(_slot);
                SetupTextTimeSave(stringDateSaved);
                return;
            }
           
            string title = _parent.PersistentPopup == PersistentPopup.Save ? "Save" : "Load";
            string titleFormat = string.Format("Do you want to {0}" , title);
            await UIInfoAlert.Instance.OpenConfirmationBox(
               title: string.Format("[center]{0}[/center]", titleFormat),
               onCancel: () =>
               {

               },
               onConfirm: () =>
               {
                   var stringDateSaved = _parent.OnInteraction(_slot);
                   SetupTextTimeSave(stringDateSaved);
               });
            return;
        }


    }

}
