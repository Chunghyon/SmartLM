package command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.CommandParameter;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Door.Access.Data.AbstractTransaction;
import Door.Access.Util.TimeUtil;
import Face.Data.CardTransaction;
import Face.Data.e_TransactionDatabaseType;
import Face.Door.OpenDoor;
import Face.Transaction.Parameter.ReadTransactionDatabase_Parameter;
import Face.Transaction.ReadTransactionDatabase;
import Face.Transaction.Result.ReadTransactionDatabase_Result;
import access.CommandAllocator;

/**
 * Read facial recognition records (based on the index number, only uncollected records are read, records that have already been collected will not be re collected. If re collection is required, the index number can be reset, and the index number can be set by referring to the WriteTransactionDB2ReadIndex class)
 */
public class ReadTransactionCommand extends AbstractCommand {
    /**
     * Door opening command class
     */
    public ReadTransactionCommand(CommandDetail cmdDtl) {
        super(cmdDtl);
    }

    /**
     * Door opening command class
     */
    @Override
    public void execute() {

        ReadTransactionDatabase_Parameter parameter = new ReadTransactionDatabase_Parameter(cmdDtl, e_TransactionDatabaseType.OnCardTransaction);
        /**
         * Create command object
         */
        ReadTransactionDatabase cmd = new ReadTransactionDatabase(parameter);
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
                System.out.println("Facial recognition record reading successful");
                ReadTransactionDatabase_Result rResult = (ReadTransactionDatabase_Result) result;
                System.out.println("Reading quantity：" + rResult.Quantity);
                if (rResult.Quantity == 0) {
                    return;
                }
                for (AbstractTransaction transaction : rResult.TransactionList) {
                    System.out.println("------------------------------------------");
                    CardTransaction t = (CardTransaction) transaction;
                    System.out.println("Record number：" + t.getRecordSerialNumber());
                    System.out.println("User ID：" + t.getUserCode());
                    System.out.println("Record time:" + TimeUtil.FormatTime(t.TransactionDate()));
                    System.out.println("Direction of entry and exit:" + t.getAccessType());//1-Entry 2-Exit
                    System.out.println("Whether have any recorded photos:" + t.getPhoto());//1-Have Picture 0-No Picture
                    System.out.println("Verification record type:" + t.TransactionCode());

                    /**
                     * Verification Record<br>
                     * TransactionCode Meaning Table of Event Codes：<br>
                     * 1	Card verification<br>
                     * 2	Fingerprint verification<br>
                     * 3	Face verification<br>
                     * 4	Fingerprint + Card<br>
                     * 5	Face + Fingerprint<br>
                     * 6	Face + Card<br>
                     * 7	Card + Password<br>
                     * 8	Face + Password<br>
                     * 9	Fingerprint + Password<br>
                     * 10	Manually enter user ID and password to verification<br>
                     * 11	Fingerprint+Card+Password<br>
                     * 12	Face+Card+Password<br>
                     * 13	Face+Fingerprint+Password<br>
                     * 14	Face+Fingerprint+Card<br>
                     * 15	Repeated verification<br>
                     * 16	Validity Expired<br>
                     * 17	The opening time zone has expired<br>
                     * 18	Not open the door during holidays<br>
                     * 19	Unregistered user<br>
                     * 20	Detection lock<br>
                     * 21	The number of valid times has been exhausted<br>
                     * 22	Verify when locked, prohibit opening the door<br>
                     * 23	Lost reported card<br>
                     * 24	Blacklist card<br>
                     * 25	Open the door without verification -- Open the door without verification - when pressing the fingerprint, the user number is 0, and when swiping the card, the user number is the card number<br>
                     * 26	Prohibit card swiping verification  --   [Permission authentication method] Disable to swipe card<br>
                     * 27	Prohibit fingerprint verification  --   [Permission authentication method] Disable to identify fingerprint<br>
                     * 28	The controller has expired<br>
                     * 29	Verified - Validity period is ready to expire<br>
                     */
                }
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
}
