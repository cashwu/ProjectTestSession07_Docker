using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dapper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Sample.Repository;
using Sample.Repository.Database;
using Sample.RepositoryTests.Misc;

namespace Sample.RepositoryTests
{
    [TestClass()]
    [DeploymentItem(@"DbScripts\SampleDB_SchoolInfo_Create.sql")]
    [DeploymentItem(@"DbScripts\SampleDB_SchoolInfo_Data.sql")]
    public class SchoolInfoRepositoryTests
    {
        private IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.DatabaseConnectionFactory = Substitute.For<IDatabaseConnectionFactory>();

            this.DatabaseConnectionFactory
                .Create()
                .Returns(new SqlConnection(TestHook.SampleDbConnection));
        }

        public TestContext TestContext { get; set; }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            CreateTable();
            PrepareData();
        }

        private static void CreateTable()
        {
            using (var conn = new SqlConnection(TestHook.SampleDbConnection))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var script = File.ReadAllText(@"SampleDB_SchoolInfo_Create.sql");
                    conn.Execute(sql: script, transaction: trans);
                    trans.Commit();
                }
            }
        }

        private static void PrepareData()
        {
            using (var conn = new SqlConnection(TestHook.SampleDbConnection))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    var script = File.ReadAllText(@"SampleDB_SchoolInfo_Data.sql");
                    conn.Execute(sql: script, transaction: trans);
                    trans.Commit();
                }
            }
        }

        [ClassCleanup()]
        public static void TestClassCleanup()
        {
            using (var conn = new SqlConnection(TestHook.SampleDbConnection))
            {
                conn.Open();
                string sqlCommand = TableCommands.DropTable("SchoolInfo");
                conn.Execute(sqlCommand);
            }
        }

        private SchoolInfoRepository GetSystemUnderTest()
        {
            var sut = new SchoolInfoRepository(this.DatabaseConnectionFactory);
            return sut;
        }

        //---------------------------------------------------------------------

        [TestMethod()]
        public void GetAllSchoolInfosTest()
        {
            // arrange
            int expected = 4035;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetAllSchoolInfos();

            // assert
            actual.Should().NotBeNull();
            actual.Any().Should().BeTrue();
            actual.Count.Should().Be(expected);
        }

        [TestMethod]
        public void GetByCity_CityId輸入0000_應取得台北市所有學校資料()
        {
            // arrange
            string cityId = "0000";

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByCity(cityId);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.CityId == cityId).Should().BeTrue();
        }

        [TestMethod]
        public void GetByDistrict_CityId輸入0001_DistrictId輸入247_應取得新北市蘆洲區的所有學校資料()
        {
            // arrange
            string cityId = "0001";
            int districtId = 247;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByDistrict(cityId, districtId);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.CityId == cityId && x.DistrictId == districtId).Should().BeTrue();
        }

        [TestMethod]
        public void GetByConditions_CityId輸入0008_DistrictId輸入320_Caegory為5_應取得桃園市中壢區的所有大學資料()
        {
            // arrange
            string cityId = "0008";
            int districtId = 320;
            int category = 5;

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetByConditions(cityId, districtId, category);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.CityId == cityId && x.DistrictId == districtId && x.Category == category).Should().BeTrue();
        }

        [TestMethod]
        public void Get_Id輸入104526_應取得縣立阿里山國中小的學校資料()
        {
            // arrange
            string id = "104526";

            string expected = "縣立阿里山國中(小)";

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.Get(id);
            
            // assert
            actual.Should().NotBeNull();
            actual.Name.Should().Be(expected);
        }
    }
}