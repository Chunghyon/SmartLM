using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.Cmd
{
    public class CmdSetCenterScreenMessageSetting : CmdBase
    {
        const int CENTER_SCREEN_MSG_LEN = 100;

        public const string MSG_KEY = "SetCenterScreenMessage";
        CenterScreenMessageSetting setting;
        
        public CmdSetCenterScreenMessageSetting(CenterScreenMessageSetting setting) 
            : base()
        {
            this.setting = setting;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);

            if (setting.center_screen_message.Length > CENTER_SCREEN_MSG_LEN)
                setting.center_screen_message = setting.center_screen_message.Substring(0, CENTER_SCREEN_MSG_LEN);
            byte[] name_binary_capsule = new byte[CENTER_SCREEN_MSG_LEN * 2];
            byte[] name_data = Encoding.Unicode.GetBytes(setting.center_screen_message);
            Array.Copy(name_data, name_binary_capsule, name_data.Length);
            AppendTag(ref result, "center_screen_message", Convert.ToBase64String(name_binary_capsule));

            AppendTag(ref result, "center_screen_message_color", "FF" + setting.center_screen_message_color.ToString("X6"));
            AppendTag(ref result, "center_screen_message_border_color", "FF" + setting.center_screen_message_border_color.ToString("X6"));
            AppendTag(ref result, "verify_disable", setting.verify_disable);

            AppendEndup(ref result);

            return result;
        }
        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
 
    }
}
