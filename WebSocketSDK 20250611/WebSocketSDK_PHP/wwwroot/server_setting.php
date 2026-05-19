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
			var hour = "";
			var min = "";
			var sync_interval = "";
			var txt = "";

			x = xml.getElementsByTagName("ParamName");
			if (x.length > 0 && x[0].childNodes[0].nodeValue == "WebServerUrl")
			{
				x = xml.getElementsByTagName("Value1"); // value1 = web server url
				if (x.length > 0)
				{
					url = x[0].childNodes[0].nodeValue;
					txt = "WebServerUrl = " + url;
				}
			}

			document.getElementById("server").value = url;
			set_result(txt);
		}

		function get_web_server() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetDeviceInfoExt";
			messageElem.appendChild(requestElem);
			
			var paramElem = doc.createElement("ParamName");
			paramElem.innerHTML = "WebServerUrl";
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

		function set_web_server() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetDeviceInfoExt";
			messageElem.appendChild(requestElem);
			
			var paramElem = doc.createElement("ParamName");
			paramElem.innerHTML = "WebServerUrl";
			messageElem.appendChild(paramElem);

			var valueElem1 = doc.createElement("Value1");
			valueElem1.innerHTML = document.getElementById("server").value;
			messageElem.appendChild(valueElem1);

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
			<td colspan="2"><button onclick="get_web_server(); return false;">Get Web Server URL</button></td>
		</tr>
		<tr>
			<td><button onclick="set_web_server(); return false;">Set Web Server URL</button></td>
			<td><input type='text' id='server' style="width:400px;"/></td>
		</tr>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>