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

		function on_cmd_result(xml) {
			var x;
			var txt = "Result";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt += " = " + x[0].childNodes[0].nodeValue;
			set_result(txt);
		}

		function do_send_cmd(req) {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = req;
			messageElem.appendChild(requestElem);
			
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
				else if (res == "EmptyTimeLog" ||
					res == "EmptyManageLog" ||
					res == "EmptyAllData" ||
					res == "EmptyUserEnrollmentData")
					on_cmd_result(xml);
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
			<td><button style="width:280px" onclick="do_send_cmd('EmptyTimeLog'); return false;">Empty Time Log</button></td>
		</tr>
		<tr>
			<td><button style="width:280px" onclick="do_send_cmd('EmptyManageLog'); return false;">Empty Manage Log</button></td>
		</tr>
		<tr>
			<td><button style="width:280px" onclick="do_send_cmd('EmptyAllData'); return false;">Empty All Data</button></td>
		</tr>
		<tr>
			<td><button style="width:280px" onclick="do_send_cmd('EmptyUserEnrollmentData'); return false;">Empty User Enrollment Data</button></td>
		</tr>
   	</table>
	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>