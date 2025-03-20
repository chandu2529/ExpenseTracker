namespace ExpenseTracker
{
    public class Expense
    {
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Expense (decimal amount, string category, DateTime date, string description)
        {
            Amount = amount;
            Category = category;
            Date = date;
            Description = description;
        }

        public override string ToString()
        {
            return $"{Date.ToString("yyyy-MM-dd HH:mm:ss")} | {Category} | {Amount} | {Description}";
        }
    }
}
