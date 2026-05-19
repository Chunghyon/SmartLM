package access;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.*;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Data.Door8800WatchTransaction;
import Door.Access.Packet.PacketDecompileAllocator;
import Face.System.SendConnectTestResponse;

/**
 * Global monitoring
 */
public class MyConnectorEvent extends ConnectorEvent {

    /**
     * Monitoring events (occurring when devices push records or send heartbeats)
     *
     * @param detail
     * @param inData
     */
    @Override
    public void WatchEvent(ConnectorDetail detail, INData inData) {
        if (inData instanceof Door8800WatchTransaction) {
            Door8800WatchTransaction watchEvent = (Door8800WatchTransaction) inData;
            CommandAllocator.addDevice(detail, watchEvent.SN);
            if (watchEvent.CmdIndex == 0xA0) { //Perform callback response when receiving device test connection
                Device device = CommandAllocator.getDevice(watchEvent.SN); //Obtain device information based on SN
                CommandDetail cmdDtl = CommandAllocator.getCommandDetail(device);//Obtain command connection information based on the device
                SendConnectTestResponse cmd = new SendConnectTestResponse(new CommandParameter(cmdDtl)); //Create connection response command of testing
                CommandAllocator.addCommand(cmd);//Add to execution queue
            }
        }

    }

    /**
     * Connection error
     *
     * @param cmd
     * @param isStop
     */
    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {
        System.out.println("The command has stopped:" + cmd);
    }

    /**
     * Communication password error
     *
     * @param cmd
     */
    @Override
    public void PasswordErrorEvent(INCommand cmd) {
        System.out.println("Device communication password error");
    }

    /**
     * Connection error
     *
     * @param detail
     */
    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {
        System.out.println("Connection error:" + detail.toString());
    }

    /**
     * Send info when the device is online
     *
     * @param connectorDetail
     */
    @Override
    public void ClientOffline(ConnectorDetail connectorDetail) {
        CommandAllocator.removeConnectorDetail(connectorDetail);
    }

    /**
     * Occurrence when the device is offline (offline triggering has a certain delay and cannot be used as an offline standard)
     *
     * @param connectorDetail
     */
    @Override
    public void ClientOnline(ConnectorDetail connectorDetail) {

        CommandAllocator.addConnectorDetail(connectorDetail);
        INConnector conn = CommandAllocator.getConnector(connectorDetail); //Obtain the connection channel of the device
        /**Open the connection channel for long connections*/
        conn.OpenForciblyConnect();
        /**Add a data parser for data monitoring*/
        conn.AddWatchDecompile(connectorDetail, PacketDecompileAllocator.GetDecompile(E_ControllerType.Face_Fingerprint));
    }
}
