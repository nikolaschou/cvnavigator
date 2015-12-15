using System;
using System.Collections;

namespace Cvm.Web.Code
{
    /// <summary>
    /// Contains helper methods to hold progress bar data.
    /// </summary>
    public class ProgressBarHelper
    {
        public static IEnumerable SearchCvWizardSteps
        {
            get
            {
                return GetProgressBar("SearchCvWizard",3);
            }
        }

        private static IEnumerable GetProgressBar(String contentPrefix, int size)
        {
            for (int i = 0; i < size; i++)
            {
                yield return Utl.ContentHlp(contentPrefix + "." + size);
            }
        }
    }
}
