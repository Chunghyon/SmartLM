# Face recognition device browser UI interface document

- **Based on  HTTP  Protocol Communication**

- **Document Revision	2.0**

- **Release Date	December 9th, 2022**

- **Modifier Lai Jinjie**

## **Updated Records**

| **Version No.** | **Modify the date** | **Description* |
| :--------------- | :----------------- | :------------- |
|                  |                    |                |

# **HTTP Protocol Description**

- **Support HTTP 1.1

- **GZIP Compression Techniques**

# API Unified interface return format

### **Returns format field description**


| Field    | Type   | Length | Required | Description                    |
| :------ | :----- | :--- | :--- | :---------------------- |
| result  | bool   | 5    | Yes   | Return result status true/false |
| content | object | 1    | Yes   | Return result load          |
| errCode | int    | 4    | Yes   | Return the error code when the operation is wrong  |
| error   | string |      | No   | Error description text            |


### **Return format example**

```json
{
  "result": false, 
  "content": null,
  "errCode": 401,
  "error": "Unauthorized"
}
```

# Heartbeat

**Brief Description：**


- Heartbeat interface, used to check whether the network connection is normal.

**Request URL:**

- 示例：http://192.168.1.150/api/heartBeat

- http://Device IP:port/api/heartBeat
- Example: http://192.168.1.150/api/heartBeat

**Request Mode**

- GET



### **Return value description**

**Content-Type**
	- application/json; charset=utf-8

```json
{
    "result": true,
    "content": "OK"
}
```

# Obtain Device SN

**Brief Description:**

- Heartbeat interface, used to check whether the device network connection is normal

**Request URL：**

- http://Device IP:port/api/GetDeviceSN
- Example：http://192.168.1.150/api/GetDeviceSN

**Request Mode:**

- GET



### **Return value description**

**Content-Type**

 - application/json; charset=utf-8

```json
{
    "result": true,
    "content": "Device SN"
}
```

# User Management

## API-Login

**Breif Description**

- When you first open the device browser page, call this interface to generate a Login Token for subsequent access to the API interface.
- The Token after login will remain valid for 24 hours
- If you enter the wrong password 5 times during login, the system will prohibit you from logging in within 5 minutes
- The device can store up to 100 tokens

**Request URL:**

- http://Device IP:port/api/User/Login
- Example: http://192.168.1.150/api/User/Login

**Request Mode:**

- POST

 **Content-Type**

- application/json

### **Request parameters**

| Field     | Type | Length | Required | Description                             |
| :------- | :--- | :--- | :--- | :------------------------------- |
|          |      |      |      |                                  |
| password | int  | 1    | Yes   | The device login password <br> is also the menu management password |

**Request parameter example**

```json
{
    "password":"admin"
}
```

### **Return value description**

Returns the JWT Token for subsequent access

**Return Value Example**

```json
//Returned when verification succeeds
{
  "result": true, 
  "content": {
      "token":"eyJhbGciOiJodHRwOi8vd3", //token character string

      
      "expiration":123456789 //token expiration time
  }
}
```

```json
//Return if validation fails
{
  "result": false, 
  "errCode": 1,
  "error": "Wrong Password!"
}
```

**Wrong Code**

| Code  | Description                         |
| :---- | :--------------------------- |
| 1     | Wrong Password!                   |
| 2     | Password error too many times, locked!   |
| 10001 | Wrong method requested           |
| 10002 | Request Content-Type error    |
| 10003 | Request body, gzip decompression error  |
| 10004 | The Json format in the request body is incorrect |


## API-log out

**Brief Description:**

- When you need to log out of the system, calling the logout function will delete the corresponding Token. 


**Request URL:**


- http://Device IP:port/api/User/Logout
- Example: http://192.168.1.150/api/User/Logout

**Request Mode:**

- Get



### **Request parameters
**


- None


### **Return value description**


**Return value example**

```json
//Returned when verification succeeds
{
  "result": true, 
  "content": "ok"
}
```



## API-Verify whether the Token is invalid

**Brief Description:**

- API for checking if the token has expired after login

**Request URL：**

- http://Device IP:port/api/User/CheckLoginToken
- Example: http://192.168.1.150/api/User/CheckLoginToken

**Request Mode:**

- GET

**Verify Toekn**

- Verify

**Content-Type**

- application/json

**Authorization**

- Bearer token character string

### **Return value description**

Return Token expiration time, unix timestamp

**Return value example**

```json
//Returned when verification succeeds

{
  "result": true, 
  "content": 1234567899
}
```

```json
//Returned when verification fails
{
  "result": false, 
  "errCode": 10000,
  "error": "Token is invalid"
}
```

**Wrong Code**


| Code  | Description                     |
| :---- | :----------------------- |
| 10000 | token is expired, please log in again |
| 10001 | Wrong request method       |

|       |                          |

## API-Renew for Token

**Brief Description:**

- Renew the Token that is about to expire


**Request URL:**


- http://Device IP:port/api/User/TokenExtension
- Example: http://192.168.1.150/api/User/TokenExtension

**Request Mode:**

- GET

**Verify Toekn**

- Verify

**Content-Type**

- application/json

**Authorization**

- Bearer token character string

### **Return value description**

Return Token expiration time, unix timestamp

****Return value example****

```json
//Returned when verification succeeds
{
  "result": true, 
  "content": 
}
```

```json
//Returned when verification fails
{
  "result": false, 
  "errCode": 10000,
  "error": "Token is invalid"
}
```

**Wrong Code**


| Code  | Description                     |
| :---- | :----------------------- |
| 10000 | token is expired, please log in again |
| 10001 | Wrong request method       |

## API-Modify the management password

**Brief Description:**

- Changing the device's management password will also change the menu password

**Request URL:**


- http://Device IP:port/api/User/EditPassword
- Example:http://192.168.1.150/api/User/EditPassword

**Request Mode:**

- POST

**Verify Toekn**

- Verify

**Content-Type**

- application/json

**Authorization**

- Bearer token character string



### **Request parameters**


| Field        | Type | Length | Required | Description         |
| :---------- | :--- | :--- | :--- | :----------- |
| OldPassword | int  | 8    | Yes   | Old login password |
| NewPassword | int  | 8    | Yes   | New login password |

**Request parameter example

**

```json
{
    "OldPassword":"123456",
    "NewPassword":"888888"
}
```



### **Return value description**




**Return value example**

```json
//Returned when verification succeeds

{
  "result": true, 
}
```

```json
//Returned when verification fails
{
  "result": false, 
  "errCode": 1,
  "error": "old password error"
}
```

**Wrong Code**

| Code  | Description                                                         |
| :---- | :----------------------------------------------------------- |
| 10000 | token is expired, please log in again                                     |
| 10001 | Wrong request method                                           |
| 10002 | Request Content-Type error                                    |
| 10003 | Request body, gzip decompression error                                  |
| 10004 | The Json format in the request body is incorrect                                 |
| 1     | Old password error                                                 |
| 2     | The new password format is incorrect, it must be a number<br> and cannot be more than 8 digits, cannot be less than 4 digits, and cannot be empty |

# Device Management

## **Device Working Parameters**

### Basic Info of Device SystemInfo


| Field        | Type  | Description                      |
| :--------- | :---  | :------------------------ |
| DeviceSN   | string| Serial No.                     |
| DeviceName | string | Device Name |
|  |  |  |
| FirmwareVerson | string | Firmware Version          |
| FingerprintVerson | string | Fingerprint Algorithm Version     |
| FaceVerson | string | Face Algorithm Version |
|  |  |  |
| Manufacturer   | string | Manufacturer
               |
| ManufacturerPhone   | string | Manufacturer Telephone           |
| Website        | string | Website                 |
| ProductionDate | string | Production Date              |
| OEMText | string | OEM custom text, up to 200 characters can be filled in |
|  |  |  |
| AutoRestart | int | Daily automatic restart switch |
| AutoRestartTime | string | Automatic restart time every day, format  HH:mm |



### Device Working Status Status


| Field        | Type  | Description                      |
| :--------- | :---  | :------------------------ |
| RunDays   | int| Days of system operation                     |
| FormatCount | int | Formatting times          |
| WatchDogCount | int | Number of watchdog reset times     |
| BootTime | int | Boot Time Unix timestamp (seconds) |
| RelayStatus | int | Relay status<br>0--indicates COM and NC are normally closed<br>1--indicates COM and NO are normally closed |
| KeepOpenStatus | int |Normally open state<br>0--indicates normally closed<br>1--indicates normally open |
| DoorSensorStatus | int |Door sensor status<br>0--indicates closed<br>1--indicates open |
| LockDoorStatus | int |Lock status<br>0--indicates unlocked<br>1--indicates locked |
| AlarmStatus | string |Door alarm status<br>Empty string means no alarm, otherwise there will be a specific alarm name<br/>fire--fire alarm<br/>blacklist--blacklist alarm<br>anti--anti-dismantling alarm<br/>illegal--illegal authentication<br/>password--duress alarm password<br/>openTimeout--door opening timeout alarm<br/>doorSensor--door magnetic alarm<br>When there are multiple alarms, use commas to separate fire,blacklist |


### Region and language  Language


| Field        | Type  | Description                      |
| :--------- | :---  | :------------------------ |
| Language     | int    | Language 1 - Chinese；<br>2 - English；3 - Traditional Chinese;4 - French;5 - Russian<br>6 - Portuguese;7 - Spanish;8 - Italian;9 - Japanese<br>10 - Korean;11 - Thai ;12 - Arabic;13 - Portugal<br>14 - Turkish ,15 - Indonesian,16 - Ukrainian,17-Vietnamese |
| SystemTime | int   | Device time Unix timestamp (seconds)      |
| UseNTP | int | Enable NTP automatic time calibration 1--enable; 0--disable |
| TimeZone   | int  | Device time zone, value range:-12  -  +14 |
| Volume        | int  | Volume (range 0-10)  |
| Voice     | int  | Voice playback switch 0, no playback; 1, playback |

### Human-machine interaction UI

| Field       | Type | Description    |
| :--------- | :--- | :-----|
| DisplayBrightness | int  | Screen brightness Settings 1-10 |
| MenuPassword | string | Menu password, pure numbers, 4-8 digits or blank |
| ShowIR | int | Display infrared images on the device 1-- Enable; 0-- Disable |
| ShowPersonPhoto | int | Display the person's portrait after recognition 1--enable; 0--disable |
| PlayPersonName | int | Announce the name of the person after recognition 1--enable; 0--disable |
| RecognitionButon | int | Click the recognition button before recognition 1--Enable; 0--Disable |
| UnregisteredWarn | int | Unregistered personnel reminder 1--Enable; 0--Disable |
| ShowPersonName | int | Whether to display the person's name after identification 1--Enable; 0--Disable |
| FillLight  | int  | Fill light mode: 0: Normally closed; 1: Normally open; 2: Automatic; |
| UseQRCode | int | QR code recognition switch 1--Enable; 0--Disable |



### Data Storage Storage


| Field              | Type | Description                                   |
| :---------------- | :--- | :------------------------------------- |
| RecordAutoCycle | int  | Record full  cycle 1--record full cycle, 0--record full no cycle, waiting to be cleared |
| SaveUnregistered | int  | Save unregistered personnel records, 0: do not store, 1: store<br>Unregistered personnel refers to personnel who are not registered in the system during face recognition,<br>or card numbers that are not registered in the system when swiping a card |
| SaveRecordPicture | int  | Save the scene picture 0, do not save; 1, save          |
| PeopleStorageInfo | class | Personnel storage details |
| RecordStorageInfo | class | Record storage details |

- **Personnel Storage Info peopleInfo **

```json
{

{
"Person": {"Max“: 5000, "Current":0 },//Personnel storage capacity   Max maximum capacity;  Current current storage quantity
"Face": {"Max“: 5000, "Current":0 },//Face Storage Capacity
"Card": {"Max“: 5000, "Current":0 },//Card Capacity Storage
"Fingerprint": {"Max“: 5000, "Current":0 },//Fingerprint storage capacity
"PalmVein": {"Max“: 5000, "Current":0 },//Palm print storage capacity
"Pasword": {"Max“: 5000, "Current":0 },//Password storage capacity
"Admin": {"Max“: 5000, "Current":0 }//Administrator storage capacity
}
```

- **Record storage information recordInfo**

```json
{

"VerifyRecord": {"Max“: 5000, "Current":0 },//Access record storage capacity
"DoorRecord": {"Max“: 5000, "Current":0 },//Door sensor record storage info
"SystemRecord": {"Max“: 5000, "Current":0 },//System record storage capacity
"RecordPhoto": {"Max“: 5000, "Current":0 }//On-site photo storage capacity
}
```

### Face Recognition Face


| Field              | Type | Description                                                         |
| :---------------- | :--- | :----------------------------------------------------------- |
| FaceIR            | int  | Liveness detection, 1 on, 0 off                                       |
| FaceIRThreshold   | int  | Liveness detection threshold 1-99                                            |
| FaceDistance      | int  | Recognition Distance 1--Short Distance(0.2-0.5m); 2--Middle Distance(0.2-1.5m);3--Long Distance(0.2-1.5m or above) |
| FaceThreshold     | int  | Face recognition threshold 1-99 The larger the face recognition threshold, the higher the accuracy                 |
| FPComparison      | int  | Fingerprint comparison threshold  Value range:1-100                                |
| FaceMask          | int  | Mask Detection                                                     |
| FaceMaskThreshold | int  | Mask threshold 1-99, the larger the face recognition threshold, the higher the accuracy                    |

### Body Temperature Detection BodyTemperature


| Field                      | Type  | Decription                                            |
| :------------------------ | :---- | :---------------------------------------------- |
| UseBodyTemperature        | Int   | Temperature measurement mode switch. 0: Non-temperature measurement mode 1: Temperature measurement mode         |
| UseFahrenheitDisplay      | int   | Turn on Fahrenheit temperature display, 1: on, 0: off                      |
| TemperatureCompensate     | float | Temperature compensation value -10.0  -- +10.0                      |
| TemperatureAlarmThreshold | float | Temperature alarm threshold  example 37.5                         |
| TemperatureDisplay        | int   | Whether to display body temperature `` 0-disable; 1-enable |


### Server Parameter NetworkServer


| Field                          | Type   | Description                                                         |
| :---------------------------- | :----- | :----------------------------------------------------------- |
| UseTCPClient                  | Int    | Use TCPClient to connect to the server 1--enable; 0--disable;                |
| UseUDPClient                  | Int    | Use UDPClient to connect to the server 1--enable; 0--disable;                |
| ServerAddress                 | string | Server address TCP or UDP protocol server address                        |
| ServerPort                    | int    | Server port number                                                 |
| KeepaliveTime                 | int    | Keep-alive packet interval 1-65535 seconds                                    |
|                               |        |                                                              |
| UseHTTPClient                 | int    | Whether to enable HTTPClient protocol 1--enable; 0--disable;                |
| HTTPClient_ServerAddr         | string | HTTP protocol server address                                          |
| HTTPClient_KeepaliveTime      | int    | HTTP protocol keep-alive packet interval                                    |
| HTTPClient_UseGZIP            | int    | Whether to use GZIP compression when requesting HTTP protocol 0--do not use; 1--use           |
| HTTPClient_ProtocolType       | int    | HTTPClient Protocol Type<br>100 --- HTTPv1<br>200 ---HTTPv2     |
|                               |        |                                                              |
| UseMQTTClient                 | int    | Whether to start the MQTTClient protocol 1--enable; 0--disable;                 |
| UseMQTTSSL                    | int    | Whether to enable SSL secure socket for MQTT 1--enable; 0--disable;               |
| MQTTServerAddr                | string | MQTT Server Address   www.abc.com                                 |
| MQTTPort                      | int    | MQTT Server port number                                             |
| MQTTLoginName                 | string | User name for logging in to the MQTT protocol                                       |
| MQTTLoginPassword             | string | Password for logging in to MQTT protocol                                         |
| MQTTPublishTopic              | string | The topic used by devices to send data in the MQTT protocol                          |
| MQTTSubscribeTopic            | string | Topic In the MQTT protocol, the topic that the device needs to subscribe to in order to receive data                      |
| MQTT_KeepaliveTime            | int    | The keep-alive packet interval of the MQTT protocol                                    |
| MQTT_UseGZIP                  | int    | Whether to use GZIP compression for mqtt 0--do not use; 1--use                     |
|                               |        |                                                              |
| UseWebsocketClient            | int    | Whether to start WebsocketClient protocol 1--enable; 0--disable;            |
| WebsocketClient_ServerAddr    | string | Websocket protocol server address<br>  ws://192.168.1.1/websocket   or  wss://192.168.1.1/websocket |
| WebsocketClient_KeepaliveTime | int    | WebsocketClient protocol keepalive packet interval                         |
| WebsocketClient_UseGZIP       | int    | Websocket Whether to use GZIP compression 0--do not use; 1--use                |
| WebsocketClient_ProtocolType  | int    | Websocket protocol type                                         |
|                               |        |                                                              |
| UseYZW                        | int    | Whether to enable Yunzhu network HTTPClient protocol 1-- Enable; 0-- Disable;          |
| YZWAddr                       | string | Yunzhu network protocol server address                                        |


### 

### Device Network Parameters Network


| Field            | Type   | Description                                         |
| :-------------- | :----- | :------------------------------------------- |
| ConnectPassword | string | Device communication password 32 characters                        |
|                 |        |                                              |
| UseWired        | int    | Wired network switch, 1: on, 0: off                   |
| WiredDHCP       | int    | Wired network automatic ip, 1: on, 0: off                    |
| WiredIP         | string | Wired network IP address (“//192.168.0.110”)          |
| WiredIPMask     | string | Wired network subnet mask ("//255.255.255.0")          |
| WiredGteway     | string | Wired network gateway (“//192.168.0.1”)                |
| WiredDNS        | string | Dns(“//192.168.0.1”)                         |
| WiredMAC        | string | Wired network MAC address                              |
|                 |        |                                              |
| UseWIFI         | int    | Wireless network switch, 1: on, 0: off                         |
| WIFIAPName      | string | Wireless network account                                   |
| WIFIAPPassword  | string | Wireless network password                                   |
| WIFIMAC         | string | Wireless network MAC address                              |
| WIFIDHCP        | int    | Wireless network automatic ip, 1: on, 0: off                    |
| WIFIIP          | string | Wireless network IP address(“//192.168.0.110”)          |
| WIFIIPMask      | string | Wireless network subnet mask(“//255.255.255.0”)          |
| WIFIGteway      | string | Wireless network gateway (“//192.168.0.1”)                |
| WIFIDNS         | string | Wireless network Dns (“//192.168.0.1”)                 |
|                 |        |                                              |
|                 |        |                                              |
| UseWebPage      | int    | Web page management switch, 1: on, 0: off                    |
| HTTPPort        | int    | Web management page port number, 1-65534                   |
| HTTPSPort       | int    | Web management page port number, 1-65534                   |
| WebPageUseSSL   | int    | Enable SSL on the device web page. Use OpenSSL self-signed SSL certificate |
|                 |        |                                              |
| UseUDP          | int    | UDP port switch, 1: on, 0: off                       |
| UDPPort         | int    | UDP port number (used by UDP protocol)                   |
|                 |        |                                              |
| UseTelnet       | int    | Linux Telnet function switch, 1: on, 0: off             |
| TelnetPort      | int    | Telnet Port Number                                 |
|                 |        |                                              |
| UseRTSP         | int    | RTSP video stream, 1: on, 0: off                        |
| RTSPPort        | int    | RTSP Port Number                                   |
| RTSPUser        | string | RTSP User Name                                   |
| RTSPPassword    | string | RTSP Password                                     |

### Access Control Parameters Door


| Field                    | Type   | Required | Description                                                         |
| :---------------------- | :----- | :--- | :----------------------------------------------------------- |
| CardBytes               | int    | Yes   | Card number byte; 3, 4, 8; 0--disable card reading                           |
| AccessType              | int    | Yes   | Entry and exit type 0, entry; 1, exit                                      |
| WgFormat                | int    | Yes   | WG output format 26 / 34/ 66                                        |
| WGContent               | int    | Yes   | WG output content: 1--User ID,2--Card Number                              |
| ReleaseTime             | int    | Yes   | Door open hold time 0-65535 (s). 0 means 0.5 seconds                        |
| DelayOpenDoorTime       | int    | Yes   | Delay unlocking time 0-65535 (s). 0 -- disable                         |
| FreeOpen                | int    | Yes   | Open door without verification 1--Enable; 0--Disable                                  |
| OpenInterval            | int    | Yes   | Repeat recognition interval 0--disabled; 1-65535（ms）                         |
| OpenInterval_SaveRecord | int    | Yes   | Repeat interval record storage setting 0, do not save; 1, save                        |
| Relay                   | int    | Yes   | Whether the relay supports bistable state. 1 -- support, 0 -- not support                        |
| ShortMessage            | string | Yes   | Short message after legal verification                                           |
| VerificationType        | int    | Yes   | Verification method<br> 1. Standard mode; 2. **Face****/Fingerprint/Palm vein/Card** **+** **Password** .... For details, please see the description below |
| OverdueRemind           | int    | Yes   | Access level expiration reminder 1--enable; 0--disable                                |
| OverdueRemind_Day       | int    | Yes   | Access level expiration reminder validity threshold, the minimum remaining valid days. If the number is lower than this, a prompt will be displayed after recognition that the validity period is about to expire. The value range is: 1-255. 0--means closed. |
|                         |        |      |                                                              |
| TimingOpen              | int    | Yes   | Timed normally open function 1--Enable; 0--Disable                               |
| TimingOpen_mode         | int    | Yes   | Timed normally open. Automatic opening mode: ``1. After legal authentication, it can be normally open within the specified time period``2. If the authorization is marked as normally open, it can be normally open after authentication within the specified time period``3. Automatic opening and closing, automatically opening and closing the door at the time |
| TimingOpen_timegroup    | object | Yes   | Timed normally open.  Normally Open Period  Use Weekly Period Structure                             |
|                         |        |      |                                                              |
| TimingLocked            | int    | Yes   | Timing lock function 1--Enable; 0--Disable                               |
| TimingLocked_timegroup  | object | Yes   | Timed lock.  Lock period  Use weekly period structure                             |
|                         |        |      |                                                              |
| VisitorRootPassword     | string | Yes   | Visitor Root Password                                                   |
| MultiPerson             | int    | Yes   | Multi-group door opening ,number of people ;1-50；                                   |

- VerificationType Verification Method

  ```
  
  1. Standard Mode  Default Value
  2. Face/Fingerprint/Palm Print/Card + Password
  3. Card+Face/Fingerprint/Palm Print/Password
  4. Multiple People Attendance
  5. Person and Identification Comparison
  6. Card + Face/Fingerprint/Palm Print + Password
  7. Card + Fingerprint/Palm Print + Face
  8. Fingerprint/Palm Print + Face + Password
  9. Fingerprint + Palm Print + Face
  10. Palm Print + Face
  11. Fingerpint + Face
  12. Only use palm print
  13. Only use fingerprint
  14. Only use card
  15. Only use passsword
  16.  Person and Identification Comparison Open + Registered Person (Identification card + face + registered)
  ```
  
- **Weekly Time Period Format*

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

  
    - Week1 means Monday
  - Week2 means Tuesday
  - Week3 means Wednesday
  - Week4 means Thursday
  - Week5 means Friday
  - Week6 means Saturday
  - Week7 means Sunday
  - Each week field represents the time period of a day.
  - Can set 8 sub-periods in one day, and the format is start time-end time/start time-end time/.....

  ```
  "01:00-01:59/02:00-02:59/03:00-03:59/04:00-04:59/05:00-05:59/06:00-06:59/07:00-07:59/08:00-08:59"
  The string above defines 8 sub-periods in a day. A maximum of 8 sub-periods can be defined in a day.
  
  "01:00-01:59/02:00--02:59"
  The above string defines two sub-periods of the day, and the other six periods are empty and invalid.
  
  ```

  - If the week of the day is empty and the time period is not set, this field can be omitted

  ```json
  {
  		//This period is defined only week1 and week7  Other time periods are empty  
		
  		Week1:"01:00-02:00",
  		Week7:"03:00-04:00"
  }
  ```
  
  

### Elevator Function Parameters Elevator

| Field          | Type        | Required | Description                                   |
| :------------ | :---------- | :--- | :------------------------------------- |
| UseElevator   | int         | Yes   | Elevator function switch, 1: on, 0: off                 |
| ElevatorPorts | [] Array of Objects  | Yes   | ElevatorPort Array of Objects Define a list of elevator ports |

Elevator Port Object


| Field        | Type | Required | Description                                                         |
| :---------- | :--- | :--- | :----------------------------------------------------------- |
| Num         | int  | Yes   | Elevator port number 1-64                                              |
| RelayType   | int  | Yes   | Elevator relay (COM&NO normally closed, COM&NC normally closed)`<br>`Value range: 1. COM&NC normally closed (default value); 2. COM&NO normally closed |
| ReleaseTime | int  | Yes   | Output duration when unlocking. Maximum 65535 seconds. 0 means 0.5 seconds                      |
| TimingOpen  | obj  | Yes   | Timed normally open function structure                                             |

- timingOpen Timed normally open function
| Field        | Type | Required | Description|
| :---------- | :--- | :--- | :----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Use        | int  | Yes   | Function switch, 0-disable; 1-enable                                                                                                                                    |
| Open       | int  | Yes   | Automatic opening mode: `<br>`1. After passing the legal authentication, the door can be opened normally within the specified time period. `<br>`2. If the authorization is marked as the normally open privilege, the door can be opened normally within the specified time period after passing the authentication. `<br>`3. Automatic opening and closing, automatically opening and closing the door at the specified time |
| Timegroup  | obj  | Yes   | Normally opening period, using weekly period structure                                                                                                                                      |


### Alarm Parameter Alarm


| Field                          | Type   | Required | Description                                                         |
| :---------------------------- | :----- | :--- | :----------------------------------------------------------- |
| FireAlarm                     | int    | Yes   | Fire alarm, 0, Disable; 1, Enable                                      |
|                               |        |      |                                                              |
| DoorLongOpenAlarm             | int    | Yes   | Door opening timeout alarm switch, 1: Enable, 0: Disable                                   |
| DoorLongOpenTime              | int    | Yes   | Door opening timeout, alarm will be triggered if the door is opened for more than this time	1-65535（s）        |
|                               |        |      |                                                              |
| DoorSensorAlarm               | int    | Yes   | Door sensor alarm, 0, Disable; 1, Enable                                      |
| DoorSensorAlarmTimegroup      | class  | Yes   | Door sensor alarm or non-alarm period, weekly period format                                |
|                               |        |      |                                                              |
| BlacklistAlarm                | int    | Yes   | Blacklist alarm, 0, Disable; 1, Enable                                    |
|                               |        |      |                                                              |
| AntiDisassemblyAlarm          | int    | Yes   | Tamper Alarm Function Switch,0,Disable;1,Enable                              |
|                               |        |      |                                                              |
| IllegalVerificationAlarm      | int    | Yes   | Illegal verification alarm function, 0, Disable; 1, Enable                              |
| IllegalVerificationAlarmLimit | int    | Yes   | Illegal verification alarm function-number of illegal authentications,1-255                          |
|                               |        |      |                                                              |
| UseUserCloseAlarm             | int    | Yes   | Allow the user to verify the alarm release switch, 0, Disable; 1, Enable                    |
|                               |        |      |                                                              |
| PasswordAlarm                 | int    | Yes   | Duress alarm password function, 0, Disable; 1, Enable                              |
| PasswordAlarm_Password        | string | Yes   | Duress alarm password. An alarm will occur when enter this password. The password only supports numbers and can contain 0. |
| PasswordAlarm_Mode            | string | Yes   | Working mode when duress alarm occurs ``1--Do not open the door, alarm output``2--Open the door, alarm output ``3--Lock the door, alarm, can only be unlocked by software`` |

### Device Door Opening time period Timegroup


| Field       | Type        | Required | Description         |
| :--------- | :---------- | :--- | :----------- |
| TimeGroups | [] Array of objects | Yes   | Device Opening Time Period |

**Device opening door time period format description**

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

- The maximum num of device opening time periods is 64, which means the device has a maximum of 64 sets of opening time periods.
- week1 means Monday
- week1 means Monday
- week2 means Tuesday
- week3 means Wednesday
- week4 means Thursday
- week5 means Friday
- week6 means Saturday
- week7 mean Sunday
- Each week field indicates the time period of a day
- Can set 8 sub-periods in a day. The format is start time-end time/start time-end time/.....

  ```
  "01:00-01:59/02:00-02:59/03:00-03:59/04:00-04:59/05:00-05:59/06:00-06:59/07:00-07:59/08:00-08:59"
  The string above defines 8 sub-periods in a day. A maximum of 8 sub-periods can be defined in a day.
  
  "01:00-01:59/02:00-02:59"
  The above string defines two sub-periods of the day, and the other six periods are empty and invalid.
  ```
- If the week is empty and the time period is not set, this field can be omitted

  ```
  {
		Num:64,  //This time period is defined only week1 and week7 other time periods are empty. 
  		Week1:"01:00-02:00",
  		Week7:"03:00-04:00"
  	}
  ```

### Device Holidays Holiday

| Field     | Type | Required | Description   |
| :------- | :--- | :--- | :----- |
| Holidays | []   | Yes   | Holidays |

```json
[
 {"Num":1,"Date":"2020-10-01","Type":1,"Cycle":1},
 {"Num":2,"Date":"2020-10-02","Type":2,"Cycle":0},
...
]
```

Holidays use an object array format, and each object contains two fields, num and date.

The device currently supports 360 groups of holidays

On holidays, it is forbidden to open the door (can set access level for holidays)

**Holiday object field description**


| Field  | Type   | Required | Description                                                         |
| :---- | :----- | :--- | :----------------------------------------------------------- |
| Num   | int    | Yes   | The serial number of holidays, use when binding personnel access level                              |
| Date  | string | Yes   | Holiday Date Year-Month-Day Example:2020-10-01                         |
| Type  | int    | No   | Holiday control range, 1--all day; 2--00:00-12:00 in the morning; 3--afternoon (12:00-23:59), the default value is 1; |
| Cycle | int    | No   | Whether to cycle every year, 1--cycle every year; 0--no cycle; the default value is 0;             |



### **Alarm Clock** AlarmClock


#### **Up to 24 groups of alarm clock **



| Field     | Type | Required | Description   |

| :------- | :--- | :--- | :----- |
| AlarmClocks | []   | Yes   | Alarm Clock |

```json
[
 {"Num":1,"Clock":"12:00","Times":10},
 {"Num":2,"Clock":"13:00","Times":10},
 {"Num":3,"Clock":"14:00","Times":10},
...
]
```

The alarm clock uses an object array format, and each object contains three fields: Num, Clock, and Times.

The device currently supports 24 sets of alarm clock

**Alarm clock object field description**

| Field  | Type   | Required | Description                                                         |
| :---- | :----- | :--- | :------ |
| Num   | int    | Yes   | Alarm clock serial number, value range1-24 |
| Clock  | int | Yes   | Alarm clock time point  HHmm format example: 1230 means 12:30    |
| Times  | int    | No   | Alarm ringing duration, unit: seconds, value range: 0-255|



## **API-Obtain the device function list**

**Brief description:**

- After logging in, call this interface to obtain the function list of the device and adjust the interface accordingly.

**Request URL:**


- http://Server IP:port/api/Device/FunctionList

**Request Mode:**

- POST

**Verify Toekn**

- Verify

**Content-Type**

- application/json

**Authorization**

- Bearer token character string

### **Return example**

```json
//Return function list supported by the device
{ 
    "result": true , 
    "content": {
        //Body Temperature Detection
		
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
        //excel export and import
	
        "ExcelFile": true,
        //zip import
	
        "ZipFile": true,
        //Number of time period
	
        "TimeGreoup": true,
        //Wireless Network
	
        "WIFI": true,
        //HTTPClient v1
        "HTTPClient_V1": true,
        //HTTPClient v2
        "HTTPClient_V2": true,
        //MQTT
        "MQTT": true,
        //Yunzhu Network
	
        "YZW": true,
        //Websocket V1
        "Websocket_V1": true,
        //Websocket V2
        "Websocket_V2": true
    }
}

//Log Out

{
  "result": false, 
  "errCode": 401,
  "error": "token verification failed"
  
}

```



## **API-Obtain device operating parameters**

**Brief Description:**

- After logging in, call this interface to obtain the working status of the device

**Reques URL:**

- http://server IP:port/api/Device/GetDetail

**Request Mode:**

- POST

**Verify Toekn**

- Verify

**Content-Type**

- application/json

**Authorization**

- Bearer token character string

### **Request parameter**


| Field Name          | Type | Required | Description         |
| :-------------- | :--- | :--- | :----------- |
| SystemInfo      | bool | No   | Device Basic Info |
| Status          | bool | No   | Device Working Status |
| Language        | bool | No   | Region and language   |
| UI              | bool | No   | Man-machine ineraction     |
| Storage         | bool | No   | Data storage     |
| Face            | bool | No   | Face recognition     |
| BodyTemperature | bool | No   | Body temperature detection     |
| NetworkServer   | bool | No   | Server Parameter   |
| Network         | bool | No   | Device Network Parameter |
| Door            | bool | No   | Access Control Parameter     |
| Elevator        | bool | No   | Elevator Function Parameter |
| Alarm           | bool | No   | Alarm Parameter     |
| TimeGroups      | bool | No   | Device Opening Time Period |
| Holidays        | bool | No   | Device Holidays   |
| AlarmClocks     | bool | No   | Alarm Clock         |

~~~json
{
    "SystemInfo": true,
    "Status": true,
    "Language": true,
    "UI": true,
    "Storage": true,
    "Face": true,
    "BodyTemperature": true,
    "NetworkServer": true,
    "Network": true,
    "Door": true,
    "Elevator": true,
    "Alarm": true,
    "TimeGroups": true,
    "Holidays": true,
    "AlarmClocks": true
}
~~~





**Return Parameter Description**


| Field               | Type   | Required | Description    |
| :----------------- | :----- | :--- | :------ |
| deviceId           | string | Yes   | Device ID |
| Other function parameters of the device |        |      |         |{


```json	
//Return on successful verification

{
  "result": true, 
  "content": {
  
	//Alarm Parameters Alarm

	"FireAlarm":	1,
	"DoorLongOpenAlarm":	1,
	"DoorLongOpenTime":	60,
	"DoorSensorAlarm":	1,
	"DoorSensorAlarmTimegroup":	{
		"Week1":	"00:00-00:50/01:00-01:50/02:00-02:50/03:00-03:50/04:00-04:50/05:00-05:50/06:00-06:50/07:00-07:50",
		"Week2":	"01:01-01:51/02:01-02:51/03:01-03:51/04:01-04:51/05:01-05:51/06:01-06:51/07:01-07:51/08:01-08:51",
		"Week3":	"02:02-02:52/03:02-03:52/04:02-04:52/05:02-05:52/06:02-06:52/07:02-07:52/08:02-08:52/09:02-09:52",
		"Week4":	"03:03-03:53/04:03-04:53/05:03-05:53/06:03-06:53/07:03-07:53/08:03-08:53/09:03-09:53/10:03-10:53",
		"Week5":	"04:04-04:54/05:04-05:54/06:04-06:54/07:04-07:54/08:04-08:54/09:04-09:54/10:04-10:54/11:04-11:54",
		"Week6":	"05:05-05:55/06:05-06:55/07:05-07:55/08:05-08:55/09:05-09:55/10:05-10:55/11:05-11:55/12:05-12:55",
		"Week7":	"06:06-06:56/07:06-07:56/08:06-08:56/09:06-09:56/10:06-10:56/11:06-11:56/12:06-12:56/13:06-13:56"
	},
	"BlacklistAlarm":	1,
	"AntiDisassemblyAlarm":	1,
	"IllegalVerificationAlarm":	1,
	"IllegalVerificationAlarmLimit":	5,
	"UseUserCloseAlarm":	1,
	"PasswordAlarm":	1,
	"PasswordAlarm_Password":	"110110",
	"PasswordAlarm_Mode":	2,
	
	
	
	//Alarm Clock AlarmClock

	"AlarmClocks":	[{
			"Num":	1,
			"Clock":	"00:01",
			"Times":	1
		}, {
			"Num":	24,
			"Clock":	"23:24",
			"Times":	24
		}],
		
		
	
	//BodyTemperature Detection BodyTemperature

	"UseBodyTemperature":	1,
	"UseFahrenheitDisplay":	1,
	"TemperatureCompensate":	-1.5,
	"TemperatureAlarmThreshold":	37.5,
	"TemperatureDisplay":	1,
	
	
	
	
	//Access Control Parameters Door

	"CardBytes":	4,
	"AccessType":	0,
	"WgFormat":	34,
	"WGContent":	1,
	"ReleaseTime":	3,
	"FreeOpen":	1,
	"OpenInterval":	30,
	"OpenInterval_SaveRecord":	1,
	"Relay":	1,
	"ShortMessage":	"Good morning",

	"VerificationType":	1,
	"OverdueRemind":	1,
	"OverdueRemind_Day":	3,
	"TimingOpen":	1,
	"TimingOpen_mode":	3,
	"TimingOpen_timegroup":	{
		"Week1":	"00:00-00:50/01:00-01:50/02:00-02:50/03:00-03:50/04:00-04:50/05:00-05:50/06:00-06:50/07:00-07:50",
		"Week7":	"06:06-06:56/07:06-07:56/08:06-08:56/09:06-09:56/10:06-10:56/11:06-11:56/12:06-12:56/13:06-13:56"
	},
	"TimingLocked":	1,
	"TimingLocked_timegroup":	{
		"Week1":	"00:00-00:50/01:00-01:50/02:00-02:50/03:00-03:50/04:00-04:50/05:00-05:50/06:00-06:50/07:00-07:50",
		"Week7":	"06:06-06:56/07:06-07:56/08:06-08:56/09:06-09:56/10:06-10:56/11:06-11:56/12:06-12:56/13:06-13:56"
	},
	"VisitorRootPassword":	"123456",
	"MultiPerson":	1,
		
	
	
	
	//Elevator Function Parameter Elevator

	"UseElevator":	1,
	"ElevatorPorts":	[{
			"Num":	1,
			"RelayType":	0,
			"ReleaseTime":	3,
			"TimingOpen":	{
				"Use":	1,
				"Open":	3,
				"Timegroup":	{
					"Week1":	"00:00-00:50/01:00-01:50/02:00-02:50/03:00-03:50/04:00-04:50/05:00-05:50/06:00-06:50/07:00-07:50",
					"Week7":	"06:06-06:56/07:06-07:56/08:06-08:56/09:06-09:56/10:06-10:56/11:06-11:56/12:06-12:56/13:06-13:56"
				}
			}
		}, {
			"Num":	64,
			"RelayType":	0,
			"ReleaseTime":	3,
			"TimingOpen":	{
				"Use":	1,
				"Open":	3,
				"Timegroup":	{
					"Week1":	"00:00-00:50/01:00-01:50/02:00-02:50/03:00-03:50/04:00-04:50/05:00-05:50/06:00-06:50/07:00-07:50",
					"Week7":	"06:06-06:56/07:06-07:56/08:06-08:56/09:06-09:56/10:06-10:56/11:06-11:56/12:06-12:56/13:06-13:56"
				}
			}
		}],
		
		
	
	//Face Recogmnition Face
	"FaceIR":	1,
	"FaceIRThreshold":	80,
	"FaceDistance":	1,
	"FaceThreshold":	65,
	"FPComparison":	80,
	"FaceMask":	0,
	"FaceMaskThreshold":	65,
	
	
	
	
	//Device Holidays Holiday
	"Holidays":	[{
			"Num":	1,
			"Date":	"2024-01-01",
			"Type":	1,
			"Cycle":	1
		}, {
			"Num":	30,
			"Date":	"2024-01-30",
			"Type":	1,
			"Cycle":	1
		}],
		
	
	//Region & Language  Language
	"Language":	1,
	"SystemTime":	1710489007,
	"TimeZone":	8,
	"Volume":	10,
	"Voice":	1,
	
	
	//Device Networking Parameters Network

	"ConnectPassword":	"12345678",
	"UseWired":	1,
	"WiredDHCP":	1,
	"WiredIP":	"192.168.1.150",
	"WiredIPMask":	"255.255.255.0",
	"WiredGteway":	"192.168.1.1",
	"WiredDNS":	"8.8.8.8",
	"WiredMAC":	"01-02-03-04-05-06",
	"UseWIFI":	1,
	"WIFIDHCP":	1,
	"WIFIIP":	"192.168.1.150",
	"WIFIIPMask":	"255.255.255.0",
	"WIFIGteway":	"192.168.1.1",
	"WIFIDNS":	"8.8.8.8",
	"WIFIMAC":	"01-02-03-04-05-06",
	"WIFIAPName":	"abcd",
	"WIFIAPPassword":	"12345678",
	"UseWebPage":	1,
	"WebPagePort":	80,
	"WebPageUseSSL":	1,
	"UseUDP":	1,
	"UDPPort":	8101,
	"UseTelnet":	1,
	"TelnetPort":	23,
	"UseRTSP":	1,
	"RTSPPort":	554,
	"RTSPUser":	"admin",
	"RTSPPassword":	"12345678",
	
	
	
	//Server Parameters NetworkServer

		"ServerAddress":	"www.pc15.net",
		"ServerPort":	9003,
		"KeepaliveTime":	30,
		"ServerProtocol":	1,
		"HttpRequestUseGZIP":	1,
		
		
	
	//Device Working Status Status

	"RunDays":	10,
	"FormatCount":	66,
	"WatchDogCount":	13,
	"BootTime":	1710489007,
	"RelayStatus":	1,
	"KeepOpenStatus":	1,
	"DoorSensorStatus":	1,
	"LockDoorStatus":	1,
	"AlarmStatus":	"fire,blacklist",
	
	//Data Storage Storage

	
	"RecordAutoCycle":	1,
	"SaveUnregistered":	1,
	"SaveRecordPicture":	1,
	"PeopleStorageInfo":	{
		"Person":	{
			"Max":	10000,
			"Current":	100
		},
		"Face":	{
			"Max":	20000,
			"Current":	200
		},
		"Card":	{
			"Max":	30000,
			"Current":	300
		},
		"Fingerprint":	{
			"Max":	40000,
			"Current":	400
		},
		"PalmVein":	{
			"Max":	50000,
			"Current":	500
		},
		"Pasword":	{
			"Max":	60000,
			"Current":	600
		},
		"Admin":	{
			"Max":	70000,
			"Current":	700
		}
	},
	"RecordStorageInfo":	{
		"VerifyRecord":	{
			"Max":	80000,
			"Current":	800
		},
		"DoorRecord":	{
			"Max":	90000,
			"Current":	900
		},
		"SystemRecord":	{
			"Max":	11000,
			"Current":	110
		},
		"RecordPhoto":	{
			"Max":	12000,
			"Current":	120
		}
	},
	
	
	
	//Device Basic Info SystemInfo

	"DeviceSN":	"FC-8190H12345678",
	"DeviceName":	"FC-8190H",
	"FirmwareVerson":	"8.23",
	"FingerprintVerson":	"8.23",
	"FaceVerson":	"6.01",
	"Manufacturer":	"Guangzhou XXXXXXXXXX Technology Co., Ltd.",
	"ManufacturerPhone":	"020-12345678",
	"Website":	"www.abc123.com",
	"ProductionDate":	"2024-01-01",
	"OEMText":	"abcdefg12345 Hi",
	"AutoRestart":	0,
	"AutoRestartTime":	0,
	
	
	//Device Opening Time Period Timegroup

	"TimeGroups":	[{
			"Num":	1,
			"Week1":	"00:00-00:50/01:00-01:50/02:00-02:50/03:00-03:50/04:00-04:50/05:00-05:50/06:00-06:50/07:00-07:50",
			"Week7":	"06:06-06:56/07:06-07:56/08:06-08:56/09:06-09:56/10:06-10:56/11:06-11:56/12:06-12:56/13:06-13:56"
		},  {
			"Num":	64,
			"Week1":	"00:00-00:50/01:00-01:50/02:00-02:50/03:00-03:50/04:00-04:50/05:00-05:50/06:00-06:50/07:00-07:50",
			"Week7":	"06:06-06:56/07:06-07:56/08:06-08:56/09:06-09:56/10:06-10:56/11:06-11:56/12:06-12:56/13:06-13:56"
		}],
	
	
	
	//Man-machine interaction UI
	"DisplayBrightness":	10,
	"MenuPassword":	"123456",
	"ShowIR":	1,
	"ShowPersonPhoto":	1,
	"PlayPersonName":	1,
	"RecognitionButon":	1,
	"UnregisteredWarn":	1,
	"ShowPersonName":	1,
	"FillLight":	2
	
	}

}
```


```json	
//Log out
{
  "result": false, 
  "errCode": 401,
  "error": "token verification failed"
}
```

## **API-update device working parameters**

**Brief Description:：**

- When the user modifies the device working parameters, use this interface to upload the modified working parameters
**Request URL:**
- http://Server IP:port/api/Device/UpdateParameter
**Request Mode**
- POST

**Verify Toekn**

- Verify

**Content-Type**

- application/json

**Authorization**

- Bearer token character string

### **Request Parameters**

- Device working parameter fields that need to be updated

### **Request example**

~~~json
{
    "DeviceSN": "FC-8190H12345678",
    "DeviceName": "FC-8190H",
    "Manufacturer": "Manufacturer Manufacturer Manufacturer",
    "ManufacturerPhone": "020-12345678",
    "Website": "www.abc123.com",
    "ProductionDate": "2024-01-01",
    "OEMText": "OEMText OEMText OEMText",
    "AutoRestartTime": "23:59",
    "RecordAutoCycle": 1,
    "SaveUnregistered": 1,
    "SaveRecordPicture": 1,
    "AutoRestart": 0
}
~~~



### **Return example**

```json
//Update success return

{ "result": true , "content": "OK" }

//Log out

{
  "result": false, 
  "errCode": 401,
  "error": "token verification failed"

}

//Parameter error return

{
  "result": false, 
  "errCode": 1,
  "error": "parameter error:  ..... "
}
```


## **API-send device control command**

**Brief Description ：**

- Send when the user clicks on a remote control command on the device page

**Request URL:**

- http://Server IP:port/api/Device/Remote

**Request Mode**

- POST

**Verify Toekn**

- Verify

**Content-Type**

- application/json

**Authorization**

- Bearer token character string
### **Request Parameters**

| Command       | Type | Description                | Required |
| :--------- | ---- | :----------------- | ---- |
| OpenDoor   | bool | Remote door opening           | No   |
| CloseDoor  | bool | Remote door closing           | No   |
| KeepOpen   | bool | Remote normally open           | No   |
| LockDoor   | bool | Remote lock           | No   |
| UnlockDoor | bool | Remote unlock       | No   |
| FireAlarm  | bool | Let the device have a fire alarm | No   |
| CloseAlarm | bool | Disable alarm           | No   |
| Restart    | bool | Restart the device         | No   |
| Recover    | bool | Reset the device to factory default     | No   |

### **Request example**

```json
//Remote door opening
{
  "OpenDoor": true
}
```

### **Return example**

```json
//Return when verification succeeds
{
  "result": true
}

//Log out

{
  "result": false, 
  "errCode": 401,
  "error": "token verification failed"

}

//Parameter error return

{
  "result": false, 
  "errCode": 1,
  "error": "no action to be performed"
}
```

## **API-update device firmware**
**Brief Description：**
- When the user is on the device page, called when uploading new firmware
**Request URL:**
- http://Server IP:port/api/Device/UploadSoftware

**Request Mode**

- POST

**Verify Toekn**
- Verify
**Content-Type**

- multipart/form-data

**Authorization**

- Bearer token character string
### **Request Parameters**


### **Request Parameters**

| Field         | Type                     | Length | Required | Description                                                          |
| :----------- | :----------------------- | :--- | :--- | :----------------------------------------------------------- |
| SoftwareMD5  | string                   | 30   | Yes   | MD5 value of the firmware                                                  |
| SoftwareFile | application/octet-stream | 30   | Yes   | Firmware File<br/>This field must contain Content-Type: application/octet-stream |

### .**Request example**

#### **Request example**

```
POST /note/insertNoteFace HTTP/1.1
Accept: */*
Host: localhost:5000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Type: multipart/form-data; boundary=--------------------------506873351428002157394455
Content-Length: 17842

----------------------------506873351428002157394455
Content-Disposition: form-data; name="softwareMD5"

abcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefgabcdefg
----------------------------506873351428002157394455
Content-Disposition: form-data; name="softwareFile"; filename="softwareFile.pkg"
Content-Type: application/octet-stream

*****jpeg file binary content*****

----------------------------506873351428002157394455--
```
### **Return example**

```json
//Return when verification succeeds

{
  "result": true
}
```

# Personnel Management

## **API-obtain a list of users **

**Brief Description:**

- When querying users on thr UI, call this interface 

**Request URL:**

- http://Server IP:port/api/People/Search

**Request mode:**

- POST

**Authorization**

- Bearer token character string
**Request required parameters**

| Field      | Type   | Length | Required | Description                |
| :-------- | :----- | :--- | :--- | :----------------- |
| PageIndex | uint32 |      | Yes   | Current page index, starting from 1 |
| PageSize  | uint32 |      | Yes   | Number of people per page requested   |

**Request optional parameters**

- Optional parameters for filtering queries

| Field          | Type   | Length | Required                                                         | Description |
| :------------ | :----- | :--- | :----------------------------------------------------------- | :--- |
| UserID        | string | No   | User ID (characters < 32 bits)                                       |      |
| Name          | string | No   | User Name (characters < 32 bits)                                        |      |
| Job           | string | No   | Position (characters < 32 bits)                                            |      |
| Department    | string | No   | Department (characters < 32 bits)                                            |      |
| AccessType    | int    | No   | Role 0--Staff;1--Admin;2--Blacklist                       |      |
| Timegroup     | int    | No   | Opening time group 1-64                                              |      |
| Photo         | int    | No   | Are there any photos? 1--Yes;0--No                              |      |
| CardNum       | string | No   | IC card number pure number maximum support uint64                                |      |
| IdentityCard  | string | No   | Identity Card Number (Can be empty)                                          |      |
| Fingerprint   | int    | No   | Are there any fingerprints? 1--Yes;0--No                                      |      |
| Palmprint     | int    | No   | Are there any palm prints? 1--Yes;0--No                                      |      |
| OrderByColumn | string | No   | The column to sort by, the default is UserID<br> The optional contains UserID,Name,Department，Job，CardNum |      |
| OrderByType   | string | No   | ASC or  DESC    Sort ordering, ascending or descending order<br> Default is ASC           |      |

**Request example**

```json
{
    "PageIndex":1,
	"PageSize":20,
	"UserName":"Wang" //This field indicates that you are searching for people whose usernames contain "Wang"
}
```

**Return example**

```json
{
	"result": true,
	"errCode":0,
	"msg":"",

	"content": {
		"TotalCount": 2000,//The total number of users queried

		"PageIndex":1,//Current page index

		"PageSize":20,//Number of users currently on the page

		"DataList":[//Returned User List

			{
                "UserID": "1",
		"Name": "Test Name1",
                "Job": "",
		"Department": "Sales Department",
                "Password": "",
                "CardNum": "0",
                "AccessType": 0,
                "ExpirationDate": 4102415940,
                "OpenTimes": 65535,
                "KeepOpen": 0,
                "Timegroup": 1,
                "FaceFeature": 0,
                "photo": "",
                "Fingerprint": 0,
                "Palmprint": 0
            },
            {
                "UserID": "10",
                "Name": "11",
		"Job": "None",
		"Department": "Default Department",

                "Password": "",
                "CardNum": "0",
                "AccessType": 0,
                "ExpirationDate": 1909065540,
                "OpenTimes": 65535,
                "KeepOpen": 0,
                "Timegroup": 1,
                "FaceFeature": 1,
                "photo": "/data/attend_data/photo/frame_1711509862529292.jpg",
                "Fingerprint": 0,
                "Palmprint": 0
            }
		]

	}
}
```

## **API-obtain personnel details**

**Brief Description:：**
- Call this interface when modifying a user on the UI

**Request URL:**
- http://Server IP:port/api/People/GetDetail

**Request Mode:**

- Post

**Authorization**

- Bearer token character string

**Parameter:**

| Field        | Type   | Length | Required | Description                      |
| :---------- | :----- | :--- | :--- | :----------------------- |
| UserID      | string | 32   | Yes   | The User ID to be obtained       |
| PhotoBase64 | int    | 1    | No   | Request to return the base64 encoding of the photo |

**Request example**

```json
{
	"UserID": "3007465",
}
```

**Return example**

```json
{
    "result": true,
    "content": {
        "UserID": "3",
        "Name": "888888",
        "Job": "Developing",	
	
        "Department": "Sales Department",	

        "IdentityCard": "",
        "Attachment": "",
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
        "FaceFeature": "AxwtAQEbRcY3HDIu9A/9AP/46Q7dT ....",
        "Fingerprints": [],
        "Palmveins": [],
        "Photo": "/9//4AAQSkZJRgABAQAAAQABAA//4g/YS....."
    }
}
```

## **API-Obtain new user ID**
**Brief Description:**
- Call this interface to get the available person ID before adding people on the UI
**Request URL:**

- http://Server IP:port/api/People/GetNewID

**Request Mode:**

- Post

**Authorization**

- Bearer token character string


**Return example**

```json
{
    "result": true,
    "content": {
        "NewUserID": 3
    }
}
```

## 

## **API-Add new user**

**Brief Description:**


- This interface is called when adding a new person on the UI. If a person with the same number already exists, it will be overwritten.

**Request URL:**

- http://Server IP:port/api/People/New

**Request Mode:**

- POST

 **Content-Type**

- multipart/form-data

**Authorization**

- Bearer token character string

**Parameter:**

| Field       | Type   | Length  | Required | Description                      |
| :--------- | :----- | :---- | :--- | :----------------------- |
| PeopleJson | string |       | Yes   | Personnel details, Json string      |
| Photo      | file   | 500KB | No   | This parameter is required only when the person has a photo |


#### **PeopleJson Personnel Data Format**


| Parameter         | Type   | Required? | Description                                                        |
| :------------- | :----- | :------- | :--------------------------------------------------------- |
| UserID         | string | Yes       | User ID (number, maximum value 4294967295, type UINT32)            |
| Name           | string | No       | User Name(Characters < 64 bits)                                      |
| Job            | string | No       | Position(character < 64 bits)                                          |
| Department     | string | No       | Department(character < 64 bits)）                                          |
| IdentityCard   | string | No       | Identity Card Number (Can be empty) (characters < 64)                           |
| Attachment     | string | No       | Other users info (characters < 200)                                 |
| Photo          | string |          | User photo, can be empty<br>Support base64 encoding of photo files or file upload |
| PhotoMD5       | string | No       | MD5 hexadecimal string format of the photo (character = 32 bits)                |
| PhotoLen       | int    | No       | The maximum supported photo length is 400KB                             |
| Password       | string | No       | Password, pure numbers, length: (0 / 4-8）                              |
| CardNum        | string | No       | Card number (number, maximum value 18446744073709551615, type UINT62)    |
| QRCode         | string | No       | User QR code info (characters <128 bits)                              |
| AccessType     | int    | No       | Role 0--Staff; 1--Admin; 2--Blacklist                     |
| ExpirationDate | uint32 | No       | Access Level Expiration Date<br> unix Timestamp Second      0 means no time limit        |
| OpenTimes      | int    | No       | Opening Times 0-65535； <br>65535--no limit，0--no entry  |
| KeepOpen       | int    | No       | Is the normally open card? 1--Yes; 0--No                                 |
| Timegroup      | int    | No       | Door opening time group 1-64; 0--restricted                             |
| Holidays       | string | No       | Holidays Limit<br/>Comma Separated:1,2,3,4,5                         |
| Fingerprints   | []     | No       | Fingerprint object                                                   |
| Palmveins      | []     | No       | Palmprint object                                                   |
| Elevators      | string | No       | Elevator Port Access Level Group<br/>Comma Separated:1,2,3,4,5                   |
| FaceFeature    | string | No       | Face feature code base64 encoded face feature code                         |
| FaceFeatureMD5 | string | No       | MD5 value of the face feature code HEX character string format                            |

**Characters refer to bytes. A Chinese character occupies 3-4 bytes (utf8 encoding), and an English character occupies 1 byte.**

##### **Holidays Holidays**

- Empty string or no such field means no holiday restriction
- Number the specific restricted holidays,Comma Separated:1,2,3,4,5
- * means all holidays are subject to restrictions



- **Fingerprint Fingerprint object**



| Serial Number| Type | Required   | Description |
| ---| ---  | ----- | -------|
| Num    | int    | Yes       | Fingerprint index number               |
| Data   | string | Yes       | Base64 encoded value of the fingerprint feature code  |
| MD5 | string | No | MD5 value of the feature code |


~~~json
[ //Structure Example
    {
        Num: 1,
        Data: "asdfasdfasfdasdfafsd",
    },
    {
        Num: 2,
        Data: "asdfasdfasfdasdfafsd",
    }
]
~~~




- **Palmvein Palmprint object**


| Serial Number| Type | Required   | Description  |
| ---| ---  | ----- | -------|
| Num    | int    | Yes       | Palmprint index number               |
| Data   | string | Yes       | base64 encoded value of palmprint feature code |
| MD5 | string | No | MD5 value of the feature code |

  ~~~json
[ //Structure Example
    {
        Num: 1,
        Data: "asdfasdfasfdasdfafsd",
    },
    {
        Num: 2,
        Data: "asdfasdfasfdasdfafsd",
    }
]
  ~~~


- **Elevator Elevator Access Control**

```json
  //Indicates that this person has elevator access to floors 1-5
 [
     1,2,3,4,5
 ]
 //Indicates that this person does not have elevator access level
 
 [
   
 ]
  //Indicates that this person only has access to one elevator on the 10th floor
 
 [
     10
 ]
```



**Request example**

```
POST /api/People/New HTTP/1.1
Accept: */*
Host: localhost:5000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Type: multipart/form-data; boundary=--------------------------506873351428002157394455
Content-Length: 17842

----------------------------506873351428002157394455
Content-Disposition: form-data; name="PeopleJson"

{
	"UserID": 3007465,
	"UserName": "User 3007465",
	"Job": "Staff",
	"Password": "123456",
	"PhotoMD5": "rgssPASg5/rG4HYlUwb6CA==",
	"AccessType": 0,
	"Timegroup": 2
}
----------------------------506873351428002157394455
Content-Disposition: form-data; name="Photo"; filename="Postman_file.jpg"
Content-Type: image/jpeg

*****jpeg file binary content*****

----------------------------506873351428002157394455--
```

### Return parameter


| Field             | Type   | Description                                                         |
| :--------------- | :----- | :----------------------------------------------------------- |
| result           | bool   | Operation result, true means success and false means failure                       |
| errCode          | int    | Error code, including common code<br>2--System busy, please try again later<br>3--Request parameter error<br>4--Request payload is too large and exceeds the limit<br>10--JSON parsing failed<br>11--Personnel parameter verification failed<br>12--Photo md5 verification failed<br>13--Photo size verification failed<br>20--Face feature code duplicated<br>21--Fingerprint feature code duplicated<br>22--Palm vein feature code duplicated<br>23--Card number duplicated<br>24--Personnel storage is full<br>25--Error in querying data<br>26--Error in saving personnel photos<br>27--Face feature code format error<br>28--Fingerprint feature code format error<br>29--Palm vein feature code format error<br>30--Photo cannot be recognized<br>31--Error in saving fingerprint feature code<br>32--Error in saving palm vein feature code |
| RepetitionUserID | string | Duplicate user ID<br>Can be empty, only returned when the face or card is repeated<br>Indicates who the card or face is repeated with |

**Return example**

```json
//Added successfully
//Add

{
	"result": true
}
```
## **API-Delete user**
**Brief  Description:**
- Call this interface when modifying a user on the UI
**Request URL:**
- http://Server IP:port/api/People/Delete
**Request Mode:**

- POST

 **Content-Type**

- application/json

**Authorization**

- Bearer token character string
**Parameter:**
| Field      | Type | Length | Required | Description                                                  |
| :-------- | :--- | :--- | :--- | :---------------------------------------------------- |
| UserIDs   | []   |      | No   |  User ID list of the user to be deleted                                  |
| DeleteALL | int  |      | No   | Clear all users, 1- means clear all users; 0- means only delete the specified user |

**Request example**
```
//Delete the specified user
{
	"UserIDs":[
		1,
		2,
		3,
		4
	]
}
```

```
//Delete all users

{
	"DeleteALL":1
}
```

**Return example**

```json
{
	"result": true
}
```



# Department Management
## **API-Obtain department list**
**Brief Desccription:**
- Call this interface when querying departments on the UI
**Request URL:**
- http://Server IP:port/api/Department/Search

**Request Mode:**
- POST
**Authorization**

- Bearer token character string

**Request required parameters**


| Field      | Type   | Length | Required | Description               |
| :-------- | :----- | :--- | :--- | :----------------- |
| PageIndex | uint32 |      | Yes   | Index of the current page, starting with 1 |
| PageSize  | uint32 |      | Yes   | Number of departments per page requested   |
**Request optional parameters**
- Optional parameters for filtering queries

| Field   | Type   | Length | Required                  | Description |
| :----- | :----- | :--- | :-------------------- | :--- |
| DeptID | int    | No   | Department ID 1-1024       |      |
| Name   | string | No   | Department Name(Characters < 20 bits) |      |


**Request example**

```json
{
    "PageIndex":1,
	"PageSize":20,
	"Name":"Development Department"
}
```

**Return example**

```json
{
	"result": true,
	"errCode":0,
	"msg":"",

	"content": {
		"TotalCount": 20,//Total number of departments queried
		"PageIndex":1,//Current page index
		"PageSize":20,//Current page department number
		"DataList":[//List of returned departments
			{
                "DeptID": "1",
		"Name": "Department1",

            },
            {
                "UserID": "2",
		"Name": "Department2"

            }
		]

	}
}
```

## **API-Obtain new department ID details**
**Brief Description:**
- Call this interface to obtain the available department ID before adding a department on the UI.
**请求URL：**
**Request URL:**

- http://Server IP:port/api/Department/GetNewID
**Request Mode:**
- Post

**Authorization**

- Bearer token character string



**Return example**

```json
{
    "result": true,
    "content": {
        "NewDeptID": 3
    }
}
```

## **API-Add new/Modify department*

**Brief Description:**
- When calling this interface to add a new department on the UI, if the department with the same number already exists, it will be overwritten.
**Request URL:**
- http://Server IP:port/api/Department/New
**Request Mode:**

- POST

 **Content-Type**

- multipart/form-data

**Authorization**

- Bearer token character string

**Parameter:**

#### **Department Format**


| Parameter Name | Type   | Required? | Description                   |
| :----- | :----- | :------- | :-------------------- |
| DeptID | int    | Yes       | Department ID              |
| Name   | string | No       | User Name (characters < 32 bits) |

**Request example**

```
{
	"DeptID": 1,
	"Name": "Department1"
}
```

### Return parameter

| Field    | Type | Description                                                         |
| :------ | :--- | :----------------------------------------------------------- |
| result  | bool | Operation result, true means success, false means failure                       |
| errCode | int  | Error code, including common code<br>2--System busy, please try again later<br>3--Request parameter error<br>4--Department name duplicated |


**Return example**

```json
//Added successfully

{
	"result": true
}
```

## **API-Delete department**

**Brief Description:**
- Call this interface when deleting a department on the UI.
**Request URL:**
- http://Server IP:port/api/Department/Delete
**Request Mode:**

- POST

 **Content-Type**

- application/json

**Authorization**

- Bearer token character string
**Parameter:**

| Field      | Type | Length  | Required | Description                                                  |
| :-------- | :--- | :--- | :--- | :---------------------------------------------------- |
| DeptIDs   | []   |      | No   | ID list of the department to be deleted                                      |
| DeleteALL | int  |      | No   | Clear all departments, 1- means clear all departments; 0- means only delete the specified department |

**Request example**

```
//Delete specified user

{
	"DeptIDs":[
		1,
		2,
		3,
		4
	]
}
```

```
//Delete all users

{
	"DeleteALL":1
}
```

**Return example**

```json
{
	"result": true
}
```







# Records Management

## API-Query punch in records

**Brief Description:**

- Call this interface when querying records&events on the UI

**Request URL：**

- http://Server IP:port/api/Record/Identify/Search

**Request Mode:**

- POST

**Authorization**

- Bearer token character string

#### **Request required parameters**


| Field      | Type   | Length | Required | Description                         |
| :-------- | :----- | :--- | :--- | :-------------------------- |
| PageIndex | uint32 |      | Yes   | Current page index, starting from 1          |
| PageSize  | uint32 |      | Yes   | Request the number of records per page   1-1000   |
| BeginDate | uint32 |      | Yes   | Start time Unix timestamp milliseconds |
| EndDate   | uint32 |      | Yes   | End time Unix timestamp milliseconds |

**Request optional parameter**

- Optional parameters for filtering queries

| Field          | Type   | Required | Description                                                         |
| :------------ | :----- | :--- | :----------------------------------------------------------- |
| UserID        | string | No   | User ID (characters < 32 bits)                                        |
| Name          | string | No   | User Name (characters < 32 bits)                                        |
| Department    | string | No   | Department (characters < 32 bits)                                           |
| Job           | string | No   | Position (characters < 32 bits)                                           |
| CardNum       | string | No   | IC card number Pure numbers Maximum supported: uint64                                |
| IdentityCard  | string | No   | Identity Card Number (Can be empty)                                          |
| RecordTypes   | string | No   | List of record types Comma separated, example: 1,2,3                          |
| PhotoBase64   | int    | No   | Whether the image uses Base64 encoding, 1 - use Base64, 0 - do not use;         |
| RecordID      | int    | No   | Query records that are greater than or equal to a certain record number. The operator is >=                   |
| OrderByColumn | string | No   | Sorted columns, default is RecordDate<br> The alternatives are RecordID,RecordDate,UserID,Name,Department，Job，CardNum |
| OrderByType   | string | No   | ASC or  DESC    Sort order, ascending or descending order<br>Default is ASC           |

**Request example**

```json
{
    "PageIndex":1,
	"PageSize":20,
	"UserName":"Wang"//This field indicates that the record with the username containing Wang is searched

}
```

**Return example**

```json
{
	"result": true,
	"errCode":0,
	"msg":"",
	"content": {
		"TotalCount": 2000,//The total number of records queried
		"PageIndex":1,//Current page index
		"PageSize":20,//Number of current page records
		"DataList":[//List of returned records

			{
				"RecordID": 1000,//Record ID,each record should have an ID

				 "UserID": "1",
		  "Name": "Test Name 1",

                  "IdentityCard": "",
                  "Job": "",                
                  "Department": "Sales Department",

                  "CardNum": "0",
				"RecordType": 1,
				"IsEntry": 0,//Is it an entry record?

				"RecordDate": 1710467919,
				"BodyTemp": 0,//Body Temperature

				"PhotoLen": 17842,
				"Photo": "/RecordImage/1000.jpg", //The address of the on-site photo

			}
		]

	}
}
```

#### **Record Field Description**

| Field         | Type   | Length  | Description                             |
| :----------- | :----- | :--- | -------------------------------- |
| RecordID     | long   | 10   | Record serial number                         |
| UserID       | long   | 20   | User number, user ID                 |
| Name         | string | 30   | User Name                         |
| IdentityCard | string | 30   | Identity Card                           |
| Job          | string | 30   | Position                             |
| Department   | string | 30   | Department                             |
| CardNum      | string | 10   | Card Number                             |
| QRCode       | string | 128  | QR Code                           |
| RecordType   | int    | 5    | Event Type                         |
| IsEntry      | int    | 1    | Whether to enter, 1 means entering, 0 means leaving |
| RecordDate   | string | 20   | Record time unix timestamp Second        |
| BodyTemp     | float  | 5    | Body Temperature Measurement                     |
| PhotoLen     | int    | 10   | Image file length 0 indicates that there is no image      |
| Photo        | string |      | Photo address or base64                 |

#### **RecordType Event Type**
| Value   | Explanation                                                |
| ---- | --------------------------------------------------- |
| 1    | Card                                            |
| 2    | Fingerprint                                            |
| 3    | Face                                            |
| 4    | Card + Fingerprint                                         |
| 5    | Face + Fingerprint                                         |
| 6    | Card + Face                                         |
| 7    | Card + Password                                         |
| 8    | Face + Password                                         |
| 9    | Fingerprint + Password                                         |
| 10   | Password verification  User ID+Password                              |
| 11   | Card + Fingerprint + Password                                  |
| 12   | Card + Face + Password                                  |
| 13   | Fingerprint + Face + Password                                  |
| 14   | Card + Fingerprint + Face                                  |
| 15   | Repeat Verification                                            |
| 16   | Expiration date has expired                                          |
| 17   | Opening period expired                                        |
| 18   | Not open on holidays                                    |
| 19   | Unregistered user                                          |
| 20   | Detection lock                                            |
| 21   | The valid times have been used up                                      |
| 22   | Verify when locked, prohibit opening the door                                |
| 23   | Report lost card                                              |
| 24   | Blacklist card                                            |
| 25   | Open door without verification -- When press the fingerprint, the user number is 0, and when swipe the card, the user number is the card number |
| 26   | Disable card swiping verification -- When card swiping is disabled in [Authorization Method]      |
| 27   | Disable fingerprint verification -- When fingerprint is disabled in [Authorization Method]      |
| 28   | Controller expired                                        |
| 29   | Verification passed—validity period is about to expire                             |
| 30   | Abnormal body temperature, entry denied                                  |
| 31   | Visitor password to open the door                                        |
| 32   | Scan the dynamic QR code to open the door                                  |
| 33   | Add a new user in the device menu                                |
| 34   | Modify user in the device menu                                |
| 35   | Delete user in the device menu                                |
| 36   | Palmprint                                          |
| 37   | Card + Palmprint + Face                                |
| 38   | Palmprint + Password                                       |
| 39   | Card + Palmprint                                       |
| 40   | Face + Palmprint                                       |
| 41   | Card + Palmprint + Password                                |
| 42   | Palmprint + Face + Password                                |
| 43   | Fingerprint + Palmprint + Face                                |
| 44   | Combined verification--wait for the next person                            |
| 45   | Combination verification failed                                        |
| 46   | Combination verification completed                                        |
| 47   | Person and identity card comparison                                            |
## API-Query door sensor records
**Brief Description:**
- Call this interface when querying record events on the UI
**Request URL:**
- http://Server IP:port/api/Record/DoorSensor/Search
**Request Mode:**

- POST

**Authorization**

- Bearer token character string

#### **Request required parameters**

| Field      | Type   | Length  | Required | Description                        |
| :-------- | :----- | :--- | :--- | :-------------------------- |
| PageIndex | uint32 |      | Yes   | Current page index, starting from 1          |
| PageSize  | uint32 |      | Yes   | Request the number of records per page            |
| BeginDate | uint32 |      | Yes   | Start time unix timestamp milliseconds |
| EndDate   | uint32 |      | Yes   | End time unix timestamp milliseconds |

**Request optional parameter**

- Optional parameters for filtering queries

| Field          | Type   | Required | Description                                                         |
| :------------ | :----- | :--- | :----------------------------------------------------------- |
| RecordTypes   | string | No   | List of record types, comma-separated, example: 1,2,3                          |
| RecordID      | int    | No   | Query records larger than a certain record number                                   |
| OrderByColumn | string | No   | Sorted column,default is RecordDate<br> The alternatives are RecordID,RecordDate |
| OrderByType   | string | No   | ASC or  DESC    Sort order, ascending or descending order<br> The default is ASC           |

**Request example**

```json
{
    "PageIndex":1,
	"PageSize":20
}
```

**return example**
```json
{
	"result": true,
	"errCode":0,
	"msg":"",
	"content": {
		"TotalCount": 2000,//The total number of records queried
		"PageIndex":1,//Current page index
		"PageSize":20,//Number of current page records
		"DataList":[//List of returned records
			{
				"RecordID": 1000,//record ID, each record should have an ID
				"RecordType": 1,
				"RecordDate": 1710467919,
			}
		]

	}
}
```

#### **Record field description**

| Field         | Type   | Length | Description                             |
| :----------- | :----- | :--- | -------------------------------- |
| RecordID     | long   | 10   | Record Number                         |
| RecordType   | int    | 5    | Events Type                         |
| RecordDate   | string | 20   | Recording time unix timestamp seconds         |

#### **RecordType Event Type


| Value    | Explanation                   |
| ---- | ---------------------- |
| 1    | Door Sensor-open the door              |
| 2    | Door Sensor-close the door              |
| 3    | Enter the door status sensor alarm detection state   |
| 4    | Exit the door status sensor alarm detection state   |
| 5    | Door open               |
| 6    | Open the door with a button           |
| 7    | The door is locked when the button opens     |
| 8    | The controller has expired when the button opens the door |


## API-Query system record

**Brief Description:**
-Call this interface when querying record events on the UI
**Request UR: **
- http://Server IP:port/api/Record/System/Search
**Request Mode:**
- POST

**Authorization**
- Bearer token character string
#### **Request required parameter**


| Field      | Type   | Length | Required | Description                        |
| :-------- | :----- | :--- | :--- | :-------------------------- |
| PageIndex | uint32 |      | Yes   | Index of the current page, starting with 1          |
| PageSize  | uint32 |      | Yes   | Request the number of records per page            |
| BeginDate | uint32 |      | Yes   | Start Time unix timestamp in milliseconds |
| EndDate   | uint32 |      | Yes   | End Time unix timestamp in milliseconds

**Request optional parameter**
- Optional parameters for filter queries

| Field          | Type   | Required | Description                                                         |
| :------------ | :----- | :--- | :----------------------------------------------------------- |
| RecordTypes   | string | No   | List of record types,comma separated, example: 1,2,3                          |
| RecordID      | int    | No   | Query records larger than a certain record number                                   |
| OrderByColumn | string | No   | Sorted column,default is RecordDate<br> The alternatives are RecordID,RecordDate |
| OrderByType   | string | No   | ASC or  DESC    Sort order, ascending or descending order <br> The default is ASC           |

**Request example**
```json
{
    "PageIndex":1,
	"PageSize":20
}
```

**Return example**
```json
{
	"result": true,
	"errCode":0,
	"msg":"",
	"content": {
		"TotalCount": 2000,//The total number of records queried
		"PageIndex":1,//Current page index
		"PageSize":20,//Number of current page records

		"DataList":[//List of returned records
			{
				"RecordID": 1000,//Record ID, Each record should have an ID


				"RecordType": 1,
				"RecordDate": 1710467919,
			}
		]

	}
}
```

#### **Record Field Description**

| Field       | Type   | Length | Description                      |
| :--------- | :----- | :--- | ------------------------- |
| RecordID   | long   | 10   | Record serial number                  |
| RecordType | int    | 5    | Event Type                  |
| RecordDate | string | 20   | Record time unix timestamp seconds |

#### **RecordType Event Type**


| Value   | Explanation                               |
| ---- | ---------------------------------- |
| 1    | Software Opening                           |
| 2    | Software Closing                           |
| 3    | Software Normally Open                           |
| 4    | The controller automatically enters the normally open state                 |
| 5    | Controller automatically closes the door                   |
| 6    | Long press the exit button to open normally                   |
| 7    | Long press the exit button to close normally                   |
| 8    | Software locked                           |
| 9    | Software unlocked                       |
| 10   | Controller timed lock - automatically lock when the time comes     |
| 11   | Controller timed lock - automatically unlock when the time comes |
| 12   | Alarm--Locked                         |
| 13   | Alarm--Unlocked                     |
| 14   | Illegal Authentication Alarm                       |
| 15   | Door Sensor Alarm                           |
| 16   | Duress Alarm                           |
| 17   | Door Opening Timeout Alarm                       |
| 18   | Blacklist Alarm                         |
| 19   | Fire Alarm                           |
| 20   | Tamper Alarm                           |
| 21   | Remove illegal authentication alarm                   |
| 22   | Remove door sensor alarm                       |
| 23   | Remove duress alarm                       |
| 24   | Remove door opening timeout alarm                   |
| 25   | Remove blacklist alarm                     |
| 26   | Remove fire alarm                       |
| 27   | Remove tamper alarm                       |
| 28   | System Power On                           |
| 29   | System error reset (watchdog)             |
| 30   | Device format record                     |
| 31   | Card reader connected reversely                       |
| 32   | The card reader line is not connected properly                 |
| 33   | Unrecognized card reader                   |
| 34   | Network cable disconnected                         |
| 35   | Network cable is inserted                         |
| 36   | WIFI connected                        |
| 37   | WIFI disconnected                        |
| 38   | Bluetooth door opening                           |
| 39   | Roll Call Timeout                           |
| 40   | Clear all users in the device menu           |
| 41   | Back up users to USB flash drive in the device menu          |
| 42   | Import users from USB flash drive in the device menu          |
| 43   | Indoor unit remote door opening                     |
| 44   | Delete all records                       |
| 45   | Delete all users                       |


## API-Clear all records
**Brief Description:**
- Call this interface, when clear all records
**Request URL:**
- http://Server IP:port/api/Record/Delete/All
**Request Mode:**
- POST

**Authorization**

- Bearer token character string

### **Return example**

```json
//Returned when verification succeeds

{
  "result": true
}
```





## API-Clear records of the specified type
**Brief Description:**
- Clearing all records is invoked by this interface
**Request URL:**

- http://Server IP:port/api/Record/Delete/ByType

**Request Mode:**

- POST

**Authorization**

- Bearer token character string

**Request required parameter**
| Field       | Type | Required | Description                                                      |

| :--------- | :--- | :--- | :-------------------------------------------------------- |
| RecordType | int  | Yes   | Record Type<br>1--Identify punch records<br>2--Door sensor records<br>3--System records |

### **Request example**
```json
{
  "RecordType": 1 //Clear identification punch record

}
```

### **Return example**
```json
//Returned when verification succeeds

{
  "result": true
}
```








# Return error code
**Common error code**

| Code Value | Description                    |

| :----- | :---------------------- |
| 10000  | Authorization error     |
| 10001  | method error            |
| 10002  | Content-Type error      |
| 10003  | Content-Encoding  error |
| 10004  | json parse error        |
| 10005  | Content-Length error    |