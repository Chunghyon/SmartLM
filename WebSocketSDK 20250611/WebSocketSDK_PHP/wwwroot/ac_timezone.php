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

		function on_get_ac_timezone(xml) {
			var x;
			var val = "";
			var txt = "";

			var i;
			for (i = 0; i < 7; i++)
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
			}
			
			txt = "Get Access Time Zone OK";
			set_result(txt);
		}

		function get_ac_timezone() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetAccessTimeZone";
			messageElem.appendChild(requestElem);

			var tzElem = doc.createElement("TimeZoneNo");
			tzElem.innerHTML = parseInt(document.getElementById("tz_no").value) - 1;
			messageElem.appendChild(tzElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_ac_timezone(xml) {
			var x;
			var ret = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt = "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_ac_timezone() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetAccessTimeZone";
			messageElem.appendChild(requestElem);
			
			var tzElem = doc.createElement("TimeZoneNo");
			tzElem.innerHTML = parseInt(document.getElementById("tz_no").value) - 1;
			messageElem.appendChild(tzElem);

			var i;
			for (i = 0; i < 7; i++)
			{
				var paramItem = doc.createElement("TimeSection_" + i);

				var item_value = "";
				var int_val = parseInt(document.getElementById("start_hour" + i).value) * 60 + parseInt(document.getElementById("start_minute" + i).value);
				item_value += int_val;
				int_val = parseInt(document.getElementById("end_hour" + i).value) * 60 + parseInt(document.getElementById("end_minute" + i).value);
				item_value += "," + int_val;
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
				else if (res == "GetAccessTimeZone")
					on_get_ac_timezone(xml);
				else if (res == "SetAccessTimeZone")
					on_set_ac_timezone(xml);
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
			<td><button style="width:240px" onclick="get_ac_timezone(); return false;">Get Access Time Zone</button></td>
			<td rowspan="2">
				<select id="tz_no" style="width:210px">
					<?php
						for ($i = 1; $i <= 50; $i++)
							echo "<option value='$i'>Timezone$i</option>";
					?>
				</select>
			</td>
		</tr>
		<tr>
			<td><button style="width:240px" onclick="set_ac_timezone(); return false;">Set Access Time Zone</button></td>
		</tr>
	</table>
	<table>
		<tr>
			<th>No</th>
			<th>Start(HH:mm)</th>
			<th>End(HH:mm)</th>
		</tr>
		<?php
			for ($i = 0; $i < 7; $i++)
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

				echo "</tr>";
			}
		?>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>