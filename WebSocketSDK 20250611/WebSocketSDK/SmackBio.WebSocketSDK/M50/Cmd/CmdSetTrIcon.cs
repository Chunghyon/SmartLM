using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;
using SmackBio.WebSocketSDK.DB;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdSetTrIcon : CmdBase
    {
        public const string MSG_KEY = "SetTrIcon";

        int icon_no;
        int icon_status;
        bool is_delete;
        byte[] photo_data;

        public CmdSetTrIcon(int no, int status, bool need_delete, byte[] photo) 
            : base()
        {
            if (!need_delete && (photo == null || photo.Length > UserInfo.MAX_PHOTO_SIZE_64K))
                throw new ArgumentException("Invalid Icon Data");
            this.icon_no = no;
            this.icon_status = status;
            this.is_delete = need_delete;
            this.photo_data = photo;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "IconNo", icon_no);
            AppendTag(ref result, "Status", icon_status);
            if (is_delete)
                AppendTag(ref result, "Delete", "Yes");
            else
                AppendTag(ref result, "Delete", "No");

            if (!is_delete)
            {
                if (photo_data != null)
                {
                    AppendTag(ref result, "IconSize", photo_data.Length);
                    string str_photo_data = Convert.ToBase64String(photo_data);
                    AppendTag(ref result, "IconData", str_photo_data);
                }
                else
                {
                    AppendTag(ref result, "IconSize", 0);
                }
            }

            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdSetTrIconResponse);
        }
    }

    public class CmdSetTrIconResponse : Response
    {
        public string fail_reason;

        public override bool Parse(XmlDocument doc)
        {
            fail_reason = ParseTag(doc, "Reason");
            return true;
        }
    }
}
