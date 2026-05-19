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

		function on_get_device_info_ext(xml) {
			var x;
			var url = "";
			var hour = 0;
			var min = 0;
			var sync_interval = "";
			var txt = "";

			x = xml.getElementsByTagName("ParamName");
			if (x.length > 0 && x[0].childNodes[0].nodeValue == "NTPServer")
			{
				x = xml.getElementsByTagName("Value1");
				if (x.length > 0)
					url = x[0].childNodes[0].nodeValue;
				document.getElementById("server").value = url;	// value1 = NTP server URL

				x = xml.getElementsByTagName("Value1");
				if (x.length > 0)
					url = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("Value2");	// value2 = Timezone(hour * 60 + minute)
				if (x.length > 0)
				{
					var v = parseInt(x[0].childNodes[0].nodeValue);
					var sign = 0;
					if (v < 0)
					{
						sign = 1;
						v = -v;
					}
					min = v % 60;
					hour = (v - min) / 60;
					if (sign)
						hour = -hour;
				}

				x = xml.getElementsByTagName("Value3"); // value3 = sync interval in minutes
				if (x.length > 0)
					sync_interval = x[0].childNodes[0].nodeValue;

				txt = "Get NTP Server OK";
			}

			document.getElementById("server").value = url;
			document.getElementById("tz_hour").value = hour;
			document.getElementById("tz_minute").value = min;
			document.getElementById("sync_interval").value = sync_interval;

			set_result(txt);
		}

		function get_ntp_server() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetDeviceInfoExt";
			messageElem.appendChild(requestElem);
			
			var paramElem = doc.createElement("ParamName");
			paramElem.innerHTML = "NTPServer";
			messageElem.appendChild(paramElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_device_info_ext(xml) {
			var x;
			var ret = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt = "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_ntp_server() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetDeviceInfoExt";
			messageElem.appendChild(requestElem);
			
			var paramElem = doc.createElement("ParamName");
			paramElem.innerHTML = "NTPServer";
			messageElem.appendChild(paramElem);

			var valueElem1 = doc.createElement("Value1");
			valueElem1.innerHTML = document.getElementById("server").value;
			messageElem.appendChild(valueElem1);

			var hour = parseInt(document.getElementById("tz_hour").value);
			var min = parseInt(document.getElementById("tz_minute").value);
			var tzval = hour * 60 + min;
			if (hour < 0)
				tzval = hour * 60 - min;

			var valueElem2 = doc.createElement("Value2");
			valueElem2.innerHTML = tzval;
			messageElem.appendChild(valueElem2);

			var valueElem3 = doc.createElement("Value3");
			valueElem3.innerHTML = document.getElementById("sync_interval").value;
			messageElem.appendChild(valueElem3);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
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
				else if (res == "GetDeviceInfoExt")
					on_get_device_info_ext(xml);
				else if (res == "SetDeviceInfoExt")
					on_set_device_info_ext(xml);
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
			<td><button style="width:180px" onclick="get_ntp_server(); return false;">Get NTP Server</button></td>
			<td><button style="width:180px" onclick="set_ntp_server(); return false;">Set NTP Server</button></td>
		</tr>
		<tr>
			<td>Server Name:</td>
			<td><input type='text' id='server' style="width:400px;"/></td>
		</tr>
		<tr>
			<td>Timezone(UTC):</td>
			<td><input type='text' id='tz_hour' style="width:60px;"/>&nbsp;&nbsp;:&nbsp;&nbsp<input type='text' id='tz_minute' style="width:60px;"/></td>
		</tr>
		<tr>
			<td>Auto Sync Intervel:</td>
			<td><input type='text' id='sync_interval' style="width:400px;"/></td>
		</tr>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>