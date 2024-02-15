using FinalProject;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;

StreamReader rdr = new StreamReader(@"C:\Users\emper\source\repos\FINAL_PROJECT\FinalProject\database.json");
var data = rdr.ReadToEnd();
rdr.Close();

User user = JsonConvert.DeserializeObject<User>(data);

while (true)
{
    Console.WriteLine("Do you want to use the ATM? (y/n)");
    string ans = Console.ReadLine();
    if (ans == "n") break;

    CardDetails cardDetails = enterCardDetails();

    if (cardIsValid(cardDetails, user))
    {
        Console.WriteLine("\nEnter Pin");
        string pinCode = Console.ReadLine();
        if (pinCode == user.pinCode)
        {
            Console.WriteLine($"\nHello {user.firstName} {user.lastName}: \n\n" +
                " 1. Check balance\n 2. Take amount\n 3. Get last 5 transactions\n 4. Add amount\n 5. Change Pin");
            int operation = Convert.ToInt32(Console.ReadLine());
            List<Transaction> history = user.transactionHistory;
            int newAmount = 0;
            string type = "";
            bool operationDone = false;

            switch (operation)
            {
                case 1:
                    newAmount = Operation1(history);
                    operationDone = true;
                    type = "Check balance";
                    break;
                case 2:
                    newAmount = Operation2(history);
                    operationDone = (newAmount != history[history.Count-1].amount) && (history.Count != 0);
                    type = "Take amount";
                    break;
                case 3: Operation3(history);
                    operationDone = history.Count >= 5;
                    if (history.Count != 0)
                        newAmount = history[history.Count - 1].amount;
                    type = "Get last 5 transactions\n";
                        break;
                case 4:
                    newAmount = Operation4(history);
                    operationDone = true;
                    type = "Add amount";
                    break;
                case 5:
                    user.pinCode = Operation5(user.pinCode);
                    operationDone = true;
                    type = "Change pin";
                    if(history.Count != 0) 
                        newAmount = history[history.Count - 1].amount;
                    break;
            }

            if (operationDone)
            {
                user.transactionHistory.Add(new Transaction(DateTime.Now.ToString(), type, newAmount));
                var serialized = JsonConvert.SerializeObject(user, Formatting.Indented);

                File.WriteAllText(@"C:\Users\emper\source\repos\FINAL_PROJECT\FinalProject\database.json", serialized);
            }
        }
        else
        {
            Console.WriteLine("Please provide correct Pin\n");
        }
    }
    else
    {
        Console.WriteLine("This card does not exist.\n");
    }
}

CardDetails enterCardDetails()
{
    Console.WriteLine("Enter card details: \n  1. Card number\n  2. Expiration date\n  3. CVC");
    string cardNumber = Console.ReadLine();
    string expirationDate = Console.ReadLine();
    string cvc = Console.ReadLine();

    return new CardDetails(cardNumber, expirationDate, cvc);
}

bool cardIsValid(CardDetails cardDetails, User user)
{
    return (cardDetails.cardNumber == user.cardDetails.cardNumber &&
         cardDetails.expirationDate == user.cardDetails.expirationDate &&
         cardDetails.CVC == user.cardDetails.CVC);
}
int Operation1(List<Transaction> history)
{
    int money;
    if (history.Count == 0)
    {
        money = 0;
    }
    else money = history.ElementAt(history.Count - 1).amount;
    Console.WriteLine($"\nAmount: \nGEL: {money}");
    return money;
}
int Operation2(List<Transaction> history)
{
    if (!NoMoney(history))
    {
        Console.WriteLine("\nHow much money do you want to take?\n");
        int take = Convert.ToInt32(Console.ReadLine());
        while (InvalidCases(take, history))
        {
            Console.WriteLine("\nPlease enter a valid amount: ");
            take = Convert.ToInt32(Console.ReadLine());
        }

        int newAmount = TakeAmount(take, history);
        Console.WriteLine($"\nYou took out {take}GEL.\nCurrent amount is: {newAmount}GEL.");
        return newAmount;
    }
    return 0;
}

bool NoMoney(List<Transaction> history)
{
    if (history.ElementAt(history.Count - 1).amount == 0 || history.Count == 0)
    {
        Console.WriteLine("You don't have any money.\n");
        return true;
    }
    return false;
}
bool InvalidCases(int take, List<Transaction> history)
{
    if (take > history.ElementAt(history.Count - 1).amount)
    {
        Console.WriteLine("You don't have enough money.\n");
        return true;
    }
    if (take == 0)
    {
        Console.WriteLine("You haven't taken any amount of money.\n");
        return true;
    }
    if (take < 0)
    {
        Console.WriteLine("Invalid amount.\n");
        return true;
    }
    return false;
}
int TakeAmount(int take, List<Transaction> history)
{
    int newAmount = history.ElementAt(history.Count - 1).amount - take;
    return newAmount;
}

void Operation3(List<Transaction> history)
{
    if(history.Count < 5)
    {
        Console.WriteLine("There has not been 5 transactions yet.\n");
        return;
    }

    for(int i=history.Count-5; i<history.Count; i++)
    {
        Console.WriteLine($"\nTransaction number {i}:");
        Console.WriteLine($"Transaction date: {history[i].trasnactionDate}");
        Console.WriteLine($"Transaction type: {history[i].transactionType}");
        Console.WriteLine($"Amount after transaction: {history[i].amount}GEL");
    }
}

int Operation4(List<Transaction> history)
{
    Console.WriteLine("\nHow much money do you want to add?\n");
    int add = Convert.ToInt32(Console.ReadLine());
    int newMoney = AddAmount(add, history);
    Console.WriteLine($"\nYou added {add}GEL.\nCurrent amount is: {newMoney}GEL.");
    return newMoney;
}

int AddAmount(int add, List<Transaction> history)
{
    if(history.Count == 0)
    {
        return add;
    }
    return history.ElementAt(history.Count - 1).amount + add;
}

string Operation5(string pin)
{
    Console.WriteLine("\nEnter new Pin: ");
    string pinCode = Console.ReadLine();
    while (pinCode == pin || pinCode.Length!=4)
    {
        Console.WriteLine("This pin is already used or does not have 4 numbers. Enter a new Pin: ");
        pinCode = Console.ReadLine();
    }
    Console.WriteLine($"\nYou changed your pin. Your new Pin Code is: {pinCode}\n");
    return pinCode;
}