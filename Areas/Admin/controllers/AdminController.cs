using buingocluan_buoi4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using buingocluan_buoi4.Areas.Admin.ViewModels;

namespace buingocluan_buoi4.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                    TempData["SuccessMessage"] = "Vai trò đã được tạo thành công!";
                    return RedirectToAction("RoleManagement");
                }
                else
                {
                    ModelState.AddModelError("", "Vai trò đã tồn tại!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tên vai trò không được để trống!");
            }
            return View();
        }

        public async Task<IActionResult> RoleManagement()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public async Task<IActionResult> UserManagement()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> EditUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                UserRoles = userRoles.ToList(),
                AllRoles = allRoles
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(UserRolesViewModel model, string[] selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            // Xóa tất cả vai trò hiện tại
            await _userManager.RemoveFromRolesAsync(user, userRoles);

            // Thêm các vai trò được chọn
            if (selectedRoles != null)
            {
                await _userManager.AddToRolesAsync(user, selectedRoles);
            }

            TempData["SuccessMessage"] = "Cập nhật vai trò thành công!";
            return RedirectToAction("UserManagement");
        }
    }
}