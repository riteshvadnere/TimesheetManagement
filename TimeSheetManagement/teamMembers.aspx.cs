using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TimeSheetManagement
{
    public partial class teamMembers : System.Web.UI.Page
    {
        string projectID = "";
        SqlConnection conn;
        string connstring = @"Data Source=DESKTOP-5F9S36O\SQLEXPRESSNEW;Initial Catalog=TimesheetMgmt;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public void syncemp()
        {
            projectID = Request.QueryString["pid"];

            using ( conn = new SqlConnection(connstring))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("DisplayTeam", conn);
                    command.Parameters.AddWithValue("Project_ID", projectID);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dataAdp = new SqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    dataAdp.Fill(ds);

                    GridViewTeamMember.DataSource = ds;
                    GridViewTeamMember.DataBind();
                    conn.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            GridViewTeamMember.Visible = true;

            syncemp();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddTeamMember.aspx?pid="+projectID);
        }

        protected void GridViewTeamMember_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string Pid = Convert.ToString(GridViewTeamMember.DataKeys[e.RowIndex].Values["Project_ID"]);
            string Empid= Convert.ToString(GridViewTeamMember.DataKeys[e.RowIndex].Values["Emp_ID"]);
            
            using (conn = new SqlConnection(connstring))
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DeleteTeamMember", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Project_ID", Pid);
                    cmd.Parameters.AddWithValue("@Emp_ID", Empid);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    syncemp();
                }
                catch (Exception ex)
                { 
                }
        }

        protected void GridViewTeamMember_RowEditing(object sender, GridViewEditEventArgs e)
        {
           
            GridViewTeamMember.EditIndex = e.NewEditIndex;
            syncemp();
        }

        protected void GridViewTeamMember_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewTeamMember.EditIndex = -1;
            syncemp();
        }

        protected void GridViewTeamMember_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                conn = new SqlConnection(connstring);
                conn.Open();
                
                string ProjectID = Convert.ToString(GridViewTeamMember.DataKeys[e.RowIndex].Values["Project_ID"]);
                string EmpID = Convert.ToString(GridViewTeamMember.DataKeys[e.RowIndex].Values["Emp_ID"]);
                
                TextBox invol = GridViewTeamMember.Rows[e.RowIndex].FindControl("Invol") as TextBox;
                TextBox bill = GridViewTeamMember.Rows[e.RowIndex].FindControl("Bill") as TextBox;
                
                //Label1.Text = invol.Text + bill.Text;

                SqlCommand cmd = new SqlCommand("EditTeamMember", conn);
                ////SqlCommand cmd = new SqlCommand(" UPDATE Team_Members SET Involvement='"+invol.Text+ "',Billable='" + bill.Text + "'" +
                //" WHERE Project_ID = '"+ProjectID+"' and Emp_ID ='"+EmpID+"' ", conn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@Emp_ID", EmpID);
                cmd.Parameters.AddWithValue("@Project_ID", ProjectID);
                cmd.Parameters.AddWithValue("@Involvement", invol.Text);
                cmd.Parameters.AddWithValue("@Billable", bill.Text);

                cmd.ExecuteNonQuery();
                conn.Close();
                GridViewTeamMember.EditIndex = -1;
                syncemp();
            }
            catch (Exception ex)
            {

            }
        }
    }
}