using System;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Cvm.Web.CommonCtrl
{
    public partial class EditUserCtrl : System.Web.UI.UserControl
    {
        protected MembershipUser user;

        protected override void OnLoad(EventArgs e)
        {
            string userName = UserName;
            if (userName == null)
            {
                //Do nothing
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
                // Load the User Roles into checkboxes.
                UserRoles.DataSource = Roles.GetAllRoles();
                UserRoles.DataBind();

                user = Membership.GetUser(userName);
                //UserUpdateMessage.Text = "";

                // Disable checkboxes if appropriate:
                if (UserInfo.CurrentMode != DetailsViewMode.Edit)
                {
                    foreach (ListItem checkbox in UserRoles.Items)
                    {
                        checkbox.Enabled = false;
                    }
                }

                if (!IsPostBack)
                {
                    // Bind these checkboxes to the User's own set of roles.
                    string[] userRoles = Roles.GetRolesForUser(userName);
                    foreach (string role in userRoles)
                    {
                        ListItem checkbox = UserRoles.Items.FindByValue(role);
                        checkbox.Selected = true;
                    }
                    this.UserInfo.DataBind();
                }
            }
        }

        public string UserName;

        protected void UserInfo_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            //Need to handle the update manually because MembershipUser does not have a
            //parameterless constructor  

            user.Email = (string)e.NewValues[0];
            user.Comment = (string)e.NewValues[1];
            user.IsApproved = (bool)e.NewValues[2];

            try
            {
                // Update user info:
                Membership.UpdateUser(user);
                
                // Update user roles:
                UpdateUserRoles();

                //UserUpdateMessage.Text = "Update Successful.";

                e.Cancel = true;
                UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
            }
            catch (Exception)
            {
                //UserUpdateMessage.Text = "Update Failed: " + ex.Message;

                e.Cancel = true;
                UserInfo.ChangeMode(DetailsViewMode.ReadOnly);
            }
        }

        

        private void UpdateUserRoles()
        {
            foreach (ListItem rolebox in UserRoles.Items)
            {
                if (rolebox.Selected)
                {
                    if (!Roles.IsUserInRole(UserName, rolebox.Text))
                    {
                        Roles.AddUserToRole(UserName, rolebox.Text);
                    }
                }
                else
                {
                    if (Roles.IsUserInRole(UserName, rolebox.Text))
                    {
                        Roles.RemoveUserFromRole(UserName, rolebox.Text);
                    }
                }
            }
        }

        protected void DeleteUser(object sender, EventArgs e)
        {
            //Membership.DeleteUser(username, false); // DC: My apps will NEVER delete the related data.
            Membership.DeleteUser(UserName, true); // DC: except during testing, of course!
            Response.Redirect("users.aspx");
        }

        protected void UnlockUser(object sender, EventArgs e)
        {
            // Unlock the user.
            user.UnlockUser();

            // DataBind the GridView to reflect same.
            UserInfo.DataBind();
        }

        public override void DataBind()
        {
           
            //Do nothing, must call base.DataBind() to run general databinding for this form.
        }
    }
}