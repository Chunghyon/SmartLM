using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmackBio.WebSocketSDK.Cmd
{
    /// <summary>
    /// Base command class that can be sent to devices.
    /// </summary>
    /// <seealso cref="BaseMessage"/>
    public abstract class AbstractCommand : BaseMessage
    {
        /// <summary>
        /// Virtual function is used for checking valid response.
        /// </summary>
        /// <param name="response">Response from the device which is usually passed from PacketStream.</param>
        /// <returns>command execution result</returns>
        /// <seealso cref="DeviceConnector"/>
        /// <seealso cref="PacketStream"/>
        public abstract CommandExeResult check(BaseMessage response);
    }

    public enum CommandExeResult
    {
        /// <summary>
        /// Command execution failed.
        /// </summary>
        Fail,

        /// <summary>
        /// Command execution success.
        /// </summary>
        OK,

        /// <summary>
        /// Command is invalid.
        /// </summary>
        InvalidParam,

        /// <summary>
        /// Device is not ready to execute command.
        /// </summary>
        DeviceNotReady,

        /// <summary>
        /// Unknown result of command execution.
        /// </summary>
        Unknown,
    };
}
