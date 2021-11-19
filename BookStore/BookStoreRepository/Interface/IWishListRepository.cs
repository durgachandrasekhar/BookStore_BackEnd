using BookStoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStoreRepository.Interface
{
    public interface IWishListRepository
    {
        bool AddToWishList(WishListModel wishListModel);
        bool RemoveFromWishList(int wishListId);
        public List<WishListModel> GetFromWishList(int userId);
    }
}
