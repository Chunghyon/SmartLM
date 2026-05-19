using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using System.IO;
using SmackBio.WebSocketSDK;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public enum SetUserMode
    {
        Set,
        Delete
    }
    public class CmdSetUserData : CmdBase
    {
        public const string MSG_KEY = "SetUserData";
        SetUserMode mode;
        
        public UserInfo user;
        private const int MAX_NAME_LEN = 24;

        public CmdSetUserData(UserInfo user, SetUserMode mode)
            : base()
        {
            this.user = user;
            this.mode = mode;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user.user_id);
            string mode_str = null;
            switch (mode)
            {
                case SetUserMode.Set:
                default:
                    mode_str = "Set";
                    break;
                case SetUserMode.Delete:
                    mode_str = "Delete";
                    break;
            }
            AppendTag(ref result, "Type", mode_str);

            if (mode == SetUserMode.Delete)
            {
                AppendEndup(ref result);
                return result;
            }

            if (user.name != null)
            {
                if (user.name.Length > MAX_NAME_LEN)
                    user.name = user.name.Substring(0, 12);
                    byte[] name_binary_capsule = new byte[(MAX_NAME_LEN + 1) * 2];
                    byte[] name_data = Encoding.Unicode.GetBytes(user.name);
                    Array.Copy(name_data, name_binary_capsule, name_data.Length);
                    AppendTag(ref result, "Name", Convert.ToBase64String(name_binary_capsule));
            }

            AppendTag(ref result, "Depart", user.depart);

            switch (user.privilege)
            {
                case UserPrivilege.NormalUser:
                    AppendTag(ref result, "Privilege", "User");
                    break;
                case UserPrivilege.Manager:
                    AppendTag(ref result, "Privilege", "Manager");
                    break;
                case UserPrivilege.Administrator:
                    AppendTag(ref result, "Privilege", "Administrator");
                    break;
                default:
                    break;
            }

            AppendTag(ref result, "Enabled", user.enabled ? "Yes" : "No");

            if (user.timeset1 >= 0 && user.timeset1 <= 49)
                AppendTag(ref result, "TimeSet1", user.timeset1);
            if (user.timeset2 >= 0 && user.timeset2 <= 49)
                AppendTag(ref result, "TimeSet2", user.timeset2);
            if (user.timeset3 >= 0 && user.timeset3 <= 49)
                AppendTag(ref result, "TimeSet3", user.timeset3);
            if (user.timeset4 >= 0 && user.timeset4 <= 49)
                AppendTag(ref result, "TimeSet4", user.timeset4);
            if (user.timeset5 >= 0 && user.timeset5 <= 49)
                AppendTag(ref result, "TimeSet5", user.timeset5);

            AppendTag(ref result, "UserPeriod_Used", user.period_use ? "Yes" : "No");
            AppendTag(ref result, "UserPeriod_Start", ((user.period_start.Year - 2000) << 16) + (user.period_start.Month << 8) + user.period_start.Day);
            AppendTag(ref result, "UserPeriod_End", ((user.period_end.Year - 2000) << 16) + (user.period_end.Month << 8) + user.period_end.Day);

            if (user.card >= 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(ms))
                    {
                        writer.Write((uint)user.card);
                        AppendTag(ref result, "Card", Convert.ToBase64String(ms.ToArray()));
                    }
                }
            }

            if (user.qr >= 0)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(ms))
                    {
                        writer.Write((uint)user.qr);
                        AppendTag(ref result, "QR", Convert.ToBase64String(ms.ToArray()));
                    }
                }
            }

            if (user.password != null)
                AppendTag(ref result, "PWD", user.password);

            if (user.face != null && user.face.face_data != null)
                AppendTag(ref result, "FaceData", Convert.ToBase64String(user.face.face_data));

            AppendEndup(ref result);

            return result;
        }

        public override CommandExeResult check(BaseMessage response)
        {
            CommandExeResult result = base.check(response);
            if (result != CommandExeResult.OK)
                return result;

            CmdSetUserDataResponse res = (CmdSetUserDataResponse)response;
            if (this.user.user_id != res.user_id)
                return CommandExeResult.Unknown;

            return CommandExeResult.OK;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdSetUserDataResponse);
        }
    }

    public class CmdSetUserDataResponse : GeneralResponse
    {
        public Int64 user_id;
        public SetUserMode mode;
        public string error_reason;

        public override bool Parse(XmlDocument doc)
        {
            error_reason = ParseTag(doc, "Error");

            bool ret = base.Parse(doc);
            base.ParseResult(doc);
            if (!ret || result != CommandExeResult.OK)
                return false;

            user_id = Convert.ToUInt32(ParseTag(doc, "UserID"));
            switch (ParseTag(doc, "Type"))
            {
                case "Set":
                    mode = SetUserMode.Set;
                    break;
                case "Delete":
                    mode = SetUserMode.Delete;
                    break;
            }

            return ret;
        }
    }
}
