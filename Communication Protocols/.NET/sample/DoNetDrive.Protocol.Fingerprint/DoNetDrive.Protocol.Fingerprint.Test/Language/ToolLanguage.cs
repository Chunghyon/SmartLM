using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DoNetDrive.Protocol.Fingerprint.Test
{
    public class ToolLanguage
    {
        private static Dictionary<string, Dictionary<string, string>> UILanguage;

        public static void LoadLanguage(string sFile)
        {
            UILanguage = new Dictionary<string, Dictionary<string, string>>();
            XmlDocument xml = new XmlDocument();
            xml.Load(sFile);
            var xRoot = xml.DocumentElement;
            foreach (XmlNode xElement in xRoot.ChildNodes)
            {
                var lngDic = new Dictionary<string, string>();
                UILanguage.Add(xElement.Name, lngDic);
                foreach (XmlNode xNode in xElement.ChildNodes)
                {
                    lngDic.Add(xNode.Name, xNode.Attributes["value"].Value);
                }

            }
        }

        public static string GetLanguage(string sElementKey, string sNodeKey)
        {
            if (UILanguage.ContainsKey(sElementKey))
            {
                var dic = UILanguage[sElementKey];
                if (dic.ContainsKey(sNodeKey))
                {
                    return dic[sNodeKey];
                }
            }

            return "NULL";
        }

        /// <summary>
        /// 获得数值代表的星期
        /// </summary>
        /// <param name="index">数值（0-6，0代表星期一...6代表星期日）</param>
        /// <returns></returns>
        public static string GetWeekStr(int index)
        {
            string weekStr = string.Empty;
            if (index == 0)
            {
                return "星期一";
            }
            else if (index == 1)
            {
                return "星期二";
            }
            else if (index == 2)
            {
                return "星期三";
            }
            else if (index == 3)
            {
                return "星期四";
            }
            else if (index == 4)
            {
                return "星期五";
            }
            else if (index == 5)
            {
                return "星期六";
            }
            else if (index == 6)
            {
                return "星期日";
            }
            return weekStr;
        }

    }
}
