package command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Connector.ConnectorEvent;
import access.CommandAllocator;

/**
 * Command abstraction
 */
public abstract class AbstractCommand {

    /**
     * Command details
     */
    protected CommandDetail cmdDtl;

    public AbstractCommand(CommandDetail cmdDtl) {
        this.cmdDtl = cmdDtl;
        /**
         * Create command monitoring
         */
        this.cmdDtl.Event = getConnectorEventHandler();
    }

    /**
     * Obtain event handling
     * @return
     */
    protected abstract ConnectorEvent getConnectorEventHandler();

    /**
     * Execute command
     */
    public abstract void execute();

    /**
     * Add command
     * @param cmd
     */
    protected  void  addCommand(INCommand cmd){
        CommandAllocator.addCommand(cmd);
    }
}
