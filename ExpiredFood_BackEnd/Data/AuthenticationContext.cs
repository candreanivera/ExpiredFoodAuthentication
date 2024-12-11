using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpiredFood_BackEnd.Data
{
    public class AuthenticationContext : IdentityDbContext<IdentityUser>
    {
        //Constructor
        public AuthenticationContext(DbContextOptions options) : base(options)
        {

        }
    }
}