using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreManger.Interface
{
    public interface ICartManager
    {
        bool AddToCart(CartModel details);
        bool UpdateCart(CartModel cartDetail);
        List<CartModel> GetCartItems(int userId);
        bool DeleteFromCart(int cartId);

    }
}
