using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SmackBio.WebSocketSDK
{
    /// <summary>
    /// Accepts or rejects login requests from M50 device.
    /// </summary>
    /// <remarks>
    /// Web applications can implement this interface and register the class name in &lt;appSettings&gt; section
    /// of Web.config to let the WebSocketSDK runtime to instantiate and invoke when needed.
    /// </remarks>
    /// <example>
    ///   &lt;appSettings&gt;
    ///     ...
    ///     &lt;add key="smackbio.websocketsdk.deviceLoginManager" value="Sample.DeviceLoginManager, Sample"/&gt;
    ///     ...
    ///   &lt;/appSettings&gt;
    /// </example>
    public interface IDeviceLoginManager
    {
        bool Verify(string deviceId, string token);

        string GetRegisterInfo(string model, string deviceId);

        void OnDeviceEvent(XmlDocument xml);

    }
}
