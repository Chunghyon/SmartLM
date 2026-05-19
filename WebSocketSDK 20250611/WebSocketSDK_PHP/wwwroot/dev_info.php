<html>
<head>
	<meta charset="utf-8">
	<link rel="stylesheet" type="text/css" href="css/style.css">
	<script src="js/xml.js"></script>
	<script src="js/ws_client.js"></script>
	<script type="text/javascript">

		function on_error_report(xml) {
			var err = "";
			var x = xml.getElementsByTagName("Error");
			if (x.length > 0)
				err = "Error: " + x[0].childNodes[0].nodeValue;
			if (err != "")
				document.getElementById("result").innerHTML = err;
		}

		function on_get_time(xml) {
			var x;
			var txt = "";

			x = xml.getElementsByTagName("Time");
			if (x.length > 0)
				txt += "Current Time = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function get_time() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetTime";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_common_response(xml) {
			var x;
			var txt = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt += "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_time() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetTime";
			messageElem.appendChild(requestElem);
			
			var curTime = new Date();
			var timeElem = doc.createElement("Time");
			var month = curTime.getMonth() + 1;
			timeElem.innerHTML = curTime.getFullYear() + '-' + month + '-' + curTime.getDate() + '-T' + curTime.getHours() + ':' + curTime.getMinutes() + ':' + curTime.getSeconds() + 'Z';
			messageElem.appendChild(timeElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function enable_device(enable) {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "EnableDevice";
			messageElem.appendChild(requestElem);
			
			var enableElem = doc.createElement("Enable");
			enableElem.innerHTML = enable;
			messageElem.appendChild(enableElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_get_device_info(xml) {
			var x;
			var txt = "";
			var param_name = "";

			x = xml.getElementsByTagName("ParamName");
			if (x.length > 0)
			{
				txt += x[0].childNodes[0].nodeValue;
				param_name = x[0].childNodes[0].nodeValue;
			}

			x = xml.getElementsByTagName("Value");
			if (x.length > 0)
			{
				if (param_name === 'UtcTimezoneMinutes')
					txt += " = " + (x[0].childNodes[0].nodeValue << 0);	// (<< 0: to 32bit-signed)
				else
					txt += " = " + x[0].childNodes[0].nodeValue;
			}
			set_result(txt);
		}

		function get_device_info() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetDeviceInfo";
			messageElem.appendChild(requestElem);
			
			var paramElem = doc.createElement("ParamName");
			paramElem.innerHTML = document.getElementById("opt_dev_info").value;
			messageElem.appendChild(paramElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_get_device_info_all(str) {
			set_result("Get Device Info All OK");
			document.getElementById("raw_xml").innerText = str;
		}

		function get_device_info_all() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetDeviceInfoAll";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function set_device_info() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetDeviceInfo";
			messageElem.appendChild(requestElem);
			
			var paramElem = doc.createElement("ParamName");
			paramElem.innerHTML = document.getElementById("opt_dev_info").value;
			messageElem.appendChild(paramElem);

			var valueElem = doc.createElement("Value");
			valueElem.innerHTML = document.getElementById("dev_info_val").value;
			messageElem.appendChild(valueElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_get_device_status(xml) {
			var x;
			var txt = "";

			x = xml.getElementsByTagName("ParamName");
			if (x.length > 0)
				txt += x[0].childNodes[0].nodeValue;

			x = xml.getElementsByTagName("Value");
			if (x.length > 0)
				txt += " = " + x[0].childNodes[0].nodeValue;
			set_result(txt);
		}

		function get_device_status() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetDeviceStatus";
			messageElem.appendChild(requestElem);
			
			var paramElem = doc.createElement("ParamName");
			paramElem.innerHTML = document.getElementById("opt_dev_status").value;
			messageElem.appendChild(paramElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_get_device_status_all(str) {
			set_result("Get Device Status All OK");
			document.getElementById("raw_xml").innerText = str;
		}

		function get_device_status_all() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetDeviceStatusAll";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_get_lock_status(xml) {
			const lockStatuses = ["Unknown", "ForceOpen", "ForceClose", "NormalOpen", "AutoRecover", "Close", "Watching close", "IllegalOpen"];
			var x;
			var txt = "LockStatus";

			x = xml.getElementsByTagName("Mode");
			if (x.length > 0)
				txt += " = " + lockStatuses[parseInt(x[0].childNodes[0].nodeValue)];

			set_result(txt);
		}

		function get_lock_status() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "LockControlStatus";
			messageElem.appendChild(requestElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function do_lock_control() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "LockControl";
			messageElem.appendChild(requestElem);
			
			var modeElem = doc.createElement("Mode");
			modeElem.innerHTML = document.getElementById("lock_status").value;
			messageElem.appendChild(modeElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_tr_icon(xml) {
			
			var x;
			var txt = "Set Tr Icon: ";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt += "Result = " + x[0].childNodes[0].nodeValue;

			x = xml.getElementsByTagName("Reason");
			if (x.length > 0)
				txt += ",  Reason = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_tr_icon() {
			var doc = document.implementation.createDocument("", "", null);

			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetTrIcon";
			messageElem.appendChild(requestElem);

			var useridElem = doc.createElement("IconNo");
			useridElem.innerHTML = document.getElementById("icon_no").value;
			messageElem.appendChild(useridElem);

			var useridElem = doc.createElement("Status");
			useridElem.innerHTML = document.getElementById("icon_status").value;
			messageElem.appendChild(useridElem);

			var deleteElem = doc.createElement("Delete");
			if (document.getElementById("chk_delete").checked)
			{
				deleteElem.innerHTML = "Yes";
				messageElem.appendChild(deleteElem);

				send_relay_message(doc, document.getElementById("session").value, messageElem);
				set_result("");
			}
			else
			{
				deleteElem.innerHTML = "No";
				messageElem.appendChild(deleteElem);

				let input = document.createElement('input');
				input.type = 'file';
				input.onchange = _this => {
					var reader = new FileReader();
					reader.readAsArrayBuffer(input.files[0]);
					reader.onload = function() {
						if (this.result.byteLength > 64 * 1024)
						{
							set_result("Icon file size is invalid.");
							return;
						}

						var bytearray = new Uint8Array(this.result);
						var sizeElem = doc.createElement("IconSize");
						sizeElem.innerHTML = bytearray.length;
						messageElem.appendChild(sizeElem);

						var photoElem = doc.createElement("IconData");
						photoElem.innerHTML = btoa(String.fromCharCode.apply(null, bytearray));
						messageElem.appendChild(photoElem);

						send_relay_message(doc, document.getElementById("session").value, messageElem);
						set_result("");
					};
				};
				input.click();
			}			
		}

		function init() {
			var port = <?php include '../config.inc.php'; echo $Port; ?>;
			var use_wss = <?php include '../config.inc.php'; echo $Use_WSS; ?>;
			ws_init(port, use_wss);

			// Set event handlers.
			ws.onmessage = function(e) {
				// e.data contains received string.
				var xml = parseXml (e.data);
				
				var res = "";
				if (xml.getElementsByTagName("Response").length > 0)
					res = xml.getElementsByTagName("Response")[0].childNodes[0].nodeValue;
				
				if (res == "ErrorReport")
					on_error_report(xml);
				else if (res == "GetTime")
					on_get_time(xml);
				else if (res == "SetTime")
					on_common_response(xml);
				else if (res == "EnableDevice")
					on_common_response(xml);
				else if (res == "GetDeviceInfo")
					on_get_device_info(xml);
				else if (res == "SetDeviceInfo")
					on_common_response(xml);
				else if (res == "GetDeviceInfoAll")
					on_get_device_info_all(e.data);
				else if (res == "GetDeviceStatus")
					on_get_device_status(xml);
				else if (res == "GetDeviceStatusAll")
					on_get_device_status_all(e.data);
				else if (res == "LockControlStatus")
					on_get_lock_status(xml);
				else if (res == "LockControl")
					on_common_response(xml);
				else if (res == "SetTrIcon")
					on_set_tr_icon(xml);
			};
		}

		function set_result(str) {
			document.getElementById("result").innerHTML = str;
		}

	</script>
</head>

<body onload="init();" onunload="ws_exit();">
	<div id="result" class="result"></div>

	<table>
		<tr>
			<td><button style="width:210px" onclick="get_time(); return false;">Get Time</button></td>
			<td><button style="width:210px" onclick="set_time(); return false;">Set Time</button></td>
		</tr>
		<tr>
			<td><button style="width:210px" onclick="enable_device('Yes'); return false;">Enable Device</button></td>
			<td><button style="width:210px" onclick="enable_device('No'); return false;">Disable Device</button></td>
		</tr>
		<tr>
			<th colspan="2">Device Information</th>
		</tr>
		<tr>
			<td><button style="width:210px" onclick="get_device_info(); return false;">Get Device Info</button></td>
			<td>
				<select id="opt_dev_info" style="width:300px;">
					<option value="ManagersNumber">ManagersNumber</option>
					<option value="MachineID">MachineID</option>
					<option value="Language">Language</option>
					<option value="LockReleaseTime">LockReleaseTime</option>
					<option value="SLogWarning">SLogWarning</option>
					<option value="GLogWarning">GLogWarning</option>
					<option value="ReverifyTime">ReverifyTime</option>
					<option value="Baudrate">Baudrate</option>
					<option value="IdentifyMode">IdentifyMode</option>
					<option value="LockMode">LockMode</option>
					<option value="DoorSensorType">DoorSensorType</option>
					<option value="DoorOpenTimeout">DoorOpenTimeout</option>
					<option value="AutoSleepTime">AutoSleepTime</option>
					<option value="EventSendType">EventSendType</option>
					<option value="WiegandFormat">WiegandFormat</option>
					<option value="CommPassword">CommPassword</option>
					<option value="UseProxyInput">UseProxyInput</option>
					<option selected="selected" value="ProxyDlgTimeout">ProxyDlgTimeout</option>
					<option value="SoundVolume">SoundVolume</option>
					<option value="ShowRealtimeCamera">ShowRealtimeCamera</option>
					<option value="UseFailLog">UseFailLog</option>
					<option value="FaceEngineThreshold">FaceEngineThreshold</option>
					<option value="FaceEngineUseAntispoofing">FaceEngineUseAntispoofing</option>
					<option value="NeedWearingMask">NeedWearingMask</option>
					<option value="SuggestWearingMask">SuggestWearingMask</option>
					<option value="UseMeasureTemperature">UseMeasureTemperature</option>
					<option value="UseVisitorMode">UseVisitorMode</option>
					<option value="ShowRealtimeTemperature">ShowRealtimeTemperature</option>
					<option value="AbnormalTempDisableDoorOpen">AbnormalTempDisableDoorOpen</option>
					<option value="MeasuringDurationType">MeasuringDurationType</option>
					<option value="MeasuringDistanceType">MeasuringDistanceType</option>
					<option value="TemperatureUnit">TemperatureUnit</option>
					<option value="AbnormalTempThreshold_Celsius">AbnormalTempThreshold_Celsius</option>
					<option value="AbnormalTempThreshold_Fahrenheit">AbnormalTempThreshold_Fahrenheit</option>
					<option value="UtcTimezoneMinutes">UtcTimezoneMinutes</option>
					<option value="BackgroundColor">BackgroundColor</option>
				</select>
			</td>
		</tr>
		<tr>
			<td><button style="width:210px" onclick="set_device_info(); return false;">Set Device Info</button></td>
			<td><input type="text" style="width:300px" id="dev_info_val"/></td>
		</tr>
		<tr>
			<th colspan="2">Device Status</th>
		</tr>
		<tr>
			<td><button style="width:210px" onclick="get_device_status(); return false;">Get Device Status</button></td>
			<td>
				<select id="opt_dev_status" style="width:300px;">
					<option selected="selected" value="ManagerCount">ManagerCount</option>
					<option value="UserCount">UserCount</option>
					<option value="FaceCount">FaceCount</option>
					<option value="FpCount">FpCount</option>
					<option value="CardCount">CardCount</option>
					<option value="PwdCount">PwdCount</option>
					<option value="QRCount">QRCount</option>
					<option value="DoorStatus">DoorStatus</option>
					<option value="AlarmStatus">AlarmStatus</option>
				</select>
			</td>
		</tr>
		<tr>
			<th colspan="2">Lock Control</th>
		</tr>
		<tr>
			<td><button style="width:210px" onclick="get_lock_status(); return false;">Lock Control Status</button></td>
			<td></td>
		</tr>
		<tr>
			<td><button style="width:210px" onclick="do_lock_control(); return false;">Lock Control</button></td>
			<td>
				<select id="lock_status" style="width:300px;">
					<option selected="selected" value="1">ForceOpen</option>
					<option value="2">ForceClose</option>
					<option value="3">NormalOpen</option>
					<option value="4">AutoRecover</option>
					<option value="5">Restart</option>
					<option value="6">CancelWarning</option>
				</select>
			</td>
		</tr>
		<tr>
			<td><button style="width:210px" onclick="get_device_info_all(); return false;">Get Device Info All</button></td>
			<td><button style="width:210px" onclick="get_device_status_all(); return false;">Get Device Status All</button></td>
		</tr>

		<tr>
			<th colspan="2">TR Icon Settings(M91 German Only)</th>
		</tr>
		<tr>
			<td>Icon No(1~8):</td>
			<td><input type="text" style="width:300px" id="icon_no" value="1"/></td>
		</tr>
		<tr>
			<td>Status(0~2):</td>
			<td><input type="text" style="width:300px" id="icon_status" value="0"/></td>
		</tr>
		<tr>
			<td>Delete:</td><td><input type="checkbox" id="chk_delete"/></td>
		</tr>
		<tr>
			<td></td>
			<td><button style="width:210px" onclick="set_tr_icon(); return false;">Set</button></td>
		</tr>
	</table>

	<br><span id="raw_xml"></span>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>