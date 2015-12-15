using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cvm.Backend.Business.DataAccess;
using Cvm.Backend.Business.Resources;

namespace Cvm.Web.CommonCtrl
{
    public partial class ResourceResultGrid : System.Web.UI.UserControl
    {

        /// <summary>
        /// Determines whether the checkboxes should be visible.
        /// </summary>
        public bool ShowCheckBox = true;

        private IdfrStringList stringList;

        public IList<SysResource> ResourceList
        {
            set
            {
                this.ResultGrid.DataSource = value;
                AddFields(Fields);
                this.DataBind();
                if (ResultGrid.Rows.Count==0)
                {
                    Utl.Msg.PostMessage("ResourceResultGrid.NoResults");
                }
            }
        }

        public IdfrStringList ChosenResourceIds
        {
            get
            {
                if (this.stringList==null)
                {
                    this.stringList = new IdfrStringList(this.SelResourcesHiddenField.Value);                    
                }
                return stringList;
            }
        }

        /// <summary>
        /// Contains a list of fields that will be used as columns for the grid.
        /// </summary>
        public String[] Fields = new string[] { "FirstName", "LastName", "SysPrice" };

        /// <summary>
        /// Sets the fields as a string delimited by comma or semicolon.
        /// </summary>
        public String FieldsStr
        {
            set
            {
                Fields = value.Split(new char[] {',',';'});
            }
        }
        /// <summary>
        /// Adds fields to the resultset.
        /// </summary>
        /// <param name="fieldNames"></param>
        private void AddFields(params string[] fieldNames)
        {
            foreach (String fieldName in fieldNames)
            {
                BoundField field = new BoundField()
                                       {
                                           HeaderText = Utl.ContentHlp("ResourceResultGrid." + fieldName),
                                           DataField = fieldName,
                                           DataFormatString = "{0:d}",
                                           HtmlEncode = false
                                       };
                this.ResultGrid.Columns.Add(field);
            }
        }

        /// <summary>
        /// Clears the selected resource ID's.
        /// </summary>
        public void ClearSelection()
        {
            this.SelResourcesHiddenField.Value = "";
        }
    }
}