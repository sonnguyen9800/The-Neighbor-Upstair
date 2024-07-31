using Events;
using Godot;
using SisyphusFramework.Popup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Events.UIMainGameEvents;
using static TCU.TCUGameDefine;


namespace TCU.UI
{
    public partial class LoadPopup : PopupBase, ISavePersistent
    {


        [Export] private UISaveItem saveSlot1 = null;
        [Export] private UISaveItem saveSlot2 = null;
        [Export] private UISaveItem saveSlot3 = null;


        PersistentPopup ISavePersistent.PersistentPopup => PersistentPopup.Load;

        public string OnInteraction(SaveSlot slot)
        {

            var data = DialogicUtils.GetInfoDialogicSaveSlot(slot);
            if (string.IsNullOrEmpty(data))
            {
                UIInfoAlert.Instance.PlayText("Invalid");
                return string.Empty;

            }
            var dateTime = DateUtils.ParseDateString(data);
            UIMainGameEvents.Instance.Publish(UIGameEvent.OnGameLoaded, new LoadGameArgs
            {
                Slot = slot,
                SaveTime =  dateTime
            });
            UIMainGameEvents.Instance.Publish(UIGameEvent.LockUIButton, new LockUIToggleArgs { IsLocked = true });
            ClosePopup();
            return String.Empty;
        }

        public override void OnShowing(object param = null)
        {
            base.OnShowing(param);
            var _saveItems = new List<UISaveItem>();
            _saveItems.Add(saveSlot1);
            _saveItems.Add(saveSlot2);
            _saveItems.Add(saveSlot3);
            for (byte i = 0; i < _saveItems.Count; i++)
            {
                _saveItems[i].Setup(this, (SaveSlot)i);

            }
        }
    }

}
