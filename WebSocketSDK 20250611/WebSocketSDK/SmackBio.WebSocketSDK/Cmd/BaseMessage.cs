using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace SmackBio.WebSocketSDK.Cmd
{
    /// <summary>
    /// Base class for all messages.
    /// It supports basic functions and constant variables for XML parsing and building.
    /// </summary>
    public class BaseMessage
    {
        // Message processing delegates
        /// <summary>
        /// Parse XML and extract useful information for itself.
        /// </summary>
        /// <param name="protocol_ver">Protocol version which is contained in packet.</param>
        /// <param name="doc">Message content represented by XML.</param>
        /// <seealso cref="Packet"/>
        public virtual bool Parse(XmlDocument doc) { return false; }

        /// <summary>
        /// PacketStream will use this function to generate packet.
        /// </summary>
        /// <seealso cref="PacketStream">function: "convert2Packet"</seealso>
        public virtual string Build() { return ""; }

        #region Constants for Tag Names
        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string TAG_MODEL = "TerminalType";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string TAG_DEV_ID = "TerminalID";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string TAG_DEV_SN = "DeviceSerialNo";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string TAG_REQUEST = "Request";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string TAG_RESPONSE = "Response";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string TAG_EVENT = "Event";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string TAG_RESULT = "Result";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string STR_OK = "OK";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string STR_FAIL = "Fail";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string STR_FAIL_UNKNOWN_TOKEN = "FailUnknownToken";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string STR_INVALID_PARAM = "Invalid Param";

        /// <summary>
        /// One of constant tag name
        /// </summary>
        public const string STR_DEV_NOT_READY = "Device Not Ready";

        public const string TAG_FW_VER = "FWVersion";
        public const string TAG_CLOUD_ID = "CloudId";
        #endregion

        /// <summary>
        /// Append specific tag with a specific value into msg_xml
        /// </summary>
        /// <param name="msg_xml">Target</param>
        /// <param name="tag">Tag name</param>
        /// <param name="value">InnerText, every value will be interpreted as a string</param>
        public void AppendTag(ref string msg_xml, string tag, object value)
        {
            if (value == null)
                msg_xml += "\r\n\t<" + tag + ">" + "</" + tag + ">";
            else
                msg_xml += "\r\n\t<" + tag + ">" + value.ToString() + "</" + tag + ">";
        }

        /// <summary>
        /// Parse device model in msg_xml
        /// </summary>
        /// <param name="msg_xml">XML message content</param>
        public static string GetDeviceModel(string msg_xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg_xml);
            return GetDeviceModel(doc);
        }

        /// <summary>
        /// Parse device model in doc
        /// </summary>
        /// <param name="doc">XML message content</param>
        public static string GetDeviceModel(XmlDocument doc)
        {
            return ParseTag(doc, TAG_MODEL);
        }

        /// <summary>
        /// Parse device id in msg_xml (which is useful for OCX)
        /// </summary>
        /// <param name="msg_xml">XML message content</param>
        public static Int32 ParseDeviceID(string msg_xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg_xml);
            return ParseDeviceID(doc);
        }

        /// <summary>
        /// Parse device id in doc (which is useful for OCX)
        /// </summary>
        /// <param name="doc">XML message content</param>
        public static Int32 ParseDeviceID(XmlDocument doc)
        {
            return Convert.ToInt32(ParseTag(doc, TAG_DEV_ID));
        }

        /// <summary>
        /// Parse device serial number in msg_xml
        /// </summary>
        /// <param name="msg_xml">XML message content</param>
        public static string ParseDeviceSerialNo(string msg_xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(msg_xml);
            return ParseDeviceSerialNo(doc);
        }

        /// <summary>
        /// Parse device serial number in doc
        /// </summary>
        /// <param name="doc">XML message content</param>
        public static string ParseDeviceSerialNo(XmlDocument doc)
        {
            return ParseTag(doc, TAG_DEV_SN);
        }

        /// <summary>
        /// Parse specific tag value.
        /// </summary>
        /// <param name="doc">XML message content</param>
        /// <param name="tag">Tag name</param>
        public static string ParseTag(XmlDocument doc, string tag)
        {
            XmlNodeList node_list = doc.GetElementsByTagName(tag);
            if (node_list.Count <= 0)
                return null;

            return node_list.Item(0).InnerText;
        }

        /// <summary>
        /// Parse request message key. (tag name = "Request")
        /// </summary>
        /// <param name="doc">XML message content</param>
        public static string ParseRequestKey(XmlDocument doc)
        {
            return ParseTag(doc, TAG_REQUEST);
        }

        /// <summary>
        /// Parse request message key. (tag name = "Response")
        /// </summary>
        /// <param name="doc">XML message content</param>
        public static string ParseResponseKey(XmlDocument doc)
        {
            return ParseTag(doc, TAG_RESPONSE);
        }

        public static bool IsResponseKey(XmlDocument doc, string msg_key)
        {
            string resp = ParseTag(doc, TAG_RESPONSE);
            if (resp != null && resp.Trim() == msg_key)
                return true;
            return false;
        }
        /// <summary>
        /// Parse event message key. (tag name = "Event")
        /// </summary>
        /// <param name="doc">XML message content</param>
        public static string ParseEventKey(XmlDocument doc)
        {
            return ParseTag(doc, TAG_EVENT);
        }
        public static bool IsEventKey(XmlDocument doc, string evt_key)
        {
            string resp = ParseTag(doc, TAG_EVENT);
            if (resp != null && resp.Trim() == evt_key)
                return true;
            return false;
        }

        public static byte ParseFwVersion(XmlDocument doc)
        {
            string fw_ver = ParseTag(doc, TAG_FW_VER);
            try
            {
                return Convert.ToByte(fw_ver);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Message header constants.
        /// </summary>
        public static string MessageHeader
        {
            get { return "<?xml version=\"1.0\"?>\r\n<Message>"; }
        }

        /// <summary>
        /// Append message end.
        /// </summary>
        public static void AppendEndup(ref string msg_xml)
        {
            msg_xml += "\r\n</Message>";
        }

        /// <summary>
        /// Check bool tag value.
        /// </summary>
        public static bool TagIsBooleanTrue(XmlDocument doc, String tag)
        {
            XmlNodeList node_list = doc.GetElementsByTagName(tag);
            if (node_list.Count <= 0)
                return false;

            switch (node_list.Item(0).InnerText)
            {
                case "Yes":
                case "True":
                case "Y":
                case "T":
                    return true;
                default:
                    return false;
            }
        }
    }
}
