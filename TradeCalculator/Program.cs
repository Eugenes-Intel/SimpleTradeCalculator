// See https://aka.ms/new-console-template for more information

namespace TradeCalculator;

public class Program
{
    private const decimal OuncesPerLot = 100m;
    private const decimal PointValue = 0.01m;

    public static void Main(string[] args)
    {
        while (true)
        {
            ShowMainMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    RunProfitLossCalculator();
                    break;
                case "2":
                    RunMaxLotSizeCalculator();
                    break;
                case "9":
                    Console.WriteLine("\nExiting... Happy trading!");
                    return;
                default:
                    Console.WriteLine("\nInvalid selection. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("=== XAUUSD Trade Calculator ===");
        Console.WriteLine("1 - Calculate Profit/Loss");
        Console.WriteLine("2 - Calculate Max Lot Size for Risk Management");
        Console.WriteLine("9 - Exit");
        Console.Write("Enter your choice: ");
    }

    private static void RunProfitLossCalculator()
    {
        bool repeat = true;
        while (repeat)
        {
            Console.Clear();
            Console.WriteLine("📈 Profit/Loss Calculator");

            decimal entryPrice = ReadDecimal("Enter entry price: ");
            decimal exitPrice = ReadDecimal("Enter exit price: ");
            decimal lotSize = ReadDecimal("Enter lot size (e.g., 0.1): ");

            decimal pointsMoved = CalculatePointsMoved(entryPrice, exitPrice);
            decimal pnl = CalculateProfitLoss(lotSize, pointsMoved);

            Console.WriteLine($"\nPoints moved: {pointsMoved:F1}");
            Console.WriteLine($"Profit/Loss at {lotSize} lot: ${pnl:F2}");

            repeat = PromptRepeatOrBack();
        }
    }

    private static void RunMaxLotSizeCalculator()
    {
        bool repeat = true;
        while (repeat)
        {
            Console.Clear();
            Console.WriteLine("🛡️ Max Lot Size Calculator");

            decimal accountBalance = ReadDecimal("Enter account balance: ");
            decimal riskPercent = ReadDecimal("Enter risk percentage (e.g., 2 for 2%): ");
            decimal stopLossPoints = ReadDecimal("Enter stop loss in points: ");

            decimal maxLot = CalculateMaxLotSize(accountBalance, riskPercent, stopLossPoints);
            Console.WriteLine($"\nMax lot size for {riskPercent}% risk: {maxLot:F3} lots");

            repeat = PromptRepeatOrBack();
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        decimal value;
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out value))
                return value;

            Console.WriteLine("Invalid input. Please enter a numeric value.");
        }
    }

    private static bool PromptRepeatOrBack()
    {
        Console.WriteLine("\nPress 0 to return to main menu, or 1 to rerun this calculation.");
        string input = Console.ReadLine();
        return input == "1";
    }

    private static decimal CalculatePointsMoved(decimal entryPrice, decimal exitPrice)
    {
        return Math.Abs(entryPrice - exitPrice) / PointValue;
    }

    private static decimal CalculateProfitLoss(decimal lotSize, decimal pointsMoved)
    {
        decimal ounces = lotSize * OuncesPerLot;
        decimal valuePerPoint = ounces * PointValue;
        return pointsMoved * valuePerPoint;
    }

    private static decimal CalculateMaxLotSize(decimal accountBalance, decimal riskPercent, decimal stopLossPoints)
    {
        decimal maxRisk = accountBalance * (riskPercent / 100m);
        decimal valuePerPointPerLot = OuncesPerLot * PointValue;
        return maxRisk / (stopLossPoints * valuePerPointPerLot);
    }
}