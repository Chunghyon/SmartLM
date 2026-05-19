package command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Face.Door.OpenDoor;
import access.CommandAllocator;

/**
 * Door opening command class
 */
public class OpenDoorCommand extends AbstractCommand {

    /**
     * Door opening command class
     */
    public OpenDoorCommand( CommandDetail cmdDtl ) {
      super(cmdDtl);
    }

    /**
     * Door opening command
     */
    @Override
    public void execute() {

        /**
         * Create command object
         */
        OpenDoor cmd = new OpenDoor(new CommandParameter(cmdDtl));
        /**
         * Add the command to be executed to the queue and executed by the allocator
         */
        CommandAllocator.addCommand(cmd);
    }
    @Override
    protected ConnectorEvent getConnectorEventHandler() {
        return new ConnectorEvent() {
            /**
             * Command successful
             * @param cmd
             * @param result
             */
            @Override
            public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
                System.out.println("Remote door opening successful");
            }

            /**
             * Command timeout 
             * @param cmd
             */
            @Override
            public void CommandTimeout(INCommand cmd) {
                System.out.println("Remote door opening command timeout");
            }
        };
    }
}
