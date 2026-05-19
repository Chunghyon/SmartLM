<html>
<head>
	<meta charset="utf-8">
	<link rel="stylesheet" type="text/css" href="css/style.css">
	<script src="js/xml.js"></script>
	<script src="js/utf16.js"></script>
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

		function check_user_id(str) {

			if (str.length < 1)
			{
				set_result("Please input user id.");
				return false;
			}

			return true;
		}

		function on_get_user_attend_only(xml) {
			var x;
			var txt = "GetUserAttendOnly: ";

			var success = false;
			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
			{
				txt += "Result = " + x[0].childNodes[0].nodeValue;
				success = (x[0].childNodes[0].nodeValue == "OK");
			}

			// AttendOnly
			var attend_only = false;
			x = xml.getElementsByTagName("Value");
			if (x.length > 0 && x[0].childNodes[0].length > 0)
				attend_only = (x[0].childNodes[0].nodeValue == "Yes");

			if (success)
			{
				document.getElementById("attend_only").checked = attend_only;
			}

			set_result(txt);
		}

		function get_user_attend_only() {

			if (!check_user_id(document.getElementById("user_id").value))
				return;

			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetUserAttendOnly";
			messageElem.appendChild(requestElem);
			
			var useridElem = doc.createElement("UserID");
			useridElem.innerHTML = document.getElementById("user_id").value;
			messageElem.appendChild(useridElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_user_attend_only(xml) {
			var x;
			var txt = "SetUserAttendOnly: ";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt += "Result = " + x[0].childNodes[0].nodeValue;

			x = xml.getElementsByTagName("Error");
			if (x.length > 0)
				txt += ",  Error = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_user_attend_only() {

			if (!check_user_id(document.getElementById("user_id").value))
				return;

			var doc = document.implementation.createDocument("", "", null);

			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetUserAttendOnly";
			messageElem.appendChild(requestElem);

			var useridElem = doc.createElement("UserID");
			useridElem.innerHTML = document.getElementById("user_id").value;
			messageElem.appendChild(useridElem);

			var attendOnlyElem = doc.createElement("Value");
			if (document.getElementById("attend_only").checked)
				attendOnlyElem.innerHTML = "Yes";
			else
				attendOnlyElem.innerHTML = "No";
			messageElem.appendChild(attendOnlyElem);

   			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_get_user_message(xml) {
			var x;
			var txt = "GetUserMessage: ";
			var user_message = '';

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
			{
				txt += "Result = " + x[0].childNodes[0].nodeValue;
				if (x[0].childNodes[0].nodeValue == 'OK')
				{
					x = xml.getElementsByTagName("UserMessage");
					if (x.length > 0)
						user_message = utf16Decode(atob(x[0].childNodes[0].nodeValue))
				}
			}

			document.getElementById("txtUserMessage").value = user_message;
			set_result(txt);
		}

		function get_user_message() {
			if (!check_user_id(document.getElementById("user_id").value))
				return;

			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetUserMessage";
			messageElem.appendChild(requestElem);
			
			var useridElem = doc.createElement("UserID");
			useridElem.innerHTML = document.getElementById("user_id").value;
			messageElem.appendChild(useridElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_user_message(xml) {
			var x;
			var txt = "SetUserMessage: ";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt += "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function validate_msg_to_device(msg_str)
		{
			return msg_str.length > 100 ? msg_str.substring(0, 100) : msg_str;
		}

		function set_user_message() {
			if (!check_user_id(document.getElementById("user_id").value))
				return;

			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetUserMessage";
			messageElem.appendChild(requestElem);

			var useridElem = doc.createElement("UserID");
			useridElem.innerHTML = document.getElementById("user_id").value;
			messageElem.appendChild(useridElem);

			{
				var paramElem = doc.createElement("UserMessage");
				paramElem.innerHTML = btoa(utf16Encode(validate_msg_to_device(document.getElementById("txtUserMessage").value)));
				messageElem.appendChild(paramElem);
			}
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function validate_color_from_device(color_str)
		{
			return (parseInt(color_str, 16) & 0xFFFFFF).toString(16).toUpperCase().padStart(6, '0');
		}
		function validate_color_to_device(color_str)
		{
			return 'FF' + (parseInt(color_str, 16) & 0xFFFFFF).toString(16).toUpperCase().padStart(6, '0');
		}

		function on_get_user_message_color(xml) {
			var x;
			var txt = "GetUserMessageColor: ";
			var message_color = '0';
			var message_bk_color = '0';

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
			{
				txt += "Result = " + x[0].childNodes[0].nodeValue;
				if (x[0].childNodes[0].nodeValue == 'OK')
				{
					x = xml.getElementsByTagName("MessageColor");
					if (x.length > 0)
						message_color = validate_color_from_device(String(xml.getElementsByTagName("MessageColor")[0].childNodes[0].nodeValue));
					x = xml.getElementsByTagName("MessageBkColor");
					if (x.length > 0)
						message_bk_color = validate_color_from_device(String(xml.getElementsByTagName("MessageBkColor")[0].childNodes[0].nodeValue));
				}
			}

			document.getElementById("txtMessageColor").value = message_color;
			document.getElementById("txtMessageBkColor").value = message_bk_color;
			set_result(txt);
		}

		function get_user_message_color() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetUserMessageColor";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_user_message_color(xml) {
			var x;
			var txt = "SetUserMessageColor: ";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt += "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_user_message_color() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetUserMessageColor";
			messageElem.appendChild(requestElem);

			{
				var paramElem = doc.createElement("MessageColor");
				paramElem.innerHTML = validate_color_to_device(document.getElementById("txtMessageColor").value);
				messageElem.appendChild(paramElem);
			}
			{
				var paramElem = doc.createElement("MessageBkColor");
				paramElem.innerHTML = validate_color_to_device(document.getElementById("txtMessageBkColor").value);
				messageElem.appendChild(paramElem);
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
				else if (res == "GetUserAttendOnly")
					on_get_user_attend_only(xml);
				else if (res == "SetUserAttendOnly")
					on_set_user_attend_only(xml);
				else if (res == "GetUserMessage")
					on_get_user_message(xml);
				else if (res == "SetUserMessage")
					on_set_user_message(xml);
				else if (res == "GetUserMessageColor")
					on_get_user_message_color(xml);
				else if (res == "SetUserMessageColor")
					on_set_user_message_color(xml);
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
			<td>UserID:</td>
			<td><input type="text" id="user_id" /></td>
		</tr>
		<tr>
			<td></td>
			<td>
				<table>
					<tr>
						<td>AttendOnly:</td>
						<td><input type="checkbox" id="attend_only" checked/></td>
					</tr>
				</table>
			</td>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="get_user_attend_only(); return false;">Get</button></td>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="set_user_attend_only(); return false;">Set</button></td>
		</tr>
		<tr>
			<td></td>
			<td>
				<table>
					<tr>
						<td>User Message: (for M91)</td>
					</tr>
					<tr>
						<td>
							<textarea id="txtUserMessage" rows="5" cols="100"></textarea>
						</td>
					</tr>
				</table>
			</td>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="get_user_message(); return false;">Get</button></td>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="set_user_message(); return false;">Set</button></td>
		</tr>
		<tr>
			<td colspan="2">
				<table>
					<tr>
						<td>Message Color (RGB, HEX) :</td>
						<td>
							<input type="text" id="txtMessageColor" />
						</td>
					</tr>
					<tr>
						<td>Message Back Color (RGB, HEX) :</td>
						<td>
							<input type="text" id="txtMessageBkColor" />
						</td>
					</tr>
				</table>
			</td>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="get_user_message_color(); return false;">Get</button></td>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="set_user_message_color(); return false;">Get</button></td>
		</tr>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>