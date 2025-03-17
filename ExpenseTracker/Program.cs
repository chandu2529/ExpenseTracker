namespace ExpenseTracker
{
    class Program
    {
        static List<Expense> expenses = new List<Expense>(); 

        static void Main(string[] args)
        {
            LoadExpenses();
            bool running = true;
            while (running)
            {
                Console.WriteLine("\nPersonal Expense Tracker");
                Console.WriteLine("1.  Add Expense");
                Console.WriteLine("2.  View All Expenses");
                Console.WriteLine("3.  View Total Expenses");
                Console.WriteLine("4.  View Category Totals");
                Console.WriteLine("5.  Delete the Expense");
                Console.WriteLine("6.  Filter by Date");
                Console.WriteLine("7.  Search by Category");
                Console.WriteLine("8.  Edit the Expense");
                Console.WriteLine("9.  Clear All Expenses");
                Console.WriteLine("10. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddExpense();
                        break;
                    case "2":
                        ViewExpenses();
                        break;
                    case "3":
                        ViewTotal();
                        break;
                    case "4":
                        ViewCategoryTotals();
                        break;
                    case "5":
                        DeleteExpense();
                        break;
                    case "6":
                        FilterByDate();
                        break;
                    case "7":
                        SearchByCategory();
                        break;
                    case "8":
                        EditExpense();
                        break ;
                    case "9":
                        ClearAllExpenses();
                        break;
                    case "10":
                        SaveExpenses();
                        running = false;
                        Console.WriteLine("GoodBye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again.");
                        break;
                }
            }
        }

        static void AddExpense()
        {
            Console.Write("Enter amount: $");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            Console.Write("Enter category (e.g., Food, Travel): ");
            string category = Console.ReadLine();

            Console.Write("Enter date (yyyy-MM-dd, or press Enter for today): ");
            string dateInput = Console.ReadLine();
            DateTime date = string.IsNullOrEmpty(dateInput) ? DateTime.Now : DateTime.Parse(dateInput); 

            Expense expense = new Expense(amount, category, date);
            expenses.Add(expense);
            Console.WriteLine("Expense added successfully!");
        }

        static void ViewExpenses()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses recorded yet.");
                return;
            }

            Console.WriteLine("\nAll Expenses:");
            foreach (Expense exp in expenses)
            {
                Console.WriteLine(exp);
            }
        }

        static void ViewTotal()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses to total.");
                return;
            }

            decimal total = 0;
            foreach (Expense exp in expenses)
            {
                total += exp.Amount;
            }
            Console.WriteLine($"\nTotal Expenses: ${total}");
        }

        static void ViewCategoryTotals()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses to summarize.");
                return;
            }
            
            Dictionary<string , decimal> CategoryTotals = new Dictionary<string , decimal>();

            foreach (Expense exp in expenses)
            {
                if (CategoryTotals.ContainsKey(exp.Category))
                {
                    CategoryTotals[exp.Category] += exp.Amount;
                }
                else
                {
                    CategoryTotals[exp.Category] = exp.Amount;
                }
            }
            Console.WriteLine("\nExpenses by Category");
            foreach (var pair in  CategoryTotals)
            {
                Console.WriteLine($"{pair.Key} : ${pair.Value}");
            }
        }

        static void DeleteExpense()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses to Delete.");
                return;
            }

            Console.WriteLine("\nSelect an expense to delete:");
            for (int i = 0; i < expenses.Count; i++)
            {
                Console.WriteLine($"{i+1}. {expenses[i]}");
            }
            Console.Write("Enter the number of the expense to delete: ");
            if (int.TryParse(Console.ReadLine() , out int index) && index >= 1 && index <= expenses.Count)
            {
                expenses.RemoveAt(index - 1);
                try
                {
                    SaveExpenses();
                    Console.WriteLine("Expense Deleted Successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save changes: {ex.Message}");
                }
                
            }
            else
            {
                Console.WriteLine("Invalid selection, nothing deleted.");
            }

        }

        static void FilterByDate()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses to Filter");
            }

            Console.Write("Enter date to filter (yyyy-MM-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime filterDate))
            {
                Console.WriteLine($"\nExpenses for {filterDate.ToShortDateString()}");
                bool found = false;
                foreach (Expense exp in expenses)
                {
                    if (exp.Date.Date == filterDate.Date)
                    {
                        Console.WriteLine(exp);
                        found = true;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("No expenses found for that date.");
                }
            }
            else
            {
                Console.WriteLine("Invalid date format. Use yyyy-MM-dd (e.g., 2025-03-17).");
            }

        }

        static void EditExpense()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses to Edit.");
                return;
            }

            Console.WriteLine("\nSelect an expense to edit:");
            for (int i = 0; i < expenses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {expenses[i]}");
            }
            Console.Write("Enter the number of the expense to edit: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= expenses.Count)
            {
                Expense exp = expenses[index-1];
                Console.Write($"New Amount (current: ${exp.Amount} , press enter to Keep): ");
                string amountInput = Console.ReadLine();
                decimal amount = string.IsNullOrEmpty(amountInput) ? exp.Amount : Convert.ToDecimal(amountInput) ;

                Console.Write($"New Category (current: {exp.Category} , press enter to Keep): ");
                string categoryInput = Console.ReadLine();
                string category = string.IsNullOrEmpty(categoryInput) ? exp.Category : categoryInput;

                Console.Write($"New Date (current: {exp.Date} , press enter to Keep): ");
                string dateInput = Console.ReadLine();
                DateTime date = string.IsNullOrEmpty(dateInput) ? exp.Date : DateTime.Parse(dateInput);

                expenses[index - 1] = new Expense(amount, category, date);

                try
                {
                    SaveExpenses();
                    Console.WriteLine("Expense Edited Successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to save changes: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid selection, nothing edited.");
            }

        }

        static void SearchByCategory()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses to search.");
                return;
            }

            Console.Write("Enter category to search (e.g., Food): ");
            string searchCategory = Console.ReadLine();

            Console.WriteLine($"\nExpenses in category '{searchCategory}':");
            bool found = false;
            foreach (Expense exp in expenses)
            {
                if (exp.Category.Equals(searchCategory , StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(exp);
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("No expenses found in that category.");
            }

        }

        static void ClearAllExpenses()
        {
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses to clear.");
                return;
            }

            Console.WriteLine("Are you sure you want to clear all expenses? (y/n): ");
            string confirmation = Console.ReadLine().Trim().ToLower();
            if (confirmation == "y")
            {
                expenses.Clear();
                SaveExpenses();
                Console.WriteLine("All expenses cleared!");
            }
            else
            {
                Console.WriteLine("Clear operation canceled.");
            }

        }

        static void SaveExpenses()
        {
            string FilePath = "expenses.txt";
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                foreach (Expense exp in expenses)
                {
                    writer.WriteLine($"{exp.Date.ToString("yyyy-MM-dd HH:mm:ss")},{exp.Category},{exp.Amount}");
                }
            }
            Console.WriteLine("Expenses saved to file.");
        }

        static void LoadExpenses()
        {
            string FilePath = "expenses.txt";
            if (File.Exists(FilePath))
            {
                expenses.Clear();
                string[] lines = File.ReadAllLines(FilePath);
                foreach (var  line in lines)
                {
                    string[] parts = line.Split(',');
                    DateTime date = DateTime.Parse(parts[0]);
                    string category = parts[1];
                    decimal amount = decimal.Parse(parts[2]);
                    expenses.Add(new Expense(amount , category , date));
                }
                Console.WriteLine("Expenses loaded from file.");
            }
        }
    }
}