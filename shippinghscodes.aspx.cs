using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
public partial class admin_shippinghscodes : System.Web.UI.Page
{
    string SelectString = "SELECT  hid, hscode, description FROM  thscodes order by ";
    string AddNewString = "SELECT  hid, hscode, description FROM  thscodes order by ";
    string DeleteString = "delete from thscodes where hid = @hid ";
    string UpdateNewString = "insert into thscodes (hscode, description) values (@hscode, @description)";
    string UpdateModifyString = "update thscodes set hscode = @hscode, description = @description where hid = @hid";
    string DefaultSortField = "hscode";
    //public DataView m_div1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGrid();
        }
    }
    void BindGrid()
    {
        try
        {
            SqlDataAdapter da = new SqlDataAdapter(SelectString + SortField, utils.GetConnection());
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            DataView v = ds.Tables["t"].DefaultView;
            dg1.DataSource = v;
            dg1.DataBind();
        }
        catch (Exception ex)
        {
            message.Text = ex.Message;
        }
    }
    public bool AddingNew
    {
        get
        {
            Object o = ViewState["AddingNew"];
            if (o == null)
                return false;
            return Convert.ToBoolean(o);
        }
        set
        {
            ViewState["AddingNew"] = value;
        }
    }
    public bool isEditing
    {
        get
        {
            Object o = ViewState["isEditing"];
            if (o == null)
                return false;
            return Convert.ToBoolean(o);
        }
        set
        {
            ViewState["isEditing"] = value;
        }
    }
    protected string SortField
    {
        get
        {
            object o = ViewState["SortField"];
            return (o == null) ? DefaultSortField : (String)o;
        }
        set
        {
            ViewState["SortField"] = value;
        }
    }
    public int GetSelectedComboIndex(DataView dv, Object oID)
    {
        string nID = oID.ToString() + "";
        if (nID == "")
            return -1;
        if (AddingNew)
            return -1;
        for (int a = 0; a < dv.Count; a++)
        {
            if (Convert.ToString(dv[a]["col1"]) == nID)
                return a;
        }
        return -1;
    }
    public string GetSelectedComboString(DataView dv, Object oID)
    {
        string nID = oID.ToString() + "";
        if (nID == "")
            return "";
        for (int a = 0; a < dv.Count; a++)
        {
            if (Convert.ToString(dv[a]["col1"]) == nID)
                return Convert.ToString(dv[a]["col2"]);
        }
        return "";
    }
    void CheckIsEditing(string CommandName)
    {
        if (dg1.EditItemIndex != -1)
        {
            if (CommandName != "Cancel" && CommandName != "Update")
            {
                message.Text = "Your changes have not been saved.<br/>Please press update to save your changes,<br/>or cancel to discard your changes, before selecting another item.";
                isEditing = true;
            }
        }
    }
    protected void dg1_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        dg1.EditItemIndex = -1;
        message.Text = "";
        BindGrid();
        isEditing = false;
        AddingNew = false;
    }
    protected void dg1_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        try
        {
            if (!isEditing)
            {
                message.Text = "";
                int keyValue = Convert.ToInt32(dg1.DataKeys[e.Item.ItemIndex]);
                SqlCommand cmd = new SqlCommand(DeleteString, utils.GetConnection());
                cmd.Parameters.AddWithValue("@hid", keyValue);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                dg1.CurrentPageIndex = 0;
                dg1.EditItemIndex = -1;
                BindGrid();
            }
        }
        catch (Exception ex)
        {
            message.Text = ex.Message;
        }
    }
    protected void dg1_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (!isEditing)
        {
            dg1.EditItemIndex = e.Item.ItemIndex;
            message.Text = "";
            BindGrid();
        }
    }
    protected void dg1_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        CheckIsEditing(e.CommandName);
    }
    protected void dg1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
    {
        if (!isEditing)
        {
            dg1.EditItemIndex = -1;
            dg1.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
    protected void dg1_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        dg1.CurrentPageIndex = 0;
        if (!isEditing)
        {
            if (SortField.EndsWith(" DESC"))
            {
                SortField = e.SortExpression; ;
            }
            else
            {
                if (SortField == e.SortExpression)
                    SortField = e.SortExpression + " DESC";
                else
                    SortField = e.SortExpression;
            }
            BindGrid();
        }
    }
    protected void dg1_UpdateCommand(object source, DataGridCommandEventArgs e)
    {
        try
        {
            message.Text = "";
            string sttitle = ((TextBox)dg1.Items[e.Item.ItemIndex].FindControl("txthscode")).Text.Trim();
            if (sttitle.Trim() == "")
                message.Text += "HS CODE is required<br/>";
            string st1 = ((TextBox)dg1.Items[e.Item.ItemIndex].FindControl("txtdescription")).Text.Trim(); 
            if (st1.Trim() == "")
                message.Text += "Description is required<br/>";
            
            if (message.Text != "")
                return;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = utils.GetConnection();
            if (AddingNew)
            {
                cmd.CommandText = UpdateNewString;
            }
            else
            {
                cmd.CommandText = UpdateModifyString;
                cmd.Parameters.AddWithValue("@hid", Convert.ToInt32(dg1.DataKeys[e.Item.ItemIndex]));
            }
            cmd.Parameters.AddWithValue("@hscode", sttitle);
            cmd.Parameters.AddWithValue("@description", st1);

            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            AddingNew = false;
            dg1.EditItemIndex = -1;
            BindGrid();
        }
        catch (Exception ex)
        {
            message.Text = ex.Message;
        }
    }
    protected void AddNew_Click(object sender, EventArgs e)
    {
        try
        {
            CheckIsEditing("");
            if (!isEditing)
            {
                AddingNew = true;
                message.Text = "";
                SqlDataAdapter da = new SqlDataAdapter(AddNewString + SortField, utils.GetConnection());
                DataSet ds = new DataSet();
                da.Fill(ds, "t");
                DataView v = ds.Tables["t"].DefaultView;
                object[] o = { 0, "", "" };
                ds.Tables["t"].Rows.Add(o);
                int recordCount = v.Count;
                //int recordCount = ds.Tables["Table1"].Rows.Count;
                if (recordCount > 1)
                {
                    recordCount -= 1;
                    dg1.CurrentPageIndex = recordCount / dg1.PageSize;
                    dg1.EditItemIndex = (int)System.Data.SqlTypes.SqlInt32.Mod(recordCount, dg1.PageSize);
                }
                else
                {
                    dg1.EditItemIndex = 0;
                }
                dg1.DataSource = v;
                dg1.DataBind();
            }
        }
        catch (Exception ex)
        {
            message.Text = ex.Message;
        }
    }
    protected void dg1_ItemCreated(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            LinkButton btn = (LinkButton)e.Item.Cells[1].Controls[0];
            btn.Attributes.Add("onclick", "return confirm('Do you want to delete this title?');");
        }
    }
}