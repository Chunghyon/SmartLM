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
    public class CmdGetUserData : CmdBase
    {
        public const string MSG_KEY = "GetUserData";
        public Int64 user_id { get; set; }

        public CmdGetUserData(Int64 user_id) 
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

        public override CommandExeResult check(BaseMessage response)
        {
            CommandExeResult result = base.check(response);
            if (result != CommandExeResult.OK)
                return result;

            CmdGetUserDataResponse res = (CmdGetUserDataResponse)response;
            if (this.user_id != res.user.user_id)
                return CommandExeResult.Fail;

            return CommandExeResult.OK;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetUserDataResponse);
        }
    }

    public class CmdGetUserDataResponse : GeneralResponse
    {
        public UserInfo user = new UserInfo();
        
        public override bool Parse(XmlDocument doc)
        {
            bool ret = base.Parse(doc);
            base.ParseResult(doc);
            if (!ret || result != CommandExeResult.OK)
                return false;

            user.user_id = Convert.ToInt64(ParseTag(doc, "UserID"));
            string base64_name = ParseTag(doc, "Name");
            if (base64_name != null)
            {
                try
                {
                    byte[] name_binary = Convert.FromBase64String(base64_name);
                    user.name = Encoding.Unicode.GetString(name_binary, 0, name_binary.Length - 2); // name from device include \0 in the last
                }
                catch (Exception)
                {
                	
                }
            }
            switch (ParseTag(doc, "Privilege"))
            {
                case "Administrator":
                    user.privilege = UserPrivilege.Administrator;
                    break;
                case "Manager":
                    user.privilege = UserPrivilege.Manager;
                    break;
                case "User":
                default:
                    user.privilege = UserPrivilege.NormalUser;
                    break; 
            }

            user.enabled = TagIsBooleanTrue(doc, "Enabled");

            string temp;
            temp = ParseTag(doc, "Depart");
            try
            {
                user.depart = Convert.ToByte(temp);
            }
            catch (Exception){}

            int[] timesets = new int[5];
            for (int i = 0; i < 5; ++i)
            {
                temp = ParseTag(doc, "TimeSet" + (i + 1));
                try
                {
                    timesets[i] = Convert.ToInt32(temp);
                }
                catch (Exception)
                {
                    timesets[i] = -1;
                }
            }
            user.timeset1 = timesets[0];
            user.timeset2 = timesets[1];
            user.timeset3 = timesets[2];
            user.timeset4 = timesets[3];
            user.timeset5 = timesets[4];

            user.period_use = false;
            user.period_start = DateTime.Now;
            user.period_end = DateTime.Now;

            if (ParseTag(doc, "UserPeriod_Used") != null)   // It is the new version firmware in this device.
            {
                user.period_use = TagIsBooleanTrue(doc, "UserPeriod_Used");
                if (user.period_use)
                {
                    int yy, mm, dd;

                    temp = ParseTag(doc, "UserPeriod_Start");
                    int start_period = Convert.ToInt32(temp);
                    yy = start_period >> 16; mm = (start_period & 0xFF00) >> 8; dd = start_period & 0xFF;
                    user.period_start = new DateTime(yy + 2000, mm, dd);

                    temp = ParseTag(doc, "UserPeriod_End");
                    int end_period = Convert.ToInt32(temp);
                    yy = end_period >> 16; mm = (end_period & 0xFF00) >> 8; dd = end_period & 0xFF;
                    user.period_end = new DateTime(yy + 2000, mm, dd);
                }
            }

            temp = ParseTag(doc, "Card");
            if (temp == null)
                user.card = 0;
            else
            {
                byte[] card = Convert.FromBase64String(temp);
                using (MemoryStream ms = new MemoryStream(card))
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        user.card = reader.ReadUInt32();
                    }
                }
            }

            temp = ParseTag(doc, "QR");
            if (temp == null)
                user.qr = 0;
            else
            {
                byte[] qr = Convert.FromBase64String(temp);
                using (MemoryStream ms = new MemoryStream(qr))
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        user.qr = reader.ReadUInt32();
                    }
                }
            }

            user.password = ParseTag(doc, "PWD");
            user.face.enrolled = TagIsBooleanTrue(doc, "FaceEnrolled");
            if (user.face.enrolled)
            {
                string str_face_data = ParseTag(doc, "FaceData");
                try
                {
                    user.face.enrolled = true;
                    user.face.face_data = Convert.FromBase64String(str_face_data);
                }
                catch (Exception)
                {
                	
                }
            }

            temp = ParseTag(doc, "Fingers");
            try
            {
                UInt32 fingers_flag = Convert.ToUInt32(temp);
                for (int i = 0; i < M50Device.MAX_FINGERS_PER_USER; ++i)
                {

                    if (((fingers_flag >> (i * 2)) & 1) != 0)
                    {
                        user.fingerprints[i].enrolled = true;
                        if (((fingers_flag >> (i * 2 + 1)) & 1) != 0)
                        {
                            user.fingerprints[i].duress = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            	
            }
            
            return true;
        }
    }
}
