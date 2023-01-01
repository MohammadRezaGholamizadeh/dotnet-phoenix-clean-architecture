using Phoenix.Application.Common.Models;

namespace Phoenix.Application.Identity.Users;

public class UserListFilter : PaginationFilter
{
    public bool? IsActive { get; set; }
}
