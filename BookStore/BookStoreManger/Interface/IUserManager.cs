using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManger.Interface
{
    public interface IUserManager
    {
        bool Register(UserModel user);
        string Login(LoginModel loginDetails);
        //string ForgotPassword(string email);
        //bool ResetPassword(ResetPassword resetpassword);
        string GenerateToken(string userName);

    }
}
