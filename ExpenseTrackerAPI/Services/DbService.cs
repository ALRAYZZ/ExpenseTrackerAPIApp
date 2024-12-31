using ExpenseTrackerAPI.DataAccess;
using ExpenseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Services
{
	
	public class DbService
	{
		private readonly EFDbContext _dbContext;
		public DbService(EFDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public async Task<IEnumerable<ExpenseModel>> GetExpensesByUserIdAsync(string userId)
		{
			return await _dbContext.Expenses.Where(x => x.UserId == userId).ToListAsync();
		}
		public async Task<ExpenseModel> GetExpenseByIdAsync(int id, string userId)
		{
			var expense =  await _dbContext.Expenses.FindAsync(id);
			if (expense == null)
			{
				return null;
			}

			if (expense.UserId != userId)
			{
				return null;
			}
			return expense;
		}

		public async Task<ExpenseModel> CreateExpense(ExpenseModel expense)
		{
			_dbContext.Expenses.Add(expense);
			await _dbContext.SaveChangesAsync();
			return expense;
		}
		public async Task<ExpenseModel> UpdateExpense(ExpenseModel expense)
		{
			_dbContext.Expenses.Update(expense);
			await _dbContext.SaveChangesAsync();
			return expense;
		}
		public async Task DeleteExpense(int id)
		{
			var expense = await _dbContext.Expenses.FindAsync(id);
			if (expense != null)
			{
				_dbContext.Expenses.Remove(expense);
				await _dbContext.SaveChangesAsync();
			}
			
		}
	}
}
