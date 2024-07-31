
using static TCU.TCUGameDefine;

namespace TCU.UI
{
    public enum PersistentPopup
    {
        Save,
        Load
    }
    internal interface ISavePersistent
    {
        public string OnInteraction(SaveSlot slot);
        public PersistentPopup PersistentPopup { get;  }
        
    }
}
