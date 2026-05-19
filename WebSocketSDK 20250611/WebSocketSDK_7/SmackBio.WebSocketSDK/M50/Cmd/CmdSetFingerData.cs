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
    public class CmdSetFingerData : CmdBase
    {
        public const string MSG_KEY = "SetFingerData";

        Int64 user_id;
        int finger_no;
        bool duress;
        byte[] fp_data; 
        UserPrivilege privilege;
        bool duplicationCheck;

        public CmdSetFingerData(Int64 user_id, int finger_no, bool duress, byte[]fp_data, 
                    UserPrivilege privilege, bool duplicationCheck) 
            : base()
        {
            if (fp_data == null || fp_data.Length != Fingerprint.FP_SIZE)
                throw new ArgumentException("Invalid finger");
            this.user_id = user_id;
            this.finger_no = finger_no;
            this.duress = duress;
            this.fp_data = fp_data;
            this.privilege = privilege;
            this.duplicationCheck = duplicationCheck;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            AppendTag(ref result, "Privilege", privilege);
            AppendTag(ref result, "FingerNo", finger_no);
            AppendTag(ref result, "DuplicationCheck", duplicationCheck ? 1 : 0);
            AppendTag(ref result, "Duress", duress ? 1 : 0);

            if (fp_data != null)
            {
                string finger_data = Convert.ToBase64String(fp_data);
                AppendTag(ref result, "FingerData", finger_data);
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
