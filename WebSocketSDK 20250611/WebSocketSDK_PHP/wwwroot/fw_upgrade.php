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

		function on_get_fw_version(xml) {
			var x;
			var txt = "Get Firmware Version OK: ";

			x = xml.getElementsByTagName("Version");
			if (x.length > 0)
				txt += "Version = " + x[0].childNodes[0].nodeValue;

			x = xml.getElementsByTagName("BuildNumber");
			if (x.length > 0)
				txt += ", BuildNumber = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function get_fw_version() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetFirmwareVersion";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_fw_upgrade_result(xml) {
			var x;
			var ret = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				ret = x[0].childNodes[0].nodeValue;

			if (ret == 'OK')
				txt = "Send Firmware Download Url Success. (Starting download and upgrade now...)";
			else
				txt = "Result = " + ret;

			set_result(txt);
		}

		function do_fw_upgrade() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "FirmwareUpgradeHttp";
			messageElem.appendChild(requestElem);
			
			var fw_path = document.getElementById("fw_path").value;
			var paramElem = doc.createElement("Data");
			paramElem.innerHTML = btoa(fw_path);
			messageElem.appendChild(paramElem);

			var sizeElem = doc.createElement("Size");
			sizeElem.innerHTML = fw_path.length;
			messageElem.appendChild(sizeElem);

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
				else if (res == "GetFirmwareVersion")
					on_get_fw_version(xml);
				else if (res == "FirmwareUpgradeHttp")
					on_fw_upgrade_result(xml);
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
			<td>Get Firmware Version:</td>
			<td><button style="width:210px;" onclick="get_fw_version(); return false;">Get Firmware Version</button></td>
		</tr>
		<tr>
			<td colspan="2">Firmware Upgrade from Http(s). (ex) http://yqall02.baidupcs.com/file/58a7fdfffdf9...</td>
		</tr>
		<tr>
			<td><button style="width:210px;" onclick="do_fw_upgrade(); return false;">Upgrade</button></td>
			<td><input type='text' id='fw_path' style="width:400px;"/></td>
		</tr>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>