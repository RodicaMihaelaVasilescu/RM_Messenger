using System;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
public class Handler : IHttpHandler
{

  public void ProcessRequest(HttpContext context)
  {
    SqlConnection con = new SqlConnection();
    con.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    // Create SQL Command 
    SqlCommand cmd = new SqlCommand();
    cmd.CommandText = "Select ImageName,Image from Images where ID =@ID";
    cmd.CommandType = System.Data.CommandType.Text;
    cmd.Connection = con;
    SqlParameter ImageID = new SqlParameter("@ID", System.Data.SqlDbType.Int);
    ImageID.Value = context.Request.QueryString["ID"];
    cmd.Parameters.Add(ImageID);
    con.Open();
    SqlDataReader dReader = cmd.ExecuteReader();
    dReader.Read();
    context.Response.BinaryWrite((byte[])dReader["Image"]);
    dReader.Close();
    con.Close();
  }
  public bool IsReusable
  {
    get
    {
      return false;
    }
  }


}