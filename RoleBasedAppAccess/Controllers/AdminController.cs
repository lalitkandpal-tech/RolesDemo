using RoleBasedAppAccess.CustomFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace RoleBasedAppAccess.Controllers
{
    public class AdminController : Controller
    {
       [AuthLog(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Upload/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Create a DataTable.
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Id", typeof(string)),
                                new DataColumn("Email", typeof(string)),
                                //new DataColumn("EmailConfirmed",typeof(bool)),
                                new DataColumn("PasswordHash",typeof(string)),
                                new DataColumn("SecurityStamp", typeof(string)),
                                new DataColumn("PhoneNumber",typeof(string)),
                               // new DataColumn("PhoneNumberConfirmed", typeof(bool)),
                                //new DataColumn("TwoFactorEnabled",typeof(bool)),
                                //new DataColumn("LockoutEndDateUtc",typeof(DateTime)),
                                //new DataColumn("LockoutEnabled",typeof(bool)),
                                 //new DataColumn("AccessFailedCount",typeof(int)),
                                 new DataColumn("UserName",typeof(string)),
                                 new DataColumn("CommunicationAddress",typeof(string))
                });


                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);

                //Execute a loop over the rows.
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        dt.Rows.Add();
                        int i = 0;

                        //Execute a loop over the columns.
                        foreach (string cell in row.Split(','))
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell;
                            i++;
                        }
                    }
                }

                string conString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.AspNetUsers";

                        //[OPTIONAL]: Map the DataTable columns with that of the database table
                        sqlBulkCopy.ColumnMappings.Add("Id", "Id");
                        sqlBulkCopy.ColumnMappings.Add("Email", "Email");
                        //sqlBulkCopy.ColumnMappings.Add("EmailConfirmed", "EmailConfirmed");
                        sqlBulkCopy.ColumnMappings.Add("PasswordHash", "PasswordHash");
                        sqlBulkCopy.ColumnMappings.Add("SecurityStamp", "SecurityStamp");
                        sqlBulkCopy.ColumnMappings.Add("PhoneNumber", "PhoneNumber");
                        //sqlBulkCopy.ColumnMappings.Add("PhoneNumberConfirmed", "PhoneNumberConfirmed");
                        //sqlBulkCopy.ColumnMappings.Add("TwoFactorEnabled", "TwoFactorEnabled");
                        //sqlBulkCopy.ColumnMappings.Add("LockoutEndDateUtc", "LockoutEndDateUtc");
                        //sqlBulkCopy.ColumnMappings.Add("LockoutEnabled", "LockoutEnabled");
                        //sqlBulkCopy.ColumnMappings.Add("AccessFailedCount", "AccessFailedCount");
                        sqlBulkCopy.ColumnMappings.Add("UserName", "UserName");
                        sqlBulkCopy.ColumnMappings.Add("CommunicationAddress", "CommunicationAddress");

                        con.Open();
                        sqlBulkCopy.BulkCopyTimeout = 300;
                        sqlBulkCopy.WriteToServer(dt);
                        
                        con.Close();
                    }
                }
            }

            return View();
        }
    }
}