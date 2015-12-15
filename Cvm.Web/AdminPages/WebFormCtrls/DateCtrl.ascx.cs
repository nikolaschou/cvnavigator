using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Napp.Web.Auto.Annotations;

namespace Cvm.Web.AdminPages.WebFormCtrls
{
    public partial class DateCtrl : System.Web.UI.UserControl, ISimpleWebControl, IControlWithInit, IControlWithRequired
    {
        /// <summary>
        /// Gets and set dates on one of the formats:
        /// - dd-mm-yy(yy)?
        /// - dd/mm/yy(yy)?
        /// - dd.mm.yy(yy)?
        /// - ddmmyy(yy)?
        /// </summary>
        public object ValueOfControl
        {
            get
            {
                string text = this.TextBox0.Text;
                int year=0, month=0, day=0;
                try
                {
                if (text==null || text.Trim().Length==0) return null;
                char[] seps = new char[] {'-','/','.'};
                if (text.IndexOfAny(seps)>=-1)
                {
                    String[] parts = text.Split(seps);
                    year = int.Parse(parts[2]);
                    month = int.Parse(parts[1]);
                    day = int.Parse(parts[0]);
                }else
                {
                    day = int.Parse(text.Substring(0, 2));
                    month = int.Parse(text.Substring(2, 2));
                    year = int.Parse(text.Substring(4));
                }

                if (year < 100)
                {
                    //yy is used
                    year += 2000;
                }
                    return new DateTime(year, month, day);
                } catch(Exception e)
                {
                    InvalidDataException e2 = new InvalidDataException("Date invalid: dd-mm-yyyy="+day+"-"+month+"-"+year,e);
                    e2.Data["Date"] = text;
                    throw e2;
                }
            }
            set
            {
                if (value==null) this.TextBox0.Text = null;
                else
                {
                    DateTime val = (DateTime)value;
                    this.TextBox0.Text =Pad(val.Day) + "-" + Pad(val.Month) + "-" + val.Year;
                    /*
                     * If we use JQuery Tools, include this:
                     * DateTime val = (DateTime)value;
                    //As we use Jquery Tools calender feature 
                    //we must output a rfc3339 formatted string yyyy-mm-dd
                    //It will be converted by JTools date handler

                    this.TextBox0.Text = val.ToString("yyyy'-'MM'-'dd");
                     * */
                }
            }
        }

        public string FieldContentId
        {
            get { return this.TextBox0.FieldContentId; }
            set { this.TextBox0.FieldContentId = value; }
        }

        private string Pad(int n)
        {
            if (n<10) return "0" + n;
            else return "" + n;
        }

        public void Initialize(params string[] initParms)
        {
            if (initParms.Length>=1)
            {
                String css = initParms[0];
                this.TextBox0.CssClass = css;
            }
        }

        public bool Required
        {
            set { this.TextBox0.Required = value; }
        }
    }
}