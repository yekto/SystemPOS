using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using POSApi.Model;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace POSApi.Services
{
    public class POSService
    {
        private static object _lockObject = new object();

        public static DataTable ListToDataTable<T>(List<T> list, string _tableName)
        {
            DataTable dt = new DataTable(_tableName);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T t in list)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        internal DataTable Login(reqLog data,string _conString)
        {
            lock (_lockObject)
            {
                DataTable dt = new DataTable();
                using(SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[Login]"; 
                        command.CommandTimeout = 1000;

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Username", data.Uname);
                        command.Parameters.AddWithValue("@Password", data.Pass);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return dt;
            }
        }
        internal bool PostItem(DataTable datas, string _conString)
        {
            lock ( _lockObject)
            {
                bool conBool = false;
                using (SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[PostItem]";
                        command.CommandTimeout = 1000;

                        command.Parameters.AddWithValue("@DataItem", datas);

                        int ret = command.ExecuteNonQuery();

                        if (ret >= 1)
                        {
                            conBool = true;
                        }
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return conBool;
            }
        }
        internal DataTable GetItem(string Username,string _conString)
        {
            lock( _lockObject)
            {
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[GetItem]";
                        command.CommandTimeout = 1000;

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Username", Username);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return dt;
            }
        }
        public async Task<bool> UpdateItem(respItem updatedItem, string conString)
        {
            using (var connection = new SqlConnection(conString))
            {
                string query = "UPDATE [dbo].Item SET ItemName = @ItemName, Category = @Category, Price = @Price, Qty = @Qty, Path = @Path WHERE id = @id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", updatedItem.Id);
                    command.Parameters.AddWithValue("@ItemName", updatedItem.ItemName);
                    command.Parameters.AddWithValue("@Category", updatedItem.Category);
                    command.Parameters.AddWithValue("@Price", updatedItem.Price);
                    command.Parameters.AddWithValue("@Qty", updatedItem.Qty);
                    command.Parameters.AddWithValue("@Path", updatedItem.Path);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0; // Return true if an item was updated
                }
            }
        }
        public async Task<bool> DeleteItem(int id, string conString)
        {
            using (var connection = new SqlConnection(conString))
            {
                string query = "DELETE FROM [dbo].[Item] WHERE id = @id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0; // Return true if an item was deleted
                }
            }
        }
        internal DataTable GetCategory(string Username, string _conString)
        {
            lock (_lockObject)
            {
                DataTable dt = new DataTable();

                using (SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[GetCategory]";
                        command.CommandTimeout = 1000;

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Username", Username);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return dt;
            }
        }
        public string CreateJwtToken(string audiencePOS, string issuerPOS, string signingPOS,string name, string username, reqLog req)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingPOS));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim ("Username",username),
                new Claim ("Name", name)
            };
            var token = new JwtSecurityToken(
                issuer: issuerPOS,
                audience: audiencePOS,
                claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddDays(1))
                ;

            var tokken = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine(tokken);
            return tokken;
        }
    
        internal DataTable InputSales(POSmodel data, string _conString)
        {
            lock(_lockObject)
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(_conString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand();
                    try
                    {
                        command.Connection = con;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "[dbo].[InsertPOS]";
                        command.CommandTimeout = 1000;

                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@itemId", data.itemId);
                        command.Parameters.AddWithValue("@itemName", data.itemName);
                        command.Parameters.AddWithValue("@price", data.price);
                        command.Parameters.AddWithValue("@qty", data.qty);
                        command.Parameters.AddWithValue("@totalPrice", data.totalPrice);
                        command.Parameters.AddWithValue("@date", data.date);
                        command.Parameters.AddWithValue("@username", data.username);

                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = command;
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                return dt;
            }
        }
    }
}
