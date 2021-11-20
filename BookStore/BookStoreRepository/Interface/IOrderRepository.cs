using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreRepository.Interface
{
    public interface IOrderRepository
    {
        bool PlaceTheOrder(List<CartModel> orderdetails);
        List<OrderModel> GetOrderList(int userId);
    }
}
