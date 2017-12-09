using System.Collections.Generic;
using Sample.Repository.Models;

namespace Sample.Repository
{
    public interface ISchoolInfoRepository
    {
        /// <summary>
        /// 取得所有學校資訊.
        /// </summary>
        /// <returns>List&lt;SchoolInfoModel&gt;.</returns>
        List<SchoolInfoModel> GetAllSchoolInfos();

        /// <summary>
        /// 依據縣市取得學校資料.
        /// </summary>
        /// <param name="cityid">The cityid.</param>
        /// <returns>List&lt;SchoolInfoModel&gt;.</returns>
        List<SchoolInfoModel> GetByCity(string cityid);

        /// <summary>
        /// 依據縣市、鄉鎮市區取得學校資料.
        /// </summary>
        /// <param name="cityid">The cityid.</param>
        /// <param name="countyId">The county identifier.</param>
        /// <returns>List&lt;SchoolInfoModel&gt;.</returns>
        List<SchoolInfoModel> GetByDistrict(string cityid, int countyId);

        /// <summary>
        /// 依據縣市、鄉鎮市區、類別取得學校資料.
        /// </summary>
        /// <param name="cityid">The cityid.</param>
        /// <param name="countyId">The county identifier.</param>
        /// <param name="category">The category.</param>
        /// <returns>List&lt;SchoolInfoModel&gt;.</returns>
        List<SchoolInfoModel> GetByConditions(string cityid, int countyId, int category);

        /// <summary>
        /// 依據學校Id取得學校資料.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>SchoolInfoModel.</returns>
        SchoolInfoModel Get(string id);
    }
}