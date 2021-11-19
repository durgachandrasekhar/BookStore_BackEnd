//using BookStoreManger.Interface;
//using BookStoreModel;
//using BookStoreRepository.Interface;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace BookStoreManger.Manager
//{
//    public class BookManager : IBookManager
//    {
//        private readonly IBookRepository repository;
//        public BookManager(IBookRepository repository)
//        {
//            this.repository = repository;
//        }
//        public List<BookModel> GetAllBooks()
//        {
//            try
//            {
//                return this.repository.GetAllBooks();
//            }
//            catch (Exception e)
//            {
//                throw new Exception(e.Message);
//            }
//        }

//        public bool AddBook(BookModel bookDetails)
//        {
//            try
//            {
//                return this.repository.AddBook(bookDetails);
//            }
//            catch (Exception e)
//            {
//                throw new Exception(e.Message);
//            }
//        }

//        public BookModel GetBookDetails(int bookId)
//        {
//            try
//            {
//                return this.repository.GetBookDetails(bookId);


//            }
//            catch (Exception e)
//            {
//                throw new Exception(e.Message);
//            }
//        }

//        public bool UpdateBooks(BookModel bookDetails)
//        {
//            try
//            {
//                return this.repository.UpdateBooks(bookDetails);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public bool RemoveBooks(int bookId)
//        {
//            try
//            {
//                return this.repository.RemoveBooks(bookId);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//        public bool AddCustomerFeedBack(FeedBackModel feedbackModel)
//        {
//            try
//            {
//                return this.repository.AddCustomerFeedBack(feedbackModel);
//            }
//            catch (Exception e)
//            {
//                throw new Exception(e.Message);
//            }
//        }
//        public List<FeedBackModel> GetCustomerFeedBack(int bookid)
//        {

//            try
//            {
//                return this.repository.GetCustomerFeedBack(bookid);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception(ex.Message);
//            }
//        }

//    }
//}
