using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManger.Interface
{
    public interface IAddressManager
    {
        public bool AddUserAddress(AddressModel userDetails);
        List<AddressModel> GetUserDetails(int userId);
        bool EditAddress(AddressModel details);
    }
}
