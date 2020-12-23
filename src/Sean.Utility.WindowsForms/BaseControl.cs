using System;
using System.Windows.Forms;

namespace Sean.Utility.WindowsForms
{
    public class BaseControl : Control
    {
        public void InvokeIfRequired(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
    }
}
