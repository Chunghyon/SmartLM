using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmackBio.WebSocketSDK.Cmd
{
    /// <summary>
    /// Abstract base class for event message from device.
    /// </summary>
    public abstract class EvtBase : BaseMessage
    {
        /// <summary>
        /// Device model.
        /// </summary>
        public string device_model;

        /// <summary>
        /// Device serial number.
        /// </summary>
        public string device_sn;

        /// <summary>
        /// Device ID (for OCX, not need for TCP real time communication)
        /// </summary>
        public Int32 device_id;

        /// <summary>
        /// Parse raw packet to message.
        /// </summary>
        /// <param name="protocol_ver">Packet protocol version.</param>
        /// <param name="doc">XML document to parse.</param>
        /// <returns>Succession of parsing.</returns>
        public override bool Parse(XmlDocument doc)
        {
            device_model = GetDeviceModel(doc);
            device_sn = ParseDeviceSerialNo(doc);
            device_id = ParseDeviceID(doc);
            if (device_model == null || device_sn == null)
                return false;

            return true;
        }

        /// <summary>
        /// Generates XML message command header part.
        /// </summary>
        /// <returns>XML string.</returns>
        public string StartBuild()
        {
            string result = MessageHeader;
            AppendTag(ref result, "TerminalType", device_model);
            AppendTag(ref result, "TerminalID", device_id);
            AppendTag(ref result, "DeviceSerialNo", device_sn);
            return result;
        }

        /// <summary>
        /// Abstract function to generate response message.
        /// </summary>
        /// <param name="success">Succession of process</param>
        /// <returns>Response message.</returns>
//         public abstract BaseMessage GenerateResponse(bool success = true);
    }

}
