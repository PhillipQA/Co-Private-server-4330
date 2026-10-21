using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Conquer.Account.Data;

public class AccountDbContext(DbContextOptions<AccountDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Server> Servers => Set<Server>();
}
