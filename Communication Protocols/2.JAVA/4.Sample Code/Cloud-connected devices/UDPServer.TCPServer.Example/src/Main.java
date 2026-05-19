import Door.Access.Command.CommandDetail;
import access.CommandAllocator;
import access.command.*;

public class Main {

    public static void main(String[] args) {

        /**
         * Initialize the TCP UDP SERVER monitoring
         * After successful monitoring, wait for the device to connect
         * After the device is successfully connected, commands can be sent to the device
         */
        CommandAllocator.initializeListen(9000);

        /**
         * Simulate client sending call instructions using backend threads
         */
        Thread thread = new CommandThread();
        thread.start();

    }


}
