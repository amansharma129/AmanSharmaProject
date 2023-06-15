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
            SqlCommand cmd = new SqlCommand("select * from Orders", con);
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
            SqlCommand cmd = new SqlCommand("INSERT INTO Orders (CustomerName, DeliveryAddress, FoodItem, Quantity) VALUES (@CustomerName, @Address, @FoodItem, @Quantity)", con);
            cmd.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
            cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@FoodItem", txtFoodItem.Text);
            cmd.Parameters.AddWithValue("@Quantity", int.Parse(txtQuantity.Text));
            cmd.ExecuteNonQuery();
            con.Close();
            binddata();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString);
            int OrderID = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM tblOrder WHERE OrderID = @OrderID", con);
            cmd.Parameters.AddWithValue("@OrderID", OrderID);
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
            TextBox id = GridView1.Rows[e.RowIndex].FindControl("Orderid") as TextBox;
            TextBox CustomerName = GridView1.Rows[e.RowIndex].FindControl("txtCustomerName") as TextBox;
            TextBox DeliveryAddress = GridView1.Rows[e.RowIndex].FindControl("txtAddress") as TextBox;
            TextBox FoodItem = GridView1.Rows[e.RowIndex].FindControl("txt3") as TextBox;
            con.Open();
            string query = "UPDATE Orders SET CustomerName = @CustomerName, DeliveryAddress = @Address, FoodItem = @FoodItem, Quantity = @Quantity WHERE OrderID = @OrderID";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@CustomerName", txtCustomerName.Text);
            command.Parameters.AddWithValue("@Address", txtAddress.Text);
            command.Parameters.AddWithValue("@FoodItem", txtFoodItem.Text);
            command.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
            command.ExecuteNonQuery();
            con.Close();
            GridView1.EditIndex = -1;
            binddata();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            SearchData(searchTerm);
        }
        private void SearchData(string searchTerm)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM Orders WHERE CustomerName LIKE '%' + @searchTerm + '%'", con);
            cmd.Parameters.AddWithValue("@searchTerm", searchTerm);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count != 0)
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            binddata();
        }
    }
}