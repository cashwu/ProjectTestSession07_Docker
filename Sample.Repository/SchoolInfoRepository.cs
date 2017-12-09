using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Sample.Repository.Database;
using Sample.Repository.Models;

namespace Sample.Repository
{
    public class SchoolInfoRepository : ISchoolInfoRepository
    {
        private IDatabaseConnectionFactory DatabaseConnectionFactory { get; }

        public SchoolInfoRepository(IDatabaseConnectionFactory connectionFactory)
        {
            this.DatabaseConnectionFactory = connectionFactory;
        }

        /// <summary>
        /// 取得所有學校資訊.
        /// </summary>
        /// <returns>List&lt;SchoolInfoModel&gt;.</returns>
        public List<SchoolInfoModel> GetAllSchoolInfos()
        {
            using (var conn = this.DatabaseConnectionFactory.Create())
            {
                var sqlCommand = new StringBuilder();
                sqlCommand.AppendLine(@"SELECT [Id]");
                sqlCommand.AppendLine(@"      ,[Category]");
                sqlCommand.AppendLine(@"      ,[Name]");
                sqlCommand.AppendLine(@"      ,[IsPublic]");
                sqlCommand.AppendLine(@"      ,[CityId]");
                sqlCommand.AppendLine(@"      ,[DistrictId]");
                sqlCommand.AppendLine(@"      ,[Address]");
                sqlCommand.AppendLine(@"      ,[Phone]");
                sqlCommand.AppendLine(@"      ,[Url]");
                sqlCommand.AppendLine(@"  FROM [SchoolInfo]");

                var models = conn.Query<SchoolInfoModel>(sqlCommand.ToString());
                return models.ToList();
            }
        }

        /// <summary>
        /// 依據縣市取得學校資料.
        /// </summary>
        /// <param name="cityid">The cityid.</param>
        /// <returns>List&lt;SchoolInfoModel&gt;.</returns>
        public List<SchoolInfoModel> GetByCity(string cityid)
        {
            using (var conn = this.DatabaseConnectionFactory.Create())
            {
                var sqlCommand = new StringBuilder();
                sqlCommand.AppendLine(@"SELECT [Id]");
                sqlCommand.AppendLine(@"      ,[Category]");
                sqlCommand.AppendLine(@"      ,[Name]");
                sqlCommand.AppendLine(@"      ,[IsPublic]");
                sqlCommand.AppendLine(@"      ,[CityId]");
                sqlCommand.AppendLine(@"      ,[DistrictId]");
                sqlCommand.AppendLine(@"      ,[Address]");
                sqlCommand.AppendLine(@"      ,[Phone]");
                sqlCommand.AppendLine(@"      ,[Url]");
                sqlCommand.AppendLine(@"  FROM [SchoolInfo]");
                sqlCommand.AppendLine(@"  where CityId = @CityId ");

                var models = conn.Query<SchoolInfoModel>
                (
                    sql: sqlCommand.ToString(),
                    param: new
                    {
                        CityId = cityid
                    }
                );

                return models.ToList();
            }
        }

        /// <summary>
        /// 依據縣市、鄉鎮市區取得學校資料.
        /// </summary>
        /// <param name="cityid">The cityid.</param>
        /// <param name="countyId">The county identifier.</param>
        /// <returns>List&lt;SchoolInfoModel&gt;.</returns>
        public List<SchoolInfoModel> GetByDistrict(string cityid, int countyId)
        {
            using (var conn = this.DatabaseConnectionFactory.Create())
            {
                var sqlCommand = new StringBuilder();
                sqlCommand.AppendLine(@"SELECT [Id]");
                sqlCommand.AppendLine(@"      ,[Category]");
                sqlCommand.AppendLine(@"      ,[Name]");
                sqlCommand.AppendLine(@"      ,[IsPublic]");
                sqlCommand.AppendLine(@"      ,[CityId]");
                sqlCommand.AppendLine(@"      ,[DistrictId]");
                sqlCommand.AppendLine(@"      ,[Address]");
                sqlCommand.AppendLine(@"      ,[Phone]");
                sqlCommand.AppendLine(@"      ,[Url]");
                sqlCommand.AppendLine(@"  FROM [SchoolInfo]");
                sqlCommand.AppendLine(@"  where CityId = @CityId and DistrictId = @DistrictId ");

                var models = conn.Query<SchoolInfoModel>
                (
                    sql: sqlCommand.ToString(),
                    param: new
                    {
                        CityId = cityid,
                        DistrictId = countyId
                    }
                );

                return models.ToList();
            }
        }

        /// <summary>
        /// 依據縣市、鄉鎮市區、類別取得學校資料.
        /// </summary>
        /// <param name="cityid">The cityid.</param>
        /// <param name="countyId">The county identifier.</param>
        /// <param name="category">The category.</param>
        /// <returns>List&lt;SchoolInfoModel&gt;.</returns>
        public List<SchoolInfoModel> GetByConditions(string cityid, int countyId, int category)
        {
            using (var conn = this.DatabaseConnectionFactory.Create())
            {
                var sqlCommand = new StringBuilder();
                sqlCommand.AppendLine(@"SELECT [Id]");
                sqlCommand.AppendLine(@"      ,[Category]");
                sqlCommand.AppendLine(@"      ,[Name]");
                sqlCommand.AppendLine(@"      ,[IsPublic]");
                sqlCommand.AppendLine(@"      ,[CityId]");
                sqlCommand.AppendLine(@"      ,[DistrictId]");
                sqlCommand.AppendLine(@"      ,[Address]");
                sqlCommand.AppendLine(@"      ,[Phone]");
                sqlCommand.AppendLine(@"      ,[Url]");
                sqlCommand.AppendLine(@"  FROM [SchoolInfo]");
                sqlCommand.AppendLine(@"  where CityId = @CityId ");
                sqlCommand.AppendLine(@"  and DistrictId = @DistrictId ");
                sqlCommand.AppendLine(@"  and Category = @Category ");

                var models = conn.Query<SchoolInfoModel>
                (
                    sql: sqlCommand.ToString(),
                    param: new
                    {
                        CityId = cityid,
                        Districtid = countyId,
                        Category = category
                    }
                );

                return models.ToList();
            }
        }

        /// <summary>
        /// 依據學校Id取得學校資料.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SchoolInfoModel.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public SchoolInfoModel Get(string id)
        {
            using (var conn = this.DatabaseConnectionFactory.Create())
            {
                var sqlCommand = new StringBuilder();
                sqlCommand.AppendLine(@"SELECT [Id]");
                sqlCommand.AppendLine(@"      ,[Category]");
                sqlCommand.AppendLine(@"      ,[Name]");
                sqlCommand.AppendLine(@"      ,[IsPublic]");
                sqlCommand.AppendLine(@"      ,[CityId]");
                sqlCommand.AppendLine(@"      ,[DistrictId]");
                sqlCommand.AppendLine(@"      ,[Address]");
                sqlCommand.AppendLine(@"      ,[Phone]");
                sqlCommand.AppendLine(@"      ,[Url]");
                sqlCommand.AppendLine(@"  FROM [SchoolInfo]");
                sqlCommand.AppendLine(@"  where Id = @Id ");

                var model = conn.QueryFirst<SchoolInfoModel>
                (
                    sql: sqlCommand.ToString(),
                    param: new
                    {
                        Id = id
                    }
                );

                return model;
            }
        }
    }
}