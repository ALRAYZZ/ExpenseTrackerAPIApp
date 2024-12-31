using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.DataAccess
{
	public class EFDbContext : DbContext
	{
		public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
		{
		}
		public DbSet<UserModel> Users { get; set; }
		public DbSet<ExpenseModel> Expenses { get; set; }
	}
}
