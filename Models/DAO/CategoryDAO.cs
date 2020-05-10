using Abp.Application.Services.Dto;
using Models.Constants;
using Models.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class CategoryDAO : BaseDAO
    {
        public PagedResultDto<Category> GetAllCategoryByKeyWord(string search, int currentPage, int pageSize)
        {
            IQueryable<Category> categories = DBContext.Categories.OrderByDescending(x => x.Id);
            if (!string.IsNullOrWhiteSpace(search))
                categories = categories.Where(x => x.Name.Contains(search.Trim()));
            int totalCount = categories.Count();
            categories = categories.Skip((currentPage - 1) * pageSize).Take(pageSize);
            return new PagedResultDto<Category>(totalCount, categories.ToList());
        }

        public async Task<long> CreateCategoryAsync(Category category)
        {
            DBContext.Categories.Add(category);
            await DBContext.SaveChangesAsync();
            return category.Id;
        }

        public async Task<bool> EditCategoryAsync(Category category)
        {
            var categoryEdit = await DBContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            categoryEdit.Name = category.Name;
            return await DBContext.SaveChangesAsync() > 0;
        }

        public async Task<CategoryStatus> ChangeStatusAsync(long id)
        {
            var category = await DBContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            category.CategoryStatus = category.CategoryStatus == CategoryStatus.Active ? CategoryStatus.Deactive : CategoryStatus.Active;
            await DBContext.SaveChangesAsync();
            return category.CategoryStatus;
        }

        public async Task<ActionStatus> DeleteAsync(long id)
        {
            var check = await DBContext.Products.AnyAsync(x => x.CategoryId == id);
            var categoryDelete = await DBContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            var status = ActionStatus.DeleteSuccess;
            if (!check && categoryDelete != null)
                DBContext.Categories.Remove(categoryDelete);
            else if (check && categoryDelete != null)
            {
                categoryDelete.CategoryStatus = categoryDelete.CategoryStatus == CategoryStatus.Active ? CategoryStatus.Deactive : CategoryStatus.Active;
                status = ActionStatus.ChangeStatus;
            }
            else
                status = ActionStatus.DeleteFail;
            await DBContext.SaveChangesAsync();
            return status;
        }

        public async Task<Category> GetCategoryByIdAsync(long id)
        {
            return await DBContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Category> GetAllCategoryActive()
        {
            return DBContext.Categories.Where(x => x.CategoryStatus == CategoryStatus.Active).ToList();
        }
    }
}
