namespace ExpenseTrackerAPI.Models
{
	public class CreateExpenseModel
	{
		public string Description { get; set; }
		public decimal Amount { get; set; }
		public DateTime Date { get; set; } = DateTime.Now;
		public ExpenseCategory Category { get; set; }
	}

}
