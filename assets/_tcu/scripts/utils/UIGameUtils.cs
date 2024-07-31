using Godot;

namespace TCU {


    public static class UIGameUtils {

        public static void ControlVisible(Control controlNode, bool enable){
            controlNode.Visible = enable;
        }
    }
}