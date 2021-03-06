using BookStoreModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManger.Interface
{
    public interface IBookManager
    {
        List<BookModel> GetAllBooks();
        bool AddBook(BookModel bookDetails);
        BookModel GetBookDetails(int bookId);
        bool UpdateBooks(BookModel bookDetails);
        public bool RemoveBooks(int bookId);
        bool AddCustomerFeedBack(FeedBackModel feedbackModel);
        List<FeedBackModel> GetCustomerFeedBack(int bookid);
    }
}
