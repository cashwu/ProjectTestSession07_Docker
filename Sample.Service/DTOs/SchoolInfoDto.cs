namespace Sample.Service.DTOs
{
    /// <summary>
    /// Class SchoolInfoDto.
    /// </summary>
    public class SchoolInfoDto
    {
        /// <summary>
        /// 學校ID.
        /// </summary>
        /// <value>The school id.</value>
        public string Id { get; set; }

        /// <summary>
        /// 學校類別 2:國小; 3:國中; 4:高中職; 5:大專院校.
        /// </summary>
        /// <value>The school type identifier.</value>
        public int Category { get; set; }

        /// <summary>
        /// 學校名稱.
        /// </summary>
        /// <value>The name of the school.</value>
        public string Name { get; set; }

        /// <summary>
        /// 公立/私立.
        /// </summary>
        /// <value>The is public.</value>
        public string IsPublic { get; set; }

        /// <summary>
        /// 縣市ID.
        /// </summary>
        /// <value>The City Id.</value>
        public string CityId { get; set; }

        /// <summary>
        /// 鄉鎮市區ID.
        /// </summary>
        /// <value>The District Id.</value>
        public int DistrictId { get; set; }

        /// <summary>
        /// 學校地址.
        /// </summary>
        /// <value>The address.</value>
        public string Address { get; set; }

        /// <summary>
        /// 學校電話.
        /// </summary>
        /// <value>The Phone.</value>
        public string Phone { get; set; }

        /// <summary>
        /// 學校網址.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }
    }
}