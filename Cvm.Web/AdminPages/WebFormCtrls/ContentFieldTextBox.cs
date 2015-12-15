using System.Web.UI.WebControls;
using Napp.Backend.Contents;
using Napp.Web.ExtControls;

namespace Nshop.Web.AdminPages.WebFormCtrls
{
    /// <summary>
    /// This provides a text box for editing a single content value
    /// for a single language. Typically there will be multiple of these
    /// text boxes, one for each language.
    /// </summary>
    public class ContentFieldTextBox : ExtTextBox
    {
        private IContentField contentField;
        private ILanguage language;

        public IContentField ContentField
        {
            get { return contentField; }
            set { contentField = value; }
        }

        public ILanguage Language
        {
            get { return language; }
            set { language = value; }
        }


        public void PopulateBack()
        {
            contentField[language] = this.Text;
        }
    }
}
