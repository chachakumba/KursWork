using System;
using System.Collections.Generic;
using System.Text;

namespace EntityContext
{
    [Serializable]
    public class Save
    {
        public List<string> categories = new List<string>();
        public List<string> reservedCategories = new List<string>();
        public List<Account> accounts =new List<Account>();
        public Save() { categories.Add("Untagged"); accounts.Add(new Account()); reservedCategories.Add("Moved to another account"); reservedCategories.Add("Moved from another account"); }
    }
    [Serializable]
    public class Account
    {
        public string name = "Default account";
        public float money = 0;
        public List<Operation> operations = new List<Operation>();
        public Account(string accountName) => name = accountName;
        public Account() { }
    }
    public static class Extension
    {
        public static bool Operate(this Account acc, float amount, string category)
        {
            if (acc.money + amount < 0) return false;
            acc.operations.Add(new Operation(amount, category, acc.name));
            acc.money += amount;
            return true;
        }
        public static bool Operate(this Account acc, float amount)
        {
            if (acc.money + amount < 0) return false;
            acc.operations.Add(new Operation(amount, acc.name));
            acc.money += amount;
            return true;
        }
    }
    [Serializable]
    public class Operation
    {
        public string attachedToAccount;
        public DateTime date;
        public float addition = 0;
        public string category = "Untagged";
        public Operation(float added, string toCategory, string accName) { addition = added; category = toCategory; attachedToAccount = accName; date = DateTime.Now; }
        public Operation(float added, string accName) { addition = added; attachedToAccount = accName; date = DateTime.Now; }
    }
}
