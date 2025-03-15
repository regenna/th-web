using buingocluan_buoi4.Models;
using Microsoft.EntityFrameworkCore;

namespace buingocluan_buoi4.Repositories
{
    public class EFProductRepository : IProductRepository
    {
        //Biến lưu trữ đối tượng ApplicationDbContext, đại diện cho kết nối đến cơ sở dữ liệu.
        private readonly ApplicationDbContext _context;

        //Constructor nhận một đối tượng ApplicationDbContext để sử dụng trong toàn bộ lớp.
        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync() //GetAllAsync: Lấy danh sách tất cả sản phẩm từ cơ sở dữ liệu.
        {
            // Truy cập bảng Products
            return await _context.Products
            .Include(p => p.Category) // Include thông tin về category
            .ToListAsync();// chuyeern thanh danh sach
        }
        public async Task<Product> GetByIdAsync(int id)// Lấy thông tin một sản phẩm dựa trên Id
        {
            // return await _context.Products.FindAsync(id);
            // lấy thông tin kèm theo category
            return await _context.Products.Include(p =>
           p.Category).FirstOrDefaultAsync(p => p.Id == id);//FirstOrDefaultAsync tra ve sp dau tien or null
        }

        /// <summary>
        /// Thêm một sản phẩm mới vào cơ sở dữ liệu
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Xóa một sản phẩm dựa trên Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
