package command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Door.Access.Data.AbstractTransaction;
import Door.Access.Util.TimeUtil;
import Face.Data.CardTransaction;
import Face.Transaction.ReadTransactionDatabaseDetail;
import Face.Transaction.Result.ReadTransactionDatabaseDetail_Result;
import Face.Transaction.Result.ReadTransactionDatabase_Result;

public class ReadTransactionDatabaseDetailCommand extends AbstractCommand{
    public ReadTransactionDatabaseDetailCommand(CommandDetail cmdDtl) {
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
            /**
             * Command successful
             * @param cmd
             * @param result
             */
            @Override
            public void CommandCompleteEvent(INCommand cmd, INCommandResult result) {

                ReadTransactionDatabaseDetail_Result rResult = (ReadTransactionDatabaseDetail_Result) result;
                System.out.println("Record the ending number：" + rResult.DatabaseDetail.CardTransactionDetail.WriteIndex);
                System.out.println("Record breakpoints：" + rResult.DatabaseDetail.CardTransactionDetail.ReadIndex);
            }

            /**
             * Command timeout 
             * @param cmd
             */
            @Override
            public void CommandTimeout(INCommand cmd) {
                System.out.println("Reading facial recognition records timed out");
            }
        };
    }

    /**
     * Execute command
     */
    @Override
    public void execute() {
        CommandParameter  parameter=  new CommandParameter(cmdDtl);
        ReadTransactionDatabaseDetail cmd = new ReadTransactionDatabaseDetail(parameter);
        addCommand(cmd);
    }
}
