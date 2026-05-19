using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public class CmdSetDeviceInfoExt : CmdBase
    {
        public const string MSG_KEY = "SetDeviceInfoExt";

        DevInfoExtParamType type;
        string Value1;
        string Value2;
        string Value3;
        string Value4;
        string Value5;

        public CmdSetDeviceInfoExt(DevInfoExtParamType type, string Value1, string Value2 = null, string Value3 = null, string Value4 = null, string Value5 = null)
            : base()
        {
            this.type = type;
            this.Value1 = Value1;
            this.Value2 = Value2;
            this.Value3 = Value3;
            this.Value4 = Value4;
            this.Value5 = Value5;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "ParamName", type.ToString());
            AppendTag(ref result, "Value1", Value1);
            if (Value2 != null)
                AppendTag(ref result, "Value2", Value2);
            if (Value3 != null)
                AppendTag(ref result, "Value3", Value3);
            if (Value4 != null)
                AppendTag(ref result, "Value4", Value4);
            if (Value5 != null)
                AppendTag(ref result, "Value5", Value5);

            AppendEndup(ref result);
            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}
