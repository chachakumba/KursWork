using System;
using System.Collections.Generic;
using System.IO;
using EntityContext;

namespace EntityService // Contains all the logic
{
    public static class Service
    {
        static Save save;
        public static bool LoadSave()
        {
            Save newSave = Context.LoadSaveBinary();
            if (newSave == null)
            {
                save = new Save();
                return false;
            }
            save = newSave;
            return true;
        }
        public static void SaveFile()
        {
            Context.SaveBinar(save);
        }
        public static void DeleteSave()
        {
            Context.DeleteSave();
            save = new Save();
        }

        //accounts
        #region
        public static bool AddAccount(string name)
        {
            foreach (Account acc in save.accounts)
            {
                if (acc.name == name) return false;
            }
            save.accounts.Add(new Account(name));
            return true;
        }
        public static bool DeleteAccount(string name)
        {
            foreach (Account acc in save.accounts)
            {
                if (acc.name == name)
                {
                    save.accounts.Remove(acc);
                    return true;
                }
            }
            return false;
        }
        public static Found ChangeAcc(string oldName, string newName)
        {
            foreach (Account acc in save.accounts)
            {
                if (acc.name == oldName)
                {
                    foreach (Account account in save.accounts) if (account.name == newName) return Found.foundSame;
                    acc.name = newName;
                    return Found.changed;
                }
            }
            return Found.unfound;
        }
        public static List<Account> ShowAllAccounts()
        {
            return save.accounts;
        }
        public static float ShowMoneyOnAccount(string name)
        {
            foreach (Account acc in save.accounts)
            {
                if (acc.name == name)
                {
                    return acc.money;
                }
            }
            return -1;
        }
        #endregion

        // categories
        #region 
        public static bool AddCategory(string name)
        {
            if (name == "Untagged") return false;
            if (save.reservedCategories.Contains(name)) return false;
            foreach (string cat in save.categories)
            {
                if (cat == name)
                {
                    return false;
                }
            }
            save.categories.Add(name);
            return true;
        }
        public static bool DeleteCategory(string name)
        {
            if (name == "Untagged") return false;
            if (save.reservedCategories.Contains(name)) return false;
            foreach (string cat in save.categories)
            {
                if (cat == name)
                {
                    save.categories.Remove(name);

                    foreach (Account acc in save.accounts)
                    {
                        foreach (Operation oper in acc.operations)
                        {
                            if (oper.category == name)
                            {
                                oper.category = "Untagged";
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        public static int ChangeCategory(string oldName, string newName)
        {
            int count = 0;
            if (save.reservedCategories.Contains(oldName) || save.reservedCategories.Contains(newName)) return -1;
            if (oldName == "Untagged")
            {
                if (!save.categories.Contains(newName)) save.categories.Add(newName);
                else return -1;


                foreach (Account acc in save.accounts)
                {
                    foreach (Operation oper in acc.operations)
                    {
                        if (oper.category == "Untagged")
                        {
                            oper.category = newName;
                            count++;
                        }
                    }
                }
            }
            else
            {
                if (!save.categories.Contains(oldName)) return -2;
                if (!save.categories.Contains(newName)) save.categories.Add(newName);
                else return -1;

                foreach (Account acc in save.accounts)
                {
                    foreach (Operation oper in acc.operations)
                    {
                        if (oper.category == oldName)
                        {
                            oper.category = newName;
                            count++;
                        }
                    }
                }
                save.categories.Remove(oldName);
            }
            return count;
        }
        public static List<string> ShowAllCategories()
        {
            return save.categories;
        }
        #endregion

        //operations
        #region
        public static OperationArg AddMoney(string name, float amount, string category)
        {
            if (save.reservedCategories.Contains(category)) return OperationArg.noCategory;
            if (!save.categories.Contains(category)) return OperationArg.noCategory;
            foreach (Account acc in save.accounts)
            {
                if (acc.name == name)
                {
                    if (!acc.Operate(amount, category)) return OperationArg.noMoney;
                    return OperationArg.sucsess;
                }
            }
            return OperationArg.unfound;
        }
        public static OperationArg AddMoney(string name, float amount)
        {
            foreach (Account acc in save.accounts)
            {
                if (acc.name == name)
                {
                    if (!acc.Operate(amount)) return OperationArg.noMoney;
                    return OperationArg.sucsess;
                }
            }
            return OperationArg.unfound;
        }
        public static OperationArg MoveMoneyFromOneToAnother(string fromName, string toName, float amount)
        {
            foreach (Account accFr in save.accounts)
            {
                if (accFr.name == fromName)
                {
                    if (accFr.money - amount >= 0)
                    {
                        foreach (Account accTo in save.accounts)
                        {
                            if (accTo.name == toName)
                            {
                                accFr.Operate(-amount, "Moved to another account");
                                accTo.Operate(amount, "Moved from another account");
                                return OperationArg.sucsess;
                            }
                        }
                        return OperationArg.unfound;
                    }
                    return OperationArg.noMoney;
                }
            }
            return OperationArg.unfound;
        }
        public static List<Operation> ShowOperations(DateTime dateFrom, DateTime dateTo)
        {
            List<Operation> returned = new List<Operation>();

            foreach (Account acc in save.accounts)
            {
                foreach (Operation op in acc.operations)
                {
                    if (op.date >= dateFrom && op.date <= dateTo)
                    {
                        returned.Add(op);
                    }
                }
            }

            return returned;
        }
        public static List<Operation> ShowOperations(string category)
        {
            List<Operation> returned = new List<Operation>();

            foreach (Account acc in save.accounts)
            {
                foreach (Operation op in acc.operations)
                {
                    if (op.category == category)
                    {
                        returned.Add(op);
                    }
                }
            }

            return returned;
        }
        public static List<Operation> ShowOperations(float moneyFrom, float moneyTo)
        {
            List<Operation> returned = new List<Operation>();

            foreach (Account acc in save.accounts)
            {
                foreach (Operation op in acc.operations)
                {
                    if (op.addition >= moneyFrom && op.addition <= moneyTo)
                    {
                        returned.Add(op);
                    }
                }
            }

            return returned;
        }
        #endregion
        static void Main(string[] args) { }
    }
    public enum Found { changed, unfound, foundSame }
    public enum OperationArg { unfound, noMoney, noCategory, sucsess }
}
