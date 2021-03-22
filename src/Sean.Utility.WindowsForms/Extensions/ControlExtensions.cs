using System;
using System.Windows.Forms;

namespace Sean.Utility.WindowsForms.Extensions
{
    /// <summary>
    /// Extensions for <see cref="Control"/>
    /// </summary>
    public static class ControlExtensions
    {
        public static void InvokeIfRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }
    }
}
