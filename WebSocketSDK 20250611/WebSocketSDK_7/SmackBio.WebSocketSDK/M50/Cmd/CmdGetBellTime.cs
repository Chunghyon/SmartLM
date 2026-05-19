using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdGetBellTime : CmdBase
    {
        public const string MSG_KEY = "GetBellTime";

        public CmdGetBellTime() : base() { }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetBellTimeResponse);
        }
    }

    public class CmdGetBellTimeResponse : Response
    {
        public BellSetting setting = new BellSetting();
        public override bool Parse(XmlDocument doc)
        {
            string temp = ParseTag(doc, "BellRingTimes");
            try
            {
                setting.RingCount = Convert.ToUInt32(temp);
            }
            catch (Exception) { }

            temp = ParseTag(doc, "BellCount");
            try
            {
                setting.BellCount = Convert.ToUInt32(temp);
            }
            catch (Exception) { }

            try
            {
                string tag;
                if (setting.BellCount == 0)
                    return false;

                setting.bells = new Belling[setting.BellCount];

                for (int i = 0; i < setting.BellCount; ++i)
                {
                    setting.bells[i] = new Belling();
                    tag = "Bell_" + i;
                    temp = ParseTag(doc, tag);
                    if (temp != null)
                    {
                        try
                        {
                            string[] items = temp.Split(new char[]{','});
                            if (items.Length != 4)
                                return false;

                            setting.bells[i].valid = Convert.ToInt32(items[0]) > 0;
                            setting.bells[i].type = (BellType)Convert.ToByte(items[1]);
                            setting.bells[i].hour = Convert.ToByte(items[2]);
                            setting.bells[i].minute = Convert.ToByte(items[3]);
                        } catch (Exception) {
                            return false;
                        }
                    }
                }
            }
            catch (Exception)
            {
                str_result = ParseTag(doc, TAG_RESULT);
            }

            return true;
        }
    }
}
