using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceWebServer.DTO.HTTPv1_Protocol
{
    /// <summary>
    /// HTTPv1 协议设备推送记录请求   /note/insertNoteFace 
    /// </summary>
    public class HTTPPushRecordRequest
    {
        /// <summary>
        /// 记录详情，Json字符串
        /// </summary>
        public string RecordJson { get; set; }

        /// <summary>
        /// 记录中图片的地址
        /// </summary>
        public string Pic { get; set; }
    }

    /// <summary>
    /// 设备推送来的记录详情
    /// </summary>
    public class HTTPPushRecordDetail
    {
        /// <summary>
        /// 设备 ID 
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// 事件类型
        /// <para>  1--正常通行(已注册或者陌生人免验证)       (开门事件)   </para>
        /// <para>  2--未注册（陌生人）                       (不开门事件) </para>
        /// <para>  3--有效期已过期或未到起始时间             (不开门事件) </para>
        /// <para>  4--通行次数已用尽                         (不开门事件) </para>
        /// <para>  5--不在开门时段范围                       (不开门事件) </para>
        /// <para>  6--重复验证                               (不开门事件) </para>
        /// <para>  7--体温异常                               (不开门事件) </para>
        /// <para>  8--使用出门按钮开门                       (开门事件)   </para>
        /// <para>  9--免验证开门                             (开门事件)   </para>
        /// <para> 10--黑名单验证           (不开门事件) </para>
        /// <para> 11--系统锁定时验证       (不开门事件) </para>
        /// <para> 12--体温异常，拒绝进入   (不开门事件) </para>
        /// <para> 13--组合验证             (不开门事件) </para>
        /// <para> 14--组合验证失败         (不开门事件) </para>
        /// <para>100--开门超时报警                         (系统事件)   </para>
        /// <para>101--门磁报警                             (系统事件)   </para>
        /// <para>102--恢复出厂设置（清空所有人和记录）     (系统事件)   </para>
        /// <para>103--删除所有人                           (系统事件)   </para>
        /// <para>104--开机                                 (系统事件)   </para>
        /// <para>106--WIFI 已连接                          (系统事件 - 启用WIFI时才记录)</para>
        /// <para>107--WIFI 连接已断开                      (系统事件 - 启用WIFI时才记录)</para>
        /// <para>108--消防报警                             (系统事件)                   </para>
        /// <para>109--关闭开门超时报警 </para>
        /// <para>110--关闭门磁报警     </para>
        /// <para>111--关闭消防报警   </para>
        /// <para>112--黑名单报警     </para>
        /// <para>113--关闭黑名单报警 </para>
        /// <para>114--门磁开         </para>
        /// <para>115--门磁关         </para>
        /// <para>116--删除所有记录         </para>
        /// </summary>
        public int EventType { get; set; }

        /// <summary>
        /// 打卡时间（年-月-日 时：分：秒，例如：”2020-03-06 16:45:20”） 
        /// </summary>
        public DateTime NoteTime { get; set; }

        /// <summary>
        /// 雇员 ID 
        /// </summary>
        public long EmployeeID { get; set; }

        /// <summary>
        /// 人员姓名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 相似度
        /// </summary>
        public float NotePity { get; set; }


        /// <summary>
        /// 打卡是否通过（若设备只在打卡成功后才做记录，可删除此字段） 
        /// </summary>
        public int NotePass { get; set; }

        /// <summary>
        /// 0:刷脸，1:打卡，2：编号+密码
        /// </summary>
        public int NoteWay { get; set; }

        /// <summary>
        /// 人体测量温度 （摄氏度）
        /// </summary>
        public float HumTemp { get; set; }

        /// <summary>
        /// 图片长度
        /// </summary>
        public int Pic_Len { get; set; }

        /// <summary>
        /// 卡号
        /// </summary>
        public ulong CardNo { get; set; }

        /// <summary>
        /// 是否为进入，1表示进入，0表示出门
        /// </summary>
        public int IsEntry { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgURL { get; set; }

        /// <summary>
        /// 二维码
        /// </summary>
        public string? QRCode { get; set; }
    }
}
