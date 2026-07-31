using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.People
{
    /// <summary>
    /// 通过文件夹导入人员请求参数
    /// </summary>
    public class InputPeopleByPathRequestDTO
    {
        public string PhotoPath { get; set; }   
    }
}
