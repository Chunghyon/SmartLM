using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;
using System.Xml;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetProxyDept : CmdBase
    {
        public const string MSG_KEY = "GetProxyDept";
        int dept_no;

        public CmdGetProxyDept(int dept_no)
            : base()
        {
            this.dept_no = dept_no;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "ProxyNo", dept_no);
            AppendEndup(ref result);
            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetProxyDeptResponse);
        }
    }

    public class CmdGetProxyDeptResponse : Response
    {
        string name;
        public string Name { get { return name; } }

        public override bool Parse(XmlDocument doc)
        {
            string base64_name = ParseTag(doc, "Name");
            if (base64_name != null)
            {
                try
                {
                    byte[] name_binary = Convert.FromBase64String(base64_name);
                    int index = 0;
                    for (int i = 0; i < name_binary.Length - 1; i += 2)
                    {
                        if (name_binary[i] == 0 && name_binary[i + 1] == 0)
                        {
                            index = i;
                            break;
                        }
                    }

                    name = Encoding.Unicode.GetString(name_binary, 0, index);
                }
                catch (Exception)
                {
                }
            }
            return true;
        }
    }
}
