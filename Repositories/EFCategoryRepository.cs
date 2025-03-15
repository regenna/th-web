using buingocluan_buoi4.Models;
using Microsoft.EntityFrameworkCore;

namespace buingocluan_buoi4.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {

        /// <summary>
        /// Khai báo biến context để sử dụng database
        /// </summary>
        private readonly ApplicationDbContext _context;


        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="context"></param>
        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }



        /// <summary>
        /// Lấy danh sách tất cả category
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync();
            return await _context.Categories
            .Include(p => p.Products) // Include thông tin về category
            .ToListAsync();
        }


        /// <summary>
        /// Lấy thông tin một category dựa trên Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category> GetByIdAsync(int id)
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Categories.Include(p => p.Products).FirstOrDefaultAsync(p => p.Id == id);
        }



        /// <summary>
        ///     Thêm một category mới vào cơ sở dữ liệu
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task AddAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        ///    Cập nhật thông tin category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        ///   Xóa một category dựa trên Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
