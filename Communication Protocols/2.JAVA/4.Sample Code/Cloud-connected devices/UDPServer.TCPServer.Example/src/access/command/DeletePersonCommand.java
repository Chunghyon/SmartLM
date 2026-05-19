package access.command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Face.Person.DeletePerson;
import Face.Person.Parameter.DeletePerson_Parameter;
import access.CommandAllocator;

import java.util.ArrayList;

/**
 * Delete personnel
 */
public class DeletePersonCommand extends AbstractCommand {
    /**
     * Delete personnel
     * @param cmdDtl
     */
    public DeletePersonCommand(CommandDetail cmdDtl) {
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
                    System.out.println("Personnel deleted successfully");
            }

            @Override
            public void CommandTimeout(INCommand cmd) {
                System.out.println("The command to delete personnel has timed out");
            }
        };
    }

    /**
     * Execute command
     */
    @Override
    public void execute() {
        /**
         * User ID to be deleted
         */
        ArrayList<Long> userCodeList=new ArrayList<>();
        userCodeList.add(10000l);
        /**
         * Command Parameters
         */
        DeletePerson_Parameter parameter=new DeletePerson_Parameter(cmdDtl,userCodeList);
        /**
         * Delete personnel command
         */
        DeletePerson cmd=new DeletePerson(parameter);
        /**
         * Add command to queue
         */
        CommandAllocator.addCommand(cmd);
    }
}
