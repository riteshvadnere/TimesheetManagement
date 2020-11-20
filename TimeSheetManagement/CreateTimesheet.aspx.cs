using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace TimeSheetManagement
{
    public partial class CreateTimesheet : System.Web.UI.Page
    {
        
        string connString = @"Data Source=DESKTOP-5F9S36O\SQLEXPRESSNEW;Initial Catalog=TimesheetMgmt;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        protected void Page_Load(object sender, EventArgs e)
        {
            //email = Request.QueryString["email"];
          if (!Page.IsPostBack)
          { 
           
           using (SqlConnection conn = new SqlConnection(connString))
           {
                    try
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("FillDrpDwn", conn);
                        cmd.Parameters.AddWithValue("Email",EmployeePanelMaster.email);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds);

                        DropDownList1.DataTextField = "Project_Name";
                        DropDownList1.DataSource = ds;
                        DropDownList1.DataBind();
                        DropDownList1.Items.Insert(0, "Select");


                    }
                    catch (Exception ex)
                    { 
                    
                    }
           }
          }
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            DateTime input = Convert.ToDateTime(TextBox1.Text);
            if (input > DateTime.Now)
            {
                String cstext = "alert('Cannot Select Future Dates');";
                cs.RegisterStartupScript(cstype, "PopupScript", cstext, true);
            }
            else
            {
                if (DropDownList1.SelectedValue == "Select")
                {
                    String cstext = "alert('Please Select All entries');";
                    cs.RegisterStartupScript(cstype, "PopupScript", cstext, true);
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        try
                        {
                            conn.Open();
                            SqlCommand cmd = new SqlCommand("CreateTimesheet", conn);
                            cmd.Parameters.AddWithValue("Email", EmployeePanelMaster.email);
                            cmd.Parameters.AddWithValue("Project_Name", DropDownList1.Text);
                            cmd.Parameters.AddWithValue("Date", TextBox1.Text);
                            cmd.Parameters.AddWithValue("Task_ID", TextBox4.Text);
                            cmd.Parameters.AddWithValue("Task_Desc", TextBox2.Text);
                            cmd.Parameters.AddWithValue("Efforts", TextBox3.Text);
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataReader sdr = cmd.ExecuteReader();
                            String cstext = "alert('Timesheet Created');";
                            cs.RegisterStartupScript(cstype, "PopupScript", cstext, true);
                            TextBox1.Text = "";
                            TextBox2.Text = "";
                            TextBox3.Text = "";
                            TextBox4.Text = "";
                            DropDownList1.SelectedValue = null;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
        }
    }
}