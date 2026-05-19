package com.example.tcpserverexample.access.command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Face.AdditionalData.Parameter.WriteFeatureCode_Parameter;
import Face.AdditionalData.WriteFeatureCode;
import Face.Door.OpenDoor;
import com.example.tcpserverexample.access.CommandAllocator;
import org.springframework.core.io.DefaultResourceLoader;
import org.springframework.core.io.Resource;

import java.io.IOException;
import java.nio.file.Files;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

public class WriteFeatureCodeCommand {
    CompletableFuture<Boolean> futurePrice = new CompletableFuture<>();


    CommandDetail cmdDtl;

    public WriteFeatureCodeCommand( CommandDetail detail) {

        cmdDtl = detail;
        /**
         * Create command monitoring
         */
        cmdDtl.Event = getConnectorEvent();
    }

    /**
     * Open door command
     */
    public boolean execute() {

        byte[] data=readImage();
        /**
         * Create command object
         */
        WriteFeatureCode cmd = new WriteFeatureCode(new WriteFeatureCode_Parameter(cmdDtl, 10000,1,1,data));
        /**
         * Add the command to be executed to the queue and executed by the allocator
         */
        CommandAllocator.addCommand(cmd);

        try {
            return futurePrice.get(100000, TimeUnit.MILLISECONDS);
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
    private byte[] readImage() {
        String fileName = "classpath:/images/download_23374.jpg";
        // Read the file
        byte[] bytes;
        try {
//            bytes = Files.readAllBytes(Paths.get(fileName));
            DefaultResourceLoader resourceLoader = new DefaultResourceLoader();
            Resource r = resourceLoader.getResource(fileName);
            bytes = Files.readAllBytes(r.getFile().toPath());
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
        return bytes;
    }
}
