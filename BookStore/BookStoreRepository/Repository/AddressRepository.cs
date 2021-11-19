using BookStoreModel;
using BookStoreRepository.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace BookStoreRepository.Repository
{
    public class AddressRepository : IAddressRepository
    {
        public AddressRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        SqlConnection sqlConnection;
        public bool AddUserAddress(AddressModel userDetails)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            using (sqlConnection)

                try
                {

                    SqlCommand sqlCommand = new SqlCommand("spAddUserDetails", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@address", userDetails.Address);
                    sqlCommand.Parameters.AddWithValue("@city", userDetails.City);
                    sqlCommand.Parameters.AddWithValue("@state", userDetails.State);
                    sqlCommand.Parameters.AddWithValue("@type", userDetails.Type);
                    sqlCommand.Parameters.AddWithValue("@userId", userDetails.UserId);

                    int result = sqlCommand.ExecuteNonQuery();
                    if (result > 0)
                        return true;
                    else
                        return false;

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
        }
        
        public List<AddressModel> GetUserDetails(int userId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            try
            {
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("spGetUSerDetails", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader readData = cmd.ExecuteReader();
                List<AddressModel> userdetaillist = new List<AddressModel>();
                if (readData.HasRows)
                {
                    while (readData.Read())
                    {
                        AddressModel userDetail = new AddressModel();
                        userDetail.AddressId = readData.GetInt32("AddressId");
                        userDetail.Address = readData.GetString("address");
                        userDetail.City = readData.GetString("city").ToString();
                        userDetail.State = readData.GetString("state");
                        userDetail.Type = readData.GetString("type");
                        userdetaillist.Add(userDetail);
                    }
                }
                return userdetaillist;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public bool EditAddress(AddressModel userDetails)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            using (sqlConnection)

                try
                {

                    SqlCommand sqlCommand = new SqlCommand("spUpdateUserDetails", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@address", userDetails.Address);
                    sqlCommand.Parameters.AddWithValue("@city", userDetails.City);
                    sqlCommand.Parameters.AddWithValue("@state", userDetails.State);
                    sqlCommand.Parameters.AddWithValue("@type", userDetails.Type);
                    sqlCommand.Parameters.AddWithValue("@addressID", userDetails.AddressId);
                    sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                    sqlCommand.Parameters["@result"].Direction = ParameterDirection.Output;
                    sqlCommand.ExecuteNonQuery();
                    var result = sqlCommand.Parameters["@result"].Value;
                    if (result.Equals(1))
                        return true;
                    else
                        return false;

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
        }
    }
}
