//using BookStoreManger.Interface;
//using BookStoreModel;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BookStore.Controller
//{
//    [ApiController]
//    public class BookController : ControllerBase
//    {
//        private readonly IBookManager manager;
//        public BookController(IBookManager manager)
//        {
//            this.manager = manager;

//        }

//        [HttpPost]
//        [Route("GetAllBooks")]
//        public IActionResult GetAllBooks()
//        {
//            var result = this.manager.GetAllBooks();
//            try
//            {
//                if (result.Count > 0)
//                {
//                    return this.Ok(new { Status = true, Message = "All Notes", data = result });

//                }
//                else
//                {
//                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Try again" });
//                }
//            }
//            catch (Exception e)
//            {
//                return this.NotFound(new ResponseModel<string>() { Status = false, Message = e.Message });
//            }
//        }

//        [HttpPost]
//        [Route("AddBook")]
//        public IActionResult AddBook([FromBody] BookModel bookDetails)
//        {
//            try
//            {
//                var result = this.manager.AddBook(bookDetails);
//                if (result)
//                {

//                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Added New Book Successfully !" });
//                }
//                else
//                {

//                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to add new user, Try again" });
//                }
//            }
//            catch (Exception ex)
//            {

//                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

//            }
//        }

//        [HttpGet]
//        [Route("GetBookDetail")]
//        public IActionResult GetBookDetails(int bookId)
//        {
//            var result = this.manager.GetBookDetails(bookId);
//            try
//            {
//                if (result != null)
//                {
//                    return this.Ok(new { Status = true, Message = "Book is retrived", data = result });

//                }
//                else
//                {
//                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Try again" });
//                }
//            }
//            catch (Exception e)
//            {
//                return this.NotFound(new ResponseModel<string>() { Status = false, Message = e.Message });
//            }
//        }

//        [HttpDelete]
//        [Route("RemoveBooks")]
//        public IActionResult RemoveBooks(int BookId)
//        {
//            try
//            {
//                var result = this.manager.RemoveBooks(BookId);
//                if (result)
//                {

//                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Removed Book Successfully !" });
//                }
//                else
//                {

//                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to Remove Book, Try again" });
//                }
//            }
//            catch (Exception ex)
//            {

//                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

//            }
//        }

//        [HttpPut]
//        [Route("UpdateBook")]
//        public IActionResult UpdateBooks(BookModel Bookdetail)
//        {
//            try
//            {
//                var result = this.manager.UpdateBooks(Bookdetail);
//                if (result)
//                {

//                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Book updated  Successfully !" });
//                }
//                else
//                {

//                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to updated Book , Try again" });
//                }
//            }
//            catch (Exception ex)
//            {

//                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

//            }
//        }

//        [HttpPost]
//        [Route("AddCustomerFeedBack")]
//        public IActionResult AddCustomerFeedBack([FromBody] FeedBackModel feedbackModel)
//        {
//            try
//            {
//                var result = this.manager.AddCustomerFeedBack(feedbackModel);
//                if (result)
//                {

//                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Added FeedBack Successfully !" });
//                }
//                else
//                {

//                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to add feedback, Try again" });
//                }
//            }
//            catch (Exception ex)
//            {

//                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

//            }
//        }

//        [HttpPost]
//        [Route("GetFeedback")]
//        public IActionResult GetFeedback(int bookid)
//        {
//            try
//            {
//                var result = this.manager.GetCustomerFeedBack(bookid);
//                if (result.Count > 0)
//                {
//                    return this.Ok(new { Status = true, Message = "Feedbackertrived", Data = result });
//                }
//                else
//                {

//                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Failed to add to wish list, Try again" });
//                }
//            }
//            catch (Exception ex)
//            {

//                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });

//            }
//        }
//    }
//}
