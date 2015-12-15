using System;
using System.Collections.Generic;
using System.Text;

namespace Cvm.Web.Code
{
    interface IControlWithCancel
    {
        /// <summary>
        /// Will be called by a controller when the save-button is hit.
        /// </summary>
        void OnCancel();
    }
}
