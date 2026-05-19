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

		function on_get_auto_attendance(xml) {
			var x;
			var val = "";
			var txt = "";

			var i;
			for (i = 0; i < 10; i++)
			{
				val = "";
				x = xml.getElementsByTagName("TimeSection_" + i);
				if (x.length > 0)
					val = x[0].childNodes[0].nodeValue;
				
				var val_array = val.split(",");
				
				// start
				var int_val = 0;
				if (val_array.length >= 1)
					int_val = parseInt(val_array[0]);
				document.getElementById("start_hour" + i).value = (int_val - (int_val % 60)) / 60;
				document.getElementById("start_minute" + i).value = int_val % 60;

				// end
				var int_val = 0;
				if (val_array.length >= 2)
					int_val = parseInt(val_array[1]);
				document.getElementById("end_hour" + i).value = (int_val - (int_val % 60)) / 60;
				document.getElementById("end_minute" + i).value = int_val % 60;

				// end
				int_val = 0;
				if (val_array.length >= 3)
					int_val = parseInt(val_array[2]);
				document.getElementById("type" + i).value = int_val;
			}
			
			txt = "Get Auto Attendance OK";
			set_result(txt);
		}

		function get_auto_attendance() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetAutoAttendance";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_auto_attendance(xml) {
			var x;
			var ret = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt = "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_auto_attendance() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetAutoAttendance";
			messageElem.appendChild(requestElem);
			
			var i;
			for (i = 0; i < 10; i++)
			{
				var paramItem = doc.createElement("TimeSection_" + i);

				var item_value = "";
				var int_val = parseInt(document.getElementById("start_hour" + i).value) * 60 + parseInt(document.getElementById("start_minute" + i).value);
				item_value += int_val;
				int_val = parseInt(document.getElementById("end_hour" + i).value) * 60 + parseInt(document.getElementById("end_minute" + i).value);
				item_value += "," + int_val;
				item_value += "," + document.getElementById("type" + i).value;
				paramItem.innerHTML = item_value;
				messageElem.appendChild(paramItem);
			}

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
				else if (res == "GetAutoAttendance")
					on_get_auto_attendance(xml);
				else if (res == "SetAutoAttendance")
					on_set_auto_attendance(xml);
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
			<td><button style="width:210px" onclick="get_auto_attendance(); return false;">Get Auto Attendance</button></td>
			<td><button style="width:210px" onclick="set_auto_attendance(); return false;">Set Auto Attendance</button></td>
		</tr>
	</table>
	<table>
		<tr>
			<th>No</th>
			<th>Start(HH:mm)</th>
			<th>End(HH:mm)</th>
			<th>Status</th>
		</tr>
		<?php
			for ($i = 0; $i < 10; $i++)
			{
				echo "<tr>";
				
				echo "<td style='padding:2px 30px'>$i</td>";

				echo "<td style='padding:2px 30px'>";
				echo "<input style='height:28px;width:60px;text-align:center;' type='text' id='start_hour$i' value='0' /> &nbsp;:&nbsp;";
				echo "<input style='height:28px;width:60px;text-align:center;' type='text' id='start_minute$i' value='0' />";
				echo "</td>";

				echo "<td style='padding:2px 30px'>";
				echo "<input style='height:28px;width:60px;text-align:center;' type='text' id='end_hour$i' value='0' /> &nbsp;:&nbsp;";
				echo "<input style='height:28px;width:60px;text-align:center;' type='text' id='end_minute$i' value='0' />";
				echo "</td>";
				
				echo "<td style='padding:2px 30px'>";
				echo "<select style='height:28px;width:140px;' id='type$i'>";
					echo "<option value='0'>DutyOn</option>";
					echo "<option value='1'>DutyOff</option>";
					echo "<option value='2'>OvertimeOn</option>";
					echo "<option value='3'>OvertimeOff</option>";
					echo "<option value='4'>In</option>";
					echo "<option value='5'>Out</option>";
				echo "</select>";
				echo "</td>";

				echo "</tr>";
			}
		?>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>