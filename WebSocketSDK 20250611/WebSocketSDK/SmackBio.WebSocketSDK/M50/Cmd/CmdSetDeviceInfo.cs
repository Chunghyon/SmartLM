using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    /// <summary>
    /// Supported languages
    /// </summary>
    public enum DeviceLanguage
    {
        English                 = 0,
        Chinese_Simplified      = 1,
        Chinese_Traditional     = 2,
        Turkish                 = 3,
        Korean                  = 4,
        Indonesian              = 5,
        Arabic                  = 6,
    }

    /// <summary>
    /// Door sensor types.
    /// </summary>
    public enum DoorSensorTypes
    {
        /// <summary>
        /// No operation.
        /// </summary>
        DOORSENSOR_TYPE_NONE = 0,

        /// <summary>
        /// Normal open.
        /// </summary>
        DOORSENSOR_TYPE_NORMAL_OPEN = 1,

        /// <summary>
        /// Normal close.
        /// </summary>
        DOORSENSOR_TYPE_NORMAL_CLOSE = 2,
    }

    /// <summary>
    /// Supported baud rate for serial
    /// </summary>
    public enum BaudRate
    {
        _9600,
        _19200,
        _38400,
        _57600,
        _115200,
    }

    public enum EventSendType
    {
        None  = 0,
        TCP   = 1,
        RS485 = 2,
    }

    /// <summary>
    /// Wiegand output types.
    /// </summary>
    public enum WiegandOutputTypes
    {
        /// <summary>
        /// 26bit output.
        /// </summary>
        WIEGAND_26 = 0,

        /// <summary>
        /// 34bit output.
        /// </summary>
        WIEGAND_34 = 1,
    }

    /// <summary>
    /// Lock operation modes.
    /// </summary>
    public enum LockOperationModes
    {
        /// <summary>
        /// Always close.
        /// </summary>
        T_LOCK_UNCOND_CLOSE = 0,

        /// <summary>
        /// Always open.
        /// </summary>
        T_LOCK_UNCOND_OPEN = 1,

        /// <summary>
        /// Auto operation.
        /// </summary>
        T_LOCK_AUTO_RECOVER = 2,
    }

    /// <summary>
    /// Verification modes.
    /// </summary>
    /// 
    public enum VerifyModes
    {
        /// <summary>
        /// Any mode is enabled.
        /// </summary>
        SystemDefault = 0,

        /// <summary>
        /// Any mode is enabled.
        /// </summary>
        Any = 1,

        /// <summary>
        /// Activates "card + finger"
        /// </summary>
        Card_Finger = 3,

        /// <summary>
        /// Activates "card + password"
        /// </summary>
        Card_Password = 7,

        /// <summary>
        /// Activates "finger + password"
        /// </summary>
        Finger_Password = 8,

        /// <summary>
        /// Activates "finger + card + password"
        /// </summary>
        Finger_Card_Password = 9,

        /// <summary>
        /// Activates "card + face"
        /// </summary>
        Card_Face = 11,

        /// <summary>
        /// Activates "face + password"
        /// </summary>
        Face_Password = 12,

        /// <summary>
        /// Activates "face + card + password"
        /// </summary>
        Face_Card_Password = 13,

        /// <summary>
        /// Activates "face + fingerprint"
        /// </summary>
        Face_Finger = 14,
    }

    public class CmdSetDeviceInfo : CmdBase
    {
        public const string MSG_KEY = "SetDeviceInfo";

        DevInfoParamType type;
        UInt32 param_val;

        public CmdSetDeviceInfo(DevInfoParamType type, UInt32 param_val)
            : base()
        {
            this.type = type;
            this.param_val = param_val;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "ParamName", type.ToString());
            AppendTag(ref result, "Value", param_val);
            AppendEndup(ref result);
            return result;
        }

        public override Type GetResponseType()
        {
            return typeof(GeneralResponse);
        }
    }
}
