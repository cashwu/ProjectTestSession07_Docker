using System;
using System.Collections.Generic;
using AutoMapper;
using Sample.Repository;
using Sample.Repository.Models;
using Sample.Service.DTOs;

namespace Sample.Service
{
    /// <summary>
    /// Class SchoolInfoService.
    /// </summary>
    /// <seealso cref="Sample.Service.ISchoolInfoService" />
    public class SchoolInfoService : ISchoolInfoService
    {
        private ISchoolInfoRepository SchoolInfoRepository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolInfoService"/> class.
        /// </summary>
        /// <param name="schoolInfoRepository">The school information repository.</param>
        public SchoolInfoService(ISchoolInfoRepository schoolInfoRepository)
        {
            this.SchoolInfoRepository = schoolInfoRepository;
        }

        /// <summary>
        /// 取得所有學校資訊.
        /// </summary>
        /// <returns>List&lt;SchoolInfoDto&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<SchoolInfoDto> GetAllSchoolInfos()
        {
            var source = this.SchoolInfoRepository.GetAllSchoolInfos();

            var result = Mapper.Map<List<SchoolInfoModel>, List<SchoolInfoDto>>
            (
                source
            );

            return result;
        }

        /// <summary>
        /// 取得指定條件下的學校資訊.
        /// </summary>
        /// <param name="cityId">縣市ID.</param>
        /// <returns>List&lt;SchoolInfoDto&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<SchoolInfoDto> GetSchoolInfosByCity(string cityId)
        {
            if (string.IsNullOrWhiteSpace(cityId))
            {
                throw new ArgumentNullException(nameof(cityId));
            }

            var source = this.SchoolInfoRepository.GetByCity(cityId);

            var result = Mapper.Map<List<SchoolInfoModel>, List<SchoolInfoDto>>
            (
                source
            );

            return result;
        }

        /// <summary>
        /// 取得指定條件下的學校資訊.
        /// </summary>
        /// <param name="cityId">縣市ID.</param>
        /// <param name="districtId">鄉鎮市區ID.</param>
        /// <returns>List&lt;SchoolInfoDto&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<SchoolInfoDto> GetSchoolInfosByDistrict(string cityId,
                                                            int districtId)
        {
            if (string.IsNullOrWhiteSpace(cityId))
            {
                throw new ArgumentNullException(nameof(cityId));
            }
            if (districtId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(districtId));
            }

            var source = this.SchoolInfoRepository.GetByDistrict(cityId, districtId);

            var result = Mapper.Map<List<SchoolInfoModel>, List<SchoolInfoDto>>
            (
                source
            );

            return result;
        }

        /// <summary>
        /// 取得指定條件下的學校資訊.
        /// </summary>
        /// <param name="cityId">縣市ID.</param>
        /// <param name="districtId">鄉鎮市區ID.</param>
        /// <param name="category">學校類別.</param>
        /// <returns>List&lt;SchoolInfoDto&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<SchoolInfoDto> GetSchoolInfosByCondition(string cityId,
                                                             int districtId,
                                                             int category)
        {
            if (string.IsNullOrWhiteSpace(cityId))
            {
                throw new ArgumentNullException(nameof(cityId));
            }
            if (districtId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(districtId));
            }
            if (category < 2 || category > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(category));
            }

            var source = this.SchoolInfoRepository.GetByConditions
            (
                cityId, districtId, category
            );

            var result = Mapper.Map<List<SchoolInfoModel>, List<SchoolInfoDto>>
            (
                source
            );

            return result;
        }
    }
}