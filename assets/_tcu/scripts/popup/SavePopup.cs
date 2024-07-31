using Godot;
using SisyphusFramework.Popup;
using System;
using System.Collections.Generic;
using static TCU.TCUGameDefine;

namespace TCU.UI
{

	public partial class SavePopup : PopupBase, ISavePersistent
	{

        [Export] private UISaveItem saveSlot1 = null;
        [Export] private UISaveItem saveSlot2 = null;
        [Export] private UISaveItem saveSlot3 = null;


        public PersistentPopup PersistentPopup { get => PersistentPopup.Save;}


        public override void OnShowing(object param = null)
        {
            base.OnShowing(param);
            var _loadItem = new List<UISaveItem>();
            _loadItem.Add(saveSlot1);
            _loadItem.Add(saveSlot2);
            _loadItem.Add(saveSlot3);
            for (byte i = 0; i < _loadItem.Count; i++)
            {
                _loadItem[i].Setup(this, (SaveSlot)i);

            }
        }

        public string OnInteraction(SaveSlot slot)
        {
            UIInfoAlert.Instance.PlayText("Game saved!");
            return DialogicUtils.SaveDialogic(slot);
        }
    }

}
