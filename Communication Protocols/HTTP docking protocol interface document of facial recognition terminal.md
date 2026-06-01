#FC8200H The docking protocol interface document of facial recognition terminal

- **Communication protocol based on HTTP**

- **Document version	6.0**

- *Release date: June 12, 2024**

- **Modified by Lai Jinjie**

## **Update Record**

| **Version Number** | **Modified Date** | **Explanation** |
| ---------- | ------------ | -------- |
|            |              |          |

# **HTTP Protocol Description**

- **Supports HTTP 1.1

- **GZIP compression technology**

- **Supports TLS1.2 and TLS1.3**

# **HTTP API interface **

## **Call process**

-The device send a heartbeat packet to the server first, and the server responds with an OK message
- Device sends working parameters to the service
- After the server responds to the heartbeat protection package, it decides the next request to be initiated based on the response content
- **Priority syncParameter > Remote > deletePeople > addPeople**
- After processing the operations required by the server, check for any accumulated un uploaded records
- If there are accumulated records that have not been uploaded, push the records to the server
- The device needs to ensure that the interval between two heartbeat keep alive packets does not exceed the set polling interval.
- The heartbeat interval is 15 seconds. If the first heartbeat packet is sent at 10:00:00 and there is a request to delete or add personnel in the server response, the delete or add personnel interface will be called repeatedly. However, it is necessary to ensure that another heartbeat is sent at 10:00:15.
  - **Processing parameter synchronization, responding to remote operations, deleting personnel, adding personnel, and pushing records between two heartbeat intervals.**

## **Regarding HTTP Connection Maintenance**

- The device and server should first establish a connection through a keep alive packet. After the server responds successfully, it should maintain a long HTTP connection until all operations are completed before disconnecting. If there are no operations to be performed, the connection can be immediately disconnected until the next keep alive packet
- When the server supports the HTTP2.0 protocol, priority should be given to using HTTP2.0 for requests
- One device should ensure that it only maintains one HTTP connection with the server to avoid multiple concurrent connections

## **Exception handling**

### Network abnormality

- 1.Unable to connect to the server upon startup, failed to send parameter upload request
  - Record parameter upload status as' not uploaded ', communication enters sleep mode, waiting for the next interval time
  - When the interval time is up, send heartbeat packets. If the server does not respond, continue to sleep and wait
  - After the heartbeat packet responds, check the parameter upload status. If it is not successfully sent, send it again, and then continue to respond to the subsequent operations requested by the server (refer to the calling process)
- 2.The device cannot connect to the server during operation
  - Any request should be accompanied by a timeout and response retry - it is recommended to set a timeout of 2 seconds and retry the request 3 times
  - After the request fails, enter communication sleep mode and wait for the interval between sending the keep alive packet to arrive
  - Follow up process reference question 1

### The server refused the connection

- After the device sends a punch-in record to the server API URL request, the response returned by the server
- If the success field returns a value of 401/403, it indicates no permission. The device should enter request silence and wait for the next keep alive cycle. After the keep alive cycle, send the keep alive first
- When the Status status in the heap is 401/403/400, etc., it is also necessary to wait for the next keep alive packet cycle before sending a keep alive packet to detect the server status.
- Continue executing other API calls only when the server returns an HTTP status of 200 and a Success field of 1

# Connect to keep alive

## **API Device Heartbeat Active Package**

**Brief description: **

- When the device is idle, send a keep alive packet more than [keep alive interval] seconds after the last communication with the server to confirm if there are any unfinished tasks

**Request URL: **

- http://Server IP:port**/Device/Keepalive**

**Request method: **

- POST

 **Content-Type**

- application/json

#### **Request Parameters**

| Field             | Type   | Explain                                                         |
| :--------------- | :----- | ------------------------------------------------------------ |
| SN               | string | Device SN                                                       |
| RelayStatus      | int    | Relay state<br>0- indicates COM and NC are normally closed<br>1- indicates COM and NO are normally closed     |
| KeepOpenStatus   | int    | Normally open<br>0- indicates normally closed<br>1- indicates normally open                       |
| DoorSensorStatus | int    | Door sensor status<br>0- indicates closed<br>1- indicates open                           |
| LockDoorStatus   | int    | Door locked status<br>0- indicates unlocked<br>1- indicates locked                 |
| AlarmStatus      | string | Door alarm status<br>An empty string indicates no alarm, otherwise there will be a specific alarm name<br/>fire -- fire alarm<br/>blacklist -- blacklist alarm<br>anti -- tamper alarm<br/>illegal -- unauthorized verification<br/>password -- force alarm password<br/>openTimeout -- door timeout alarm<br/>doorSensor -- door sensor alarm<br>When there are multiple alarms, use commas to separate it, such as  fire, blacklist |


**Example of Request Parameters**

```json
{
    "SN":"FC-8200H12345678", 
    "RelayStatus":	0,
    "KeepOpenStatus":	0,
    "DoorSensorStatus":	0,
    "LockDoorStatus":	0,
    "AlarmStatus":	"",
}
```

#### **Parameter description of return value **

| Parameter Name              | Type | Necessary | Description                                                         |
| ------------------- | ---- | -------- | ------------------------------------------------------------ |
| Success             | int  | Yes       | 1: Success< 401/403 indicates that the server has been connected, but subsequent connections are denied due to the SN not being registered or installed|
|                     |      |          |                                                              |
| AddPeople           | int  | No       | >0- indicates that there is a need to add personnel to the device<br>After receiving the device, a request for DevicePass/SelectPassInfo needs to be initiated<br>=0 or when there is no such field, it indicates that no processing is required |
| DeletePeople        | int  | No       | >Indicates the need to delete personnel from the device<br/>After the device receives it, it needs to initiate a request for DevicePass/selectDeleteInfo |
| SyncParameter       | int  | No       | 1- Indicates that there are parameters that need to be set to the device<br/>After the device receives them, it needs to initiate a Device/DownloadWorkSetting request |
| Remote              | int  | No       | 1- Indicates that there is a remote operation that needs to be processed.<br/>After the device receives it, it needs to initiate a device/setRestart request |
| UploadWorkParameter | int  | No       | 1- Indicates that the device is required to upload its operating parameters< After receiving the device, it needs to initiate a Device/UnplanadWorkSetting request|

**Example of return value**

```json
{
    "Success":0,
    "AddPeople":1,
    "DeletePeople":1,
    "SyncParameter":1,
    "Remote":1,
    "UploadWorkParameter": 1
}

{
    "Success":0
}
```

- **Priority UploadWorkParameter >Remote > SyncParameter> DeletePeople> AddPeople**

#### Return error code

```json
{
    "Success":401   //When returning non-zero values such as 401 and 400. If the device encounters a problem on the platform, it will no longer send clock in records and device parameters to the server. But it will still periodically send keep alive packages

}
/*
When testing the connection of the device again, prompt the text based on the return value
(1) 401 prompt
① Chinese: Connected to the server, but the device is not activated
②English：Connected to server, but device is not activated
(2) 400 prompt
① Chinese: Connected to server, but SN does not comply with platform rules
② Chinese: Connected to server, but SN does not comply with platform rules
(3) 0 prompt
① Chinese: Connected to server, testing completed
②English：Connected to server, testing completed
(4) Other prompts based on message content
*/
```

# **Device operating parameters**

### Device Basic Information SystemInfo


| Field              | Type   | Explain                            |
| :---------------- | :----- | :------------------------------ |
| DeviceSN          | string | Serial number                          |
| DeviceName        | string | Device name                        |
|                   |        |                                 |
| FirmwareVerson    | string | Firmware version                        |
| FingerprintVerson | string | Fingerprint algorithm version                    |
| FaceVerson        | string | Facial algorithm version                    |
|                   |        |                                 |
| Manufacturer      | string | Manufacturer                          |
| ManufacturerPhone | string | Contact No.                        |
| Website           | string | Website                            |
| ProductionDate    | string | Date of manufacture                        |
| OEMText           | string | OEM custom text, can be filled in with 200 characters   |
|                   |        |                                 |
| AutoRestart       | int    | Daily automatic restart function switch            |
| AutoRestartTime   | string | Daily automatic restart time, format HH: mm|

### Working status Status


| Field             | Type   | Explain                                                         |
| :--------------- | :----- | :----------------------------------------------------------- |
| RunDays          | int    | System operation days                                                 |
| FormatCount      | int    | Format times                                                   |
| WatchDogCount    | int    | Watchdog reset times                                               |
| BootTime         | int    | Startup time Unix timestamp (seconds)                                    |
| RelayStatus      | int    | Relay state<br>0- indicates COM and NC are normally closed<br>1- indicates COM and NO are normally closed     |
| KeepOpenStatus   | int    | Normally open<br>0- indicates normally closed<br>1- indicates normally open开                       |
| DoorSensorStatus | int    | Door sensor status<br>0- indicates closed<br>1- indicates open                           |
| LockDoorStatus   | int    | Door locked status<br>0- indicates unlocked<br>1- indicates locked                 |
| AlarmStatus      | string | Door alarm status<br>An empty string indicates no alarm, otherwise there will be a specific alarm name<br/>fire -- fire alarm<br/>blacklist -- blacklist alarm<br>anti -- tamper alarm<br/>illegal -- unauthorized verification<br/>password -- force alarm password<br/>openTimeout -- door timeout alarm<br/>doorSensor -- door sensor alarm<br>When there are multiple alarms, use commas to separate it, such as  fire, blacklist |


### Region and Language Language


| Field       | Type | Explain                                                         |
| :--------- | :--- | :----------------------------------------------------------- |
| Language   | int  |Language 1- Chinese< Br>2- English; 3- Traditional Chinese; 4- French; 5- Russian<br>6- Portuguese; 7- Spanish; 8- Italian language; 9- Japanese<br>10- Korean; 11- Thai language; 12- Arabic; 13- Portugal<br>14- Türkiye, 15- Indonesia, 16- Ukraine, 17- Vietnam|
| SystemTime | int  | Device time Unix timestamp (seconds)                                     |
| UseNTP     | int  | Enable NTP automatic time calibration 1- Enable; 0-- Disabled                         |
| TimeZone   | int  | Time zone of the device, value range: -12-+14                              |
| Volume     | int  | Volume size (range 0-10)                                          |
| Voice      | int  | Voice playback switch 0. No broadcast 1. Broadcast                               |

### Human Computer Interaction UI


| Field              | Type   | Explain                                       |
| :---------------- | :----- | :----------------------------------------- |
| DisplayBrightness | int    | Screen brightness setting 1-10                          |
| MenuPassword      | string | Menu password, only number, 4-8 digits or blank             |
| ShowIR            | int    | Display infrared image on the device 1- Enable; 0-- Disabled      |
| ShowPersonPhoto   | int    | Display personnel picture after recognition 1-- Enable; 0-- Disabled        |
| PlayPersonName    | int    | Identify personnel and broadcast the names 1-- Enable; 0-- Disabled        |
| RecognitionButon  | int    | Before recognition, please click the recognition button 1- Enable; 0-- Disabled    |
| UnregisteredWarn  | int    | Unregistered personnel reminder 1-- Enable; 0-- Disabled            |
| ShowPersonName    | int    | Display the personnel name after recognition? 1- Enable; 0-- Disabled   |
| FillLight         | int    | Supplement light mode: 0: Normally closed; 1: Normally open; 2: Automatic; |
| UseQRCode         | int    | QR code recognition switch 1- enabled; 0-- Disabled          |



### data storage Storage


| Field              | Type  | Explain                                                         |
| :---------------- | :---- | :----------------------------------------------------------- |
| RecordAutoCycle   | int   | Record full cycle 1- Record full cycle, 0- Record full not cycle, waiting for cleaning       |
| SaveUnregistered  | int   | Save unregistered personnel records, 0: do not store, 1: store. Unregistered personnel refer to individuals who were not registered in the system, or card numbers that were not registered in the system |
| SaveRecordPicture | int   | Save on-site image 0. do not save; 1. Save                                |
| PeopleStorageInfo | class | Personnel storage details                                                 |
| RecordStorageInfo | class | Record storage details                                                 |

- **PeopleStorageInfo**

```
```json
"Person": {"Max“: 5000, "Current":0 },//Personnel storage capacity Max maximum capacity; Current current storage quantity
"Face": {"Max“: 5000, "Current":0 },//Face storage capacity
"Card": {"Max“: 5000, "Current":0 },//Card storage capacity
"Fingerprint": {"Max“: 5000, "Current":0 },//Fingerprint storage capacity
"PalmVein": {"Max“: 5000, "Current":0 },//Palm print storage capacity
"Pasword": {"Max“: 5000, "Current":0 },//Password storage capacity
"Admin": {"Max“: 5000, "Current":0 }//Administrator storage capacity
}
```

- **RecordStorageInfo**

```json
{
"VerifyRecord": {"Max“: 5000, "Current":0 },//Storage capacity for entry and exit records
"DoorRecord": {"Max“: 5000, "Current":0 },//Storage capacity for door sensor
"SystemRecord": {"Max“: 5000, "Current":0 },//Storage capacity for system records
"RecordPhoto": {"Max“: 5000, "Current":0 }//Storage capacity for live picture
}
```

### Face Recognition 


| Field              | Type | Explain                                                         |
| :---------------- | :--- | :----------------------------------------------------------- |
| FaceIR            | int  | Liveness detection, 1 on, 0 off                                       |
| FaceIRThreshold   | int  | Liveness detection threshold 1-99                                            |
| FaceDistance      | int  | Identification distance 1- near range (0.2-0.5 meters); 2- Middle distance (0.2-1.5 meters); 3-- far distance (0.2-1.5 meters or more) |
| FaceThreshold     | int  | The face recognition threshold is 1-99. The recognition threshold more  larger, the accuracy more higher                |
| FPComparison      | int  | Fingerprint comparison threshold value range: 1-100                                |
| FaceMask          | int  | Mask detection                                                     |
| FaceMaskThreshold | int  | The mask recognition threshold is 1-99. The recognition threshold more  larger, the accuracy more higher                   |

### BodyTemperature


| Field                      | Type  | Explain                                            |
| :------------------------ | :---- | :---------------------------------------------- |
| UseBodyTemperature        | Int   | Temperature measurement mode switch. 0: Non temperature measurement mode 1: Temperature measurement mode         |
| UseFahrenheitDisplay      | int   | Turn on Fahrenheit temperature display, 1: on, 0: off                      |
| TemperatureCompensate     | float | Temperature compensation value -10.0  -- +10.0                      |
| TemperatureAlarmThreshold | float | Example of temperature alarm threshold 37.5                         |
| TemperatureDisplay        | int   | Whether to display body temperature '0' - prohibit displaying body temperature; 1-- Display body temperature info |


### NetworkServer


| Fields                          | Type   | Explain                                                         |
| :---------------------------- | :----- | :----------------------------------------------------------- |
| UseTCPClient                  | Int    | Use TCPClient to connect to server 1- Enable; 0-- Disabled;                |
| UseUDPClient                  | Int    | Use UDPClient to connect to server 1- Enable; 0-- Disabled;                |
| ServerAddress                 | string | Server Address TCP or UDP Protocol Server Address                        |
| ServerPort                    | int    | Server port number                                                 |
| KeepaliveTime                 | int    | kEEP alive package interval time 1-65535 seconds                                    |
|                               |        |                                                              |
| UseHTTPClient                 | int    | Enable HTTP Client protocol 1- Enable; 0- Disabled;                |
| HTTPClient_ServerAddr         | string | HTTP protocol server address                                          |
| HTTPClient_KeepaliveTime      | int    | Time interval for keep alive packets in HTTP protocol                                    |
| HTTPClient_UseGZIP            | int    | Use GZIP compression when making HTTP protocol requests? 0- Not used; 1- Used           |
| HTTPClient_ProtocolType       | int    | Protocol type of HTTPClient<br>100 --- HTTPv1<br>200 ---HTTPv2|
|                               |        |                                                              |
| UseMQTTClient                 | int    | Whether to start MQTTClient protocol 1- Enable; 0- Disabled;                 |
| UseMQTTSSL                    | int    | Enable MQTT SSL Secure Socket 1- Enable; 0- Disabled;               |
| MQTTServerAddr                | string | MQTT server address   www.abc.com                                 |
| MQTTPort                      | int    | MQTT server port number                                             |
| MQTTLoginName                 | string | Login username in MQTT protocol                                       |
| MQTTLoginPassword             | string | Login password in MQTT protocol                                         |
| MQTTPublishTopic              | string | Topic used by devices to send data in MQTT protocol                          |
| MQTTSubscribeTopic            | string | Topics that devices in the MQTT protocol need to subscribe to when receiving data                     |
| MQTT_KeepaliveTime            | int    | The keep alive packet interval time of MQTT protocol                                    |
| MQTT_UseGZIP                  | int    | Does MQTT use GZIP compression? 0- Not used; 1- Used                     |
|                               |        |                                                              |
| UseWebsocketClient            | int    | Whether to start Websocket Client protocol 1- Enable; 0-- Disabled;            |
| WebsocketClient_ServerAddr    | string | Websocket protocol server address<br>  ws://192.168.1.1/websocket   or  wss://192.168.1.1/websocket |
| WebsocketClient_KeepaliveTime | int    | The keep alive packet interval time of Websocket Client protocol                         |
| WebsocketClient_UseGZIP       | int    | Does Websocket use GZIP compression? 0- Not used; 1- Used                |
| WebsocketClient_ProtocolType  | int    | Protocol types of Websocket                                         |
|                               |        |                                                              |
| UseYZW                        | int    | Do you want to start the HTTPClient protocol for Yunzhu platform  1- Enable; 0-- Disabled;          |
| YZWAddr                       | string | Yunzhu platform protocol  server address                                        |

### Machine network parameters


| Field            | Type   | Explain                                         |
| :-------------- | :----- | :------------------------------------------- |
| UseWired        | int    | Wired network switch, 1: on, 0: off                   |
| WiredDHCP       | int    | Wired network automatic IP, 1: on, 0: off                    |
| WiredIP         | string | Wired network IP address ("//192.168.0.110")          |
| WiredIPMask     | string | Wired network subnet mask ("//255.255.255.0")          |
| WiredGteway     | string | Wired network gateway ("//192.168.0.1")                |
| WiredDNS        | string | Dns(“//192.168.0.1”)                         |
| WiredMAC        | string | Wired network MAC address                              |
|                 |        |                                              |
| UseWIFI         | int    | Wireless network switch, 1: on, 0: off                         |
| WIFIAPName      | string | Wireless network account                                   |
| WIFIAPPassword  | string | Wireless network password                                   |
| WIFIMAC         | string | Wireless network MAC address                             |
| WIFIDHCP        | int    | Wireless network automatic IP, 1: on, 0: off                    |
| WIFIIP          | string | Wireless network IP address ("//192.168.0.110")          |
| WIFIIPMask      | string | Wireless network subnet mask(“//255.255.255.0”)          |
| WIFIGteway      | string | Wireless network gateway(“//192.168.0.1”)                |
| WIFIDNS         | string | Wireless Network Dns(“//192.168.0.1”)                 |
|                 |        |                                              |
|                 |        |                                              |
| UseWebPage      | int    | Web page management switch, 1: on, 0: off                    |
| HTTPPort        | int    | Web management page port number, 1-65534                   |
| HTTPSPort       | int    | Web management page port number, 1-65534                   |
| WebPageUseSSL   | int    | Enable SSL on device web page. SSL certificate is self signed using OpenSSL |
|                 |        |                                              |
| UseUDP          | int    | UDP port switch, 1: on, 0: off                       |
| UDPPort         | int    | UDP port number (used by UDP protocol)                   |
| ConnectPassword | string | UDP protocol communication password 32 characters                    |
|                 |        |                                              |
| UseTelnet       | int    | Linux Telnet function switch, 1: on, 0: off             |
| TelnetPort      | int    | Telnet port number                                 |
|                 |        |                                              |
| UseRTSP         | int    | RTSP video stream, 1: on, 0: off                        |
| RTSPPort        | int    | RTSP port number                                   |
| RTSPUser        | string | RTSP username                                   |
| RTSPPassword    | string | RTSP password                                     |

###Door access parameters


| Field                    | Type   | Required | Explain                                                         |
| :---------------------- | :----- | :--- | :----------------------------------------------------------- |
| CardBytes               | int    | Yes   | Card number byte; 3. 4, 8; 0- indicates disabling card reading                           |
| AccessType              | int    | Yes   | Entry and exit type 0, entry; 1, exit                                      |
| WgFormat                | int    | Yes   | WG Output format 26 / 34/66                                        |
| WGContent               | int    | Yes   | WG Output content: 1- User ID; 2-- Card number                              |
| ReleaseTime             | int    | Yes   | Opening holding time 0-65535(s). 0 is 0.5 seconds                        |
| DelayOpenDoorTime       | int    | Yes   | Delay unlocking time 0-65535 (s). 0 is prohibition                         |
| FreeOpen                | int    | Yes   | Without verification to open door 1- enabled; 0-- Disabled                                  |
| OpenInterval            | int    | Yes   | Repeat recognition interval 0- disabled; 1-65535（ms）                         |
| OpenInterval_SaveRecord | int    | Yes   | Repeat interval record storage setting 0, do not save; 1, save                        |
| Relay                   | int    | Yes   | The relay support bistability?``1 is support,0 is not support                        |
| ShortMessage            | string | Yes   | Short messages after legal verification                                          |
| VerificationType        | int    | Yes   | Verification method< br/>1. Standard mode; 2. Face/fingerprint/palm print/card+password< br/>2. Face/fingerprint/palm print/card+password< br/>3. card+face/fingerprint/palm print/password< br/>4. Multi person attendance 5. People and ID comparison< br/>6. card+face/fingerprint/palm print+password< br/>7. card+fingerprint/palm print+face recognition< br/>8. Fingerprint/palm print+face+password< br/>9. Fingerprint+palm print+face< br/>10. Palm print+face; 11. Fingerprint+facial recognition< br/>12. Only use palm print; 13. Only use fingerprints; 14. Only use card; 15. Only use password< br/>16. People and ID comparison to open the door and auto-registered (ID card+face+auto-registered); |
| OverdueRemind           | int    | Yes   | Permission expiration prompt 1--Enable; 0--Disabled                                |
| OverdueRemind_Day       | int    | Yes   | Permission expiration prompt Validity threshold, minimum remaining valid days. If the number of days is below this threshold, it will prompt that the validity period is about to expire. The value range is 1-255. 0- indicates closed. |
|                         |        |      |                                                              |
| TimingOpen              | int    | Yes   | Timing normally open function  1--Enable；0--Disabled                               |
| TimingOpen_mode         | int    | Yes   | Timing normally open. Automatic opening mode: ``Timing normally open. Automatic opening mode: 1. After passing legal authentication, it can be normally open within a specified period of time. 2. Those marked as normally open privilege in the authorization can be normally open after passing authentication within a specified period of time. 3. Automatic switch, the door will automatically open and close when the time is up |
| TimingOpen_timegroup    | object | Yes   | Timing Normally opened. The time zone of normally opened using the weekly period structure                             |
|                         |        |      |                                                              |
| TimingLocked            | int    | Yes   | Timing locked function  1--Enabled；0--Disabled                               |
| TimingLocked_timegroup  | object | Yes   | Timing locked.The time zone of timing locked using the weekly period structure                             |
|                         |        |      |                                                              |
| VisitorRootPassword     | string | Yes   | Root password of visitor                                                    |
| MultiPerson             | int    | Yes   | Multi person combination to open the door, number of people; 1-50；                                   |

- VerificationType 

  ```
  ```
  1. Standard mode default value
  2. Face/fingerprint/palm print/card+password
  3. Card+face/fingerprint/palm print/password
  4. Multi person attendance
  5. People and ID Comparison
  6. Card+face/fingerprint/palm print+password
  7. Card+fingerprint/palm print+face recognition
  8. Fingerprint/palm print+face+password
  9. Fingerprint+palm print+face
  10. Palm print+face
  11. Fingerprint+Face
  12. Only palm print
  13. Only fingerprint
  14. Only card
  15. Only password
  16. People and ID comparison to open door + auto registered（identification +face +auto registered）
  ```

- **Weekly time zone format**

  ```json
  {
  
  		Week1:"00:00-23:59", //Monday
  		Week2:"00:00-23:59",
  		Week3:"00:00-23:59",
  		Week4:"00:00-23:59",
  		Week5:"00:00-23:59",
  		week6:"00:00-23:59",
  		week7:"00:00-23:59" //Sunday
  }
  ```

  
    - Week1 is Monday
  - Week2 is Tuesday
  - Week3 is Wednesday
  - Week4 is Thursday
  - Week5 is Friday
  - Week6 is Saturday
  - Week7 is Sunday
  - Each week field represents the time one setting for a day
  - You can set up eight sub time zone in a day with the format of start time-end time/start time-end time/.....


  ```
  "01:00-01:59/02:00-02:59/03:00-03:59/04:00-04:59/05:00-05:59/06:00-06:59/07:00-07:59/08:00-08:59"
  The above string defines 8 sub time zone in a day. A maximum of 8 sub time zone can be defined per day
  "01:00-01:59/02:00--02:59"
  The above string defines 2 sub time zone in a day, and the other 6 time zone are empty and invalid.
  ```

  - If the day of the week is empty and no time zone is set, this field can be omitted

  ```json
  {
  		//	At this time, only week 1 and week 7 are defined, and other time zone are empty
  		Week1:"01:00-02:00",
  		Week7:"03:00-04:00"
  }
  ```

  

### Parameters of elevator function


| Field          | Type        | Required | Explain                                   |
| :------------ | :---------- | :--- | :------------------------------------- |
| UseElevator   | int         | Yes   | Elevator function switch, 1: on, 0: off                 |
| ElevatorPorts | [] Object array | Yes   | Elevator Port Object array defines elevator port list |


Elevator Port Object


| Field        | Type | Required | Explain                                                         |
| :---------- | :--- | :--- | :----------------------------------------------------------- |
| Num         | int  | Yes   | Elevator port number 1-64                                              |
| RelayType   | int  | Yes   | Elevator relay (COM&NO normally closed, COM&NC normally closed) `<br>` Value range: 1. COM&NC normally closed (default value); 2. COM&NO normally closed |
| ReleaseTime | int  | Yes   | The maximum output duration during unlocking is 65535 seconds. 0 represents 0.5 seconds                      |
| TimingOpen  | obj  | Yes   | Timing normally open function structure                                             |

- timingOpen Timing normally open function structure

| Field      | Type | Required | Explain                                                         |
| :-------- | :--- | :--- | :----------------------------------------------------------- |
| Use       | int  | Yes   | Function switch,0--Disabled；1--Enabled                                    |
| Open      | int  | Yes   | Automatic opening mode: 1. After passing the legal authentication, the door can be normally opened within a specified period of time. 2. Those marked as normally open privilege in the authorization can be normally opened after passing the authentication within the specified period of time. 3. Automatic switch, the door will automatically open and close when the time is up |
| Timegroup | obj  | Yes   | Normally open time zone, using weekly time zone structure                                      |

### Alarm parameters 


| Field                          | Type   | Required | Explain                                                         |
| :---------------------------- | :----- | :--- | :----------------------------------------------------------- |
| FireAlarm                     | int    | Yes   | Fire alarm,0,Diabled；1,Enabled                                      |
|                               |        |      |                                                              |
| DoorLongOpenAlarm             | int    | Yes   | Opening timeout alarm switch,1:Enabled,0:Diabled                                   |
| DoorLongOpenTime              | int    | Yes   | Opening timeout, if the door is opened for more than this time, an alarm will be triggered	1-65535（s）        |
|                               |        |      |                                                              |
| DoorSensorAlarm               | int    | Yes   | Door sensor alarm,0,Diabled；1,Enabled                                      |
| DoorSensorAlarmTimegroup      | class  | Yes   | Door sensor alarm and non alarm time zone, weekly time zone format                                |
|                               |        |      |                                                              |
| BlacklistAlarm                | int    | Yes   | Blacklist alarm,0,Diabled；1,Enabled                                    |
|                               |        |      |                                                              |
| AntiDisassemblyAlarm          | int    | Yes   | Tamper alarm function switch,0,Diabled；1,Enabled                              |
|                               |        |      |                                                              |
| IllegalVerificationAlarm      | int    | Yes   | Illegal verification alarm function,0,Diabled；1,Enabled                              |
| IllegalVerificationAlarmLimit | int    | Yes   | Illegal verification alarm function-Number of illegal authentication attempts,1-255                          |
|                               |        |      |                                                              |
| UseUserCloseAlarm             | int    | Yes   | Allow users verification to remove the alarm  switch,0,Diabled；1,Enabled                    |
|                               |        |      |                                                              |
| PasswordAlarm                 | int    | Yes   | Forced alarm password function,0,Diabled；1,Enabled                              |
| PasswordAlarm_Password        | string | Yes   | Forced alarm password, entering this password will trigger an alarm. Passwords only support numbers and can contain 0. |
| PasswordAlarm_Mode            | string | Yes   | The working mode when the forced alarm occurs: 1- Do not open the door, alarm output: 2- Open the door, alarm output: Lock the door, alarm, only can be unlocked by software`` |



### Device opening time zone Timegroup

| Filed       | Type        | Required | Explain         |
| :--------- | :---------- | :--- | :----------- |
| TimeGroups | [] Object array | Yes   | Device opening time zone |


**Format description of device opening time zone**

```json
//Parameter
TimeGroups:[
	{
		Num:1,
		Week1:"00:00-23:59", //Monday
		Week2:"00:00-23:59",
		Week3:"00:00-23:59",
		Week4:"00:00-23:59",
		Week5:"00:00-23:59",
		Week6:"00:00-23:59",
		Week7:"00:00-23:59" //Sunday
	},
	{
		Num:2,
		Week1:"01:00-01:59/02:00-02:59/01:00-01:59/02:00-02:59",
		..
		Week7:"00:00-00:00"
	},
	......
	{
		Num:64,
		Week1:"00:00-00:00",
		..
		Week7:"00:00-00:00"
	}
]
```

-The maximum num of the opening time zone of device is 64, indicating that the device has a maximum of 64 sets of opening time zone

- week1 Indicate Monday

- week2 Indicate Tuesday

- week3 Indicate Wednesday

- week4 Indicate Thursday

- week5 Indicate Friday

- week5 Indicate Saturday

- week7 Indicate Sunday

-Each week field represents the time zone setting for a day

-You can set up 8 sub time zone in a day with the format of start time end time/start time-end time/.....

  ```
  "01:00-01:59/02:00-02:59/03:00-03:59/04:00-04:59/05:00-05:59/06:00-06:59/07:00-07:59/08:00-08:59"
  The above string defines 8 sub time zone in a day. A maximum of 8 sub time zone can be defined per day
  "01:00-01:59/02:00-02:59"
  The above string defines 2 sub time zone in a day, and the other 6 time zone are empty and invalid.
  ```

- If the day of the week is empty and no time zone is set, this field can be omitted

  ```
  {
  		Num:64,  //At this time, only week 1 and week 7 are defined, and other time periods are empty
  		Week1:"01:00-02:00",
  		Week7:"03:00-04:00"
  	}
  ```

### Holiday of device


| Field     | Type | Required | Explain   |
| :------- | :--- | :--- | :----- |
| Holidays | []   | Yes   | Holiday |


```json
[
 {"Num":1,"Date":"2020-10-01","Type":1,"Cycle":1},
 {"Num":2,"Date":"2020-10-02","Type":2,"Cycle":0},
...
]
```


Use object array format during holidays, with each object containing two fields, num and date.
The device currently supports 360 sets of holidays
On holidays, it is prohibited to open doors (permission can be set for holiday passage)

*Field Description of *Holiday Object**


| Field  | Type   | Required | Explain                                                         |
| :---- | :----- | :--- | :----------------------------------------------------------- |
| Num   | int    | Yes   | Number of holidays, used when binding personnel permissions                              |
| Date  | string | Yes   | Holiday Date Year-Month-Day Example: 2020-10-01                         |
| Type  | int    | No   | Holiday control range,1--all day；2--Morning 00:00-12:00;  3--Afternoon(12:00-23:59),default value is 1; |
| Cycle | int    | No   | Cycle annually?,1--cycle annually；0--Non cycle; The default value is 0;             |




### **AlarmClock** 

#### **Up to 24 alarms **




| Field        | Type | Required | Description |
| :---------- | :--- | :--- | :--- |
| AlarmClocks | []   | Yes   | Alarm Clock |


```json
[
 {"Num":1,"Clock":"12:00","Times":10},
 {"Num":2,"Clock":"13:00","Times":10},
 {"Num":3,"Clock":"14:00","Times":10},
...
]
```

The alarm uses an object array format, each object containing three fields, Num, Clock, Times.
The device currently supports 24 sets of alarms

**Alarm Object Field Description**


| Field  | Type | Required | Decription                                         |
| :---- | :--- | :--- | :------------------------------------------- |
| Num   | int  | Yes   | The serial number of the alarm clock, with a value range of 1-24                     |
| Clock | int  | Yes   | Alarm time HHmm format example: 1230 represents 12:30 pm |
| Times | int  | No   | Alarm duration, with a value range of 0-255 in seconds         |


### Device Function List FunctionList

```json
{
    "FunctionList" :{
        //Temperature detection
        "BodyTemperature": true,
        //Fingerprint
        "Fingerprint": true,
        //Palm Print
        "Palmvein": true,
        //Face Recognition
        "Face": true,
        //QR Code
        "QRCode": true,
        //Mask Detection
        "FaceMask": true,
        //Helmet Detection
        "SafetyHelmet": true,
        //Elevator
        "Lift": true,
        //Alarm Clock
        "AlarmClock": true,
        //excel Export and Import
        "ExcelFile": true,
        //zip Import
        "ZipFile": true,
        //Quantity of time zone
        "TimeGreoup": true,
        //Wireless network
        "WIFI": true,
        //HTTPClient v1
        "HTTPClient_V1": true,
        //HTTPClient v2
        "HTTPClient_V2": true,
        //MQTT
        "MQTT": true,
        //Cloud Building Network Platform
        "YZW": true,
        //Websocket V1
        "Websocket_V1": true,
        //Websocket V2
        "Websocket_V2": true
    }
}

```



## **API-Upload current device parameters**

**Brief description: **

- Send once when the device is turned on
- When the device properties change, they will be uploaded again

**Request URL: **

- http://Server IP:port**/Device/UploadWorkSetting**

**Request method:**

- POST

 **Content-Type**

- application/json

**Request parameters:**

-  **Example of Request Parameters**

```json
{
	   "DeviceSN": "FC-8200H12345675",
        "FireAlarm": 0,
        "DoorLongOpenAlarm": 0,
        "DoorLongOpenTime": 0,
        "DoorSensorAlarm": 0,
        "DoorSensorAlarmTimegroup": {
            "Week1": "",
            "Week2": "",
            "Week3": "",
            "Week4": "",
            "Week5": "",
            "Week6": "",
            "Week7": ""
        },
        "BlacklistAlarm": 1,
        "AntiDisassemblyAlarm": 1,
        "IllegalVerificationAlarm": 0,
        "IllegalVerificationAlarmLimit": 30,
        "UseUserCloseAlarm": 1,
        "PasswordAlarm": 0,
        "PasswordAlarm_Password": "",
        "PasswordAlarm_Mode": 1,
        "AlarmClocks": [
            {
                "Num": 1,
                "Clock": "00:00",
                "Times": 0
            },
            {
                "Num": 24,
                "Clock": "00:00",
                "Times": 0
            }
        ],
        "UseBodyTemperature": 1,
        "UseFahrenheitDisplay": 1,
        "TemperatureCompensate": 0,
        "TemperatureAlarmThreshold": 37.29999,
        "TemperatureDisplay": 1,
        "CardBytes": 3,
        "AccessType": 0,
        "WgFormat": 26,
        "WGContent": 1,
        "ReleaseTime": 3,
        "FreeOpen": 0,
        "OpenInterval": 2,
        "OpenInterval_SaveRecord": 0,
        "Relay": 0,
        "ShortMessage": "",
        "VerificationType": 1,
        "OverdueRemind": 1,
        "OverdueRemind_Day": 3,
        "TimingOpen": 0,
        "TimingOpen_mode": 1,
        "TimingOpen_timegroup": {
            "Week1": "",
            "Week2": "",
            "Week3": "",
            "Week4": "",
            "Week5": "",
            "Week6": "",
            "Week7": ""
        },
        "TimingLocked": 0,
        "TimingLocked_timegroup": {
            "Week1": "",
            "Week2": "",
            "Week3": "",
            "Week4": "",
            "Week5": "",
            "Week6": "",
            "Week7": ""
        },
        "VisitorRootPassword": "",
        "MultiPerson": 1,
        "UseElevator": 0,
        "ElevatorPorts": [
            {
                "Num": 1,
                "RelayType": 2,
                "ReleaseTime": 2,
                "TimingOpen": {
                    "Use": 0,
                    "Open": 3,
                    "Timegroup": {
                        "Week1": "",
                        "Week2": "",
                        "Week3": "",
                        "Week4": "",
                        "Week5": "",
                        "Week6": "",
                        "Week7": ""
                    }
                }
            },
            {
                "Num": 64,
                "RelayType": 2,
                "ReleaseTime": 2,
                "TimingOpen": {
                    "Use": 0,
                    "Open": 3,
                    "Timegroup": {
                        "Week1": "",
                        "Week7": ""
                    }
                }
            }
        ],
        "FaceIR": 1,
        "FaceIRThreshold": 5,
        "FaceDistance": 3,
        "FaceThreshold": 58,
        "FPComparison": 80,
        "FaceMask": 0,
        "FaceMaskThreshold": 65,
        "Holidays": [],
        "Language": 1,
        "SystemTime": 1718421632,
        "UseNTP": 1,
        "TimeZone": 8,
        "Volume": 6,
        "Voice": 1,
        "ConnectPassword": "",
        "UseWired": 1,
        "WiredDHCP": 0,
        "WiredIP": "192.168.1.103",
        "WiredIPMask": "255.255.255.0",
        "WiredGteway": "192.168.1.1",
        "WiredDNS": "192.168.1.1",
        "WiredMAC": "7E-87-16-C1-E6-51",
        "UseWIFI": 0,
        "WIFIDHCP": 0,
        "WIFIIP": "192.168.1.150",
        "WIFIIPMask": "255.255.255.0",
        "WIFIGteway": "192.168.1.1",
        "WIFIDNS": "192.168.1.1",
        "WIFIMAC": "34-7D-E4-2D-63-3B",
        "WIFIAPName": "",
        "WIFIAPPassword": "",
        "UseWebPage": 1,
        "HTTPPort": 80,
        "HTTPSPort": 443,
        "WebPageUseSSL": 1,
        "UseUDP": 1,
        "UDPPort": 8101,
        "UseTelnet": 1,
        "TelnetPort": 23,
        "UseRTSP": 1,
        "RTSPPort": 554,
        "RTSPUser": "admin",
        "RTSPPassword": "12345678",
        "UseTCPClient": 0,
        "UseUDPClient": 0,
        "ServerAddress": "47.92.31.75",
        "ServerPort": 9003,
        "KeepaliveTime": 30,
        "UseHTTPClient": 0,
        "HTTPClient_ProtocolType": 100,
        "HTTPClient_ServerAddr": "http://192.168.1.100",
        "HTTPClient_KeepaliveTime": 30,
        "HTTPClient_UseGZIP": 0,
        "UseMQTTClient": 0,
        "UseMQTTSSL": 0,
        "MQTTServerAddr": "192.168.1.100",
        "MQTTPort": 0,
        "MQTTLoginName": "",
        "MQTTLoginPassword": "",
        "MQTTPublishTopic": "",
        "MQTTSubscribeTopic": "",
        "MQTT_KeepaliveTime": 30,
        "MQTT_UseGZIP": 0,
        "UseWebsocketClient": 0,
        "WebsocketClient_ProtocolType": 100,
        "WebsocketClient_ServerAddr": "ws://192.168.1.100/ws",
        "WebsocketClient_UseGZIP": 0,
        "WebsocketClient_KeepaliveTime": 30,
        "UseYZW": 0,
        "YZWAddr": "http://192.168.1.10",
        "RunDays": 0,
        "FormatCount": 0,
        "WatchDogCount": 7,
        "BootTime": 1718418194,
        "RelayStatus": 0,
        "KeepOpenStatus": 0,
        "DoorSensorStatus": 0,
        "LockDoorStatus": 0,
        "AlarmStatus": "\"\"",
        "RecordAutoCycle": 0,
        "SaveUnregistered": 1,
        "SaveRecordPicture": 1,
        "PeopleStorageInfo": {
            "Person": {
                "Max": 20000,
                "Current": 1
            },
            "Face": {
                "Max": 20000,
                "Current": 1
            },
            "Card": {
                "Max": 20000,
                "Current": 0
            },
            "Fingerprint": {
                "Max": 0,
                "Current": 0
            },
            "PalmVein": {
                "Max": 10000,
                "Current": 0
            },
            "Pasword": {
                "Max": 20000,
                "Current": 0
            },
            "Admin": {
                "Max": 5,
                "Current": 0
            }
        },
        "RecordStorageInfo": {
            "VerifyRecord": {
                "Max": 1000000,
                "Current": 4
            },
            "DoorRecord": {
                "Max": 10000,
                "Current": 4
            },
            "SystemRecord": {
                "Max": 10000,
                "Current": 7
            },
            "RecordPhoto": {
                "Max": 10000,
                "Current": 4
            }
        },
        "DeviceSN": "FC-8190H24052799",
        "DeviceName": "",
        "FirmwareVerson": "8.46",
        "FingerprintVerson": "-",
        "FaceVerson": "6.01",
        "Manufacturer": "",
        "ManufacturerPhone": "",
        "Website": "",
        "ProductionDate": "2024-06-15",
        "OEMText": "",
        "AutoRestart": 0,
        "AutoRestartTime": "02:00",
        "TimeGroups": [
            {
                "Num": 1,
                "Week1": "00:00-23:59",
                "Week2": "00:00-23:59",
                "Week3": "00:00-23:59",
                "Week4": "00:00-23:59",
                "Week5": "00:00-23:59",
                "Week6": "00:00-23:59",
                "Week7": "00:00-23:59"
            },
            {
                "Num": 64,
                "Week1": "",
                "Week7": ""
            }
        ],
        "DisplayBrightness": 6,
        "MenuPassword": "0000",
        "ShowIR": 0,
        "ShowPersonPhoto": 1,
        "PlayPersonName": 1,
        "RecognitionButon": 0,
        "UnregisteredWarn": 0,
        "ShowPersonName": 1,
        "FillLight": 2,
		"FunctionList":	{
			"BodyTemperature":	true,
			"Fingerprint":	false,
			"Palmvein":	true,
			"Face":	true,
			"QRCode":	true,
			"FaceMask":	true,
			"SafetyHelmet":	false,
			"Lift":	true,
			"AlarmClock":	true,
			"ExcelFile":	false,
			"ZipFile":	false,
			"TimeGreoup":	64,
			"WIFI":	true,
			"HTTPClient_V1":	true,
			"HTTPClient_V2":	true,
			"MQTT":	true,
			"YZW":	true,
			"Websocket_V1":	true,
			"Websocket_V2":	true
		}
}
```

**Return parameter description**

| Parameter Name  | Type   | Nnecessary | Description                                                   |
| ------- | ------ | -------- | ------------------------------------------------------ |
| Success | int    | Yes       | 1 indicates successful operation;<br>!= 1, it is necessary to indicate the error description in the Message |
| Message | string | No       | Error message description                                           |

 **Return value example**

```
{
    "Success":1
}
```

## **API-The device actively obtains working parameters**

**Brief description:**

- When the device sends a heartbeat packet and the SyncParameter field in the server's response packet is set to 1, the device will request this interface

**Request URL：**

- http://Server IP:port**/Device/DownloadWorkSetting **

**Request method: **

- POST

 **Content-Type**

- application/json
**Request parameter description**

| Parameter Name | Type   | Nnecessary | Description    
| ------ | ------ | -------- | ------- |
| SN     | string | Yes       | Device ID |


 **Example of Request Parameters**

```
{
    "SN":"FC-8200H12345678"
}
```

**Return parameter description**


| Field           | Type   | Required | Explain                                                       |
| :------------- | :----- | :--- | :--------------------------------------------------------- |
| Success        | int    | Yes   | 1-- indicates success<br>Other are error codes, and the error content needs to be indicated in the Message |
| Message        | string | No   | Error message description, please refer to Chapter 3 for details                                |
| All modifiable parameters |        |      |                                                            |

**Return value example**

```json
{
       "Success": 0,
	   "DeviceSN": "FC-8200H12345675",
        "FireAlarm": 0,
        "DoorLongOpenAlarm": 0,
        "DoorLongOpenTime": 0,
        "DoorSensorAlarm": 0,
        "DoorSensorAlarmTimegroup": {
            "Week1": "",
            "Week2": "",
            "Week3": "",
            "Week4": "",
            "Week5": "",
            "Week6": "",
            "Week7": ""
        },
        "BlacklistAlarm": 1,
        "AntiDisassemblyAlarm": 1,
        "IllegalVerificationAlarm": 0,
        "IllegalVerificationAlarmLimit": 30,
        "UseUserCloseAlarm": 1,
        "PasswordAlarm": 0,
        "PasswordAlarm_Password": "",
        "PasswordAlarm_Mode": 1,
        "AlarmClocks": [
            {
                "Num": 1,
                "Clock": "00:00",
                "Times": 0
            },
            {
                "Num": 24,
                "Clock": "00:00",
                "Times": 0
            }
        ],
        "UseBodyTemperature": 1,
        "UseFahrenheitDisplay": 1,
        "TemperatureCompensate": 0,
        "TemperatureAlarmThreshold": 37.29999,
        "TemperatureDisplay": 1,
        "CardBytes": 3,
        "AccessType": 0,
        "WgFormat": 26,
        "WGContent": 1,
        "ReleaseTime": 3,
        "FreeOpen": 0,
        "OpenInterval": 2,
        "OpenInterval_SaveRecord": 0,
        "Relay": 0,
        "ShortMessage": "",
        "VerificationType": 1,
        "OverdueRemind": 1,
        "OverdueRemind_Day": 3,
        "TimingOpen": 0,
        "TimingOpen_mode": 1,
        "TimingOpen_timegroup": {
            "Week1": "",
            "Week2": "",
            "Week3": "",
            "Week4": "",
            "Week5": "",
            "Week6": "",
            "Week7": ""
        },
        "TimingLocked": 0,
        "TimingLocked_timegroup": {
            "Week1": "",
            "Week2": "",
            "Week3": "",
            "Week4": "",
            "Week5": "",
            "Week6": "",
            "Week7": ""
        },
        "VisitorRootPassword": "",
        "MultiPerson": 1,
        "UseElevator": 0,
        "ElevatorPorts": [
            {
                "Num": 1,
                "RelayType": 2,
                "ReleaseTime": 2,
                "TimingOpen": {
                    "Use": 0,
                    "Open": 3,
                    "Timegroup": {
                        "Week1": "",
                        "Week2": "",
                        "Week3": "",
                        "Week4": "",
                        "Week5": "",
                        "Week6": "",
                        "Week7": ""
                    }
                }
            },
            {
                "Num": 64,
                "RelayType": 2,
                "ReleaseTime": 2,
                "TimingOpen": {
                    "Use": 0,
                    "Open": 3,
                    "Timegroup": {
                        "Week1": "",
                        "Week7": ""
                    }
                }
            }
        ],
        "FaceIR": 1,
        "FaceIRThreshold": 5,
        "FaceDistance": 3,
        "FaceThreshold": 58,
        "FPComparison": 80,
        "FaceMask": 0,
        "FaceMaskThreshold": 65,
        "Holidays": [],
        "Language": 1,
        "SystemTime": 1718421632,
        "UseNTP": 1,
        "TimeZone": 8,
        "Volume": 6,
        "Voice": 1,
        "ConnectPassword": "",
        "UseWired": 1,
        "WiredDHCP": 0,
        "WiredIP": "192.168.1.103",
        "WiredIPMask": "255.255.255.0",
        "WiredGteway": "192.168.1.1",
        "WiredDNS": "192.168.1.1",
        "WiredMAC": "7E-87-16-C1-E6-51",
        "UseWIFI": 0,
        "WIFIDHCP": 0,
        "WIFIIP": "192.168.1.150",
        "WIFIIPMask": "255.255.255.0",
        "WIFIGteway": "192.168.1.1",
        "WIFIDNS": "192.168.1.1",
        "WIFIMAC": "34-7D-E4-2D-63-3B",
        "WIFIAPName": "",
        "WIFIAPPassword": "",
        "UseWebPage": 1,
        "HTTPPort": 80,
        "HTTPSPort": 443,
        "WebPageUseSSL": 1,
        "UseUDP": 1,
        "UDPPort": 8101,
        "UseTelnet": 1,
        "TelnetPort": 23,
        "UseRTSP": 1,
        "RTSPPort": 554,
        "RTSPUser": "admin",
        "RTSPPassword": "12345678",
        "UseTCPClient": 0,
        "UseUDPClient": 0,
        "ServerAddress": "47.92.31.75",
        "ServerPort": 9003,
        "KeepaliveTime": 30,
        "UseHTTPClient": 0,
        "HTTPClient_ProtocolType": 100,
        "HTTPClient_ServerAddr": "http://192.168.1.100",
        "HTTPClient_KeepaliveTime": 30,
        "HTTPClient_UseGZIP": 0,
        "UseMQTTClient": 0,
        "UseMQTTSSL": 0,
        "MQTTServerAddr": "192.168.1.100",
        "MQTTPort": 0,
        "MQTTLoginName": "",
        "MQTTLoginPassword": "",
        "MQTTPublishTopic": "",
        "MQTTSubscribeTopic": "",
        "MQTT_KeepaliveTime": 30,
        "MQTT_UseGZIP": 0,
        "UseWebsocketClient": 0,
        "WebsocketClient_ProtocolType": 100,
        "WebsocketClient_ServerAddr": "ws://192.168.1.100/ws",
        "WebsocketClient_UseGZIP": 0,
        "WebsocketClient_KeepaliveTime": 30,
        "UseYZW": 0,
        "YZWAddr": "http://192.168.1.10",
        "RecordAutoCycle": 0,
        "SaveUnregistered": 1,
        "SaveRecordPicture": 1,
        "DeviceName": "",
        "Manufacturer": "",
        "ManufacturerPhone": "",
        "Website": "",
        "ProductionDate": "2024-06-15",
        "OEMText": "",
        "AutoRestart": 0,
        "AutoRestartTime": "02:00",
        "TimeGroups": [
            {
                "Num": 1,
                "Week1": "00:00-23:59",
                "Week2": "00:00-23:59",
                "Week3": "00:00-23:59",
                "Week4": "00:00-23:59",
                "Week5": "00:00-23:59",
                "Week6": "00:00-23:59",
                "Week7": "00:00-23:59"
            },
            {
                "Num": 64,
                "Week1": "",
                "Week7": ""
            }
        ],
        "DisplayBrightness": 6,
        "MenuPassword": "0000",
        "ShowIR": 0,
        "ShowPersonPhoto": 1,
        "PlayPersonName": 1,
        "RecognitionButon": 0,
        "UnregisteredWarn": 0,
        "ShowPersonName": 1,
        "FillLight": 2
}
```



# Remote control

## **API-Remote operation command**

**Brief description: **

- When the Remote value in the heartbeat packet return is 1, the device immediately requests this interface to obtain remote operation commands

**Request URL:**

- http://Server IP:port**/Device/RemoteCommand**

**Request method: **

- POST

 **Content-Type**

- application/json

**Request parameters: **


| Field | Type   | Length | Required | Explain   |
| :--- | :----- | :--- | :--- | ------ |
| SN   | string | 30   | Yes   | Device ID |


#### **Example of Request Parameters**

```json
{
    "SN":"FC-8200H12345678"
}
```

#### **Return value parameter description**


| Parameter Name        | Type   | Necessary | Description                                                         |
| ------------- | ------ | -------- | ------------------------------------------------------------ |
| Success       | int    | Yes       | 1-- indicates success<br>Other are error codes, and the error content needs to be indicated in the Message; 0: Success |
| Message       | string | No       | Error message description, please refer to Chapter 3 for details                                    |
| Restart       | int    | No       | Remote restart:``0:no restart，1:restart                                   |
| Recover       | int    | No       | Factory reset``0:Normal，1:Factory reset                            |
| Opendoor      | int    | No       | Remote door opening command`` 0:Not processed, `<br>`1--Turn on relay;  2--Keep the door open; 3--Close the door (release normally open);`<br>`4--Lock the door;5--Unlock the door |
| Closealarm    | int    | No       | Close alarm command``0:Not processed, 1:Close all current alarms and record them     |
| RepostRecord  | int    | No       | Re-upload records``0:Not processed, 1:Mark all uploaded records as not uploaded and resend them |
| PushAllPeople | int    | No       | Request to upload all stored personnel lists to the server<br/>At this point, the device calls the API [/People/PushPeople] to send the personnel list<br/>--Not processed, 1--All personnel need to be uploaded. |
| QueryPeople   | [uint] | No       | Request to upload personnel with the specified user ID to the server. At this point, the device calls the API [/People/PushPeople] to send a list of personnel. The type is an array |
| ClearRecord   | int    | No       | Delete all records;  0-- Not processed  1--Delete all records                   |

**Return value example**

```json
//Remote door opening
{
    "Success":0,
    "Opendoor":1
}

//Query designated personnel
{
    "Success":0,
    "QueryPeople":[1,2,3,4,5,6]
}


```







# Personnel management

## **API-Obtain Personnel Authorization Information**

**Brief description:**

-- When the heartbeat is kept alive and the server return value with a field for obtaining personnel and this value is 1, the device immediately initiates this request and repeats the request

- Stop request condition: 1. The server returns Success:1, but the personnel list is empty; 2. Success==0

  

**Request URL：**

- http://Server IP:port**/People/DownloadPeopleList**

**Request Method: **

- POST

 **Content-Type**

- application/json

#### **Request parameters**

| Field  | Type   | Length | Required | Description                                                         |
| :---- | :----- | :--- | :--- | ------------------------------------------------------------ |
| SN    | string | 30   | Yes   | Device ID                                                       |
| Limit | int    | 10   | Yes   | The maximum number of personnel returned on each request is 1000, and the device sets this value based on its own processing capacity |

#### **Example of Request Parameters**

```json
{
	"SN":"FC-8200H12345678", 
    "Limit": 100
}
```

#### **Return value parameter description**


| Parameter Name      | Type   | Necessary | Description                                                         |
| ----------- | ------ | -------- | ------------------------------------------------------------ |
| Success     | int    | Yes       | 1--indicates success<br>Other are error codes, and the error content needs to be indicated in the Message; 0: Success |
| Message     | string | No       | Error message description, please refer to Chapter 3 for details                                    |
| PeopleCount | int    | Yes       | The number of personnel returned this time                                           |
| PeopleList  | Array   | Yes       | An array containing personnel permission information structure                                   |

#### **PeopleJson Personnel data format**


| Parameter Name         | Type   | Neccessary | Description                                                         |
| :------------- | :----- | :------- | :----------------------------------------------------------- |
| UserID         | string | Yes       | User ID (maximum number 4294967295 type UINT32)                |
| Name           | string | No       | Personnel name (characters<64 bits)                                        |
| Job            | string | No       | Position (characters<64 bits)                                            |
| Department     | string | No       | Department (characters<64 bits)                                            |
| IdentityCard   | string | No       | Identification card number (can be blank) (characters<64 digits)                             |
| Attachment     | string | No       | Other personnel information (characters<200)                                   |
| Photo          | string |          | Photo information, starting with http or https means using a URL address, otherwise it means using base64 |
| PhotoMD5       | string | No       | MD5 HEX string format for photos (characters=32 digits)                       |
| PhotoLen       | int    | No       | The maximum length of the photo supported is 400KB                               |
| Password       | string | No       | Password, only number, length: (0/4-8)                                |
| CardNum        | string | No       | Card number (digits, maximum value 1844674407909551615 type UINT62)      |
| QRCode         | string | No       | Personnel QR code information (characters<128)                                |
| AccessType     | int    | No       | Role 0--Ordinary personnel; 1-- Administrator; 2-- Blacklist                       |
| ExpirationDate | uint32 | No       | Permission expiration date<br>Unix timestamp in seconds     0  indicates an indefinite period  <br>Maximum indicates December 31, 2099 |
| OpenTimes      | int    | No       | Door opening times 0-65535; <br>65535-- indicates no restrictions, 0--indicates no passage allowed    |
| KeepOpen       | int    | No       | Normally opened card?, 1--Yes;0--No                                   |
| Timegroup      | int    | No       | Opening time zone group 1-64; 0- indicates restricted                               |
| Holidays       | string | No       | Holiday restrictions<br/>comma separated: 1, 2, 3, 4, 5                           |
| Elevators      | string | No       | Elevator port permission array<br/>comma separated: 1, 2, 3, 4, 5                     |
| FaceFeature    | string | No       | Facial feature codes starting with http or https indicate the use of a URL address,  otherwise they indicate the use of base64. Download the feature code from a file containing the content of base64 |
| FaceFeatureMD5 | string | No       | MD5 value HEX string format of facial feature code                              |
| Fingerprints   | []     | No       | Fingerprint object                                                     |
| Palmveins      | []     | No       | Palm print object                                                    |


**Characters refer to bytes, with one Chinese character occupying 3-4 bytes (encoded in UTF-8) and one English character occupying 1 byte**

##### **Holidays Holidays**

- An empty string or no field indicates no holiday restrictions
- For specific restricted holiday numbers, comma separated: 1, 2, 3, 4, 5
- *Number indicates that it is subject to all holiday restrictions



- **Fingerprint Fingerprint Object**

| Serial Number | Type   | Required | Description                                                         |
| ---- | ------ | ---- | ------------------------------------------------------------ |
| Num  | int    | Yes   | Fingerprint index number                                                  |
| Data | string | Yes   | The fingerprint signature code starting with http or https indicates the use of a URL address, otherwise it indicates the use of base64. Download the signature code from a file, and the content of the downloaded file is based on base64 |
| MD5  | string | No   | MD5 value of feature code in HEX string format                                  |

~~~json
[ //Structural Example
    {
        Num: 1,
        Data: "http://abc.com/fp/1.dat",
        MD5: "abcdefg"
    },
    {
        Num: 2,
        Data: "abcdefgrtygnfhgfjhk ... jhkghjkgj==",
        MD5: "abcdefg"
    }
]
~~~




- **Palmvein palm print object**


| Serial Number | Type   | Required | Description                                                         |
| ---- | ------ | ---- | ------------------------------------------------------------ |
| Num  | int    | Yes   | Palm print index number                                                   |
| Data | string | Yes   | The palm print feature code starting with http or https indicates the use of a URL address, otherwise it indicates the use of base64. Download the feature code from a file, and the content of the downloaded file is based on base64 |
| MD5  | string | No   | MD5 value of feature code in HEX string format                                  |

  ~~~json
[ //Structural Example
    {
        Num: 1,
        Data: "http://abc.com/palm/1.dat",
        MD5: "abcdefg"
    },
    {
        Num: 2,
        Data: "abcdefgrtygnfhgfjhk ... jhkghjkgj==",
        MD5: "abcdefg"
    }
]
  ~~~


- **Elevator Elevator Authority**

```json
 //Represents this person only has access permission on the 1st-5th floor of elevator
 [
     1,2,3,4,5
 ]
 //Represents this person does not have elevator permission
 [
   
 ]
 //Represents this person only has access permission on the 10th floor of elevator
 [
     10
 ]
```



#### **Return parameter example**

```json
{
    "Success": 0,
    "Count": 1,
    "PeopleList": [
        {
            "UserID": "3",
            "Name": "888888",
            "Job": "Development",
            "Department": "Sales Department",
            "IdentityCard": "",
            "Attachment": "",
            "Photo": "http://abc.com/photo/1.jpg",
            "PhotoMD5": "613D870CA99EDF074BEE4387BAB09070",
            "PhotoLen": 55020,
            "Password": "2222",
            "CardNum": "6666",
            "AccessType": 0,
            "ExpirationDate": 0,
            "OpenTimes": 65535,
            "KeepOpen": 1,
            "Timegroup": 6,
            "Holidays": "1,3,9,10,11,17,21,25,27,30",
            "Elevators": "1,2,3,4,5",
            "FaceFeature": "http://abc.com/face/1.dat",
            "FaceFeatureMD5": "613D870CA99EDF074BEE4387BAB09070",
            "Fingerprints": [
                {
                    Num: 1,
                    Data: "http://abc.com/fp/1.dat",
                    MD5: "abcdefg"
                }
            ],
            "Palmveins": [
                {
                    Num: 1,
                    Data: "http://abc.com/palm/1.dat",
                    MD5: "abcdefg"
                }
            ]
        },
        {
            "UserID": "444",
            "Name": "444",
            ...
            "Fingerprints": [],
            "Palmveins": []
        }
    ]
}
```

## **API-Feedback to obtain personnel storage results**

**Brief description: **

- After getting a batch of people from the server to import, call this interface to return the import results

**Request URL: **

- http://Server IP:port**/People/DownloadPeopleListResult**

**Request method: **

- POST

 **Content-Type**

- application/json

**Request parameters: **


| Field         | Type   | Length | Required | Description                                    |
| :----------- | :----- | :--- | :--- | --------------------------------------- |
| SN           | string | 30   | Yes   | Device ID                                  |
| SuccessCount | int    | 20   | Yes   | Number of successful imports                          |
| FailCount    | int    | 20   | Yes   | Number of failed imports                          |
| FailList     | []     |      | Yes   | Reason code for import failure (see error code for specific description) |

**The structure of failure information in the array 'fail Employee Id'**


| Parameter Name    | Type   | Necessary | Description                                          |
| --------- | ------ | -------- | --------------------------------------------- |
| UserID    | string | Yes       | User ID (maximum number 4294967295 type UINT32) |
| ErrorCode | int    | Yes       | Error Code                                       |
| RepeatID  | string | Yes       | Duplicate user ID, this field indicates this person is repeating with which person    |
| ErrMsg    | string | No       | Error Description                                      |


**Example of Request Parameters**

```json
{
    "SN":"FC-8200H12345678", 
    "SuccessCount":16,
    "FailCount":1, 
    "FailCount":[
    	{
    		"UserID":xxx, 
    		"ErrorCode":20,
			"RepeatID":123,
             "ErrMsg" : ""
            
    	},{
    		"UserID":”xxx”, 
    		"UserID":1,
             "ErrMsg" : ""
    ]
}
```

*errorCode Personnel failure error code**


**Return value parameter description**


| Parameter Name  | Type   | Necessary | Description                                                         |
| ------- | ------ | -------- | ------------------------------------------------------------ |
| Success | int    | Yes       | 1--indicates success<br>Other are error codes, and the error content needs to be indicated in the Message; 0: Success |
| Message | string | No       | Error message description, please refer to Chapter 3 for details                                    |

**Return value example**

```
{
    "Success":0
}
```

![image-20210616205111411](image\image-20210616205111411.png)

## **API - Obtain the personnel to be deleted**

**Brief description:**

- After sending the heartbeat packet, the server calls back when there are personnel that need to be deleted, and cycle callback
- Stop cycle condition 1,  Success:1,and deleteInfo is an empty array or does not have this field  2, Success: 0


**Request URL: **

- http://Server IP:port**/People/DeletePeopleList**

**Request method: **

- POST

 **Content-Type**

- application/json

**Request parameters: **


| Field  | Type   | Length | Required | Explain                                                 |
| :---- | :----- | :--- | :--- | ---------------------------------------------------- |
| SN    | string | 30   | Yes   | Device ID                                               |
| Limit | int    | 10   | Yes   | Limit on the number of personnel which returned on each request (default 50 if not carrying, maximum 1000) |

**Example of Request Parameters**

```
{
    "SN":"FC-8200H12345678"
}
```

**Return value parameter description**

| Parameter Name      | Type   | Necessary | Description                                                         |
| ----------- | ------ | -------- | ------------------------------------------------------------ |
| Success     | int    | Yes       | 1--indicates success<br>Other are error codes, and the error content needs to be indicated in the Message; 0: Success|
| Message     | string | No       | Error message description, please refer to Chapter 3 for details                                    |
| DeleteAll   | int    | No       | 1: Clear all personnel information 0: Delete by specified user number                     |
| DeleteCount | int    | Yes       | Number of personnel to be deleted                                               |
| DeleteList  | []     | No       | When DeleteAll=0, this parameter contains the user ID that needs to be deleted                    |



**Return value example**

```json
{
    "Success":0,
    "DeleteAll":0, 
    "DeleteList":[
        1,2,3,4,5
    ]
}
```



## **API-Feedback on the operation results of deleting personnel**

**Brief description:**

- After getting a batch of people from the server to import, call this interface to return the import results

**Request URL: **

- http://Server IP:port**/People/DeletePeopleListResult**

**Request method: **

- POST

 **Content-Type**

- application/json

**Request parameters: **

| Field        | Type   | Length | Required | Explain                               |
| :---------- | :----- | :--- | :--- | ---------------------------------- |
| SN          | string | 30   | Yes   | Device ID                             |
| DeleteCount | int    | 20   | Yes   | Delete Quantity                           |
| DeleteAll   | int    | 20   | Yes   | 1 All personnel have been deleted；0 Only delete specified personnel |
| DeleteList  | []     |      | Yes   | Deleted personnel number                     |



**Example of Request Parameters**

```json
{
    "SN":"FC-8200H12345678", 
    "DeleteCount":5,
    "DeleteAll":0, 
    "DeletList":[1,2,3,4,5]
}
```

**Return value parameter description*

| Parameter Name  | Type   | Neccessary | Description                                                         |
| ------- | ------ | -------- | ------------------------------------------------------------ |
| Success | int    | Yes       | 1--indicates success<br>Other are error codes, and the error content needs to be indicated in the Message ; 0: Success |
| Message | string | No       | Error message description, please refer to Chapter 3 for details                                    |

**Return value example**

```json
{
    "Success":0
}
```

## **API-Push Personnel Information**

**Brief description: **

- When personnel are added or modified on the device, the changed information will be uploaded to the cloud platform
- When the server requests to upload specified personnel information, push the specified personnel information to the platform
- When the server requests to upload all personnel information, push all personnel information to the platform

**Request URL: **

- http://Server IP:port**/People/PushPeople**

**Request method: **

- POST

 **Content-Type**

- multipart/form-data

**Request parameters: **


| Field     | Type   | Length | Required | Explain                                                         |
| :------- | :----- | :--- | :--- | ------------------------------------------------------------ |
| SN       | string | 30   | Yes   | Device ID                                                       |
| PushType | int    | 1    | Yes   | Types of personnel changes in device<br>1--Add New；2--Update；3--Delete；4--Query； |
| Detail   | class  |      | No   | Personnel details, if personnel do not exist, this field is not available                           |
| Photo    | file   |      | No   | When there are personnel photos, photo files will be uploaded                                   |
| UserID   | uint32 |      | Yes   | Personnel ID  The personnel ID for this push                                      |



**Request parameter examples with personnel photos, fingerprint feature codes, and palm print feature codes**

```
POST /note/insertNoteFace HTTP/1.1
Accept: */*
Host: localhost:5000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Type: multipart/form-data; boundary=--------------------------506873351428002157394455
Content-Length: 17842

----------------------------506873351428002157394455
Content-Disposition: form-data; name="SN"

FC-8200H12345678
----------------------------506873351428002157394455
Content-Disposition: form-data; name="PushType"

1
----------------------------506873351428002157394455
Content-Disposition: form-data; name="Detail"
Content-Encoding: gzip
//The following content needs to be compressed with gzip before transmission. JSON formatted website https://www.json.cn/
		{
            "UserID": "3",
            "Name": "888888",
            "Job": "Development",
            "Department": "Sales Department",
            "IdentityCard": "",
            "Attachment": "",
            "Photo": "http://abc.com/photo/1.jpg",
            "PhotoMD5": "613D870CA99EDF074BEE4387BAB09070",
            "PhotoLen": 55020,
            "Password": "2222",
            "CardNum": "6666",
            "AccessType": 0,
            "ExpirationDate": 0,
            "OpenTimes": 65535,
            "KeepOpen": 1,
            "Timegroup": 6,
            "Holidays": "1,3,9,10,11,17,21,25,27,30",
            "Elevators": "1,2,3,4,5",
            "FaceFeature": "http://abc.com/face/1.dat",
            "FaceFeatureMD5": "613D870CA99EDF074BEE4387BAB09070",
            "Fingerprints": [
                {
                    Num: 1,
                    Data: "abcdefgrtygnfhgfjhk ... jhkghjkgj==",
                    MD5: "abcdefg"
                }
            ],
            "Palmveins": [
                {
                    Num: 1,
                    Data: "abcdefgrtygnfhgfjhk ... jhkghjkgj==",
                    MD5: "abcdefg"
                }
            ]
        }
----------------------------506873351428002157394455
Content-Disposition: form-data; name="Photo"; filename="Photo.jpg"
Content-Type: image/jpeg

*****jpeg file binary content*****
----------------------------506873351428002157394455--
```

**Return value parameter description**


| Parameter Name  | Type   | Neccessary | Description                                                         |
| ------- | ------ | -------- | ------------------------------------------------------------ |
| Success | int    | Yes       | 1-- indicates success<br>Other are error codes, and the error content needs to be indicated in the Message; 0: Success |
| Message | string | No       | Error message description, please refer to Chapter 3 for details                                    |

**Return value example**

```json
{
    "Success":0
}
```



# Record Management

## **API-Upload punch in Record**

**Brief description:**

- Use this interface when there are new punch-in records in the device
- This interface only supports uploading one punch-in record at a time

**Request URL: **

- http://Server IP:port**/Record/UploadIdentifyRecord**

**Request method: **

- POST

 **Content-Type**

- multipart/form-data

**Parameters: **


| Field         | Type   | Length | Required | Explain                       |
| :----------- | :----- | :--- | :--- | -------------------------- |
| SN           | string | 30   | Yes   | Device SN                     |
| RecordDetail | string | 1000 | Yes   | Record details, JSON string       |
| Photo        | file   | 50KB | No   | This parameter is only required when there are images in the record |

#### **RecordDetail Field Description**


| Field         | Type   | Length | Required | Explain                             |
| :----------- | :----- | :--- | :--- | -------------------------------- |
| RecordID     | long   | 10   | Yes   | Serial number of record                         |
| RecordType   | int    | 5    | Yes   | Event type                         |
| RecordDate   | int64  | 20   | Yes   | Punch in time Unix timestamp              |
| UserID       | string | 20   | No   | User ID                           |
| Name         | string | 30   | No   | Personnel Name                         |
| IdentityCard | string | 5    | No   | Identity card                           |
| Job          | string | 1    | No   | Position                             |
| Department   | string | 1    | No   | Department                             |
| CardNum      | string | 20   | No   | Card No.                             |
| QRCode       | string | 128  | No   | QR Code                           |
| IsEntry      | int    | 1    | No   | Is it entry? 1 indicates entry, 0 indicates exit |
| BodyTemp     | int    | 5    | No   | Human body temperature measurement  needs to be divided by 10            |
| PhotoLen     | int    | 10   | No   | Image file length  0 indicates no image      |



#### RecordType Event type


| Value   | Explain                                                |
| ---- | --------------------------------------------------- |
| 1    | Card Verification                                           |
| 2    | Fingerprint Verification                                            |
| 3    | Facial Verification                                            |
| 4    | Card + Fingerprint                                         |
| 5    | Face + Fingerprint                                         |
| 6    | Card + Face                                         |
| 7    | Card + Password                                         |
| 8    | Face + Password                                         |
| 9    | Fingerprint + Password                                         |
| 10   | Password verification   user number and password                              |
| 11   | Card + Fingerprint + Password                                  |
| 12   | Card+ Face + Password                                  |
| 13   | Fingerprint + Face + Password                                  |
| 14   | Card + Fingerprint + Face                                  |
| 15   | Repeated Verification                                            |
| 16   | Expired Validity Period                                          |
| 17   | Opening Time Zone Expired                                        |
| 18   | Cannot open the door during holidays                                    |
| 19   | Unregistered User                                          |
| 20   | Detected Locked                                            |
| 21   | The number of valid times has been exhausted                                      |
| 22   | Verification while locked, prohibit opening the door                                |
| 23   | Lost Reported Card                                              |
| 24   | Blacklist Card                                            |
| 25   | Open the door without verification -- when pressing the fingerprint, the user number is 0, and when swiping the card, the user number is the card number |
| 26   | Prohibit card swiping verification  --  When card swiping is disabled in the [Permission Authentication Method]      |
| 27   | Prohibit fingerprint verification  --  [Permission authentication method] When fingerprint is disabled      |
| 28   | Controller Expired                                        |
| 29   | Verified Passed - Validity period is ready to expire                             |
| 30   | Abnormal body temperature, refusal to enter                                  |
| 31   | Visitor password to open the door                                       |
| 32   | Scanning dynamic QR code to open the door                                  |
| 33   | Add user to the device menu                                |
| 34   | Modify user in the device menu                                |
| 35   | Delete user from the device menu                                |
| 36   | Palm Print Recognition                                         |
| 37   | Card + Palm Print+ Face                                |
| 38   | Palm Print + Password                                       |
| 39   | Card + Palm Print                                       |
| 40   | Face + Palm Print                                       |
| 41   | Card + Palm Print + Password                                |
| 42   | Palm Print + Face + Password                                |
| 43   | Fingerprint + Palm Print+ Face                                |
| 44   | Combination verification --waiting for the next person                            |
| 45   | Combination Verification Failed                                        |
| 46   | Combination Verification Successful                                        |
| 47   | People and ID Comparison                                            |
| 48   | Card Not Registered                                            |
| 49   | Unregistered QR Code                                        |

---

#### **RecordDetail Parameter examples**

```json
{
    "RecordID":	120,
    "RecordType":	3,
    "RecordDate":	1718616771,
    "UserID":	"1",
    "Name":	"1",
    "IdentityCard":	"",
    "Job":	"",
    "Department":	"",
    "CardNum":	"0",
    "QRCode":	"",
    "IsEntry":	1,
    "BodyTemp":	0,
    "PhotoLen":	40363
}
```

#### ** Request Example**

```
POST /note/insertNoteFace HTTP/1.1
Accept: */*
Host: localhost:5000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Type: multipart/form-data; boundary=--------------------------506873351428002157394455
Content-Length: 17842
----------------------------506873351428002157394455
Content-Disposition: form-data; name="SN"

 FC-8380T12345678
----------------------------506873351428002157394455
Content-Disposition: form-data; name="recordJson"
Content-Encoding: gzip
Content-Type: application/octet-stream

//When request compression is enabled, the following content needs to be compressed by gzip before transmission when uploading.
{ "RecordID":120,"RecordType":3,"RecordDate":1718616771, "UserID":"1","Name":"1","IdentityCard":"","Job":"","Department":"","CardNum":"0","QRCode":"","IsEntry":1,"BodyTemp":0,"PhotoLen":40363}
----------------------------506873351428002157394455
Content-Disposition: form-data; name="pic"; filename="Postman_file.jpg"
Content-Type: image/jpeg

*****jpeg file binary content*****
----------------------------506873351428002157394455--
```

####* **Return Example**

```json
{
	"Success": 0
}
```

#### Return error code

```json
{
    "Success":401   //Returning 401 indicates that the device is not authorized and will no longer send punch in records. The device parameters will be sent to the server. But it will still periodically send keep alive packages
}
//The device returns 401, indicating that the record was not successfully uploaded
```

#### Request content compression

- If the recordJson field in 'fromdata' is too long, you can use the content compression option and include Content Encoding: gzip in the paragraph

  ```
  Content-Disposition: form-data; name="recordJson"
  Content-Encoding: gzip
  
  ****Compressed binary content****
  ```

 **Return Parameter Description**

| Parameter Name  | Type | Necessary| Description                                                         |
| ------- | ---- | -------- | ------------------------------------------------------------ |
| Success | int  | Yes       | 1-- indicates success<br>Other are error codes, and the error content needs to be indicated in the Message; 0: Success |







## **API-Upload System Records**

**Brief description: **

- Use this interface when there are new system records in the device
- This interface will batch upload system records

**Request URL：**

- http://Server IP:port**/Record/UploadSystemRecord**

**Request method: **

- POST

 **Content-Type**

- application/json

**Parameter: **


| Field       | Type   | Length | Required | Explain     |
| :--------- | :----- | :--- | :--- | -------- |
| SN         | string | 30   | Yes   | Device SN   |
| RecordType | int    | 1    | Yes   | Record Type |
| Records    | 【】   | 1000 | Yes   | Record List |

#### **Record Field Description**


| Field       | Type  | Length | Required | Explain                               |
| :--------- | :---- | :--- | :--- | ---------------------------------- |
| RecordID   | long  | 10   | Yes   | Record number                            |
| RecordType | int   | 5    | Yes   | Event type<br>1- Door Sensor Record; 2-- System Record |
| RecordDate | int64 | 20   | Yes   | Punch in time Unix timestamp                |

#### **RecordType Field Description**


| Value   | Explain     |
| :--- | -------- |
| 1    | Door Sensor Record |
| 2    | System Record |

#### RecordType -- Door sensor record event type


| Value   | Explain                   |
| ---- | ---------------------- |
| 1    | Door Sensor-Open Door             |
| 2    | Door Sensor-Close Door             |
| 3    | Enter the door sensor alarm detection state   |
| 4    | Exit the door sensor alarm detection state   |
| 5    | The door is not closed properly               |
| 6    | Use the button to open the door           |
| 7    | When the button is pressed to open the door, the door is already locked     |
| 8    | The controller has expired when use the button to open the door |



#### RecordType -- System record event type


| Value   | Explain                               |
| ---- | ---------------------------------- |
| 1    | Software Open Door                           |
| 2    | Software Close Door                           |
| 3    | Software Normally Opened                           |
| 4    | The controller automatically enters normally open mode                 |
| 5    | The controller automatically close the door                   |
| 6    | Press and hold the exit button to  enters normally opened mode                   |
| 7    | Press and hold the exit button to  enters normally closed mode                   |
| 8    | Software Locked                           |
| 9    | Software Removed Locked                       |
| 10   | Controller timing locked--automatic locking at specified time     |
| 11   | Controller timing locked--automatic remove locking at specified time |
| 12   | Alarm--Locked                         |
| 13   | Alarm--Removed Locked                     |
| 14   | Illegal authentication Alarm                       |
| 15   | Door Sensor Alarm                           |
| 16   | Forced Alarm                           |
| 17   | Opening Time Out Alarm                       |
| 18   | Blacklist Alarm                         |
| 19   | Fire Alarm                           |
| 20   | Tamper Alarm                           |
| 21   | Illegal Authentication Alarm Removed                   |
| 22   | Door Sensor Alarm Removed                       |
| 23   | Forced Alarm Removed                       |
| 24   | Timing Opening Timeout Alarm Removed                   |
| 25   | Blacklist Alarm Removed                     |
| 26   | Fire Alarm Removed                       |
| 27   | Tamper Alarm Removed                       |
| 28   | System Powered On                           |
| 29   | System Error Reset (Watchdog)             |
| 30   | Device Formatting Record                     |
| 31   | Card Reader Reversed Connection                       |
| 32   | The card reader circuit is not properly connected                 |
| 33   | Unrecognized card reader                  |
| 34   | The network cable has been disconnected                         |
| 35   | The network cable has been inserted                         |
| 36   | WIFI Connected                        |
| 37   | WIFI Disconnected                        |
| 38   | Bluetooth Door Opening                           |
| 39   | Call the Roll Timeout                           |
| 40   | Clear all personnel from the device menu           |
| 41   | Backup personnel to USB drive in the device menu          |
| 42   | Import personnel from USB drive in the device menu         |
| 43   | Remote door opening for indoor unit                     |
| 44   | Delete all records                       |
| 45   | Delete all personnel                       |

---

#### **RecordDetail Parameter examples**

```json
 [
     {
         "RecordID":	1,
         "RecordType":	1,
         "RecordDate":	1718616771
     },
     {
         "RecordID":	2,
         "RecordType":	1,
         "RecordDate":	1718616772
     }
 ]

```

#### **Parameter examples**

~~~json
 {
     "SN":	"FC-8380T12345678",
     "RecordType":	1,
     "Records":	 [
         {
             "RecordID":	1,
             "RecordType":	1,
             "RecordDate":	1718616771
         },
         {
             "RecordID":	2,
             "RecordType":	1,
             "RecordDate":	1718616772
         }
     ]
 }
~~~







#### **Return Example**

```json
{
	"Success": 0
}
```

#### Return error code

```json
{
    "Success":401   //Returning 401 indicates that the device is not authorized and will no longer send punch in records. The device parameters will be sent to the server. But it will still periodically send keep alive packages
}
//The device returns 401, indicating that the record was not successfully uploaded this time
```


 **Return parameter description**


| Parameter Name  | Type | Necessary | description                                                         |
| ------- | ---- | -------- | ------------------------------------------------------------ |
| Success | int  | Yes       | 1- indicates success<br>Other are error codes, and the error content needs to be indicated in the Message; 0: Success |







