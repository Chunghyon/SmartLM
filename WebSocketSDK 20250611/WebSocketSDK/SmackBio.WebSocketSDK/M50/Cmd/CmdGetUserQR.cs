using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using System.IO;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetUserQR : CmdBase
    {
        public const string MSG_KEY = "GetUserQR";

        Int64 user_id;

        public CmdGetUserQR(Int64 user_id)
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
            return typeof(CmdGetUserQRResponse);
        }
    }

    public class CmdGetUserQRResponse : Response
    {
        uint qr;
        Int64 user_id;

        public uint QR { get { return qr; } }
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

            string str_qr = ParseTag(doc, "QR");
            try
            {
                byte[] qr_array = Convert.FromBase64String(str_qr);
                using (MemoryStream ms = new MemoryStream(qr_array))
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        qr = reader.ReadUInt32();
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
