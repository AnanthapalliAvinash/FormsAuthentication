using FormsAuthentication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FormsAuthentication.Service
{
    public class login
    {
        public string md5_string(string password)
        {
            string md5_password = string.Empty;
            using (MD5 hash = MD5.Create())
            {
                md5_password = string.Join("", hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("x2")));
            }

            return md5_password;
        }
        public async Task<ResponseModel<string>> Login(LoginViewModel user)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            if (user != null)
            {
                string md5_password = md5_string(user.Password);

                using (SqlConnection conn = new SqlConnection("Data Source=192.168.0.30;Initial Catalog=SqlPractice;User ID=User5;Password=CDev005#8"))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = string.Format("Select * FROM tbl_Users WHERE password = '{0}' and Email='{1}'", md5_password, user.Email);
                    cmd.Connection = conn;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    await Task.Run(() => da.Fill(dt));

                    if (dt.Rows.Count > 0)
                    {
                        response.Data = JsonConvert.SerializeObject(dt);
                        response.resultCode = 200;
                    }
                    else
                    {
                        response.message = "User Not Found!";
                        response.resultCode = 500;
                    }


                }
            }
            return response;
        }
    }
}