using Models.DTO;
using Models.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class CartDAO : BaseDAO
    {
        public async Task<List<CartDto>> GetAllCartByAccount(long accountId)
        {
            var cards = await DBContext.Cards.Where(x => x.AccountId == accountId).ToListAsync();
            var assets = DBContext.Assets;
            var result = (from x in cards
                          join p in assets on x.ProductId equals p.ProductId
                          group p by x into gr
                          select new CartDto
                          {
                              Id = gr.Key.Id,
                              Price = gr.Key.Price,
                              ProductName = gr.Key.Product.Name,
                              QuantityPurchased = gr.Key.QuantityPurchased,
                              ProductId = gr.Key.ProductId,
                              Image = gr.Select(x => new Image { Name = x.Name, Path = x.Path }).FirstOrDefault()
                          }).ToList();
            return result;
        }

        public async Task<int> CreateCart(Cart cart)
        {
            var cartProduct = await DBContext.Cards.FirstOrDefaultAsync(x => x.AccountId == cart.AccountId && x.ProductId == cart.ProductId);
            if (cartProduct == null)
                DBContext.Cards.Add(cart);
            else
            {
                cartProduct.QuantityPurchased += cart.QuantityPurchased;
                cartProduct.Price += cart.Price * cart.QuantityPurchased;
            }
            return DBContext.SaveChanges();
        }

        public async Task<int> EditCart(long id, long quantity, long price)
        {
            var cartProduct = await DBContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            cartProduct.QuantityPurchased = quantity;
            cartProduct.Price = price;
            return DBContext.SaveChanges();
        }

        public async Task<int> DeleteCart(long id)
        {
            var cartProduct = await DBContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            DBContext.Cards.Remove(cartProduct);
            return await DBContext.SaveChangesAsync();
        }

        public async Task<List<CartDto>> GetAllProductOrder(long accountId)
        {
            var cards = await DBContext.Cards.Where(x => x.AccountId == accountId).ToListAsync();
            if (cards.Count == 0)
                return null;
            var list = cards.Select(x => new CartDto
            {
                Id = x.Id,
                ProductId = x.ProductId,
                QuantityPurchased = x.QuantityPurchased,
                Price = x.Price
            }).ToList();
            DBContext.Cards.RemoveRange(cards);
            await DBContext.SaveChangesAsync();
            return list;
        }
    }
}
