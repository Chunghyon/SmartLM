package command;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorEvent;
import Face.Data.Person;
import Face.Person.AddPerson;
import Face.Person.Parameter.Person_Parameter;
import Face.Person.Result.Person_Result;
import access.CommandAllocator;

import java.util.ArrayList;

/**
 * Door opening command class
 */

public class AddPersonCommand extends AbstractCommand {

    /**
     * Door opening command class
     *
     * @param cmdDtl
     */
    public AddPersonCommand(CommandDetail cmdDtl) {
        super(cmdDtl);
    }

    /**
     * Door opening command class
     */
    @Override
    public void execute() {
        Person person = new Person();
        person.PName = "Testers";
        person.UserCode = 10000;//User ID
        person.PCode = "4433";//Personnel ID
        person.Dept = "Development Department";//Department
        person.Job = "Developers";//Position
        person.CardData = 123456;//Card number
        person.Password = "12345678";//User password (only number with 8 digits) click on the "Face Recognition" circle in the bottom left corner of the device, enter the user number first, and then enter the card number to open the door
        person.Expiry.set(2099, 12 - 1, 31, 23, 59);
        person.TimeGroup = 1; //Opening time zone 1-64, please refer to AddTimeGroup
        person.OpenTimes = 65535;//Opening times 0 times means no number of times, 65535 times means unlimited times
        person.Identity = 0; //User identity, 0-regular user, 1-administrator
        person.CardType = 0;//Card type 0 -- regular card ； 1 --  Normally open (the door will enter a normally open state after swiping the card)
        person.EnterStatus = 0;//Entry and exit marker  0 -entry and exit are valid；1 -Entry is valid；2-Exit is valid

        ArrayList<Person> personList = new ArrayList<>();
        personList.add(person);
        Person_Parameter parameter = new Person_Parameter(cmdDtl, personList);
        AddPerson cmd = new AddPerson(parameter);
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

                Person_Result pResult = (Person_Result) result;
                System.out.println("Upload personnel successfully executed");
                if (pResult.FailTotal > 0) {
                    for (long userCoed : pResult.UserCodeList) {
                        System.out.println("Upload personnel failed, personnel number:" + userCoed);
                    }
                }
            }

            /**
             * Command timeout 
             * @param cmd
             */
            @Override
            public void CommandTimeout(INCommand cmd) {
                System.out.println("Command timeout of Remote door opening");
            }
        };
    }
}
