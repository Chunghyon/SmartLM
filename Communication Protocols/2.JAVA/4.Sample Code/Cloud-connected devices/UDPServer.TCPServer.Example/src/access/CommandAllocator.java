package access;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Connector.INConnector;
import Door.Access.Connector.TCPServer.TCPServerClientDetail;
import Door.Access.Door8800.Door8800Identity;

import java.util.HashMap;
import java.util.Map;

public class CommandAllocator {

    /**
     * Connect the alloter
     */
    public static ConnectorAllocator allocator = ConnectorAllocator.GetAllocator();
    /**
     * Collection of Communication Details
     */
    private static Map<Integer, TCPServerClientDetail> connectorDetailMap = new HashMap<>();

    /**
     * Device Information Collection
     */
    private static Map<String, Device> deviceMap = new HashMap<>();

    /**
     * Initialization
     *
     * @param ip
     * @param port
     */
    public static void initializeListen(String ip, int port) {
        allocator.AddListener(new MyConnectorEvent());//Add global monitoring
        allocator.Listen(ip, port);//udp monitoring
        allocator.UDPBind(ip, port);//tcp monitoring（Two methods can be used, or only using one）
        System.out.println("Service startup successful, TCP UDP monitoring port is：" + port);
    }

    /**
     * Obtain the number of devices
     *
     * @return
     */
    public static int getDeviceSize() {
        return deviceMap.size();
    }

    /**
     * Obtain the first connected device
     *
     * @return
     */
    public static Device getFirstDevice() {
        for (Map.Entry<String, Device> entry : deviceMap.entrySet()) {
            return entry.getValue();
        }
        throw new IllegalArgumentException("The device list is empty");
    }
    public static Device getDevice(String sn) {
      Device device=  deviceMap.get(sn) ;
      if(device==null)
        throw new IllegalArgumentException("The device list is empty");
      return  device;
    }
    /**
     * Initialization
     *
     * @param port
     */
    public static void initializeListen(int port) {
        initializeListen("", port);
    }

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
    public static CommandDetail getCommandDetail(Device device) {
        Door8800Identity idt = new Door8800Identity(
                device.getDeviceSn(),  /**The device SN 16 digits is composed of English numbers and horizontal bars*/
                device.getPassword(),/**The device communication password consists of eight digits*/
                E_ControllerType.Face_Fingerprint); /**Device type*/
        CommandDetail commandDetail = new CommandDetail();
        if (!connectorDetailMap.containsKey(device.getClientID())) {
            throw new IllegalArgumentException("Device offline");
        }
        /**
         * Communication Details
         */
        commandDetail.Connector = connectorDetailMap.get(device.getClientID());
        commandDetail.Identity = idt;
        commandDetail.Timeout = 5000;/**Command timeout period*/
        commandDetail.RestartCount = 2;/**The number of attempts to resend after command failure*/
        return commandDetail;
    }

    /**
     * Add device
     *
     * @param detail
     * @param deviceSn
     */
    public static void addDevice(ConnectorDetail detail, String deviceSn) {
        TCPServerClientDetail tcpClient = (TCPServerClientDetail) detail;
        Device device;
        if (!deviceMap.containsKey(deviceSn)) {
            device = new Device(deviceSn, "FFFFFFFF", tcpClient.ClientID);
            deviceMap.put(deviceSn, device);
        } else {
            device = deviceMap.get(deviceSn);
            device.setClientID(tcpClient.ClientID);
        }
    }

    /**
     * Add TCP connection object
     *
     * @param detail
     */
    public static void addConnectorDetail(ConnectorDetail detail) {
        TCPServerClientDetail tcpClient = (TCPServerClientDetail) detail;
        if (!connectorDetailMap.containsKey(tcpClient.ClientID)) {
            connectorDetailMap.put(tcpClient.ClientID, tcpClient);
        }
        System.out.println("Device online：" + tcpClient);
    }



    /**
     * Delete TCP connection object
     *
     * @param detail
     */
    public static void removeConnectorDetail(ConnectorDetail detail) {
        TCPServerClientDetail tcpClient = (TCPServerClientDetail) detail;
        if (connectorDetailMap.containsKey(tcpClient.ClientID)) {
            connectorDetailMap.remove(tcpClient.ClientID);
        }
        System.out.println("Device offline：" + tcpClient);
    }
}
