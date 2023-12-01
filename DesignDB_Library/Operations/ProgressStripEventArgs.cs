using System;

namespace DesignDB_Library.Operations
{
    /// <summary>
    /// Event args to update progress bar on main menu
    /// </summary>
    public class ProgressStripEventArgs :   EventArgs
    {
        public int MaxCount;
        public int CurrentCount;
        public bool IsVisible;
    }
}
