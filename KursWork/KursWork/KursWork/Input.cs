using System;
using System.Collections.Generic;
using EntityService;
namespace InputManager // Gets all input and sends it to service (logic only about input and interface)
{
    class Input
    {
        static void Main(string[] args)
        {
            bool endWork = false;
            if (Service.LoadSave())
            {
                Console.WriteLine("Hello. Loaded data sucessfully");
            }
            else
            {
                Console.WriteLine("Hello. Created new cabinet");
            }
            while (!endWork)
            {
                Console.WriteLine("Choose operation:");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1.Accounts:");
                Console.ResetColor();
                Console.WriteLine(" 1.Add account\n 2.Delete account\n 3.Change account name\n 4.Show all accounts\n 5.Show balance on account");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("2.Cateories:");
                Console.ResetColor();
                Console.WriteLine(" 1.Add category\n 2.Delete category\n 3.Change category name\n 4.Show all categories");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("3.Operations:");
                Console.ResetColor();
                Console.WriteLine(" 1.Add money to account\n 2.Get money from account\n 3.Move money from one account to another\n 4.Show all operations in some period of time\n 5.Show all operations in category");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("4.Clear console");
                Console.WriteLine("5.Delete cabinet");
                Console.WriteLine("6.Exit");
                Console.ResetColor();
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1.1":
                        Console.WriteLine("Enter name of new account:");
                        input = Console.ReadLine();
                        if (Service.AddAccount(input))
                        {
                            Console.WriteLine($"Created {input} account");
                        }
                        else
                        {
                            Console.WriteLine($"Can not create account with name {input}");
                        }
                        break;
                    case "1.2":
                        Console.WriteLine("Enter name of account to delete:");
                        input = Console.ReadLine();
                        if (Service.DeleteAccount(input))
                        {
                            Console.WriteLine($"Deleted {input} account");
                        }
                        else
                        {
                            Console.WriteLine($"Can not delete account with name {input}");
                        }
                        break;
                    case "1.3":
                        Console.WriteLine("Enter name of account to change:");
                        input = Console.ReadLine();
                        Console.WriteLine("Enter new name of this account:");
                        string newAccountName = Console.ReadLine();
                        Found foundAccToChange = Service.ChangeAcc(input, newAccountName);
                        if (foundAccToChange == Found.changed)
                        {
                            Console.WriteLine($"Changed account {input} to {newAccountName}");
                        }
                        else if(foundAccToChange == Found.foundSame)
                        {
                            Console.WriteLine($"Found existing account with {newAccountName} name");
                        }
                        else
                        {
                            Console.WriteLine($"Can not find account with {input} name");
                        }
                        break;
                    case "1.4":
                        List<EntityContext.Account> allAccounts = Service.ShowAllAccounts();
                        string allAccountsString = "";
                        foreach(EntityContext.Account acc in allAccounts)
                        {
                            allAccountsString += $"{acc.name} with balance {acc.money}\n";
                        }
                        Console.WriteLine("All accounts:\n" + allAccountsString);
                        break;
                    case "1.5":
                        Console.WriteLine("Enter name of account to show balance:");
                        input = Console.ReadLine();
                        float balance = Service.ShowMoneyOnAccount(input);
                        if (balance >= 0)
                        {
                            Console.WriteLine($"Balance on {input} account is: {balance}");
                        }
                        else
                        {
                            Console.WriteLine($"Can not find account with name {input}");
                        }
                        break;
                    /////////////////
                    case "2.1":
                        Console.WriteLine("Enter name of new category:");
                        input = Console.ReadLine();
                        if (Service.AddCategory(input))
                        {
                            Console.WriteLine($"Created {input} category");
                        }
                        else
                        {
                            Console.WriteLine($"Can not create category with name {input}");
                        }
                        break;
                    case "2.2":
                        Console.WriteLine("Enter name of category to delete:");
                        input = Console.ReadLine();
                        if (Service.DeleteCategory(input))
                        {
                            Console.WriteLine($"Deleted {input} category");
                        }
                        else
                        {
                            Console.WriteLine($"Can not delete category with name {input}");
                        }
                        break;
                    case "2.3":
                        Console.WriteLine("Enter name of category to change:");
                        input = Console.ReadLine();
                        Console.WriteLine("Enter new name of this category:");
                        string newCategoryName = Console.ReadLine();
                        int foundCategoryToChange = Service.ChangeCategory(input, newCategoryName);
                        if (foundCategoryToChange >= 0)
                        {
                            Console.WriteLine($"Changed category {input} to {newCategoryName}. Changed categories of {foundCategoryToChange} operations");
                        }
                        else
                        {
                            Console.WriteLine($"Can not find category with {input} name");
                        }
                        break;
                    case "2.4":
                        List<string> allCategories = Service.ShowAllCategories();
                        string allCategoriesString = "";
                        foreach (string cat in allCategories)
                        {
                            allCategoriesString += $"{cat}\n";
                        }
                        Console.WriteLine("All categories:\n" + allCategoriesString);
                        break;
                    /////////////////
                    case "3.1":
                        Console.WriteLine("Enter name of account where to add:");
                        input = Console.ReadLine();
                        Console.WriteLine("Enter amount to add:");
                        string addedMoney = Console.ReadLine();
                        float addedMoneyFloat;
                        Console.WriteLine("Enter category of addition:");
                        string cathegoryOfAddition = Console.ReadLine();
                        if(float.TryParse(addedMoney, out addedMoneyFloat))
                        {
                            if(addedMoneyFloat > 0)
                            {
                                if(cathegoryOfAddition == "")
                                {
                                    OperationArg operationArgOfAdding = Service.AddMoney(input, addedMoneyFloat);
                                    if (operationArgOfAdding == OperationArg.unfound)
                                    {
                                        Console.WriteLine($"Can not find {input} account");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Succesfully added {addedMoney} to {input} account");
                                    }
                                }
                                else
                                {
                                    OperationArg operationArgOfAdding = Service.AddMoney(input, addedMoneyFloat, cathegoryOfAddition);
                                    if (operationArgOfAdding == OperationArg.unfound)
                                    {
                                        Console.WriteLine($"Can not find {input} account");
                                    }
                                    else if(operationArgOfAdding == OperationArg.noCategory)
                                    {
                                        Console.WriteLine($"Can not find {cathegoryOfAddition} category");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Succesfully added {addedMoney} to {input} account");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Can not add {addedMoneyFloat}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{addedMoney} is not a number");
                        }
                        break;
                    case "3.2":
                        Console.WriteLine("Enter name of account where to subtract:");
                        input = Console.ReadLine();
                        Console.WriteLine("Enter amount to add:");
                        string subtractedMoney = Console.ReadLine();
                        float subtractedMoneyFloat;
                        Console.WriteLine("Enter category of addition:");
                        string cathegoryOfsubtraction = Console.ReadLine();
                        if (float.TryParse(subtractedMoney, out subtractedMoneyFloat))
                        {
                            if (subtractedMoneyFloat < 0)
                            {
                                if (cathegoryOfsubtraction == "")
                                {
                                    Service.AddMoney(input, subtractedMoneyFloat);
                                }
                                else
                                {
                                    OperationArg operationArgOfsubtracting = Service.AddMoney(input, subtractedMoneyFloat, cathegoryOfsubtraction);
                                    if (operationArgOfsubtracting == OperationArg.unfound)
                                    {
                                        Console.WriteLine($"Can not find {input} account");
                                    }
                                    else if (operationArgOfsubtracting == OperationArg.noCategory)
                                    {
                                        Console.WriteLine($"Can not find {cathegoryOfsubtraction} category");
                                    }
                                    else if (operationArgOfsubtracting == OperationArg.noMoney)
                                    {
                                        Console.WriteLine($"Inefficient balance on {input} account");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Succesfully subtracted {subtractedMoney} to {input} account");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Can not subtract {subtractedMoneyFloat}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{subtractedMoney} is not a number");
                        }
                        break;
                    case "3.3":
                        Console.WriteLine("Enter name of account where to subtract:");
                        input = Console.ReadLine();
                        Console.WriteLine("Enter name of account where to add:");
                        string addToAccName = Console.ReadLine();
                        Console.WriteLine("Enter amount:");
                        string moneyFromOneToAnother = Console.ReadLine();
                        float moneyFromOneToAnotherFloat;
                        if (float.TryParse(moneyFromOneToAnother, out moneyFromOneToAnotherFloat))
                        {
                            if (moneyFromOneToAnotherFloat > 0)
                            {
                                OperationArg formOneToAnotherArg = Service.MoveMoneyFromOneToAnother(input, addToAccName, moneyFromOneToAnotherFloat);
                                if (formOneToAnotherArg == OperationArg.unfound)
                                {
                                    Console.WriteLine($"Can not find one of the accounts");
                                }
                                else if (formOneToAnotherArg == OperationArg.noMoney)
                                {
                                    Console.WriteLine($"Inefficient balance on {input} account");
                                }
                                else
                                {
                                    Console.WriteLine($"Succesfully transfered {moneyFromOneToAnother} from {input} to {addToAccName} account");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Can not transfer {moneyFromOneToAnotherFloat}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{moneyFromOneToAnother} is not a number");
                        }
                        break;
                    case "3.4":
                        Console.WriteLine("Enter start date in DD.MM.YYYY:");
                        input = Console.ReadLine();
                        Console.WriteLine("Enter end date in DD.MM.YYYY:");
                        string secondDateString = Console.ReadLine();
                        DateTime firstDate, secondDate;
                        if (DateTime.TryParse(input, out firstDate))
                        {
                            if (DateTime.TryParse(secondDateString, out secondDate))
                            {
                                List<EntityContext.Operation> operations = Service.ShowOperations(firstDate, secondDate);
                                string foundOperations = "";
                                foreach (EntityContext.Operation oper in operations)
                                {
                                    if (oper.addition > 0)
                                        foundOperations += $"{oper.date} Account {oper.attachedToAccount} +{oper.addition} in cathegory {oper.category}\n";
                                    else
                                        foundOperations += $"{oper.date} Account {oper.attachedToAccount} {oper.addition} in cathegory {oper.category}\n";
                                }
                                Console.WriteLine($"All found operations from {input} to {secondDateString}:{foundOperations}");
                            }
                            else
                            {
                                Console.WriteLine($"{secondDateString} is not correct");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{input} is not correct");
                        }
                        break;
                    case "3.5":
                        Console.WriteLine("Enter name of cathegory of operations to search:");
                        input = Console.ReadLine();
                        List<EntityContext.Operation> foundOperationsCathegory = Service.ShowOperations(input);
                        string foundOperationsCathegoryString = "";
                        if (foundOperationsCathegory.Count > 0)
                        {
                            foreach (EntityContext.Operation oper in foundOperationsCathegory)
                            {
                                if (oper.addition > 0)
                                    foundOperationsCathegoryString += $"{oper.date} Account {oper.attachedToAccount} +{oper.addition} in cathegory {oper.category}\n";
                                else
                                    foundOperationsCathegoryString += $"{oper.date} Account {oper.attachedToAccount} {oper.addition} in cathegory {oper.category}\n";
                            }
                            Console.WriteLine($"All found operations in category {input}:{foundOperationsCathegoryString}");
                        }
                        else
                        {
                            Console.WriteLine("Found no operations in this cathegory");
                        }
                        break;
                    /////////////////
                    case "4":
                        Console.Clear();
                        Service.SaveFile();
                        break;
                    case "4.":
                        Console.Clear();
                        break;
                    case "4.1":
                        Console.Clear();
                        break;
                    /////////////////
                    case "5":
                        DeleteSave();
                        break;
                    case "5.":
                        DeleteSave();
                        break;
                    case "5.1":
                        DeleteSave();
                        break;
                    /////////////////
                    case "6":
                        endWork = true;
                        Service.SaveFile();
                        break;
                    case "6.":
                        endWork = true;
                        break;
                    case "6.1":
                        endWork = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Please enter number of category dot and number of item");
                        Console.ResetColor();
                        break;
                }
                Console.WriteLine();
            }
        }
        static void DeleteSave()
        {
            Console.Write("If you want to ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("delete");
            Console.ResetColor();
            Console.Write(" your cabinet ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("permanently");
            Console.ResetColor();
            Console.Write(" write:\nDelete my account\n");
            string input = Console.ReadLine();
            if (input == "Delete my account")
            {
                Service.DeleteSave();
                Console.WriteLine("Account have been deleted");
            }
            else
            {
                Console.Write("Account have ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("not");
                Console.ResetColor();
                Console.Write(" been deleted\n");
            }
        }
    }
}
