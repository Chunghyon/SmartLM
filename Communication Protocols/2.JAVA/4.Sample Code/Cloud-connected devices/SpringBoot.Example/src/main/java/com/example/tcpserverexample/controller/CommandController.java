package com.example.tcpserverexample.controller;

import Door.Access.Command.CommandDetail;
import com.example.tcpserverexample.access.CommandAllocator;
import com.example.tcpserverexample.access.command.OpenDoorCommand;
import com.example.tcpserverexample.access.command.WriteFeatureCodeCommand;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/command")
public class CommandController {

    @RequestMapping("/openDoor")
    public boolean openDoor(String sn) {
        //It needs to wait for the device to connect to the TCP server
        CommandDetail detail = CommandAllocator.getTcpCommand(sn);
        WriteFeatureCodeCommand cmd = new WriteFeatureCodeCommand(detail);
        return cmd.execute();
    }
}
