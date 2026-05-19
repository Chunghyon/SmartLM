package access;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Connector.INConnector;
import Door.Access.Connector.TCPClient.TCPClientDetail;
import Door.Access.Connector.UDP.UDPDetail;
import Door.Access.Door8800.Door8800Identity;

public class CommandAllocator {
    /**
     * Connect the alloter
     */
    public static ConnectorAllocator allocator = ConnectorAllocator.GetAllocator();

    /**
     * Add the command to be executed
     *
     * @param cmd
     */
    public static void addCommand(INCommand cmd) {
        allocator.AddCommand(cmd);
    }

    /**
     * Obtain connection information of channel
     *
     * @param detail
     * @return
     */
    public static INConnector getConnector(ConnectorDetail detail) {
        return allocator.GetConnector(detail);
    }


    /**
     * Get device communication details
     *
     * @return Communication Details
     */
    public static CommandDetail getUDPCommandDetail() {
        Door8800Identity idt = new Door8800Identity(
                "FC-8400T20240415",  /**Device SN 16 digits consisting of English numerals and horizontal bars*/
                "FFFFFFFF",/**The device communication password consists of eight digits*/
                E_ControllerType.Face_Fingerprint); /**Device type*/
        CommandDetail commandDetail = new CommandDetail();
        /**
         * Communication object
         */
        commandDetail.Connector = new UDPDetail(
                "192.168.1.174",/**Device IP address, default is 192.168.1.150*/
                8866,/**Device TCP port number, default is 8000*/
                "",//Local IP address
                9000);//Local monitoring port 
        commandDetail.Identity = idt;
        commandDetail.Timeout = 5000;/**Command timeout period*/
        commandDetail.RestartCount = 2;/**The number of attempts to resend after command failure*/
        return commandDetail;
    }

}
