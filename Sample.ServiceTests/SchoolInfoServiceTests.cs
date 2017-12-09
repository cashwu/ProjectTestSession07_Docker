using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CsvHelper;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Sample.Repository;
using Sample.Repository.Models;
using Sample.Service;
using Sample.Service.Mappings;

// ReSharper disable ArrangeThisQualifier
// ReSharper disable ExpressionIsAlwaysNull

namespace Sample.ServiceTests
{
    [TestClass()]
    [DeploymentItem(@"TestData\SchoolInfoModel.csv")]
    public class SchoolInfoServiceTests
    {
        private ISchoolInfoRepository SchoolInfoRepository { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Mapper.Initialize
            (
                cfg =>
                {
                    cfg.AddProfile<ServiceMappingProfile>();
                }
            );

            this.SchoolInfoRepository = Substitute.For<ISchoolInfoRepository>();
        }

        private SchoolInfoService GetSystemUnderTest()
        {
            var sut = new SchoolInfoService(this.SchoolInfoRepository);
            return sut;
        }

        private static List<SchoolInfoModel> GetDataSourceFromCsv()
        {
            var models = new List<SchoolInfoModel>();

            using (var sr = new StreamReader(@"SchoolInfoModel.csv"))
            using (var reader = new CsvReader(sr))
            {
                var records = reader.GetRecords<SchoolInfoModel>();
                models.AddRange(records);
            }

            return models;
        }

        //---------------------------------------------------------------------

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetAllSchoolInfos")]
        public void GetAllSchoolInfos_應取得全部學校資料_4035筆()
        {
            // arrange
            var dataSource = GetDataSourceFromCsv();
            this.SchoolInfoRepository.GetAllSchoolInfos().Returns(dataSource);

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetAllSchoolInfos();

            // assert
            actual.Any().Should().BeTrue();
            actual.Count.Should().Be(4035);
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCity")]
        public void GetSchoolInfosByCity_CityId輸入0000_應取得位於縣市為台北市的所有學校資料()
        {
            // arrange
            string cityId = "0000";

            var dataSource = GetDataSourceFromCsv();

            var schoolModels = dataSource.Where(x => x.CityId == "0000").ToList();

            this.SchoolInfoRepository.GetByCity(cityId)
                .ReturnsForAnyArgs(schoolModels);

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetSchoolInfosByCity(cityId);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.Address.StartsWith("臺北市")).Should().BeTrue();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCity")]
        public void GetSchoolInfosByCity_CityId為null_應拋出ArgumentNullException()
        {
            // arrange
            string cityId = null;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByCity(cityId);

            // assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCity")]
        public void GetSchoolInfosByCity_CityId為空白_應拋出ArgumentNullException()
        {
            // arrange
            string cityId = "";

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByCity(cityId);

            // assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByDistrict")]
        public void GetSchoolInfosByDistrict_CityId為0000_DistrictId為100_應回傳台北市中正區的所有學校資料()
        {
            string cityId = "0000";
            int districtId = 100;

            var dataSource = GetDataSourceFromCsv();

            var schoolModels = dataSource.Where
                                         (
                                             x => x.CityId == "0000"
                                                  &&
                                                  x.DistrictId == 100
                                         )
                                         .ToList();

            this.SchoolInfoRepository.GetByDistrict(cityId, districtId)
                .ReturnsForAnyArgs(schoolModels);

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetSchoolInfosByDistrict(cityId, districtId);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.Address.StartsWith("臺北市") && x.Address.Contains("中正區")).Should().BeTrue();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByDistrict")]
        public void GetSchoolInfosByDistrict_CityId為null_DistrictId為0_應拋出ArgumentNullException()
        {
            string cityId = null;
            int districtId = 0;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByDistrict(cityId, districtId);

            // assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByDistrict")]
        public void GetSchoolInfosByDistrict_CityId為空白_DistrictId為0_應拋出ArgumentNullException()
        {
            string cityId = "";
            int districtId = 0;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByDistrict(cityId, districtId);

            // assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByDistrict")]
        public void GetSchoolInfosByDistrict_CityId為0000_DistrictId為0_應拋出ArgumentOutOfRangeException()
        {
            string cityId = "0000";
            int districtId = 0;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByDistrict(cityId, districtId);

            // assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByDistrict")]
        public void GetSchoolInfosByDistrict_CityId為0000_DistrictId為負1_應拋出ArgumentOutOfRangeException()
        {
            string cityId = "0000";
            int districtId = -1;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByDistrict(cityId, districtId);

            // assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCondition")]
        public void GetSchoolInfosByCondition_CityId輸入0001_DistrictId輸入220_Category輸入2_應取得新北市板橋區所有國小的學校資料()
        {
            // arrange
            string cityId = "0001";
            int districtId = 220;
            int category = 2;

            var dataSource = GetDataSourceFromCsv();
            var schoolModels = dataSource.Where
                                         (
                                             x =>
                                                 x.CityId == "0001"
                                                 &&
                                                 x.DistrictId == 220
                                                 &&
                                                 x.Category == 2
                                         )
                                         .ToList();

            this.SchoolInfoRepository.GetByConditions(cityId, districtId, category)
                .ReturnsForAnyArgs(schoolModels);

            var sut = this.GetSystemUnderTest();

            // act
            var actual = sut.GetSchoolInfosByCondition(cityId, districtId, category);

            // assert
            actual.Any().Should().BeTrue();
            actual.All(x => x.CityId == cityId && x.DistrictId == districtId && x.Category == category).Should().BeTrue();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCondition")]
        public void GetSchoolInfosByCondition_CityId為null_DistrictId為0_應拋出ArgumentNullException()
        {
            string cityId = null;
            int districtId = 0;
            int category = 0;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByCondition(cityId, districtId, category);

            // assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCondition")]
        public void GetSchoolInfosByCondition_CityId為空白_DistrictId為0_應拋出ArgumentNullException()
        {
            string cityId = "";
            int districtId = 0;
            int category = 0;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByCondition(cityId, districtId, category);

            // assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCondition")]
        public void GetSchoolInfosByCondition_CityId為0000_DistrictId為0_應拋出ArgumentOutOfRangeException()
        {
            string cityId = "0000";
            int districtId = 0;
            int category = 0;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByCondition(cityId, districtId, category);

            // assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCondition")]
        public void GetSchoolInfosByCondition_CityId為0000_DistrictId為負1_應拋出ArgumentOutOfRangeException()
        {
            string cityId = "0000";
            int districtId = -1;
            int category = 0;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByCondition(cityId, districtId, category);

            // assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCondition")]
        public void GetSchoolInfosByCondition_CityId為0000_DistrictId為100_category為0_應拋出ArgumentOutOfRangeException()
        {
            string cityId = "0000";
            int districtId = 100;
            int category = 0;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByCondition(cityId, districtId, category);

            // assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCondition")]
        public void GetSchoolInfosByCondition_CityId為0000_DistrictId為100_category為負1_應拋出ArgumentOutOfRangeException()
        {
            string cityId = "0000";
            int districtId = 100;
            int category = -1;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByCondition(cityId, districtId, category);

            // assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        [Owner("Kevin")]
        [TestCategory("SchoolInfoService")]
        [TestProperty("SchoolInfoService", "GetSchoolInfosByCondition")]
        public void GetSchoolInfosByCondition_CityId為0000_DistrictId為100_category為6_應拋出ArgumentOutOfRangeException()
        {
            string cityId = "0000";
            int districtId = 100;
            int category = 6;

            var sut = this.GetSystemUnderTest();

            // act
            Action action = () => sut.GetSchoolInfosByCondition(cityId, districtId, category);

            // assert
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}