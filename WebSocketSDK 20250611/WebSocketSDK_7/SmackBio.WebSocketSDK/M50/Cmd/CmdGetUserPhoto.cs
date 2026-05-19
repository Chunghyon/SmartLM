using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetUserPhoto : CmdBase
    {
        public const string MSG_KEY = "GetUserPhoto";

        Int64 user_id;

        public CmdGetUserPhoto(Int64 user_id)
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
            return typeof(CmdGetUserPhotoResponse);
        }
    }

    public class CmdGetUserPhotoResponse : Response
    {
        Int64 user_id;
        byte[] photo_data;

        public Int64 UserId { get { return user_id; } }
        public byte[] PhotoData { get { return photo_data; } }

        public override bool Parse(XmlDocument doc)
        {
            string str_user_id = ParseTag(doc, "UserID");
            try
            {
                user_id = Convert.ToInt32(str_user_id);
            }
            catch (Exception)
            {
                return false;
            }

			int nPhotoSize = DB.UserInfo.MAX_PHOTO_SIZE_8K;
			try
			{
				nPhotoSize = Convert.ToInt32(ParseTag(doc, "PhotoSize"));
			}
			catch
			{
			}

			string str_photo_data = ParseTag(doc, "PhotoData");
            if (str_photo_data == null)
                return false;
            try
            {
                photo_data = Convert.FromBase64String(str_photo_data);
            }
            catch (Exception)
            {
                return false;            	
            }
            return true;
        }
    }
}
