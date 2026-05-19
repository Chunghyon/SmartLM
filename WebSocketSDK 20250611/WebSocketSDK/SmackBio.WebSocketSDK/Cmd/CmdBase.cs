using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmackBio.WebSocketSDK.Cmd
{
    public abstract class CmdBase : AbstractCommand
    {
        public UInt32 db_cmd_id;

        public CmdBase()
        {
        }

        public override bool Parse(XmlDocument doc)
        {
            return true;
        }

        public string StartBuild()
        {
            string result = MessageHeader;
            return result;
        }

        public override CommandExeResult check(BaseMessage response)
        {
            if (response.GetType() != GetResponseType())
                return CommandExeResult.Unknown;

            if (response is GeneralResponse)
                return ((GeneralResponse)response).result;
            else
                return CommandExeResult.OK;
        }
        public abstract Type GetResponseType();
    }
    /*
    abstract class JobBase : AbstractJobCommand
    {
        public UInt32 db_cmd_id;

        public string device_model;
        public string device_sn;
        public Int32 device_id;
        UInt32 protocol_ver;

        public JobBase(string model, string serial_no/ *, Int32 device_id * /) // ignoring device_id
        {
            this.device_model = model;
            this.device_sn = serial_no;
        }

        public override bool Parse(UInt32 protocol_ver, XmlDocument doc)
        {
            device_model = GetDeviceModel(doc);
            device_sn = ParseDeviceSerialNo(doc);
            device_id = ParseDeviceID(doc);
            if (device_model == null || device_sn == null)
                return false;

            this.protocol_ver = protocol_ver;

            return true;
        }

        public string StartBuild()
        {
            string result = MessageHeader;
            AppendTag(ref result, "TerminalType", device_model);
            AppendTag(ref result, "TerminalID", device_id);
            AppendTag(ref result, "DeviceSerialNo", device_sn);
            return result;
        }

        public override CommandExeResult check(BaseMessage response)
        {
            if (response.GetType() != GetResponseType())
                return CommandExeResult.Unknown;

            if (response is GeneralResponse)
                return ((GeneralResponse)response).result;
            else
                return CommandExeResult.OK;
        }

        public abstract Type GetResponseType();
    }
*/
    public class Response : CmdBase
    {
        public Response() : base() { }
        public override Type GetResponseType() { return null; }

        public CommandExeResult result;
        public string str_result;

        public CommandExeResult ParseResult(XmlDocument doc)
        {
            str_result = ParseTag(doc, TAG_RESULT);
            switch (str_result)
            {
                case STR_OK:
                    result = CommandExeResult.OK;
                    break;
                case STR_FAIL:
                    result = CommandExeResult.Fail;
                    break;
                case STR_INVALID_PARAM:
                    result = CommandExeResult.InvalidParam;
                    break;
                case STR_DEV_NOT_READY:
                    result = CommandExeResult.DeviceNotReady;
                    break;
                default:
                    result = CommandExeResult.Unknown;
                    break;
            }

            return result;
        }
    }
    
    public class GeneralResponse : Response
    {
        
    }
}
