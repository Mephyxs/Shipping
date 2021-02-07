using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using Ionic.Zip;
using System.Text;
public partial class admin_loadinglist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCombo();
            BindList();
            BindGrid();
        }
    }
    void BindCombo()
    {
        try
        {
            using (SqlConnection con = utils.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("select 0 as col1, '[Select]' as col2 union SELECT countryid AS col1, country AS col2 FROM tcountries ORDER BY col2; select 0 as col1, '[Select]' as col2", con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                ddlCountry.DataSource = ds.Tables[0].DefaultView;
                ddlCountry.DataBind();
                ddlPort.DataSource = ds.Tables[1].DefaultView;
                ddlPort.DataBind();
            }
        }
        catch (Exception ex)
        {
            message.Text = ex.Message;
        }
    }
    void BindList()
    {
        try
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = utils.GetConnection())
            {
                SqlDataAdapter da;
                if (ddlPort.SelectedValue == "0")
                {
                    da = new SqlDataAdapter("w_loadinglist_containers", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@countryid", Convert.ToInt32(ddlCountry.SelectedValue));
                }
                else if (ddlPort.SelectedValue == "")
                {
                    da = new SqlDataAdapter("w_loadinglist_containers", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@countryid", Convert.ToInt32(ddlCountry.SelectedValue));
                }
                else
                {
                    da = new SqlDataAdapter("w_loadinglist_containers_ports", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@portid", Convert.ToInt32(ddlPort.SelectedValue));
                }
                da.Fill(ds, "t");
            }
            DataView v = ds.Tables["t"].DefaultView;
            v.Sort = "departureDate";
            dl1.DataSource = v;
            dl1.DataBind();
        }
        catch (Exception ex)
        {
            message.Text = ex.Message;
        }
    }
    void BindGrid()
    {
        try
        {
            if (dl1.Items.Count > 0 && dl1.SelectedIndex < 0)
                dl1.SelectedIndex = 0;
            DataSet ds = new DataSet();
            using (SqlConnection con = utils.GetConnection())
            {
                SqlDataAdapter da;
                if (ddlPort.SelectedValue == "0")
                {
                    da = new SqlDataAdapter("w_loadinglist", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@countryid", Convert.ToInt32(ddlCountry.SelectedValue));
                    if (!chkAll.Checked)
                        da.SelectCommand.Parameters.AddWithValue("@containerid", Convert.ToInt32(dl1.DataKeys[dl1.SelectedIndex]));
                    else
                        da.SelectCommand.Parameters.AddWithValue("@containerid", 0);
                    da.Fill(ds, "t");
                }
                /*else if (ddlPort.SelectedValue == "")
                {
                    da = new SqlDataAdapter("w_loadinglist", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@countryid", Convert.ToInt32(ddlCountry.SelectedValue));
                    if (!chkAll.Checked)
                        da.SelectCommand.Parameters.AddWithValue("@containerid", Convert.ToInt32(dl1.DataKeys[dl1.SelectedIndex]));
                    else
                        da.SelectCommand.Parameters.AddWithValue("@containerid", 0);
                    da.Fill(ds, "t");
                }*/
                else
                {
                    da = new SqlDataAdapter("w_loadinglist_port", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@countryid", Convert.ToInt32(ddlCountry.SelectedValue));
                    da.SelectCommand.Parameters.AddWithValue("@portid", Convert.ToInt32(ddlPort.SelectedValue));
                    if (!chkAll.Checked)
                        da.SelectCommand.Parameters.AddWithValue("@containerid", Convert.ToInt32(dl1.DataKeys[dl1.SelectedIndex]));
                    else
                        da.SelectCommand.Parameters.AddWithValue("@containerid", 0);
                    da.Fill(ds, "t");
                }
            }
            DataView v = ds.Tables["t"].DefaultView;
            v.Sort = SortField;
            dg1.DataSource = v;
            dg1.DataBind();
        }
        catch (Exception ex)
        {
            message.Text = ex.Message;
        }
    }
    protected string SortField
    {
        get
        {
            object o = ViewState["SortField"];
            return (o == null) ? "requesteddeldate" : (String)o;
        }
        set
        {
            ViewState["SortField"] = value;
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void dg1_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        message.Text = "";
        if (e.CommandName == "SELECTOR")
        {
            int keyValue = Convert.ToInt32(dg1.DataKeys[e.Item.ItemIndex]);
        }
    }
    protected void dg1_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
    {
        dg1.EditItemIndex = -1;
        dg1.CurrentPageIndex = e.NewPageIndex;
        BindGrid();
    }
    protected void dg1_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        dg1.CurrentPageIndex = 0;
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
        BindList();
        BindGrid();
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (SqlConnection con = utils.GetConnection())
        {
            string s = "select 0 col1, '[Select]' as col2 union select portid col1, port col2 from tports where countryid = @countryid";
            SqlDataAdapter da = new SqlDataAdapter(s, con);
            da.SelectCommand.Parameters.AddWithValue("@countryid", Convert.ToInt32(ddlCountry.SelectedValue));
            DataSet ds = new DataSet();
            da.Fill(ds);
            ddlPort.DataSource = ds.Tables[0].DefaultView;
            ddlPort.DataBind();
        }
        BindList();
        BindGrid();
    }
    protected void DataList1_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("Select", StringComparison.CurrentCultureIgnoreCase))
        {
            BindGrid();
        }
        else if (e.CommandName.Equals("Remover", StringComparison.CurrentCultureIgnoreCase))
        {

        }
        else if (e.CommandName.Equals("CREATEPDF", StringComparison.CurrentCultureIgnoreCase))
        {
            int keyValue = Convert.ToInt32(dl1.DataKeys[e.Item.ItemIndex]);
            CreatePDF(keyValue);
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (dl1.Items.Count == 0 || dl1.SelectedIndex < 0)
            return;
        SqlCommand cmd = new SqlCommand("update tshippingorders set containerid = @c where shippingorderid = @s", utils.GetConnection());
        cmd.Parameters.AddWithValue("@s", 0);
        cmd.Parameters.AddWithValue("@c", Convert.ToInt32(dl1.DataKeys[dl1.SelectedIndex]));
        for (int i = 0; i < dg1.Items.Count; i++)
        {
            if (((CheckBox)dg1.Items[i].FindControl("cbSelected")).Checked)
            {
                cmd.Parameters["@s"].Value = Convert.ToInt32(dg1.DataKeys[i]);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }
        BindList();
        BindGrid();
    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        SqlCommand cmd = new SqlCommand("update tshippingorders set containerid = null where shippingorderid = @s", utils.GetConnection());
        cmd.Parameters.AddWithValue("@s", 0);
        for (int i = 0; i < dg1.Items.Count; i++)
        {
            if (((CheckBox)dg1.Items[i].FindControl("cbSelected")).Checked)
            {
                cmd.Parameters["@s"].Value = Convert.ToInt32(dg1.DataKeys[i]);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
        }
        BindList();
        BindGrid();
    }
    protected void btnAddContainer_Click(object sender, EventArgs e)
    {
        if (dl1.Items.Count == 0 || dl1.SelectedIndex < 0)
            return;
        SqlCommand cmd = new SqlCommand("w_duplicatecontainer", utils.GetConnection());
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@containerid", Convert.ToInt32(dl1.DataKeys[dl1.SelectedIndex]));
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        BindList();
        BindGrid();
    }
    protected void btnDelContainer_Click(object sender, EventArgs e)
    {
        if (dl1.Items.Count == 0 || dl1.SelectedIndex < 0)
            return;
        SqlCommand cmd = new SqlCommand("w_deletecontainer", utils.GetConnection());
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@containerid", Convert.ToInt32(dl1.DataKeys[dl1.SelectedIndex]));
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        BindList();
        BindGrid();
    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void btnSelectAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < dg1.Items.Count; i++)
        {
            if (((CheckBox)dg1.Items[i].FindControl("cbSelected")).Checked == false)
            {
                ((CheckBox)dg1.Items[i].FindControl("cbSelected")).Checked = true;
            }
        }
    }
    protected void btndeSelectAll_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < dg1.Items.Count; i++)
        {
            if (((CheckBox)dg1.Items[i].FindControl("cbSelected")).Checked == true)
            {
                ((CheckBox)dg1.Items[i].FindControl("cbSelected")).Checked = false;
            }
        }

    }
    void CreatePDF(int containerid)
    {
        try
        {
            List<string> filesToInclude = new List<string>();
            clsManifest manifest = new clsManifest();
            manifest.Containerid = containerid;
            manifest.Force = true;
            manifest.ReportPath = Server.MapPath("~/pdf/manifests/" + containerid.ToString() + ".pdf");
            manifest.CreateReport();
            clsBOL bol = new clsBOL();
            bol.Force = true;
            bol.imagesfolder = Server.MapPath("~/images/");
            bol.logosFolder = Server.MapPath("~/images/logos/");
            bol.OrderID = 0;
            bol.ReportPath = "";
            SqlCommand cmd = new SqlCommand("select shippingorderid from tshippingorders where containerid = @containerid", utils.GetConnection());
            cmd.Parameters.AddWithValue("@containerid", containerid);
            cmd.Connection.Open();
            SqlDataReader r = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (r.Read())
            {
                filesToInclude.Add(Server.MapPath("~/pdf/bol/" + Convert.ToInt32(r["shippingorderid"]).ToString() + ".pdf"));
                bol.ReportPath = Server.MapPath("~/pdf/bol/" + Convert.ToInt32(r["shippingorderid"]).ToString() + ".pdf");
                bol.OrderID = Convert.ToInt32(r["shippingorderid"]);
                bol.CreateReport();
            }
            cmd.Connection.Close();


            //string sMappedPath = Server.MapPath("fodder");


            Response.Clear();
            Response.BufferOutput = false;

            System.Web.HttpContext c = System.Web.HttpContext.Current;
            String ReadmeText = "This is a zip file that was dynamically generated.\r\nIt contains a Manifest and all related Bills of Laden\r\n" +
                                ".\r\n";
            string archiveName = String.Format("manifestsBOL-{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd"));
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "inline; filename=\"" + archiveName + "\"");

            // In some cases, saving a zip directly to Response.OutputStream can present problems for the unzipper, especially on Macintosh.
            // To workaround that, you can save to a MemoryStream, then copy to the Response.OutputStream.
            using (MemoryStream ms = new MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    // the Readme.txt file will not be password-protected.
                    zip.AddEntry("Readme.txt", ReadmeText, Encoding.Default);
                    //zip.AddDirectory("manifest", "manifest");
                    //zip.AddDirectory("bol", "bol");
                    zip.AddFile(manifest.ReportPath, "");
                    // filesToInclude is a string[] or List<String>
                    if (filesToInclude.Count > 0)
                        zip.AddFiles(filesToInclude, "files");
                    zip.Save(ms);
                }
                // copy the memory stream to the Response.OutputStream
                ms.Position = 0;
                var b = new byte[1024];
                int n;
                while ((n = ms.Read(b, 0, b.Length)) > 0)
                    Response.OutputStream.Write(b, 0, n);
            }
            Response.Close();
        }
        catch (Exception ex)
        {
            message.Text = ex.Message;
        }
    }

    protected void ddlPort_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindList();
        BindGrid();
    }

    protected void btnloadedcontainer_Click(object sender, EventArgs e)
    {

        try
        {
            if (dl1.Items.Count > 0 && dl1.SelectedIndex < 0)
                dl1.SelectedIndex = 0;
            DataSet ds = new DataSet();
            using (SqlConnection con = utils.GetConnection())
            {
                SqlDataAdapter da;
                if (ddlPort.SelectedValue == "0")
                {
                    da = new SqlDataAdapter("w_loadinglist", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@countryid", Convert.ToInt32(ddlCountry.SelectedValue));
                    if (!chkAll.Checked)
                        da.SelectCommand.Parameters.AddWithValue("@containerid", Convert.ToInt32(dl1.DataKeys[dl1.SelectedIndex]));
                    else
                        da.SelectCommand.Parameters.AddWithValue("@containerid", 0);
                    da.Fill(ds, "t");
                }
                /*else if (ddlPort.SelectedValue == "")
                {
                    da = new SqlDataAdapter("w_loadinglist", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@countryid", Convert.ToInt32(ddlCountry.SelectedValue));
                    if (!chkAll.Checked)
                        da.SelectCommand.Parameters.AddWithValue("@containerid", Convert.ToInt32(dl1.DataKeys[dl1.SelectedIndex]));
                    else
                        da.SelectCommand.Parameters.AddWithValue("@containerid", 0);
                    da.Fill(ds, "t");
                }*/
                else
                {
                    da = new SqlDataAdapter("w_loadinglist_port", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@countryid", Convert.ToInt32(ddlCountry.SelectedValue));
                    da.SelectCommand.Parameters.AddWithValue("@portid", Convert.ToInt32(ddlPort.SelectedValue));
                    if (!chkAll.Checked)
                        da.SelectCommand.Parameters.AddWithValue("@containerid", Convert.ToInt32(dl1.DataKeys[dl1.SelectedIndex]));
                    else
                        da.SelectCommand.Parameters.AddWithValue("@containerid", 0);
                    da.Fill(ds, "t");
                }
            }
            DataView v = ds.Tables["t"].DefaultView;
            v.Sort = SortField;
            dg1.DataSource = v;
            dg1.DataBind();
        }
        catch (Exception ex)
        {
            message.Text = ex.Message;
        }
    }
}