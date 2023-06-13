using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AmanSharmaProject
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                binddata();
            }
        }
        private void binddata()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("select * from tblExpense", con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into tblExpense values(@uid,@name,@amount)", con);
            cmd.Parameters.AddWithValue("@uid", int.Parse(txtid.Text));
            cmd.Parameters.AddWithValue("@name", txtname.Text);
            cmd.Parameters.AddWithValue("@amount", int.Parse(txtamount.Text));
            cmd.ExecuteNonQuery();
            con.Close();
            binddata();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString);
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["ExpenseId"].ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from tblExpense where ExpenseId=@id", con);
            cmd.Parameters.AddWithValue("@id", id);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            binddata();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            binddata();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            binddata();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString);
            TextBox txtid = GridView1.Rows[e.RowIndex].FindControl("txt1") as TextBox;
            TextBox txtname = GridView1.Rows[e.RowIndex].FindControl("txt2") as TextBox;
            TextBox txtamount = GridView1.Rows[e.RowIndex].FindControl("txt3") as TextBox;
            int id = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["ExpenseId"].ToString());
            con.Open();
            SqlCommand cmd = new SqlCommand("update tblExpense set userid=@uid,receiver=@rec,Amount=@amount where ExpenseId=@id", con);
            cmd.Parameters.AddWithValue("@uid", txtid.Text);
            cmd.Parameters.AddWithValue("@rec", txtname.Text);
            cmd.Parameters.AddWithValue("@amount", txtamount.Text);
            cmd.Parameters.AddWithValue("@id", id);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            GridView1.EditIndex = -1;
            binddata();
        }
    }
}