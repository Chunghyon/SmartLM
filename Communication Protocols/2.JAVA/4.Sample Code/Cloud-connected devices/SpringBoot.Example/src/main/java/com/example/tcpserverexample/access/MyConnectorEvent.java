package com.example.tcpserverexample.access;

import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.ConnectorEvent;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Connector.INConnector;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Data.Door8800WatchTransaction;
import Door.Access.Packet.PacketDecompileAllocator;

public class MyConnectorEvent extends ConnectorEvent {


    @Override
    public void WatchEvent(ConnectorDetail connectorDetail, INData inData) {
        if (inData instanceof Door8800WatchTransaction) {
            Door8800WatchTransaction watchEvent = (Door8800WatchTransaction) inData;
          CommandAllocator. addDevice(connectorDetail, watchEvent.SN);
        }
    }

    @Override
    public void ClientOnline(ConnectorDetail connectorDetail) {
        CommandAllocator.addConnectorDetail(connectorDetail);
        INConnector conn = CommandAllocator.getConnector(connectorDetail);
        /**Open the connection channel for long connections*/
        conn.OpenForciblyConnect();
        /**Add data monitoring data parser*/
        conn.AddWatchDecompile(connectorDetail, PacketDecompileAllocator.GetDecompile(E_ControllerType.Door8900));
    }

    @Override
    public void ClientOffline(ConnectorDetail connectorDetail) {
        CommandAllocator.removeConnectorDetail(connectorDetail);
    }

    @Override
    public void ConnectorErrorEvent(ConnectorDetail detail) {
        System.out.println("Connection error:"+detail);
    }
}
