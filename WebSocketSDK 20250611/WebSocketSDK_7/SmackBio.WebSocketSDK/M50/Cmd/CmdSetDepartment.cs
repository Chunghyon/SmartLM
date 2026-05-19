using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;
using System.IO;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdSetDepartment : CmdBase
    {
        public const string MSG_KEY = "SetDepartment";
        int dept_no;
        string name;

        public CmdSetDepartment(int dept_no, string name)
            : base()
        {
            this.dept_no = dept_no;
            this.name = name;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "DeptNo", dept_no);

            if (name != null)
            {
                byte[] name_binary = Encoding.Unicode.GetBytes(name);

                AppendTag(ref result, "Data", Convert.ToBase64String(name_binary));
            }

            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}
