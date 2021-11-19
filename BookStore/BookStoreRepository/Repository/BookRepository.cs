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
    public class BookRepository : IBookRepository
    {
        public BookRepository(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        SqlConnection sqlConnection;

        public bool AddBook(BookModel bookDetails)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("storeprocedureAddBook", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@BookName", bookDetails.BookName);
                    sqlCommand.Parameters.AddWithValue("@AuthorName", bookDetails.AuthorName);
                    sqlCommand.Parameters.AddWithValue("@Price", bookDetails.Price);
                    sqlCommand.Parameters.AddWithValue("@originalPrice", bookDetails.OriginalPrice);
                    sqlCommand.Parameters.AddWithValue("@BookDescription", bookDetails.BookDescription);
                    sqlCommand.Parameters.AddWithValue("@Image", bookDetails.Image);
                    sqlCommand.Parameters.AddWithValue("@Rating", bookDetails.Rating);
                    sqlCommand.Parameters.AddWithValue("@BookCount", bookDetails.BookCount);
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

        public bool UpdateBooks(BookModel bookDetails)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));

            using (sqlConnection)

                try
                {

                    SqlCommand sqlCommand = new SqlCommand("storeprocedureUpdateBook", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", bookDetails.BookId);
                    sqlCommand.Parameters.AddWithValue("@BookName", bookDetails.BookName);
                    sqlCommand.Parameters.AddWithValue("@AuthorName", bookDetails.AuthorName);
                    sqlCommand.Parameters.AddWithValue("@Price", bookDetails.Price);
                    sqlCommand.Parameters.AddWithValue("@originalPrice", bookDetails.OriginalPrice);
                    sqlCommand.Parameters.AddWithValue("@BookDescription", bookDetails.BookDescription);
                    sqlCommand.Parameters.AddWithValue("@Image", bookDetails.Image);
                    sqlCommand.Parameters.AddWithValue("@Rating", bookDetails.Rating);
                    sqlCommand.Parameters.AddWithValue("@BookCount", bookDetails.BookCount);
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

        public BookModel GetBookDetails(int bookId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("StoreprocedureGetBook", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
                    BookModel booksModel = new BookModel();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        booksModel.AuthorName = reader["AuthorName"].ToString();
                        booksModel.BookName = reader["BookName"].ToString();
                        booksModel.BookDescription = reader["BookDescription"].ToString();
                        booksModel.Price = Convert.ToInt32(reader["Price"]);
                        booksModel.Image = reader["Image"].ToString();
                        booksModel.OriginalPrice = Convert.ToInt32(reader["OriginalPrice"]);
                        booksModel.BookCount = Convert.ToInt32(reader["BookCount"]);
                        booksModel.Rating = Convert.ToInt32(reader["Rating"]);
                    }
                    return booksModel;
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

        public bool RemoveBooks(int bookId)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {

                    SqlCommand sqlCommand = new SqlCommand("storeprocedureRemoveBook", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();


                    sqlCommand.Parameters.AddWithValue("@BookId", bookId);
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

        public List<BookModel> GetAllBooks()
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
            using (sqlConnection)
                try
                {
                    SqlCommand sqlCommand = new SqlCommand("spGetAllBooks", sqlConnection);

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();

                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        List<BookModel> bookList = new List<BookModel>();
                        while (reader.Read())
                        {
                            BookModel booksModel = new BookModel();
                            booksModel.BookId = Convert.ToInt32(reader["BookId"]);
                            booksModel.AuthorName = reader["AuthorName"].ToString();
                            booksModel.BookName = reader["BookName"].ToString();
                            booksModel.BookDescription = reader["BookDescription"].ToString();
                            booksModel.Price = Convert.ToInt32(reader["Price"]);
                            booksModel.Image = reader["Image"].ToString();
                            booksModel.OriginalPrice = Convert.ToInt32(reader["OriginalPrice"]);
                            booksModel.BookCount = Convert.ToInt32(reader["BookCount"]);
                            booksModel.Rating = Convert.ToInt32(reader["Rating"]);

                            bookList.Add(booksModel);
                        }
                        return bookList;
                    }
                    else
                    {
                        return null;
                    }
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

        //public bool AddCustomerFeedBack(FeedBackModel feedbackModel)
        //{
        //    sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
        //    using (sqlConnection)
        //        try
        //        {

        //            SqlCommand sqlCommand = new SqlCommand("storeprocedureAddFeedback", sqlConnection);
        //            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //            sqlConnection.Open();
        //            sqlCommand.Parameters.AddWithValue("@BookId", feedbackModel.bookId);
        //            sqlCommand.Parameters.AddWithValue("@UserId", feedbackModel.userId);
        //            sqlCommand.Parameters.AddWithValue("@Rating", feedbackModel.rating);
        //            sqlCommand.Parameters.AddWithValue("@FeedBack", feedbackModel.feedback);


        //            int result = sqlCommand.ExecuteNonQuery();

        //            if (result > 0)
        //                return true;
        //            else
        //                return false;

        //        }
        //        catch (Exception e)
        //        {
        //            throw new Exception(e.Message);
        //        }
        //        finally
        //        {
        //            sqlConnection.Close();
        //        }
        //}
        //public List<FeedBackModel> GetCustomerFeedBack(int bookid)
        //{
        //    sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("UserDbConnection"));
        //    try
        //    {
        //        sqlConnection.Open();
        //        SqlCommand cmd = new SqlCommand("StoreProcedurGetCustomerFeedback", sqlConnection)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };
        //        cmd.Parameters.AddWithValue("@bookid", bookid);
        //        List<FeedBackModel> feedbackList = new List<FeedBackModel>();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                FeedBackModel feedbackdetails = new FeedBackModel();
        //                feedbackdetails.userId = reader.GetInt32(0);
        //                feedbackdetails.customerName = reader.GetString("FullName");
        //                feedbackdetails.feedback = reader.GetString("Feedback");
        //                feedbackdetails.rating = reader.GetDouble("Rating");
        //                feedbackList.Add(feedbackdetails);
        //            }

        //        }
        //        return feedbackList;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //    finally
        //    {
        //        sqlConnection.Close();
        //    }
        //}

    }
}
