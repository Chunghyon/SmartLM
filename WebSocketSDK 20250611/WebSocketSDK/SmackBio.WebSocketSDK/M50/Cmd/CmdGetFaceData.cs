using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;
using SmackBio.WebSocketSDK;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetFaceData : CmdBase
    {
        public const string MSG_KEY = "GetFaceData";

        Int64 user_id;

        public CmdGetFaceData(Int64 user_id)
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
            return typeof(CmdGetFaceDataResponse);
        }
    }

    public class CmdGetFaceDataResponse : Response
    {
        Int64 user_id;
        Face face;

        public Int64 UserId { get { return user_id; } }
        public Face FaceData { get { return face; } }

        public override bool Parse(XmlDocument doc)
        {
            if (!base.Parse(doc))
                return false;

            face = new Face();
            string str_user_id = ParseTag(doc, "UserID");
            try
            {
                user_id = Convert.ToInt32(str_user_id);
            }
            catch (Exception)
            {
                return false;
            }

            string str_face_data = ParseTag(doc, "FaceData");
            if (str_face_data == null)
                return false;

            try
            {
                face.face_data = Convert.FromBase64String(str_face_data);
            }
            catch (Exception)
            {
                return false;            	
            }
            return true;
        }
    }
}
