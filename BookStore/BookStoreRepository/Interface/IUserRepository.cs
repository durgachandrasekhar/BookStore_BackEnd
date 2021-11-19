using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreRepository.Interface
{
    public interface IUserRepository
    {
        bool Register(UserModel user);
        string Login(LoginModel loginDetails);
        string ForgotPassword(string email);
        bool ResetPassword(ResetPassword resetpassword);
        string GenerateToken(string userName);
    }
}
