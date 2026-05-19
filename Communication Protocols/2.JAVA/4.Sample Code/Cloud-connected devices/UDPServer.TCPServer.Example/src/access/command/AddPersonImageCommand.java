package access.command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Face.AdditionalData.Parameter.WriteFeatureCode_Parameter;
import Face.AdditionalData.Result.WriteFeatureCode_Result;
import Face.AdditionalData.WriteFeatureCode;
import access.CommandAllocator;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;

public class AddPersonImageCommand extends AbstractCommand {
    public AddPersonImageCommand(CommandDetail cmdDtl) {
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
                WriteFeatureCode_Result wResult = (WriteFeatureCode_Result) result;
                /**
                 * Write result /// 1--Verification successful //0--Verification failed  //2--Feature code cannot be recognized //3--Personnel photo cannot be recognized //255-File not ready
                 */
                System.out.println("Write a facial photo and return the result" + wResult.Success);
                if(wResult.RepeatedCode>0){
                    System.out.println("Facial photo and【" + wResult.RepeatedCode+"】are duplicated");
                }
            }

            @Override
            public void CommandTimeout(INCommand cmd) {
                System.out.println("Writing facial photo timed out");
            }

            /**
             * Command process
             *
             * @param cmd
             */
            @Override
            public void CommandProcessEvent(INCommand cmd) {
                System.out.println("Writing progress of facial photos："+cmd.getProcessStep()+":"+cmd.getProcessMax());
            }
        };
    }

    /**
     * Execute command
     */
    @Override
    public void execute() {

        cmdDtl.Timeout=10000;
        long userCode = 10000; //User ID
        /**
         * Types of uploaded files
         * 1	Personnel profile picture
         * 2	Fingerprint
         * 3	Infrared facial feature code
         * 4	Dynamic facial feature code
         * 5	Palm vein feature code
         * 10	Boot picture
         * 11	Standby picture
         */
        int type = 1;
        /**
         * Serial number
         * The range of numerical values for personnel profile pictures：1-5
         * Range of fingerprint values：0-9
         * The range of values for the palm vein feature code is 1 or 2
         * Boot picture： Face recognition device, image size 720 * 1280, size within 300kb, default is 0
         * Standby picture：Facial recognition device, image size 720 * 1280, size within 300kb, with 8 photos, number is 1-8
         */
        int serialNumber = 1;
        /**
         * File data
         */
        byte[] data = readImage();
        /**
         * Write feature command parameters
         */
        WriteFeatureCode_Parameter parameter = new WriteFeatureCode_Parameter(cmdDtl, userCode, type, serialNumber, data);
        /**
         * Write feature command
         */
        WriteFeatureCode cmd = new WriteFeatureCode(parameter);
        /**
         * Add command to queue
         */
        CommandAllocator.addCommand(cmd);
    }

    /**
     * Read image content
     *
     * @return
     */
    private byte[] readImage() {
        String fileName = "resource/people1.jpg";
        // Read the file
        byte[] bytes;
        try {
            bytes = Files.readAllBytes(Paths.get(fileName));
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
        return bytes;
    }
}
