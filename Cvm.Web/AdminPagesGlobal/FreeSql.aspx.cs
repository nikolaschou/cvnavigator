using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Cvm.Web.Code;
using Napp.Backend.Hibernate;

public partial class AdminPages_FreeSql : System.Web.UI.Page
{

    protected override void OnPreInit(EventArgs e)
    {
        MasterPageHelper.Instance.OnPageInit(false);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.ConnectionStringList.DataSource = ConfigurationManager.ConnectionStrings;
            this.ConnectionStringList.DataBind();
        }
    }

    protected void OnClickExecute(object sender, EventArgs e)
    {
        string connectionString = this.ConnectionStringList.SelectedItem.Text;
        using (SqlConnection conn =
            (SqlConnection)
            new SqlConnection(connectionString))
        {
            conn.Open();
            if (this.DatabaseTextBox.Text.Length > 0)
            {
                conn.ChangeDatabase(this.DatabaseTextBox.Text);
            }
            else
            {
                //            conn.Open();
            }
            if (InitSql.Text.Length > 0)
            {
                using (IDbCommand cmd2 = conn.CreateCommand())
                {
                    cmd2.Connection = conn;
                    cmd2.CommandText = InitSql.Text;
                    cmd2.CommandType = CommandType.Text;

                    cmd2.ExecuteNonQuery();

                }
            }



            IDbCommand cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = InputSql.Text;
            cmd.CommandType = CommandType.Text;
            //        if (this.RunNonQuery.Checked)
            //        {
            //            
            //        }
            if (this.CheckBox1.Checked)
            {
                cmd.ExecuteNonQuery();
                msgLabel.Text += "<br/><b>Succesfully executed: " + InputSql.Text + "</b>";
            }
            else
            {
                IDbDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                this.Grid.DataSource = ds.Tables[0];
                this.Grid.DataBind();

            }

            conn.Close();
        }
    }
}
