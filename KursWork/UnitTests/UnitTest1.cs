using NUnit.Framework;
using EntityContext;
using EntityService;
using InputManager;
using System.Collections.Generic;
namespace UnitTests
{
    public class Tests
    {
        static string savePath = System.AppDomain.CurrentDomain.BaseDirectory + @"save.bgg";
        [Test]
        public void Test1()
        {
            // Arrange

            // Act

            // Assert
            Assert.Pass();
        }
        // Test context
        #region
        [Test]
        public void ContextDeleteSave()
        {
            // Arrange
            System.IO.FileStream file = System.IO.File.Create(savePath);
            file.Close();

            // Act
            Context.DeleteSave();

            // Assert
            bool isFileStillExist = true;
            if (!System.IO.File.Exists(savePath)) isFileStillExist = false;
            Assert.IsFalse(isFileStillExist);
        }
        [Test]
        public void ContextSaveBinar()
        {
            // Arrange
            System.IO.File.Delete(savePath);
            Save save = new Save();

            // Act
            Context.SaveBinar(save);

            // Assert
            bool isFileStillExist = false;
            if (System.IO.File.Exists(savePath)) isFileStillExist = true;
            Assert.IsTrue(isFileStillExist);
        }
        [Test]
        public void ContextLoadSaveBinaryExists()
        {
            // Arrange
            Save save = new Save();
            Context.SaveBinar(save);
            save = null;

            // Act
            save = Context.LoadSaveBinary();

            // Assert
            Assert.IsNotNull(save);
        }
        [Test]
        public void ContextLoadSaveBinaryNotExists()
        {
            // Arrange
            Save save = new Save();
            System.IO.File.Delete(savePath);

            // Act
            save = Context.LoadSaveBinary();

            // Assert
            Assert.IsNull(save);
        }
        #endregion
        // TestService
        #region
        [Test]
        public void ServiceLoadSaveTrue()
        {
            // Arrange
            Save save = new Save();
            Context.SaveBinar(save);

            // Act
            bool pass = Service.LoadSave();

            // Assert
            Assert.IsTrue(pass);
        }
        [Test]
        public void ServiceLoadSaveFalse()
        {
            // Arrange
            System.IO.File.Delete(savePath);

            // Act
            bool pass = Service.LoadSave();

            // Assert
            Assert.IsFalse(pass);
        }
        [Test]
        public void ServiceSaveFile()
        {
            // Arrange
            Service.LoadSave();
            System.IO.File.Delete(savePath);

            // Act
            Service.SaveFile();

            // Assert
            bool isFileExists = false;
            isFileExists = System.IO.File.Exists(savePath);
            Assert.IsTrue(isFileExists);
        }
        [Test]
        public void ServiceDeleteSave()
        {
            // Arrange
            Service.LoadSave();
            Service.SaveFile();

            // Act
            Service.DeleteSave();

            // Assert
            bool isFileExists = true;
            isFileExists = System.IO.File.Exists(savePath);
            Assert.IsFalse(isFileExists);
        }
        [Test]
        public void ServiceAddAccountTrue()
        {
            // Arrange
            Service.LoadSave();

            // Act
            bool pass = Service.AddAccount("MainAcc");

            // Assert
            Assert.IsTrue(pass);
        }
        [Test]
        public void ServiceAddAccountFalse()
        {
            // Arrange
            Service.LoadSave();

            // Act
            Service.AddAccount("MyNewAcc");
            bool pass = Service.AddAccount("MyNewAcc");

            // Assert
            Assert.IsFalse(pass);
        }
        [Test]
        public void ServiceDeleteAccountTrue()
        {
            // Arrange
            Service.LoadSave();

            // Act
            Service.AddAccount("MyNewAccToDelete");
            bool pass = Service.DeleteAccount("MyNewAccToDelete");

            // Assert
            Assert.IsTrue(pass);
        }
        [Test]
        public void ServiceDeleteAccountFalse()
        {
            // Arrange
            Service.LoadSave();

            // Act
            bool pass = Service.DeleteAccount("MyNewAccToDeleteOrNot");

            // Assert
            Assert.IsFalse(pass);
        }
        [Test]
        public void ServiceChangeAccountFoundSame()
        {
            // Arrange
            Service.LoadSave();
            Service.AddAccount("oldAccount");
            Service.AddAccount("newAccount");

            // Act
            Found pass = Service.ChangeAcc("oldAccount", "newAccount");

            // Assert
            Assert.IsTrue(pass == Found.foundSame);
        }
        [Test]
        public void ServiceChangeAccountChanged()
        {
            // Arrange
            Service.LoadSave();
            Service.AddAccount("oldAccount");
            Service.AddAccount("newAccount");

            // Act
            Found pass = Service.ChangeAcc("oldAccount", "definetelyNewAccount");

            // Assert
            Assert.IsTrue(pass == Found.changed);
        }
        [Test]
        public void ServiceChangeAccountUnfound()
        {
            // Arrange
            Service.LoadSave();
            Service.AddAccount("oldAccount");
            Service.AddAccount("newAccount");

            // Act
            Found pass = Service.ChangeAcc("waldo'sAccount", "newAccount");

            // Assert
            Assert.IsTrue(pass == Found.unfound);
        }
        [Test]
        public void ServiceShowAllAccounts()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("oldAccount");
            Service.AddAccount("newAccount");

            // Act
            List<Account> pass = Service.ShowAllAccounts();

            // Assert
            Assert.AreEqual(pass.Count, 3);
        }
        [Test]
        public void ServiceShowMoneyOnAccountMoreOrEqualToZero()
        {
            // Arrange
            Service.LoadSave();
            Service.AddAccount("account");

            // Act
            float pass = Service.ShowMoneyOnAccount("account");

            // Assert
            Assert.IsTrue(pass >= 0);
        }
        [Test]
        public void ServiceShowMoneyOnAccountLessToZero()
        {
            // Arrange
            Service.LoadSave();
            Service.AddAccount("account");

            // Act
            float pass = Service.ShowMoneyOnAccount("waldo'sAccount");

            // Assert
            Assert.IsTrue(pass < 0);
        }
        [Test]
        public void ServiceAddCategoryTrue()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            bool pass = Service.AddCategory("newCat");

            // Assert
            Assert.IsTrue(pass);
        }
        [Test]
        public void ServiceAddCategoryFalse()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddCategory("newCat");

            // Act
            bool pass = Service.AddCategory("newCat");

            // Assert
            Assert.IsFalse(pass);
        }
        [Test]
        public void ServiceDeleteCategoryFalseUntagged()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            bool pass = Service.DeleteCategory("Untagged");

            // Assert
            Assert.IsFalse(pass);
        }
        [Test]
        public void ServiceDeleteCategoryFalseUnfound()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            bool pass = Service.DeleteCategory("waldo's category");

            // Assert
            Assert.IsFalse(pass);
        }
        [Test]
        public void ServiceDeleteCategoryFalseReserved()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            bool pass = Service.DeleteCategory("Moved to another account");

            // Assert
            Assert.IsFalse(pass);
        }
        [Test]
        public void ServiceDeleteCategoryTrue()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddCategory("newCat");
            // Act
            bool pass = Service.DeleteCategory("newCat");

            // Assert
            Assert.IsTrue(pass);
        }
        [Test]
        public void ServiceChangeCategoryLessZeroArg1()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            float pass = Service.ChangeCategory("Moved to another account", "newCat");

            // Assert
            Assert.IsTrue(pass < 0);
        }
        [Test]
        public void ServiceChangeCategoryLessZeroArg2()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            float pass = Service.ChangeCategory("newCat", "Moved to another account");

            // Assert
            Assert.IsTrue(pass < 0);
        }
        [Test]
        public void ServiceChangeCategoryUntaggedLessZeroFound()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddCategory("newCat");

            // Act
            float pass = Service.ChangeCategory("Untagged", "newCat");

            // Assert
            Assert.IsTrue(pass < 0);
        }
        [Test]
        public void ServiceChangeCategoryLessZeroFoundArg1()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddCategory("newCategory");

            // Act
            float pass = Service.ChangeCategory("newCat", "newCat2");

            // Assert
            Assert.IsTrue(pass < 0);
        }
        [Test]
        public void ServiceChangeCategoryLessZeroFoundArg2()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddCategory("newCat");
            Service.AddCategory("newCat2");

            // Act
            float pass = Service.ChangeCategory("newCat", "newCat2");

            // Assert
            Assert.IsTrue(pass < 0);
        }
        [Test]
        public void ServiceChangeCategoryMoreEqualZero()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddCategory("newCat");

            // Act
            float pass = Service.ChangeCategory("newCat", "newCat2");

            // Assert
            Assert.IsTrue(pass >= 0);
        }
        [Test]
        public void ServiceShowAllCategories()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddCategory("newCat");

            // Act
            List<string> pass = Service.ShowAllCategories();

            // Assert
            Assert.IsTrue(pass.Count >= 0);
        }
        [Test]
        public void ServiceAddMoneyNoCatReserved()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc");

            // Act
            OperationArg pass = Service.AddMoney("newAcc", 100, "Moved to another account");

            // Assert
            Assert.IsTrue(pass == OperationArg.noCategory);
        }
        [Test]
        public void ServiceAddMoneyNoCatUnfound()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc");

            // Act
            OperationArg pass = Service.AddMoney("newAcc", 100, "waldo'sCategory");

            // Assert
            Assert.IsTrue(pass == OperationArg.noCategory);
        }
        [Test]
        public void ServiceAddMoneyNoAccUnfound()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddCategory("newCat");

            // Act
            OperationArg pass = Service.AddMoney("newAcc", 100, "newCat");

            // Assert
            Assert.IsTrue(pass == OperationArg.unfound);
        }
        [Test]
        public void ServiceAddMoneyNoMoney()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc");
            Service.AddCategory("newCat");

            // Act
            OperationArg pass = Service.AddMoney("newAcc", -100, "newCat");

            // Assert
            Assert.IsTrue(pass == OperationArg.noMoney);
        }
        [Test]
        public void ServiceAddMoneySucsess()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc");
            Service.AddCategory("newCat");

            // Act
            OperationArg pass = Service.AddMoney("newAcc", 100, "newCat");

            // Assert
            Assert.IsTrue(pass == OperationArg.sucsess);
        }
        [Test]
        public void ServiceAddMoneyNoMoneyNoArg()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc");

            // Act
            OperationArg pass = Service.AddMoney("newAcc", -100);

            // Assert
            Assert.IsTrue(pass == OperationArg.noMoney);
        }
        [Test]
        public void ServiceAddMoneyNoAccNoArg()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            OperationArg pass = Service.AddMoney("newAcc", 100);

            // Assert
            Assert.IsTrue(pass == OperationArg.unfound);
        }
        [Test]
        public void ServiceAddMoneySucsessNoCat()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc");

            // Act
            OperationArg pass = Service.AddMoney("newAcc", 100);

            // Assert
            Assert.IsTrue(pass == OperationArg.sucsess);
        }
        [Test]
        public void ServiceMoveMoneyFromOneToAnotherUnfound1()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc2");

            // Act
            OperationArg pass = Service.MoveMoneyFromOneToAnother("newAcc", "newAcc2", 100);

            // Assert
            Assert.IsTrue(pass == OperationArg.unfound);
        }
        [Test]
        public void ServiceMoveMoneyFromOneToAnotherUnfound2()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc");
            Service.AddMoney("newAcc", 1000);

            // Act
            OperationArg pass = Service.MoveMoneyFromOneToAnother("newAcc", "newAcc2", 100);

            // Assert
            Assert.IsTrue(pass == OperationArg.unfound);
        }
        [Test]
        public void ServiceMoveMoneyFromOneToAnotherNoMoney()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc");
            Service.AddMoney("newAcc", 10);
            Service.AddAccount("newAcc2");

            // Act
            OperationArg pass = Service.MoveMoneyFromOneToAnother("newAcc", "newAcc2", 100);

            // Assert
            Assert.IsTrue(pass == OperationArg.noMoney);
        }
        [Test]
        public void ServiceMoveMoneyFromOneToAnotherSucsess()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();
            Service.AddAccount("newAcc");
            Service.AddMoney("newAcc", 1000);
            Service.AddAccount("newAcc2");

            // Act
            OperationArg pass = Service.MoveMoneyFromOneToAnother("newAcc", "newAcc2", 100);

            // Assert
            Assert.IsTrue(pass == OperationArg.sucsess);
        }
        [Test]
        public void ServiceShowOperationsDate()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            List<Operation> pass = Service.ShowOperations(System.DateTime.MinValue, System.DateTime.Today);

            // Assert
            Assert.IsTrue(pass.Count >= 0);
        }
        [Test]
        public void ServiceShowOperationsCategory()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            List<Operation> pass = Service.ShowOperations("Untagged");

            // Assert
            Assert.IsTrue(pass.Count >= 0);
        }
        [Test]
        public void ServiceShowOperationsMoney()
        {
            // Arrange
            Service.DeleteSave();
            Service.LoadSave();

            // Act
            List<Operation> pass = Service.ShowOperations(100, 10000);

            // Assert
            Assert.IsTrue(pass.Count >= 0);
        }
        #endregion
    }
}