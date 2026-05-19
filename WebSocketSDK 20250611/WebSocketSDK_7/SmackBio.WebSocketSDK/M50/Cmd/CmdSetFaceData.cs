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
    public class CmdSetFaceData : CmdBase
    {
        public const string MSG_KEY = "SetFaceData";

        Int64 user_id;
        byte[] face_data;
        UserPrivilege privilege;
        bool duplicationCheck;

        public CmdSetFaceData(Int64 user_id, byte[] face_data, 
                    UserPrivilege privilege, bool duplicationCheck) 
            : base()
        {
            if (face_data == null || face_data.Length != Face.FACE_SIZE)
                throw new ArgumentException("Invalid face template");
            this.user_id = user_id;
            this.face_data = face_data;
            this.privilege = privilege;
            this.duplicationCheck = duplicationCheck;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            AppendTag(ref result, "Privilege", privilege);
            AppendTag(ref result, "DuplicationCheck", duplicationCheck ? 1 : 0);

            if (face_data != null)
            {
                string str_face_data = Convert.ToBase64String(face_data);
                AppendTag(ref result, "FaceData", str_face_data);
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
