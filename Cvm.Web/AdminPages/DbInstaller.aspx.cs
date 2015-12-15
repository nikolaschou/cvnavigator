using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Cvm.Web.Code;
using Napp.Backend.DataAccess;
using Napp.Backend.Hibernate;
using Napp.Common.MessageManager;
using Napp.SchemaUpdate;
using Napp.SchemaUpdate.Execution;

namespace Cvm.Web.AdminPages.DbVersions
{
    public partial class DbInstaller : System.Web.UI.Page
    {
        private const int MAX_ROWS = 100;

        private SchemaManager mgr;

        protected override void OnPreInit(EventArgs e)
        {
            MasterPageHelper.Instance.OnPageInit(true);
        }
        private string GetRootPath()
        {
            return Server.MapPath("~/AdminPages/DbVersions");
        }


        protected override void OnInit(EventArgs e)
        {
            mgr = new SchemaManager(new DirectoryInfo(GetRootPath()));
            SetupRepeaters();
            AdminPages_AdminMasterPage.IsPopup = true;
        }

        private void SetupRepeaters()
        {
            rep1.Controls.Clear();
            rep2.Controls.Clear();
            this.SqlRep.Controls.Clear();
            this.PendingContentBox.Text = null;
            this.InstalledContentBox.Text = null;

            rep1.DataSource = mgr.GetPendingVersions();
            rep1.DataBind();

            rep2.DataSource = mgr.GetInstalledVersions();
            rep2.DataBind();
            string pending = mgr.GetMostRecentPendingVersion();
            if (pending!=null)
            {
                SqlExecutor exec = new SqlExecutor();
                mgr.InstallVersion(pending, exec);
                List<string> sql = exec.GetSqlStatementsWithoutGo();
                this.SqlRep.DataSource = sql;
                this.SqlRep.DataBind();
                this.PendingContentBox.DataBind();
                this.InstalledContentBox.DataBind();
            }
        }


        protected void OnClickRunSqlBtn(object sender, RepeaterCommandEventArgs e2)
        {
            RepeaterItem repeaterItem = e2.Item;
            HandleSqlForRepeaterItem(repeaterItem);
        }

        /// <summary>
        /// Handles the sql execution for a single repeater item and return true
        /// if no error happened.
        /// </summary>
        /// <param name="repeaterItem"></param>
        /// <returns></returns>
        private bool HandleSqlForRepeaterItem(RepeaterItem repeaterItem)
        {
            TextBox textBox = repeaterItem.FindControl("SqlTextBox") as TextBox;
            String sql = textBox.Text;
            String error=this.RunSql(sql);
            if (error!=null)
            {
                SetMessage(repeaterItem, "<xmp style='color:red'>"+error+"</xmp>");
                return false;
            } else
            {
                GetExeButton(repeaterItem).Visible = false;
                GetExeButton(repeaterItem).Enabled = false;
                repeaterItem.FindControl("AutoRunCheckBox").Visible = false;
                (repeaterItem.FindControl("SqlTextBox") as TextBox).Enabled = false;
                SetMessage(repeaterItem, "<span style='color:green'>OK!</span>");
                return true;
            }
        }

        private Button GetExeButton(RepeaterItem repeaterItem)
        {
            return (repeaterItem.FindControl("runSqlBtn") as Button);
        }

        private void SetMessage(RepeaterItem repeaterItem, string message)
        {
            (repeaterItem.FindControl("messageLabel") as Label).Text = message;
        }

        private String RunSql(string sql)
        {
            using (IDbConnection conn = HibernateMgr.Current.GetDirectSqlConnection())
            {
                conn.Open();
                try
                {
                    new DbManager().ExecuteSql(conn, sql);
                }
                catch (DbSqlException e)
                {
                    return e.Message;
                } catch(Exception e)
                {
                    return e.ToString();
                }
                conn.Close();
                return null;
            }
        }

        private DataTable RunQuery(string sql)
        {
            DataTable tb = null;
            using (IDbConnection conn = HibernateMgr.Current.GetDirectSqlConnection())
            {
                conn.Open();
                tb = new DbManager().QueryToTable(conn, sql,"table1");
                conn.Close();
            }
            return tb;
        }

        protected void OnClickRunAllSqlBtn(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in this.SqlRep.Items)
            {

                Button exeButton = GetExeButton(item);
                if (exeButton.Visible)
                {
                    CheckBox autoRunCheckBox = ((CheckBox)item.FindControl("AutoRunCheckBox"));
                    if (autoRunCheckBox.Checked)
                    {  
                        //It hasn't been executed
                        bool ok = HandleSqlForRepeaterItem(item);
                        if (!ok)
                        {
                            break;
                        }
                    }
                }
            }
        }

        

        /// <summary>
        /// Returns the pending file being compared with for building 
        /// the migration script.
        /// </summary>
        /// <returns></returns>
        protected string GetPendingFileForCompare()
        {
            return this.mgr.GetMostRecentPendingVersion();
        }
        /// <summary>
        /// Returns the installed file being compared with for building 
        /// the migration script.
        /// </summary>
        /// <returns></returns>
        protected string GetInstalledFileForCompare()
        {
            return this.mgr.GetMostRecentInstallation();
        }

        protected void OnClickMoveAllPendingBtn(object sender, EventArgs e)
        {
            string[] pendings = mgr.GetPendingVersions();
            int counter = 0;
            foreach (String p in pendings)
            {
                if (counter == 0)
                {
                    mgr.MoveToInstallFolder(p);
                    MessageManager.Current.PostMessage("CbInstaller.MovingFileToInstalled ", p);
                }
                else
                {
                    mgr.MoveToSkippedFolder(p);
                    MessageManager.Current.PostMessage("CbInstaller.MovingFileToSkipped ", p);
                }
                counter++;
            }
            this.SetupRepeaters();
        }

       
       
        protected void OnClickSkipLinkBtn(object sender, RepeaterCommandEventArgs e2)
        {
            mgr.MoveToSkippedFolder((string)e2.Item.DataItem);
            SetupRepeaters();
        }

        protected void OnClickRunCustomSqlBtn(object sender, EventArgs e)
        {
            String sql=this.CustomSqlTextBox.Text;
            DataTable dataTable = RunQuery(sql);
            int count = dataTable.Rows.Count;
            if (count>MAX_ROWS )
            {
                for (int i=count-1;i>=MAX_ROWS;i--)
                {
                    dataTable.Rows.RemoveAt(i);
                }
            }
            this.CustomSqlGrid.DataSource = dataTable;
            this.CustomSqlGrid.DataBind();
        }


        protected string GetPendingFileContent()
        {
            return this.mgr.GetMostRecentPendingFileContent();
        }
        protected string GetInstalledFileContent()
        {
            return this.mgr.GetMostRecentInstalledFileContent();
        }

        protected long CountLines(string sql)
        {
            Regex counter=new Regex("\n");
            return counter.Matches(sql).Count;
        }
    }
}
