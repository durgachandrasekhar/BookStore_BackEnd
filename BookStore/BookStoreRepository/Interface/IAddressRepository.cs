﻿using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreRepository.Interface
{
    public interface IAddressRepository
    {
        bool AddUserAddress(AddressModel userDetails);
        List<AddressModel> GetUserDetails(int userId);
        bool EditAddress(AddressModel details);
    }
}
