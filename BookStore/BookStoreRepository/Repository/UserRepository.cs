using BookStoreModel;
using BookStoreRepository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreRepository.Repository
{
    public class UserRepository : IUserRepository
    {
        string secretKey;
        EmailService service;

        private readonly IConfiguration configuration;

        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            secretKey = configuration["SecretKey"];
            service = new EmailService(configuration);
        }

        SqlConnection sqlConnection;
        public bool Register(UserModel user)
        {
            sqlConnection = new SqlConnection(this.configuration.GetConnectionString("UserDbConnection"));
            //using (sqlConnection)
                try
                {
                    using (sqlConnection)
                    {
                        string storeprocedure = "StoreProcedureUserRegisteration";
                        SqlCommand sqlCommand = new SqlCommand(storeprocedure, sqlConnection);
                        user.Password = EncryptPassword(user.Password);
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        sqlCommand.Parameters.AddWithValue("@fullName", user.FullName);
                        sqlCommand.Parameters.AddWithValue("@email", user.Email);
                        sqlCommand.Parameters.AddWithValue("@password", user.Password);
                        sqlCommand.Parameters.AddWithValue("@phone", user.PhoneNumber);
                        sqlCommand.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();

                        var result = sqlCommand.Parameters["@user"].Value;
                        if (!(result is DBNull))
                        {
                            user._id = Convert.ToInt32(result);
                            return true;
                        }
                        else
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
        }
        public string Login(LoginModel loginDetails)
        {
            sqlConnection = new SqlConnection(this.configuration.GetConnectionString("UserDbConnection"));
            try
            {
                using (sqlConnection)
                {
                    string storeprocedure = "StoreProcedurLogin";
                    SqlCommand sqlCommand = new SqlCommand(storeprocedure, sqlConnection);
                    loginDetails.Password = this.EncryptPassword(loginDetails.Password);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@email", loginDetails.Email);
                    sqlCommand.Parameters.AddWithValue("@password", loginDetails.Password);
                    sqlCommand.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

                    var id = sqlCommand.Parameters["@user"].Value;
                    if (!(id is DBNull))
                    {
                        if (Convert.ToInt32(id) == 2)
                        {
                            GetUserDetails(loginDetails.Email);
                            return "Login is Successfull";
                        }
                        return "Incorrect Password";
                    }
                    return "Login Failed";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public void GetUserDetails(string email)
        {
            sqlConnection = new SqlConnection(this.configuration.GetConnectionString("UserDbConnection"));
            try
            {
                using (sqlConnection)
                {
                    string query = "SELECT * FROM UserS WHERE email = @email";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@email", email);
                    sqlConnection.Open();
                    UserModel customer = new UserModel();
                    SqlDataReader rd = sqlCommand.ExecuteReader();

                    if (rd.Read())
                    {
                        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        IDatabase database = connectionMultiplexer.GetDatabase();

                        database.StringSet(key: "FullName", rd.GetString("fullName"));
                        database.StringSet(key: "Phone", rd.GetInt64("phone").ToString());
                        database.StringSet(key: "userID", rd.GetInt32("_id").ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string ForgotPassword(string email)
        {
            sqlConnection = new SqlConnection(this.configuration.GetConnectionString("UserDbConnection"));
            try
            {
                using (sqlConnection)
                {
                    string storeprocedure = "spForgotPassword";
                    SqlCommand sqlCommand = new SqlCommand(storeprocedure, sqlConnection);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@email", email);
                    sqlCommand.Parameters.Add("@user", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

                    var id = sqlCommand.Parameters["@user"].Value;
                    if (!(id is DBNull))
                    {
                        string token = this.GenerateToken(email);

                        // Connection to Redis Server
                        ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        IDatabase database = connectionMultiplexer.GetDatabase();

                        // Set values to the Redis cache
                        database.StringSet(key: Convert.ToInt32(id).ToString(), token);

                        string msgBody = "You are receiving this mail because you(or someone else) have requested the reset of the password for your account.\n\n" +
                                    "Please click on the following link, or paste this into your browser to complete the process:\n\n" +
                                    "http://localhost:4200/reset-password/" + $"{token}/{Convert.ToInt32(id).ToString()}" + "\n\n" +
                                    "If you did not request this, please ignore this email and your password will remain unchanged.\n";


                        this.service.SendMailSmtp(email, msgBody);
                        return "Email has been sent";
                    }
                    return "User Doesnot Exists";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public bool ResetPassword(ResetPassword resetpassword)
        {
            sqlConnection = new SqlConnection(this.configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    //passing query in terms of stored procedure
                    SqlCommand sqlCommand = new SqlCommand("ResetPasssword", sqlConnection);
                    //passing command type as stored procedure
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    //adding the parameter to the strored procedure
                    var password = this.EncryptPassword(resetpassword.password);
                    sqlCommand.Parameters.AddWithValue("@id", resetpassword._id);
                    sqlCommand.Parameters.AddWithValue("@password", password);
                    sqlCommand.Parameters.Add("@result", SqlDbType.Int);
                    sqlCommand.Parameters["@result"].Direction = ParameterDirection.Output;
                    //checking the result
                    sqlCommand.ExecuteNonQuery();

                    var result = sqlCommand.Parameters["@result"].Value;
                    if (!(result is DBNull))
                        return true;
                    else
                        return false;
                }
                catch (ArgumentException ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
        }

        public string GenerateToken(string userName)
        {
            try
            {
                var key = Encoding.UTF8.GetBytes(this.configuration["SecretKey"]);
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
                SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                         new Claim(ClaimTypes.Name, userName)
                      }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                };
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
                return handler.WriteToken(token);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException(ex.Message);
            }
        }
        public string EncryptPassword(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }
    }
}
