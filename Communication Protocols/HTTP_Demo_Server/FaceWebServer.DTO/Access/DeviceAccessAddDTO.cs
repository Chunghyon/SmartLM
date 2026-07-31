using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.Access
{
    public class DeviceAccessAddDTO
    {
        /// <summary>
        /// 需要授权的设备ID列表
        /// </summary>
        public List<int> DeviceIDs { get; set; }

        /// <summary>
        /// 需要授权的人员ID列表
        /// </summary>
        public List<int>? PeopleIDs { get; set; }

        /// <summary>
        /// 人员角色 0,普通人员；1，管理员;2 黑名单
        /// </summary>
        public int AccessType { get; set; }

        /// <summary>
        /// 截止日期  unix 时间戳 秒级  
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 开门次数 0-65535；  65535--表示无限制，0--表示禁止通行
        /// </summary>
        public int OpenTimes { get; set; }


        /// <summary>
        /// 是否为常开卡，1--是；0--否
        /// </summary>
        public int KeepOpen { get; set; }

        /// <summary>
        /// 开门时段组号
        /// </summary>
        public int Timegroup { get; set; }

        /// <summary>
        /// 节假日受限，逗号分隔：1,2,3,4,5
        /// </summary>
        public string Holidays { get; set; }

        /// <summary>
        /// 电梯权限，逗号分隔：1,2,3,4,5
        /// </summary>
        public string Elevators { get; set; }
    }
}
