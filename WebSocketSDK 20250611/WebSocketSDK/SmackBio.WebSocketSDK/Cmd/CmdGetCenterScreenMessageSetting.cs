using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdGetCenterScreenMessageSetting : CmdBase
    {
        public const string MSG_KEY = "GetCenterScreenMessage";

        public CmdGetCenterScreenMessageSetting() : base() { }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetCenterScreenMessageSettingResponse);
        }
    }

    public class CmdGetCenterScreenMessageSettingResponse : Response 
    {
        public CenterScreenMessageSetting setting = new CenterScreenMessageSetting();

        public CmdGetCenterScreenMessageSettingResponse() { }

        public override bool Parse(XmlDocument doc)
        {
            var center_screen_message_str = ParseTag(doc, "center_screen_message");
            if (center_screen_message_str != null)
            {
                try
                {
                    byte[] msg_binary = Convert.FromBase64String(center_screen_message_str);
                    int index = 0;
                    for (int i = 0; i < msg_binary.Length - 1; i += 2)
                    {
                        if (msg_binary[i] == 0 && msg_binary[i + 1] == 0)
                        {
                            index = i;
                            break;
                        }
                    }

                    setting.center_screen_message = Encoding.Unicode.GetString(msg_binary, 0, index);
                }
                catch (Exception)
                {
                }
            }

            var center_screen_message_color_str = ParseTag(doc, "center_screen_message_color");
            if (center_screen_message_color_str != null)
                setting.center_screen_message_color = Convert.ToUInt32(center_screen_message_color_str, 16) & 0xFFFFF;

            var center_screen_message_border_color_str = ParseTag(doc, "center_screen_message_border_color");
            if (center_screen_message_border_color_str != null)
                setting.center_screen_message_border_color = Convert.ToUInt32(center_screen_message_border_color_str, 16) & 0xFFFFF;

            var verify_disable_str = ParseTag(doc, "verify_disable");
            if (verify_disable_str != null)
                setting.verify_disable = Convert.ToInt32(verify_disable_str);

            return true;
        }
    }
}
