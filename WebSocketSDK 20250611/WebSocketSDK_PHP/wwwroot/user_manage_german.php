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

		function on_get_user_balance_time(xml) {
			var x;
			var txt = "GetUserBalanceTime: ";
			var balance_time = 0;

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
			{
				txt += "Result = " + x[0].childNodes[0].nodeValue;
				if (x[0].childNodes[0].nodeValue == 'OK')
				{
					x = xml.getElementsByTagName("BalanceTimeInMinues");

					if (x.length > 0)
						balance_time = parseInt(x[0].childNodes[0].nodeValue);
				}
			}

			document.getElementById("txtBalanceHour").value = Math.floor(balance_time / 60);
			document.getElementById("txtBalanceMinute").value = balance_time % 60;
			set_result(txt);
		}

		function get_user_balance_time() {
			if (!check_user_id(document.getElementById("user_id").value))
				return;

			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetUserBalanceTime";
			messageElem.appendChild(requestElem);
			
			var useridElem = doc.createElement("UserID");
			useridElem.innerHTML = document.getElementById("user_id").value;
			messageElem.appendChild(useridElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_user_balance_time(xml) {
			var x;
			var txt = "SetUserBalanceTime: ";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt += "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_user_balance_time() {
			if (!check_user_id(document.getElementById("user_id").value))
				return;

			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetUserBalanceTime";
			messageElem.appendChild(requestElem);

			var useridElem = doc.createElement("UserID");
			useridElem.innerHTML = document.getElementById("user_id").value;
			messageElem.appendChild(useridElem);

			{
				var paramElem = doc.createElement("BalanceTimeInMinues");
				var balancetime = parseInt(document.getElementById("txtBalanceHour").value) * 60 + parseInt(document.getElementById("txtBalanceMinute").value);
				if (balancetime < 0 || balancetime > 65535)
				{
					set_result("Balance time should be in range 00:00~1092:15");
					return;
				}
				paramElem.innerHTML = balancetime;
				messageElem.appendChild(paramElem);
			}
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_get_user_holidays(xml) {
			var x;
			var txt = "GetUserHolidays: ";
			var holidays_in_10 = 0;

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
			{
				txt += "Result = " + x[0].childNodes[0].nodeValue;
				if (x[0].childNodes[0].nodeValue == 'OK')
				{
					x = xml.getElementsByTagName("HolidaysInDays10");

					if (x.length > 0)
						holidays_in_10 = parseInt(x[0].childNodes[0].nodeValue);
				}
			}

			document.getElementById("txtHolidays").value = holidays_in_10 / 10;
			set_result(txt);
		}

		function get_user_holidays() {
			if (!check_user_id(document.getElementById("user_id").value))
				return;

			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetUserHolidays";
			messageElem.appendChild(requestElem);
			
			var useridElem = doc.createElement("UserID");
			useridElem.innerHTML = document.getElementById("user_id").value;
			messageElem.appendChild(useridElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_user_holidays(xml) {
			var x;
			var txt = "SetUserHolidays: ";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt += "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_user_holidays() {
			if (!check_user_id(document.getElementById("user_id").value))
				return;

			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetUserHolidays";
			messageElem.appendChild(requestElem);

			var useridElem = doc.createElement("UserID");
			useridElem.innerHTML = document.getElementById("user_id").value;
			messageElem.appendChild(useridElem);

			{
				var paramElem = doc.createElement("HolidaysInDays10");
				var holidays = parseInt(parseFloat(document.getElementById("txtHolidays").value) * 10);
				if (holidays < 0 || holidays > 65535)
				{
					set_result("Holidays should be in range 0.0~6553.5");
					return;
				}

				paramElem.innerHTML = holidays;
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
				else if (res == "GetUserMessage")
					on_get_user_message(xml);
				else if (res == "SetUserMessage")
					on_set_user_message(xml);
				else if (res == "GetUserBalanceTime")
					on_get_user_balance_time(xml);
				else if (res == "SetUserBalanceTime")
					on_set_user_balance_time(xml);
				else if (res == "GetUserHolidays")
					on_get_user_holidays(xml);
				else if (res == "SetUserHolidays")
					on_set_user_holidays(xml);
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
						<td>User Message:</td>
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
			<td></td>
			<td>Balance Time:</td>
		</tr>
		<tr>
			<td></td>
			<td>
			<input type="text" id="txtBalanceHour" style="width:50px"/>
			:
			<input type="text" id="txtBalanceMinute" style="width:50px"/>
			</td>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="get_user_balance_time(); return false;">Get</button></td>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="set_user_balance_time(); return false;">Set</button></td>
		</tr>
		<tr>
			<td></td>
			<td>Holidays:</td>
		</tr>
		<tr>
			<td></td>
			<td>
			<input type="text" id="txtHolidays" style="width:200px"/>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="get_user_holidays(); return false;">Get</button></td>
			<td style="margin: 0px; padding: 0px">
				<button style="width:210px" onclick="set_user_holidays(); return false;">Set</button></td>
		</tr>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>