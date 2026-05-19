using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public enum DevInfoParamType
    {
        ManagersNumber,
        MachineID,
        Language,
        LockReleaseTime,    // in seconds
        SLogWarning,
        GLogWarning,
        ReverifyTime,       // in minutes
        Baudrate,
        IdentifyMode,
        LockMode,
        DoorSensorType,
        DoorOpenTimeout,    // in seconds
        AutoSleepTime,      // in minutes
        EventSendType,
		WiegandFormat,
		CommPassword,
        UseProxyInput,
        ProxyDlgTimeout,

        SoundVolume,
        ShowRealtimeCamera,
        UseFailLog,

        FaceEngineThreshold,
        FaceEngineUseAntispoofing,

        NeedWearingMask,
        SuggestWearingMask,

        UseMeasureTemperature,
        UseVisitorMode,
        ShowRealtimeTemperature,
        AbnormalTempDisableDoorOpen,
        MeasuringDurationType,
        MeasuringDistanceType,
        TemperatureUnit,
        AbnormalTempThreshold_Celsius,
        AbnormalTempThreshold_Fahrenheit,
	}
	public class CmdGetDeviceInfo : CmdBase
    {
        public const string MSG_KEY = "GetDeviceInfo";

        DevInfoParamType type;

        public CmdGetDeviceInfo(DevInfoParamType type)
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
            return typeof(CmdGetDeviceInfoResponse);
        }
    }

    public class CmdGetDeviceInfoResponse : Response
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
