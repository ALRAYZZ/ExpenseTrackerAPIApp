using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Controllers
{
	[ApiController]
	[Route("expenses")]
	public class ExpensesController : Controller
	{
		public readonly DbService _dbService;
		public ExpensesController(DbService dbService)
		{
			_dbService = dbService;
		}


		[HttpGet]
		public async Task<ActionResult<IEnumerable<ExpenseModel>>> GetAllExpenses()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}

			var expenses = await _dbService.GetExpensesByUserIdAsync(userId);
			return Ok(expenses);
		}
		[HttpPost]
		public async Task<ActionResult<ExpenseModel>> CreateExpense(CreateExpenseModel expenseCreateModel)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}
			var expenseModel = new ExpenseModel
			{
				UserId = userId,
				Amount = expenseCreateModel.Amount,
				Description = expenseCreateModel.Description,
				Category = expenseCreateModel.Category,
			};

			var newExpense = await _dbService.CreateExpense(expenseModel);
			return Ok(newExpense);
		}
		[HttpPut("{id}")]
		public async Task<ActionResult<ExpenseModel>> UpdateExpense(int id, CreateExpenseModel expenseCreateModel)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}
			var expense = await _dbService.GetExpenseByIdAsync(id, userId);
			if (expense == null)
			{
				return NotFound();
			}
			if (expense.UserId != userId)
			{
				return Unauthorized();
			}
			expense.Amount = expenseCreateModel.Amount;
			expense.Description = expenseCreateModel.Description;
			expense.Category = expenseCreateModel.Category;
			var updatedExpense = await _dbService.UpdateExpense(expense);
			return Ok(updatedExpense);
		}
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeleteExpense(int id)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (string.IsNullOrEmpty(userId))
			{
				return Unauthorized();
			}
			var expense = await _dbService.GetExpenseByIdAsync(id, userId);
			if (expense == null)
			{
				return NotFound();
			}
			if (expense.UserId != userId)
			{
				return Unauthorized();
			}
			await _dbService.DeleteExpense(id);
			return NoContent();
		}
	}
}
