using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManger.Interface
{
    public interface IOrderManager
    {
        bool PlaceTheOrder(List<CartModel> orderdetails);
        List<OrderModel> GetOrderList(int userId);
    }
}
