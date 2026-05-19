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
    public class CmdGetFingerData : CmdBase
    {
        public const string MSG_KEY = "GetFingerData";

        Int64 user_id;
        int finger_no;
        bool finger_only;

        public CmdGetFingerData(Int64 user_id, int finger_no, bool finger_only) 
            : base()
        {
            this.user_id = user_id;
            this.finger_no = finger_no;
            this.finger_only = finger_only;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            AppendTag(ref result, "FingerNo", finger_no);
            AppendTag(ref result, "FingerOnly", finger_only ? 1 : 0);
            AppendEndup(ref result);

            return result;
        }

        // did not override the function "check" to check if response is valid or not.
        // You need to implement this logic for your application.
        // ex: public override CommandExeResult check(BaseMessage response)

        public override Type GetResponseType()
        {
            return typeof(CmdGetFingerDataResponse);
        }
    }

    public class CmdGetFingerDataResponse : Response
    {
        Int64 user_id;
        int finger_no;
        byte[] finger_data;
        bool duress;
        UserInfo user;

        public byte[] FingerData { get { return finger_data; } }
        public bool Duress { get { return duress; } }
        public Int64 UserId { get { return user_id; } }
        public int FingerNo { get { return finger_no; } }
        public UserInfo userinfo { get { return user; } }

        public override bool Parse(XmlDocument doc)
        {
            if (!base.Parse(doc))
                return false;

            string str_user_id = ParseTag(doc, "UserID");
            try
            {
                user_id = Convert.ToInt64(str_user_id);
            } catch (Exception) { return false; }


            string str_finger_no = ParseTag(doc, "FingerNo");
            try
            {
                finger_no = Convert.ToInt32(str_finger_no);
            }
            catch (Exception)
            {
                return false;
            }

            string str_finger_data = ParseTag(doc, "FingerData");
            if (str_finger_data == null)
            {
                return false;
            }

            finger_data = Convert.FromBase64String(str_finger_data);

            duress = TagIsBooleanTrue(doc, "Duress");

            bool found_user_info = false;
            UserInfo temp_user = new UserInfo();
            switch (ParseTag(doc, "Privilege"))
            {
                case "User":
                    temp_user.privilege = UserPrivilege.NormalUser;
                    found_user_info = true;
                    break;
                case "Manager":
                    temp_user.privilege = UserPrivilege.Manager;
                    found_user_info = true;
                    break;
                case "Administrator":
                    temp_user.privilege = UserPrivilege.Administrator;
                    found_user_info = true;
                    break;
            }

            string temp;
            int[] timesets = new int[5];
            for (int i = 0; i < 5; ++i)
            {
                temp = ParseTag(doc, "TimeSet" + (i + 1));
                try
                {
                    timesets[i] = Convert.ToInt32(temp);
                    if (temp != null)
                        found_user_info = true;
                }
                catch (Exception)
                {
                    timesets[i] = -1;
                }
            }
            temp_user.timeset1 = timesets[0];
            temp_user.timeset2 = timesets[1];
            temp_user.timeset3 = timesets[2];
            temp_user.timeset4 = timesets[3];
            temp_user.timeset5 = timesets[4];

            temp_user.period_use = false;
            temp_user.period_start = DateTime.Now;
            temp_user.period_end = DateTime.Now;

            if (ParseTag(doc, "UserPeriod_Used") != null)   // It is the new version firmware in this device.
            {
                temp_user.period_use = TagIsBooleanTrue(doc, "UserPeriod_Used");
                if (temp_user.period_use)
                {
                    int yy, mm, dd;

                    temp = ParseTag(doc, "UserPeriod_Start");
                    int start_period = Convert.ToInt32(temp);
                    yy = start_period >> 16; mm = (start_period & 0xFF00) >> 8; dd = start_period & 0xFF;
                    temp_user.period_start = new DateTime(yy + 2000, mm, dd);

                    temp = ParseTag(doc, "UserPeriod_End");
                    int end_period = Convert.ToInt32(temp);
                    yy = end_period >> 16; mm = (end_period & 0xFF00) >> 8; dd = end_period & 0xFF;
                    temp_user.period_end = new DateTime(yy + 2000, mm, dd);
                }
            }

            if (found_user_info)
                user = temp_user;

            return true;
        }
    }
}
