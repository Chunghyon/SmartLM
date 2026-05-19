package com.example.tcpserverexample.access.command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;

import Face.Door.OpenDoor;
import com.example.tcpserverexample.access.CommandAllocator;

import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

public class OpenDoorCommand {

    CompletableFuture<Boolean> futurePrice = new CompletableFuture<>();

    CommandDetail cmdDtl;

    /**
     * Door opening command class
     */
    public OpenDoorCommand( CommandDetail detail) {

        cmdDtl = detail;
        /**
         * Create command monitoring
         */
        cmdDtl.Event = getConnectorEvent();
    }

    /**
     * Door opening command class
     */
    public boolean execute() {


        /**
         * Create command object
         */
        OpenDoor cmd = new OpenDoor(new CommandParameter(cmdDtl));
        /**
         * Add the command to be executed to the queue and ]executed by the allocator
         */
        CommandAllocator.addCommand(cmd);

        try {
            return futurePrice.get(5000, TimeUnit.MILLISECONDS);
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        } catch (ExecutionException e) {
            throw new RuntimeException(e);
        } catch (TimeoutException e) {
            throw new RuntimeException(e);
        }
    }

    private ConnectorEvent getConnectorEvent() {
        return new ConnectorEvent() {
            /**
             * Command successful
             * @param cmd
             * @param result
             */
            @Override
            public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
                System.out.println("Remote door opening successfully");
                futurePrice.complete(true);
            }

            /**
             * Command timeout 
             * @param cmd
             */
            @Override
            public void CommandTimeout(INCommand cmd) {
                System.out.println("Remote door opening command timeout");
                futurePrice.complete(false);
            }
        };
    }
}
