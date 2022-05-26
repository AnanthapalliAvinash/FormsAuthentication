using FormsAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FormsAuthentication.Service
{
    public class Register
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
        public async Task<ResponseModel<string>> register(LoginViewModel user)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            if (user != null)
            {
                string md5_password = md5_string(user.Password);



                using (SqlConnection conn = new SqlConnection("Data Source=192.168.0.30;Initial Catalog=SqlPractice;User ID=User5;Password=CDev005#8"))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = string.Format("Insert INTO tbl_Users(Email,Password,Reg_Date) VALUES('{0}','{1}','{2}')", user.Email, md5_password, DateTime.Now.ToString());
                    cmd.Connection = conn;

                    conn.Open();
                    var result = await cmd.ExecuteNonQueryAsync();
                    conn.Close();
                    if (result == 1) //row changes in the database - successfull
                    {
                        response.message = "User has been registered!";
                        response.resultCode = 200;
                    }
                    else
                    {
                        response.message = "Unable to register User!";
                        response.resultCode = 500;
                    }

                }
            }
            return response;
        }
    }
}