using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using System.IO;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetUserCardNo : CmdBase
    {
        public const string MSG_KEY = "GetUserCardNo";

        Int64 user_id;

        public CmdGetUserCardNo(Int64 user_id)
            : base()
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

        // did not override the function "check" to check if response is valid or not.
        // You need to implement this logic for your application.
        // ex: public override CommandExeResult check(BaseMessage response)

        public override Type GetResponseType()
        {
            return typeof(CmdGetUserCardNoResponse);
        }
    }

    public class CmdGetUserCardNoResponse : Response
    {
        uint card_no;
        Int64 user_id;

        public uint CardNo { get { return card_no; } }
        public Int64 UserId { get { return user_id; } }

        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            base.ParseResult(doc);
            if (!ret || result != CommandExeResult.OK)
                return false;

            string str_user_id = ParseTag(doc, "UserID");
            try
            {
                user_id = Convert.ToInt64(str_user_id);
            }
            catch (Exception)
            {
                return false;
            }

            string str_card_no = ParseTag(doc, "CardNo");
            try
            {
                byte[] card = Convert.FromBase64String(str_card_no);
                using (MemoryStream ms = new MemoryStream(card))
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        card_no = reader.ReadUInt32();
                    }
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
