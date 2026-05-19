<html>
<head>
	<meta charset="utf-8">
	<link rel="stylesheet" type="text/css" href="css/style.css">
	<script src="js/xml.js"></script>
	<script src="js/ws_client.js"></script>
	<script type="text/javascript">

		function on_get_device_events(xml) {
			var x, i;
			var txt = "<tr><th>DeviceUID</th><th>Content</th></tr>";

			x = xml.getElementsByTagName("Event");
			for (i = 0; i< x.length; i++) {
				txt += "<tr>";
				txt += "<td>" + x[i].getElementsByTagName("SN")[0].childNodes[0].nodeValue + "</td>";
				txt += "<td>" + x[i].getElementsByTagName("Data")[0].childNodes[0].nodeValue + "</td>";
				txt += "</tr>";
			}

			document.getElementById("event_list").innerHTML = txt;
		}

		function do_send(doc, message) {
			if (ws == null)
			{
				alert("Can't connect to server");
				return;
			}
			doc.appendChild(message);

			serializer = new XMLSerializer();
			ws.send(serializer.serializeToString(doc));
		}

		function get_device_events() {
			var doc = document.implementation.createDocument("", "", null);
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "browserGetDeviceEvents";
			messageElem.appendChild(requestElem);

			do_send(doc, messageElem);
		}

		function do_clear() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "browserClearAllDeviceEvents";
			messageElem.appendChild(requestElem);

			do_send(doc, messageElem);
			document.getElementById("event_list").innerHTML = "";
		}

		function init() {
			var port = <?php include '../config.inc.php'; echo $Port; ?>;
			var use_wss = <?php include '../config.inc.php'; echo $Use_WSS; ?>;
			ws_init(port, use_wss);

			// Set event handlers.
			ws.onopen = function() {
				get_device_events();
			};

			ws.onmessage = function(e) {
				// e.data contains received string.
				var xml = parseXml (e.data);
				
				var res = "";
				if (xml.getElementsByTagName("Response").length > 0)
					res = xml.getElementsByTagName("Response")[0].childNodes[0].nodeValue;
				
				if (res == "browserGetDeviceEvents")
					on_get_device_events(xml);
			};
		}

	</script>
</head>

<body onload="init();" onunload="ws_exit();">
	<table>
		<tr>
			<td><button style="width:200px" onclick="window.location.reload(); return false;">Refresh</button></td>
			<td><button style="width:200px" onclick="do_clear(); return false;">Clear</button></td>
		</tr>
	<table>
	<table id="event_list">
	</table>
</body>
</html>