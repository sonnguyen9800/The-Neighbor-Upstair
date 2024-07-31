using System.Threading.Tasks;

namespace TCU.Scene
{
    public interface ISceneController
    {
        public abstract Task Activate(object param = null);
        public abstract void Deactivate(object param = null);
        public abstract void Setup(object param = null);
    }
}
