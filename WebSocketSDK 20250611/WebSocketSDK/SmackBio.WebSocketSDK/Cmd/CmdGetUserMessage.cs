using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdGetUserMessage : CmdBase
    {
        public const string MSG_KEY = "GetUserMessage";

        Int64 user_id;

        public CmdGetUserMessage(Int64 user_id) : base()
        {
            this.user_id = user_id;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetUserMessageResponse);
        }
    }

    public class CmdGetUserMessageResponse : Response 
    {
        Int64 user_id;
        public string message;

        public CmdGetUserMessageResponse() { }

        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            base.ParseResult(doc);
            if (!ret || result != CommandExeResult.OK)
                return false;

            try
            {
                string str_user_id = ParseTag(doc, "UserID");
                user_id = Convert.ToInt32(str_user_id);

                var base64_str = ParseTag(doc, "UserMessage");
                if (base64_str != null)
                {
                    byte[] msg_binary = Convert.FromBase64String(base64_str);
                    int index = 0;
                    for (int i = 0; i < msg_binary.Length - 1; i += 2)
                    {
                        if (msg_binary[i] == 0 && msg_binary[i + 1] == 0)
                        {
                            index = i;
                            break;
                        }
                    }

                    message = Encoding.Unicode.GetString(msg_binary, 0, index);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
