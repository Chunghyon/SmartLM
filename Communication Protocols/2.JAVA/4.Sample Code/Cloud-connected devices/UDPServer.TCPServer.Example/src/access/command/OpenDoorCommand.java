package access.command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Face.Door.OpenDoor;
import access.CommandAllocator;

/**
 * Remote door opening
 */
public class OpenDoorCommand extends AbstractCommand {
    /**
     * Remote door opening
     *
     * @param cmdDtl
     */
    public OpenDoorCommand(CommandDetail cmdDtl) {
        super(cmdDtl);
    }

    /**
     * Obtain event handling
     *
     * @return
     */
    @Override
    protected ConnectorEvent getConnectorEventHandler() {
        return new ConnectorEvent(){
            @Override
            public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
                System.out.println("Remote door opening command successful");
            }

            @Override
            public void CommandTimeout(INCommand cmd) {
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
    }
}
