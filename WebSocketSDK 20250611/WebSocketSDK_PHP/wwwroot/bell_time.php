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

		function on_get_bell_time(xml) {
			var x;
			var val = "";
			var txt = "";

			val = "0";
			x = xml.getElementsByTagName("BellRingTimes");
			if (x.length > 0)
				val = x[0].childNodes[0].nodeValue;
			document.getElementById("ring_times").value = val;

			val = "0";
			x = xml.getElementsByTagName("BellCount");
			if (x.length > 0)
				val = x[0].childNodes[0].nodeValue;
			document.getElementById("bell_count").value = val;

			var i;
			for (i = 0; i < 24; i++)
			{
				val = "";
				x = xml.getElementsByTagName("Bell_" + i);
				if (x.length > 0)
					val = x[0].childNodes[0].nodeValue;
				
				var val_array = val.split(",");
				
				// valid
				var int_val = 0;
				if (val_array.length >= 1)
					int_val = parseInt(val_array[0]);
				if (int_val)
					document.getElementById("valid" + i).checked = true;
				else
					document.getElementById("valid" + i).checked = false;

				// type
				int_val = 0;
				if (val_array.length >= 2)
					int_val = parseInt(val_array[1]);
				document.getElementById("type" + i).value = int_val;

				// hour
				int_val = 0;
				if (val_array.length >= 3)
					int_val = parseInt(val_array[2]);
				document.getElementById("hour" + i).value = int_val;

				// minute
				int_val = 0;
				if (val_array.length >= 4)
					int_val = parseInt(val_array[3]);
				document.getElementById("minute" + i).value = int_val;
			}
			
			txt = "Get Bell Time OK";
			set_result(txt);
		}

		function get_bell_time() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetBellTime";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_bell_time(xml) {
			var x;
			var ret = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt = "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_bell_time() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetBellTime";
			messageElem.appendChild(requestElem);
			
			var paramRingTimes = doc.createElement("BellRingTimes");
			paramRingTimes.innerHTML = document.getElementById("ring_times").value;
			messageElem.appendChild(paramRingTimes);

			var paramCount = doc.createElement("BellCount");
			paramCount.innerHTML = document.getElementById("bell_count").value;
			messageElem.appendChild(paramCount);

			var i;
			for (i = 0; i < 24; i++)
			{
				var paramItem = doc.createElement("Bell_" + i);

				var item_value = "0";
				if (document.getElementById("valid" + i).checked)
					item_value = "1";

				item_value += "," + document.getElementById("type" + i).value;
				item_value += "," + document.getElementById("hour" + i).value;
				item_value += "," + document.getElementById("minute" + i).value;

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
				else if (res == "GetBellTime")
					on_get_bell_time(xml);
				else if (res == "SetBellTime")
					on_set_bell_time(xml);
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
			<td><button style="width:210px" onclick="get_bell_time(); return false;">Get Bell Time</button></td>
			<td><button style="width:210px" onclick="set_bell_time(); return false;">Set Bell Time</button></td>
		</tr>
		<tr>
			<td>Ring Times:</td>
			<td><input type="text" id='ring_times' value='0'/></td>
		</tr>
		<tr>
			<td>Bell Count:</td>
			<td><input type="text" id='bell_count' value='0'/></td>
		</tr>
	</table>
	<table>
		<tr>
			<th>No</th>
			<th>Valid</th>
			<th>Type</th>
			<th>Hour</th>
			<th>Minute</th>
		</tr>
		<?php
			for ($i = 0; $i < 24; $i++)
			{
				echo "<tr>";
				
				echo "<td style='padding:2px 30px'>$i</td>";

				echo "<td style='padding:2px 30px'>";
				echo "<input style='height:28px;' type='checkbox' id='valid$i'/>";
				echo "</td>";
				
				echo "<td style='padding:2px 30px'>";
				echo "<select style='height:28px;width:120px;' id='type$i'>";
					echo "<option value='0'>Bell1</option>";
					echo "<option value='1'>Bell2</option>";
					echo "<option value='2'>Bell3</option>";
					echo "<option value='3'>Bell4</option>";
					echo "<option value='4'>Bell5</option>";
				echo "</select>";
				echo "</td>";

				echo "<td style='padding:2px 30px'>";
				echo "<input style='height:28px;width:60px;text-align:center;' type='text' id='hour$i' value='0' />";
				echo "</td>";

				echo "<td style='padding:2px 30px'>";
				echo "<input style='height:28px;width:60px;text-align:center;' type='text' id='minute$i' value='0' />";
				echo "</td>";

				echo "</tr>";
			}
		?>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>