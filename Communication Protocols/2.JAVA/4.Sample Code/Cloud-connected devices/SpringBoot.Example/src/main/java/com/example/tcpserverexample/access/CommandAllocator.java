package com.example.tcpserverexample.access;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Connector.*;
import Door.Access.Connector.TCPServer.TCPServerClientDetail;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Data.Door8800WatchTransaction;
import Door.Access.Door8800.Door8800Identity;
import Door.Access.Packet.PacketDecompileAllocator;
import com.example.tcpserverexample.access.DeviceContext;
import com.example.tcpserverexample.access.MyConnectorEvent;

import java.util.HashMap;
import java.util.Map;

public  class CommandAllocator  {
    public static ConnectorAllocator allocator;

    /**
     * Collection of Communication Details
     */
    private static Map<Integer, TCPServerClientDetail> connectorDetailMap = new HashMap<>();

    /**
     * Device Information Collection
     */
    private static Map<String, DeviceContext> deviceMap = new HashMap<>();

   private  CommandAllocator() {

    }

    public static void initializeListen(String ip, int port) {
        allocator=ConnectorAllocator.GetAllocator();
        allocator.AddListener(new MyConnectorEvent());//Add global monitoring
        allocator.Listen(ip, port);
        System.out.println("Add TCP SERVER listener：" + port);
    }

    /**
     * Initialization
     *
     * @param port
     */
    public static void initializeListen(int port) {
        initializeListen("", port);
    }
    public static void addCommand(INCommand cmd) {
        allocator.AddCommand(cmd);
    }

    public static INConnector getConnector(ConnectorDetail detail) {
        return allocator.GetConnector(detail);
    }


    public static CommandDetail getTcpCommand(String deviceSn) {

        if (!deviceMap.containsKey(deviceSn)) {
            throw new IllegalArgumentException("Client Nothingness");
        }
        DeviceContext devic = deviceMap.get(deviceSn);
        if (!connectorDetailMap.containsKey(devic.clientID)) {
            throw new IllegalArgumentException("Client Offline");
        }
        TCPServerClientDetail tcp = connectorDetailMap.get(devic.clientID);


        Door8800Identity idt = new Door8800Identity(
                devic.deviceSn,  /**The device SN is a 16 bit code consisting of English numbers and horizontal bars*/
                devic.password,/**The device communication password consists of eight digits*/
                E_ControllerType.Door8900); /**Device type*/
        CommandDetail commandDetail = new CommandDetail();
        /**
         * Communication object
         */
        commandDetail.Connector = tcp;
        commandDetail.Identity = idt;
        commandDetail.Timeout = 50000;/**Command timeout period*/
        commandDetail.RestartCount = 2;/**The number of attempts to resend after command failure*/
        return commandDetail;
    }

    public static void addDevice(ConnectorDetail detail, String deviceSn) {
        TCPServerClientDetail tcpClient = (TCPServerClientDetail) detail;
        DeviceContext context;
        if (!deviceMap.containsKey(deviceSn)) {
            context = new DeviceContext();
            context.clientID = tcpClient.ClientID;
            context.deviceSn = deviceSn;
            context.password = "FFFFFFFF";
            deviceMap.put(deviceSn, context);
        } else {
            context = deviceMap.get(deviceSn);
            context.clientID = tcpClient.ClientID;
        }
    }

    public static void addConnectorDetail(ConnectorDetail detail) {
        TCPServerClientDetail tcpClient = (TCPServerClientDetail) detail;
        if (!connectorDetailMap.containsKey(tcpClient.ClientID)) {
            connectorDetailMap.put(tcpClient.ClientID, tcpClient);
        }
        allocator.AddWatchDecompile(tcpClient, PacketDecompileAllocator.GetDecompile( E_ControllerType.Face_Fingerprint));
        System.out.println("Client Online：" + tcpClient);
    }

    public static void removeConnectorDetail(ConnectorDetail detail) {
        TCPServerClientDetail tcpClient = (TCPServerClientDetail) detail;
        if (connectorDetailMap.containsKey(tcpClient.ClientID)) {
            connectorDetailMap.remove(tcpClient.ClientID);
        }
        System.out.println("Client Offline：" + tcpClient);
    }


}
