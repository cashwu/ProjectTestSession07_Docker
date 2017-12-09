using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.RepositoryTests.Misc;

namespace Sample.RepositoryTests
{
    [TestClass]
    public class TestHook
    {
        internal static string SampleDbConnection =>
                string.Format(TestDbConnection.LocalDb.Database, DatabaseName.SampleDB);

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            // Northwind
            var northwindDatabase = new TestDbUtilities(DatabaseName.SampleDB);
            if (northwindDatabase.IsLocalDbExists())
            {
                northwindDatabase.DeleteLocalDb();
            }
            northwindDatabase.CreateDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            var defaultDatabase = new TestDbUtilities(DatabaseName.Default);
            defaultDatabase.DeleteLocalDb(SampleDbConnection);
        }
    }
}