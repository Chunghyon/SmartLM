import Door.Access.Command.CommandDetail;
import access.CommandAllocator;
import command.*;

public class Main {
    public static void main(String[] args) {
        ReadTransactionDatabaseDetail();
    }

    /**
     * Open door command
     */
    private static void openDoor() {
        /**
         * Get command details
         */
        CommandDetail cmdDtl = CommandAllocator.getUDPCommandDetail();
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
     * Add personnel
     */
    private static void addPerson() {
        /**
         * Get command details
         */
        CommandDetail cmdDtl = CommandAllocator.getUDPCommandDetail();
        AddPersonCommand cmd = new AddPersonCommand(cmdDtl);
        cmd.execute();
    }

    /**
     * Add facial photos
     */
    private static void addPersonImage() {
        /**
         * Get command details
         */
        CommandDetail cmdDtl = CommandAllocator.getUDPCommandDetail();
        AddPersonImageCommand cmd = new AddPersonImageCommand(cmdDtl);
        cmd.execute();
    }

    /**
     * Delete facial photos
     */
    private static void deletePersonImage() {
        /**
         * Get command details
         */
        CommandDetail cmdDtl = CommandAllocator.getUDPCommandDetail();
        DeletePersonImageCommand cmd = new DeletePersonImageCommand(cmdDtl);
        cmd.execute();
    }

    private static void readTransaction() {
        CommandDetail cmdDtl = CommandAllocator.getUDPCommandDetail();
        ReadTransactionCommand cmd = new ReadTransactionCommand(cmdDtl);
        cmd.execute();
    }

    private  static  void ReadTransactionDatabaseDetail(){

        CommandDetail cmdDtl = CommandAllocator.getUDPCommandDetail();
        ReadTransactionDatabaseDetailCommand cmd = new ReadTransactionDatabaseDetailCommand(cmdDtl);
        cmd.execute();


    }


}
