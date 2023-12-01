using System;

namespace DesignDB_Library.Operations
{
    /// <summary>
    /// Event args to update BOM processing progress
    /// </summary>
    public class BOMProgressEventArgs    :   EventArgs
    {
        public int bomCount;
        public int currentBOMCount;
        public bool IsVisible;
    }
}
