using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sample.RepositoryTests.Misc;
using Sample.RepositoryTests.TestUtilities;

namespace Sample.RepositoryTests
{
    [TestClass]
    public class TestHook
    {
        internal static string SampleDbConnection { get; set; }


        private static string _databaseType;

        private static string DatabaseType
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_databaseType))
                {
                    return _databaseType;
                }


                var settingValue = ConfigurationManager.AppSettings["DatabaseType"];
                _databaseType = string.IsNullOrWhiteSpace(settingValue)
                                    ? "localdb"
                                    : settingValue.ToLower();

                return _databaseType;
            }
        }

        private static string _containerType;

        private static string ContainerType
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_containerType))
                {
                    return _containerType;
                }

                var settingValue = ConfigurationManager.AppSettings["ContainerType"];
                _containerType = string.IsNullOrWhiteSpace(settingValue)
                                     ? "Linux"
                                     : settingValue;

                return _containerType;
            }
        }

        private static string DatabaseIp { get; set; }

        private static string ContainerId { get; set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            if (DatabaseType.Equals("localdb"))
            {
                CreateLocalDB();
            }
            else
            {
                CreateDockerContainer();
            }
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (DatabaseType.Equals("localdb"))
            {
                DeleteLocalDB();
            }
            else
            {
                DeleteDockerContainer();
            }
        }

        private static void CreateLocalDB()
        {
            // SampleDB
            var sampleDbDatabase = new TestDbUtilities(DatabaseName.SampleDB);
            if (sampleDbDatabase.IsLocalDbExists())
            {
                sampleDbDatabase.DeleteLocalDb();
            }
            sampleDbDatabase.CreateDatabase();

            SampleDbConnection = string.Format(TestDbConnection.LocalDb.Database, DatabaseName.SampleDB);
        }

        private static void DeleteLocalDB()
        {
            var defaultDatabase = new TestDbUtilities(DatabaseName.Default);
            defaultDatabase.DeleteLocalDb(SampleDbConnection);
        }

        private static void CreateDockerContainer()
        {
            // 建立測試用測試資料庫的 container

            DockerSupport.CreateContainer(ContainerType, out var databaseIp, out var containerId);

            DatabaseIp = databaseIp;
            ContainerId = containerId;

            // 於 container 裡的 sql-server 建立測試用的 database
            var connectionString = string.Format(TestDbConnection.Container.Master, DatabaseIp);
            DatabaseCommand.CreateDatabase(connectionString, DatabaseName.SampleDB);

            SampleDbConnection = string.Format(TestDbConnection.Container.Database, DatabaseIp, DatabaseName.SampleDB);
        }

        private static void DeleteDockerContainer()
        {
            // 移除測試資料庫
            DockerSupport.StopContainer(ContainerId);
        }
    }
}