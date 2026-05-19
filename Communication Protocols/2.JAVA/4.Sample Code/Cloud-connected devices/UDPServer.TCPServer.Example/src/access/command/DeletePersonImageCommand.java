package access.command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Face.AdditionalData.DeleteFile;
import Face.AdditionalData.Parameter.DeleteFile_Parameter;
import access.CommandAllocator;

import java.util.ArrayList;

/**
 *Delete personnel photos
 */
public class DeletePersonImageCommand extends  AbstractCommand{

    /**
     *Delete personnel photos
     * @param cmdDtl
     */
    public DeletePersonImageCommand(CommandDetail cmdDtl) {
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
            /**
             * When the command is completed, this function callback will be triggered
             *
             * @param cmd    Details of the commands associated with this event
             * @param result The results contained in the command
             */
            @Override
            public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {
                System.out.println("Delete facial photo successfully");
            }

            /**
             * When the command timeout occurs, trigger this return function
             *
             * @param cmd The content of this command
             */
            @Override
            public void CommandTimeout(INCommand cmd) {
                System.out.println("Deleting facial photos has timed out");
            }
        };
    }

    /**
     * Execute command
     */
    @Override
    public void execute() {
         ArrayList<Boolean> faceNums=new ArrayList<>();
        faceNums.add(true);
        DeleteFile_Parameter parameter=new DeleteFile_Parameter(cmdDtl,
                10000l,//User ID
                faceNums,//Facial photo number
                null,//Fingerprint feature number
                null,//Palm vein number
                true);
        DeleteFile cmd=new DeleteFile(parameter);
        CommandAllocator.addCommand(cmd);
    }
}
