namespace ExpenseTrackerAPI.Models
{
	public enum ExpenseCategory
	{
		Groceries,
		Leisure,
		Electronics,
		Utilities,
		Clothing,
		Health,
		Others
	}
	public class ExpenseModel
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public decimal Amount { get; set; }
		public DateTime Date { get; set; } = DateTime.Now;
		public ExpenseCategory Category { get; set; }
		public string UserId { get; set; }
	}
}
