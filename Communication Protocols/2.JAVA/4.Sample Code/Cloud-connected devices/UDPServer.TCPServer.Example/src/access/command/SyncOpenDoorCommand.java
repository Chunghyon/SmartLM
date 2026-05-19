package access.command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Face.Door.OpenDoor;
import access.CommandAllocator;

import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

/**
 * Example of synchronously calling remote door opening
 */
public class SyncOpenDoorCommand extends AbstractCommand {

    CompletableFuture<Boolean> futurePrice = new CompletableFuture<>();

    /**
     * Example of synchronously calling remote door opening
     *
     * @param cmdDtl
     */
    public SyncOpenDoorCommand(CommandDetail cmdDtl) {
        super(cmdDtl);
    }

    /**
     * Obtain event handling
     *
     * @return
     */
    @Override
    protected ConnectorEvent getConnectorEventHandler() {
        return new ConnectorEvent() {
            @Override
            public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
                futurePrice.complete(true);

            }

            @Override
            public void CommandTimeout(INCommand cmd) {
                futurePrice.complete(false);
                System.out.println("Remote door opening command timeout");
            }
        };
    }

    /**
     * Execute command
     */
    @Override
    public void execute() {
        /**
         * Command Parameters
         */
        CommandParameter parameter = new CommandParameter(cmdDtl);

        /**
         * Create Command
         */
        OpenDoor cmd = new OpenDoor(parameter);
        /**
         * Add to the queue
         */
        CommandAllocator.addCommand(cmd);

        try {
            boolean result = futurePrice.get(5000, TimeUnit.MILLISECONDS);
            if (result == true) {
                System.out.println("Remote door opening command successful");
            } else {
                System.out.println("Remote door opening command timeout");
            }
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        } catch (ExecutionException e) {
            throw new RuntimeException(e);
        } catch (TimeoutException e) {
            throw new RuntimeException(e);
        }
    }
}
