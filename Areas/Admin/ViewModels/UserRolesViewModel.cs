using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace buingocluan_buoi4.Areas.Admin.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> UserRoles { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
    }
}