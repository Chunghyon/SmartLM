using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdSetUserMessage : CmdBase
    {
        const int UserMessageLength = 100;

        public const string MSG_KEY = "SetUserMessage";
        Int64 user_id;
        string message;

        public CmdSetUserMessage(Int64 user_id, string user_message)
            : base()
        {
            this.user_id = user_id;
            this.message = user_message;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);

            if (message.Length > UserMessageLength)
                message = message.Substring(0, UserMessageLength);
            byte[] name_binary_capsule = new byte[UserMessageLength * 2];
            byte[] name_data = Encoding.Unicode.GetBytes(message);
            Array.Copy(name_data, name_binary_capsule, name_data.Length);
            AppendTag(ref result, "UserMessage", Convert.ToBase64String(name_binary_capsule));

            AppendEndup(ref result);

            return result;
        }
        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
 
    }
}
