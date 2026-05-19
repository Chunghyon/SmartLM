# 1.Summary

## 1.1Brief introduction

The quick start guide for secondary development documents mainly helps developers to quickly build and interact with device commands, mainly describing how to communicate with facial and fingerprint devices through SDK commands.

## 1.2 Basic keywords

All commands of（ConnectorAllocator）are executed and sent within the communication connector

Communication connector event notification（INConnectorEvent），all command execution results and message push are returned in the event notification

Command Detail（CommandDetail），including necessary information for command execution, the connector channel for command execution, command authentication information, user additional data, timeout retry parameters

## 1.3Start

To create a listening event class of CommandEventListeners, it is necessary to inherit INConnectorEvent and implement the interface

```java
package Demo;

import Door.Access.Command.CommandDetail;
import Door.Access.Command.INCommand;
import Door.Access.Command.INCommandResult;
import Door.Access.Connector.ConnectorAllocator;
import Door.Access.Connector.ConnectorDetail;
import Door.Access.Connector.E_ControllerType;
import Door.Access.Connector.INConnectorEvent;
import Door.Access.Connector.UDP.UDPDetail;
import Door.Access.Data.INData;
import Door.Access.Door8800.Command.Door.OpenDoor;
import Door.Access.Door8800.Door8800Identity;

/**
 * Inherit the class that listens to event interfaces
 */
public class CommandEventListeners implements INConnectorEvent {

    String _localIP;
    int _localPort;
    /**
     * Communication connector
     */
    public ConnectorAllocator Allocator = ConnectorAllocator.GetAllocator();

    public CommandEventListeners(String sLocalIP, int iLocalPort) {
        _localIP = sLocalIP;
        _localPort = iLocalPort;
        //Obtain instances in the construction method
        Allocator = ConnectorAllocator.GetAllocator();
        //Add event notification
        Allocator.AddListener(this);
        //Binding is required when using UDP protocol
        Allocator.UDPBind(_localIP, _localPort);
        //Using TCP server requires listening on ports
       // _allocator.Listen(9000);
    }

    /**
     * Command execution successful listening event
     *
     * @param inCommand       The current command object being executed
     * @param inCommandResult The return result of executing a command, some commands do not return a result, so returning to this listening event is considered a successful command
     */
    @Override
    public void CommandCompleteEvent(INCommand inCommand, INCommandResult inCommandResult) {
        System.out.println(inCommand);
        if(inCommand instanceof OpenDoor){
            System.out.println("Opening door successfully");
        }
    }

    /**
     * Current command execution progress
     *
     * @param cmd The current command object being executed
     */
    @Override
    public void CommandProcessEvent(INCommand cmd) {
        System.out.println("Current command:" + cmd.getClass().toString() + ",Current progress:" + cmd.getProcessStep() + "/" + cmd.getProcessMax());
        //Current command:OpenDoor,Current progress:1/1
    }

    /**
     * Connection failed
     *
     * @param cmd    The current command object being executed
     * @param isStop Whether to manual stop
     */
    @Override
    public void ConnectorErrorEvent(INCommand cmd, boolean isStop) {

    }

    /**
     * Connection failed
     *
     * @param connectorDetail TCP or UDP connection object
     */
    @Override
    public void ConnectorErrorEvent(ConnectorDetail connectorDetail) {

    }

    /**
     * Connection timed out
     *
     * @param cmd The current command object being executed
     */
    @Override
    public void CommandTimeout(INCommand cmd) {

    }

    /**
     * Password error
     *
     * @param cmd The current command object being executed
     */
    @Override
    public void PasswordErrorEvent(INCommand cmd) {

    }

    /**
     * Verification and Error
     *
     * @param cmd The current command object being executed
     */
    @Override
    public void ChecksumErrorEvent(INCommand cmd) {

    }

    /**
     * Data monitoring
     *
     * @param connectorDetail TCP or UDP connection object
     * @param inData          Monitoring data
     */
    @Override
    public void WatchEvent(ConnectorDetail connectorDetail, INData inData) {

    }

    /**
     * Device online (triggered only when the device is connected as a client)
     *
     * @param connectorDetail TCP or UDP connection objects need to be converted to be obtained
     */
    @Override
    public void ClientOnline(ConnectorDetail connectorDetail) {
        //When the device online, the connected objects need to be saved, and subsequent commands need to use the connected objects
    }

    /**
     * Device offline (triggered only when the device is connected as a client)
     *
     * @param connectorDetail TCP or UDP connection objects need to be converted to be obtained
     */
    @Override
    public void ClientOffline(ConnectorDetail connectorDetail) {
        //When the device is offline, the device connection object needs to be deleted to avoid using offline connection objects
    }

    /**
     * Get command details object
     */
    public CommandDetail getCommandDetail() {
        /**
         * 192.168.1.171 IP address of the device
         * 8101 The UPD port of the device, factory default is 8101
         */
        UDPDetail tcpClient = new UDPDetail("192.168.1.171", 8101, _localIP, _localPort);
        tcpClient.Timeout = 5000;//Connection timeout (milliseconds)
        tcpClient.RestartCount = 0;//Reconnect times
        /**
        *Parameters that need to be modified for communication with different devices
        *FC-8400T20220888 Device SN, fixed length 16 digits, with the first 8 digits indicating device type FC-8400T and the last 8 digits indicating serial number 20220888
        *FFFFFFFF  Device communication password, fixed with 8 digits, FFFFFFFF is the default password
        *Face_Fingerprint fingerprint protocol type of facial recognition terminal
         */
        Door8800Identity idt = new Door8800Identity("FC-8400T20220888", "FFFFFFFF", E_ControllerType.Face_Fingerprint);
        CommandDetail commandDetail = new CommandDetail();
        commandDetail.Connector = tcpClient;
        commandDetail.Identity = idt;
        return commandDetail;
    }
}

```

Create command execution class

```java
package Demo;

import Door.Access.Command.CommandDetail;
import Door.Access.Door8800.Command.Door.OpenDoor;
import Door.Access.Door8800.Command.Door.Parameter.OpenDoor_Parameter;

/**
 * Command Example
 */
public class CommandDemo {
    /***
     * Event monitoring class
     */
    EventListenersDemo _eventListenersDemo;

    /***
     * Command Example
     * @param eventListenersDemo Event monitoring
     */
    public CommandDemo(EventListenersDemo eventListenersDemo) {
        _eventListenersDemo = eventListenersDemo;
    }

    /***
     * Send door opening command
     */
    public void SendOpenDoor() {
        //Get command details
        CommandDetail cmdDetail = _eventListenersDemo.getCommandDetail();
        //Create command parameter object
        OpenDoor_Parameter par = new OpenDoor_Parameter(cmdDetail);
        //Create Command
        OpenDoor cmd = new OpenDoor(par);
        //Add command to communication connector
        _eventListenersDemo.Allocator.AddCommand(cmd);
    }
}

```

**main method call**

```java
    public static void main(String[] args) {
        EventListenersDemo eventListenersDemo = new EventListenersDemo("192.168.1.110", 9000);
        CommandDemo demo = new CommandDemo(eventListenersDemo);
        demo.SendOpenDoor();
    }
```

# 2.Command List

### 1.System parameter

| Command object                                                     | Explain                                                         |
| ------------------------------------------------------------ | ------------------------------------------------------------ |
| [BeginWatch](../2.SDK Document Description/javadoc/Face/System/BeginWatch.html)              | Enable data monitoring                                              |
| [CloseWatch](../2.SDK Document Description/javadoc/Face/System/CloseWatch.html)              | Disable data monitoring                                                 |
| [ReadClientWorkMode](../2.SDK Document Description/javadoc/Face/System/ReadClientWorkMode.html) | Read the communication method in client mode                                   |
| [ReadFaceBodyTemperaturePar](../2.SDK Document Description/javadoc/Face/System/ReadFaceBodyTemperaturePar.html) | Read the temperature detection switch and temperature format                                  |
| [ReadFaceLEDMode](../2.SDK Document Description/javadoc/Face/System/ReadFaceLEDMode.html)    | Read the supplement light mode                                               |
| [ReadFaceMouthmufflePar](../2.SDK Document Description/javadoc/Face/System/ReadFaceMouthmufflePar.html) | Read the mask recognition switch                                             |
| [ReadKeepAliveInterval](../2.SDK Document Description/javadoc/Face/System/ReadKeepAliveInterval.html) | Read the keep alive interval of the client                                          |
| [ReadManageMenuPassword](../2.SDK Document Description/javadoc/Face/System/ReadManageMenuPassword.html) | Read management password                                                |
| [ReadOEM](../2.SDK Document Description/javadoc/Face/System/ReadOEM.html)                    | Read OEM information                                                 |
| [ReadSn](../2.SDK Document Description/javadoc/Face/System/ReadSn.html)                      | Read device SN                                                  |
| [ReadSystemRunStatus](../2.SDK Document Description/javadoc/Face/System/ReadSystemRunStatus.html) | Read the operating status of the system                                            |
| [ReadSystemStatus](../2.SDK Document Description/javadoc/Face/System/ReadSystemStatus.html)  | Read device status                                                 |
| [ReadVersion](../2.SDK Document Description/javadoc/Face/System/ReadVersion.html)            | Read version number                                                |
| [ReadWatchState](../2.SDK Document Description/javadoc/Face/System/ReadWatchState.html)      | Read monitoring status                                                 |
| [ReadWiegandOutput](../2.SDK Document Description/javadoc/Face/System/ReadWiegandOutput.html) | Read Wiegand parameters                                                |
| [RequireConnectServer](../2.SDK Document Description/javadoc/Face/System/RequireConnectServer.html) | Command the device to immediately reconnect to the server, notify the device to immediately reconnect to the server, disconnect immediately when already connected, and then reconnect; Suitable for TCP client mode and UDP client mode, only resend keep alive packets.|
| [SendConnectTestResponse](../2.SDK Document Description/javadoc/Face/System/SendConnectTestResponse.html) | Heartbeat packet and connection test response                                        |
| [WriteClientWorkMode](../2.SDK Document Description/javadoc/Face/System/WriteClientWorkMode.html) | Write the communication method in client mode                                     |
| [WriteFaceBodyTemperaturePar](../2.SDK Document Description/javadoc/Face/System/WriteFaceBodyTemperaturePar.html) | Set temperature detection switch and temperature format                                  |
| [WriteFaceLEDMode](../2.SDK Document Description/javadoc/Face/System/WriteFaceLEDMode.html)  | Set the supplement light mode                                               |
| [WriteFaceMouthmufflePar](../2.SDK Document Description/javadoc/Face/System/WriteFaceMouthmufflePar.html) | Set mask recognition switch                                            |
| [WriteManageMenuPassword](../2.SDK Document Description/javadoc/Face/System/WriteManageMenuPassword.html) | Write management password                                                 |
| [WriteOEM](../2.SDK Document Description/javadoc/Face/System/WriteOEM.html)                  | Write OEM information                                                  |
| [WriteWiegandOutput](../2.SDK Document Description/javadoc/Face/System/WriteWiegandOutput.html) | Write Wiegand parameters                                                |

### 2.Personnel parameters

| Command object                                                   | Explain                            |
| ------------------------------------------------------------ | -------------------------------- |
| [AddPerson](../2.SDK Document Description/javadoc/Face/Person/AddPerson.html)                | Add personnel                        |
| [AddPersonAndImage](../2.SDK Document Description/javadoc/Face/Person/AddPersonAndImage.html) | Add personnel and upload personnel identification information      |
| [ClearPersonDataBase](../2.SDK Document Description/javadoc/Face/Person/ClearPersonDataBase.html) | Clear all personnel information from the controller       |
| [DeletePerson](../2.SDK Document Description/javadoc/Face/Person/DeletePerson.html)          | Delete personnel                         |
| [ReadPersonDataBase](../2.SDK Document Description/javadoc/Face/Person/ReadPersonDataBase.html) | Read all registered personnel information from the device |
| [ReadPersonDatabaseDetail](../2.SDK Document Description/javadoc/Face/Person/ReadPersonDatabaseDetail.html) | Read personnel storage details                 |
| [ReadPersonDetail](../2.SDK Document Description/javadoc/Face/Person/ReadPersonDetail.html)  | Query personnel data details                 |
| [RegisterIdentificationData](../2.SDK Document Description/javadoc/Face/Person/RegisterIdentificationData.html) | Registered personnel fingerprint feature code or personnel profile picture |

### 3.Door parameters

| Command object                                                    | Explain                       |
| ------------------------------------------------------------ | -------------------------- |
| [CloseDoor](../2.SDK Document Description/javadoc/Face/Door/CloseDoor.html)                  | Remote to close the door                 |
| [HoldDoor](../2.SDK Document Description/javadoc/Face/Door/HoldDoor.html)                    | Door normally opening                     |
| [LockDoor](../2.SDK Document Description/javadoc/Face/Door/LockDoor.html)                    | Lock the door                    |
| [OpenDoor](../2.SDK Document Description/javadoc/Face/Door/OpenDoor.html)                    | Remote door opening                   |
| [OpenDoor_CheckNum](../2.SDK Document Description/javadoc/Face/Door/OpenDoor_CheckNum.html)  | Verification code for remote door opening             |
| [ReadExemptionVerificationOpen](../2.SDK Document Description/javadoc/Face/Door/ReadExemptionVerificationOpen.html) | Open the door without verification_Reading             |
| [ReadExpirationPrompt](../2.SDK Document Description/javadoc/Face/Door/ReadExpirationPrompt.html) | Permission expiration prompt parameter——Reading     |
| [ReadReaderIntervalTime](../2.SDK Document Description/javadoc/Face/Door/ReadReaderIntervalTime.html) | Repeat verification permission interval——Reading      |
| [ReadReaderOption](../2.SDK Document Description/javadoc/Face/Door/ReadReaderOption.html)    | Card reader byte count_Reading            |
| [ReadRelayOption](../2.SDK Document Description/javadoc/Face/Door/ReadRelayOption.html)      | Relay parameters_Reading           |
| [ReadUnlockingTime](../2.SDK Document Description/javadoc/Face/Door/ReadUnlockingTime.html)  | Output duration when unlocking            |
| [ReadVoiceBroadcastSetting](../2.SDK Document Description/javadoc/Face/Door/ReadVoiceBroadcastSetting.html) | Set up voice broadcast function_Reading      |
| [UnlockDoor](../2.SDK Document Description/javadoc/Face/Door/UnlockDoor.html)                | Unlock the door                 |
| [WriteExemptionVerificationOpen](../2.SDK Document Description/javadoc/Face/Door/WriteExemptionVerificationOpen.html) | Open the door without verification_write in              |
| [WriteExpirationPrompt](../2.SDK Document Description/javadoc/Face/Door/WriteExpirationPrompt.html) | Permission expiration prompt parameter——write in     |
| [WriteReaderIntervalTime](../2.SDK Document Description/javadoc/Face/Door/WriteReaderIntervalTime.html) | Set the interval for repeated verification permissions——write in   |
| [WriteReaderOption](../2.SDK Document Description/javadoc/Face/Door/WriteReaderOption.html)  | Byte count of card reader_write in            |
| [WriteRelayOption](../2.SDK Document Description/javadoc/Face/Door/WriteRelayOption.html)    | Relay Parameters_write in             |
| [WriteUnlockingTime](../2.SDK Document Description/javadoc/Face/Door/WriteUnlockingTime.html) | Output duration when unlocking           |
| [WriteVoiceBroadcastSetting](../2.SDK Document Description/javadoc/Face/Door/WriteVoiceBroadcastSetting.html) | Set up voice broadcast_write in          |

### 4.Opening Time Zone

| Command object                                                   | Explain         |
| ---------------------------------------------------------- | ------------ |
| [AddTimeGroup](../2.SDK Document Description/javadoc/Face/TimeGroup/AddTimeGroup.html)     | Add the door opening time zone |
| [ClearTimeGroup](../2.SDK Document Description/javadoc/Face/TimeGroup/ClearTimeGroup.html) | Clear the door opening time zone |
| [ReadTimeGroup](../2.SDK Document Description/javadoc/Face/TimeGroup/ReadTimeGroup.html)   | Read the door opening time zone |

### 5.Record operations

| Command object                                                     | Explain                                |
| ------------------------------------------------------------ | ----------------------------------- |
| [ClearTransactionDatabase](../2.SDK Document Description/javadoc/Face/Transaction/ClearTransactionDatabase.html) | Clear the record database of the specified type           |
| [ReadTransactionDatabase](../2.SDK Document Description/javadoc/Face/Transaction/ReadTransactionDatabase.html) | Read new records                          |
| [ReadTransactionDatabaseByIndex](../2.SDK Document Description/javadoc/Face/Transaction/ReadTransactionDatabaseByIndex.html) | Read database record information according to the index          |
| [ReadTransactionDatabaseDetail](../2.SDK Document Description/javadoc/Face/Transaction/ReadTransactionDatabaseDetail.html) | Read card database information from the controller       |
| [WriteTransactionDatabaseReadIndex](../2.SDK Document Description/javadoc/Face/Transaction/WriteTransactionDatabaseReadIndex.html) | Update record pointer                        |
| [WriteTransactionDatabaseWriteIndex](../2.SDK Document Description/javadoc/Face/Transaction/WriteTransactionDatabaseWriteIndex.html) | Modify the write index of the specified record database - record suffix |

### 6.Attachment data

| Command object                                                      | Explain                  |
| ------------------------------------------------------------ | -------------------- |
| [DeleteFile](../2.SDK Document Description/javadoc/Face/AdditionalData/DeleteFile.html)      | Delete files            |
| [ReadFeatureCode](../2.SDK Document Description/javadoc/Face/AdditionalData/ReadFeatureCode.html) | Read fingerprints             |
| [ReadFile](../2.SDK Document Description/javadoc/Face/AdditionalData/ReadFile.html)          | Read large files         |
| [ReadPersonAdditionalDetail](../2.SDK Document Description/javadoc/Face/AdditionalData/ReadPersonAdditionalDetail.html) | Query personnel additional data details |
| [WriteFeatureCode](../2.SDK Document Description/javadoc/Face/AdditionalData/WriteFeatureCode.html) | Write facial&fingerprint features     |

### 7.Elevator Expansion

| Command object                                                    | Explain                                   |
| ------------------------------------------------------------ | -------------------------------------- |
| [ReadRelayType](../../../2.SDK Document Description/javadoc/Face/Elevator/System/RelayType/ReadRelayType.html) | Command to read the relay output type of the elevator relay board |
| [WriteRelayType](../../../2.SDK Document Descriptionv/javadoc/Face/Elevator/System/RelayType/WriteRelayType.html) | Write the relay output type to the elevator relay board       |
| [ReadReleaseTime](../../../2.SDK Document Description/javadoc/Face/Elevator/System/ReleaseTime/ReadReleaseTime.html) | Read the unlocking output duration of the relay on the elevator relay board   |
| [WriteReleaseTime](../../../2.SDK Document Description/javadoc/Face/Elevator/System/ReleaseTime/WriteReleaseTime.html) | Write the unlocking output duration of the relay on the elevator relay board   |
| [CloseRelay](../../../2.SDK Document Description/javadoc/Face/Elevator/System/Remote/CloseRelay.html) | Remote to close the door                              |
| [HoldRelay](../../../2.SDK Document Description/javadoc/Face/Elevator/System/Remote/HoldRelay.html) | Remote normally open                               |
| [LockRelay](../../../2.SDK Document Description/javadoc/Face/Elevator/System/Remote/LockRelay.html) | Remote door closed                              |
| [OpenRelay](../../../2.SDK Document Description/javadoc/Face/Elevator/System/Remote/OpenRelay.html) | Remote door opening                               |
| [UnlockRelay](../../../2.SDK Document Description/javadoc/Face/Elevator/System/Remote/UnlockRelay.html) | Remote unlocking                              |

