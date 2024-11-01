using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PnT.SongClient.UI.Controls
{

    /// <summary>
    /// Interface for Song application user controls.
    /// </summary>
    public interface ISongControl
    {
        /// <summary>
        /// Dispose used resources from Song control.
        /// </summary>
        void DisposeControl();

        /// <summary>
        /// Dispose child Song control.
        /// </summary>
        void DisposeChildControl();

        /// <summary>
        /// Select menu option to be displayed.
        /// </summary>
        string SelectMenuOption();
    }
}
