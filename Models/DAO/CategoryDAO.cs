using Abp.Application.Services.Dto;
using Models.Constants;
using Models.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class CategoryDAO
    {
        private QuanLyBanHangCSharpDbContext dbContext = new QuanLyBanHangCSharpDbContext();
        public PagedResultDto<Category> GetAllCategoryByKeyWord(string search, int currentPage, int pageSize)
        {
            IQueryable<Category> categories = dbContext.Categories.OrderByDescending(x => x.Id);
            if (!string.IsNullOrWhiteSpace(search))
                categories = categories.Where(x => x.Name.Contains(search.Trim()));
            int totalCount = categories.Count();
            categories = categories.Skip((currentPage - 1) * pageSize).Take(pageSize);
            return new PagedResultDto<Category>(totalCount, categories.ToList());
        }

        public async Task<long> CreateCategoryAsync(Category category)
        {
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> EditCategoryAsync(Category category)
        {
            var categoryEdit = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            categoryEdit.Name = category.Name;
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<CategoryStatus> ChangeStatusAsync(long id)
        {
            var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            category.CategoryStatus = category.CategoryStatus == CategoryStatus.Active ? CategoryStatus.Deactive : CategoryStatus.Active;
            await dbContext.SaveChangesAsync();
            return category.CategoryStatus;
        }

        public async Task<ActionStatus> DeleteAsync(long id)
        {
            var check = await dbContext.Products.AnyAsync(x => x.CategoryId == id);
            var categoryDelete = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            var status = ActionStatus.DeleteSuccess;
            if (!check && categoryDelete != null)
                dbContext.Categories.Remove(categoryDelete);
            else if (check && categoryDelete != null)
            {
                categoryDelete.CategoryStatus = categoryDelete.CategoryStatus == CategoryStatus.Active ? CategoryStatus.Deactive : CategoryStatus.Active;
                status = ActionStatus.ChangeStatus;
            }
            else
                status = ActionStatus.DeleteFail;
            await dbContext.SaveChangesAsync();
            return status;
        }

        public async Task<Category> GetCategoryByIdAsync(long id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Category> GetAllCategoryActive()
        {
            return dbContext.Categories.Where(x => x.CategoryStatus == CategoryStatus.Active).ToList();
        }
    }
}
