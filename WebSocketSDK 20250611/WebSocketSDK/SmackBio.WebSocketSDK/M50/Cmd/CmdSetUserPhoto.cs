using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdSetUserPhoto : CmdBase
    {
        public const string MSG_KEY = "SetUserPhoto";

        Int64 user_id;
        byte[] photo_data;

        public CmdSetUserPhoto(Int64 user_id, byte[] photo) 
            : base()
        {
            if (photo == null || photo.Length > UserInfo.MAX_PHOTO_SIZE_32K)
                throw new ArgumentException("Invalid Photo Data");
            this.user_id = user_id;
            this.photo_data = photo;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            if (photo_data != null)
            {
                AppendTag(ref result, "PhotoSize", photo_data.Length);
                string str_photo_data = Convert.ToBase64String(photo_data);
                AppendTag(ref result, "PhotoData", str_photo_data);
            }
            else
            {
                AppendTag(ref result, "PhotoSize", 0);
            }

            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdSetUserPhotoResponse);
        }
    }

    public class CmdSetUserPhotoResponse : Response
    {
        public string fail_reason;

        public override bool Parse(XmlDocument doc)
        {
            fail_reason = ParseTag(doc, "Reason");
            return true;
        }
    }
}
