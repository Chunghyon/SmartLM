using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    /// <summary>
    /// Door sensor status.
    /// </summary>
    public enum DoorSensorStatus
    {
        /// <summary>
        /// Door is closed.
        /// </summary>
        DOOR_IS_CLOSED = 0,

        /// <summary>
        /// Door is opened.
        /// </summary>
        DOOR_IS_OPENED = 1,
    }

    public class AlarmConstants
    {
        /// <summary>
        /// Alarm status : Duress verification is detected.
        /// </summary>
        public const uint ALARM_DURESS = (1u << 0);

        /// <summary>
        /// Alarm status : Case is opened.
        /// </summary>
        public const uint ALARM_TAMPER = (1u << 1);

        /// <summary>
        /// Alarm status : Door is opened illegally.
        /// </summary>
        public const uint ALARM_ILGOPEN = (1u << 2);

        /// <summary>
        /// Alarm status : Door is not closed.
        /// </summary>
        public const uint ALARM_NOCLOSE = (1u << 3);

        /// <summary>
        /// Alarm status : Log Overflowed.
        /// </summary>
        public const uint ALARM_LOGOVERFLOW = (1u << 4);
    }

    public enum DevStatusParamType
    {
        ManagerCount,
		UserCount,
		FaceCount,
        FpCount,
		CardCount,
        PwdCount,
        DoorStatus,
		AlarmStatus,
    }

    public class CmdGetDeviceStatus : CmdBase
    {
        public const string MSG_KEY = "GetDeviceStatus";
        
        DevStatusParamType type;

        public CmdGetDeviceStatus(DevStatusParamType type)
            : base()
        {
            this.type = type;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "ParamName", type.ToString());
            AppendEndup(ref result);

            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(CmdGetDeviceStatusResponse);
        }
    }

    public class CmdGetDeviceStatusResponse : Response
    {
        public UInt32 param_val;

        public override bool Parse(XmlDocument doc)
        {
            string str_param_val = ParseTag(doc, "Value");
            if (str_param_val == null)
                str_result = ParseTag(doc, TAG_RESULT);
            else
                param_val = Convert.ToUInt32(str_param_val);

            return true;
        }
    }
}
