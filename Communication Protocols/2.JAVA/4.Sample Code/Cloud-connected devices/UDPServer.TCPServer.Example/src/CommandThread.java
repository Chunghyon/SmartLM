import Door.Access.Command.CommandDetail;
import access.CommandAllocator;
import access.Device;
import access.command.*;

public class CommandThread extends Thread {

    @Override
    public void run() {
        while (true) {
            if (CommandAllocator.getDeviceSize() > 0) { //Determine whether there is a device present
                Device device = CommandAllocator.getFirstDevice();
                openDoor(device);

                break;
            }
            try {
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                throw new RuntimeException(e);
            }
        }
    }

    /**
     * Open door command
     */
    private  void openDoor(Device device) {
        /**
         * Get command details
         */
        CommandDetail cmdDtl = CommandAllocator.getCommandDetail(device);
        /**
         * Create remote door opening command
         */
        OpenDoorCommand cmd = new OpenDoorCommand(cmdDtl);
        /**
         * Execute command
         */
        cmd.execute();
    }

    /**
     * Synchronize to call remote door opening
     */
    private  void syncOpenDoor(Device device) {
        /**
         * Get command details
         */
        CommandDetail cmdDtl = CommandAllocator.getCommandDetail(device);
        /**
         * Create remote door opening command
         */
        SyncOpenDoorCommand cmd = new SyncOpenDoorCommand(cmdDtl);
        /**
         * Execute command
         */
        cmd.execute();
    }
    /**
     * Add personnel
     */
    private  void addPerson(Device device) {
        /**
         * Get command details
         */
        CommandDetail cmdDtl = CommandAllocator.getCommandDetail(device);
        AddPersonCommand cmd = new AddPersonCommand(cmdDtl);
        cmd.execute();
    }

    /**
     * Add facial photos
     */
    private  void addPersonImage(Device device) {
        /**
         * Get command details
         */
        CommandDetail cmdDtl = CommandAllocator.getCommandDetail(device);
        AddPersonImageCommand cmd = new AddPersonImageCommand(cmdDtl);
        cmd.execute();
    }

    /**
     * Delete facial photos
     */
    private  void deletePersonImage(Device device) {
        /**
         * Get command details
         */
        CommandDetail cmdDtl = CommandAllocator.getCommandDetail(device);
        DeletePersonImageCommand cmd = new DeletePersonImageCommand(cmdDtl);
        cmd.execute();
    }

    private  void readTransaction(Device device) {
        CommandDetail cmdDtl = CommandAllocator.getCommandDetail(device);
        ReadTransactionCommand cmd = new ReadTransactionCommand(cmdDtl);
        cmd.execute();
    }
}
