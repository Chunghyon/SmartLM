<html>
<head>
	<meta charset="utf-8">
	<link rel="stylesheet" type="text/css" href="css/style.css">
	<script src="js/xml.js"></script>
	<script src="js/ws_client.js"></script>
	<script type="text/javascript">
	
		function send_get_online_devices() {
			var doc = document.implementation.createDocument("", "", null);
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "browserGetOnlineDevices";
			messageElem.appendChild(requestElem);
			doc.appendChild(messageElem);
			
			serializer = new XMLSerializer();
			ws.send(serializer.serializeToString(doc));
		}
		
		function on_get_online_devices(xml) {
			var x, i;
			var txt = "<tr><th>DeviceSN</th><th>SessionID</th><th></th></tr>";

			x = xml.getElementsByTagName("Device");
			for (i = 0; i< x.length; i++) {
				txt += "<tr>";
				txt += "<td>" + x[i].getElementsByTagName("SN")[0].childNodes[0].nodeValue + "</td>";
				txt += "<td>" + x[i].getElementsByTagName("Session")[0].childNodes[0].nodeValue + "</td>";
				txt += "<td><a style='font-weight:600' href='menu.php?" + "session="+ x[i].getElementsByTagName("Session")[0].childNodes[0].nodeValue + "'>Open</a></td>";
				txt += "</tr>";
			}

			document.getElementById("devlist").innerHTML = txt;
		}
		
		function init() {
			var port = <?php include '../config.inc.php'; echo $Port; ?>;
			var use_wss = <?php include '../config.inc.php'; echo $Use_WSS; ?>;
			ws_init(port, use_wss);

			// Set event handlers.
			ws.onopen = function() {
				send_get_online_devices();
			};
      
			ws.onmessage = function(e) {
				// e.data contains received string.
				var xml = parseXml(e.data);
				
				var res = "";
				if (xml.getElementsByTagName("Response").length > 0)
					res = xml.getElementsByTagName("Response")[0].childNodes[0].nodeValue;
				
				if (res == "browserGetOnlineDevices") // Only handle GetOnlineDevices response
					on_get_online_devices(xml);
			};
		}
	</script>
</head>

<body onload="init();" onunload="ws_exit();">
    <button onclick="window.location.reload(); return false;">Refresh</button>
	<table id="devlist"/>
</body>

</html>