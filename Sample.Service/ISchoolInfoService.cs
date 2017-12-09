using System.Collections.Generic;
using Sample.Service.DTOs;

namespace Sample.Service
{
    public interface ISchoolInfoService
    {
        /// <summary>
        /// 取得所有學校資訊.
        /// </summary>
        /// <returns>List&lt;SchoolInfoDto&gt;.</returns>
        List<SchoolInfoDto> GetAllSchoolInfos();

        /// <summary>
        /// 取得指定條件下的學校資訊.
        /// </summary>
        /// <param name="cityId">縣市ID.</param>
        /// <returns>List&lt;SchoolInfoDto&gt;.</returns>
        List<SchoolInfoDto> GetSchoolInfosByCity(string cityId);

        /// <summary>
        /// 取得指定條件下的學校資訊.
        /// </summary>
        /// <param name="cityId">縣市ID.</param>
        /// <param name="districtId">鄉鎮市區ID.</param>
        /// <returns>List&lt;SchoolInfoDto&gt;.</returns>
        List<SchoolInfoDto> GetSchoolInfosByDistrict(string cityId, int districtId);

        /// <summary>
        /// 取得指定條件下的學校資訊.
        /// </summary>
        /// <param name="cityId">縣市ID.</param>
        /// <param name="districtId">鄉鎮市區ID.</param>
        /// <param name="category">學校類別.</param>
        /// <returns>List&lt;SchoolInfoDto&gt;.</returns>
        List<SchoolInfoDto> GetSchoolInfosByCondition(string cityId, int districtId, int category);

    }
}