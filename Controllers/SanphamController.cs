using buingocluan_buoi4.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace buingocluan_buoi4.Controllers
{
    public class SanphamController : Controller
    {
        //Mục đích controller này để hiển thị layout sản phẩm cho thật đẹp
        //Và user bấm add to cart
        private readonly IProductRepository _productRepository;

        public SanphamController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

    }
}
