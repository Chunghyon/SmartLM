<html>
<head>
	<meta charset="utf-8">
	<link rel="stylesheet" type="text/css" href="css/style.css">
	<script src="js/xml.js"></script>
	<script src="js/ws_client.js"></script>
	<script type="text/javascript">

		var timer = null;
		var timer_working = false;
		var next_log_id = 0;
		var step = "";
		var ready = false;
		var canceled = false;

		var glog_total_count = 0;
		var glog_count = 0;
		var log_result = "";

		function on_error_report(xml) {
			var err = "";
			var x = xml.getElementsByTagName("Error");
			if (x.length > 0)
				err = "Error: " + x[0].childNodes[0].nodeValue;
			if (err != "")
				document.getElementById("result").innerHTML = err;

			if (timer_working)
				timer_working = false;
		}

		function check_user_id(str) {

			if (str.length < 1)
			{
				return false;
			}

			return true;
		}

		function get_next_time_log(pos) {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetNextGlog";
			messageElem.appendChild(requestElem);

			var posElem = doc.createElement("BeginLogPos");
			posElem.innerHTML = pos;
			messageElem.appendChild(posElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
		}

		function on_get_time_log(xml) {
			var x;
			var txt = "";
		
			var logid = 0;
			var UtcTimezoneMinutes = NaN;
			var timestr = "";
			var userid = "";
			var action ="";
			var attendstat = "";
			var apstat = "";
			var jobcode = "";
			var photo = "No";
			var attend_only = "";
			var expired = "";
			var result = "";
			var latitude = "";
			var longitude = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				result = x[0].childNodes[0].nodeValue;

			if (result == "OK")
			{
				x = xml.getElementsByTagName("LogID");
				if (x.length > 0)
					logid = parseInt(x[0].childNodes[0].nodeValue);
			
				x = xml.getElementsByTagName("UtcTimezoneMinutes");
				if (x.length > 0)
					UtcTimezoneMinutes = parseInt(x[0].childNodes[0].nodeValue);
			
				x = xml.getElementsByTagName("Time");
				if (x.length > 0)
					timestr = x[0].childNodes[0].nodeValue.replace("-T", " ").replace("Z", "");

				x = xml.getElementsByTagName("UserID");
				if (x.length > 0)
					userid = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("Action");
				if (x.length > 0)
					action = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("AttendStat");
				if (x.length > 0)
					attendstat = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("APStat");
				if (x.length > 0)
					apstat = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("JobCode");
				if (x.length > 0)
					jobcode = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("Photo");
				if (x.length > 0)
					photo = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("AttendOnly");
				if (x.length > 0)
					attend_only = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("Expired");
				if (x.length > 0)
					expired = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("Latitude");
				if (x.length > 0)
					latitude = x[0].childNodes[0].nodeValue;

				x = xml.getElementsByTagName("Longitude");
				if (x.length > 0)
					longitude = x[0].childNodes[0].nodeValue;

				glog_count = glog_count + 1;
				
				log_str = "LogID(" + logid + ")" + 
						", UTC(" + (isNaN(UtcTimezoneMinutes) ? "---" : UtcTimezoneMinutes) + ")" + 
						", Time(" + timestr + ")" + 
						", UserID(" + userid + ")" + 
						", AttendStat(" + attendstat + ")" + 
						", Action(" + action + ")" + 
						", Photo(" + photo + ")";
				if (attend_only == "Yes")
						log_str += ", AttendOnly(Yes)";
				if (expired == "Yes")
						log_str += ", Expired(Yes)";

				if (latitude != "")
						log_str += ", Latitude(" + latitude + ")";

				if (longitude != "")
						log_str += ", Longitude(" + longitude + ")";

				log_result += "<tr><td>" + glog_count + "</td><td> " + log_str + "</td></tr>";
				if (timer_working && !canceled)
				{
					next_log_id = logid + 1;
					txt = "Get Glog: Read " + glog_count + "/" + glog_total_count;
					step = "get_next_glog";
					ready = true;
				}
				else 
				{
					txt = "Reading : " + glog_count;
					get_next_time_log(logid + 1);
				}				
			}
			else if (result == "Fail")
			{
				if (timer_working)
				{
					step = "enable_device";
					ready = true;
				}

				txt = "Read Glog Finished. Total Count: " + glog_count;
				document.getElementById("log_list").innerHTML = log_result;
			}
			else
			{
				txt = "Read Glog Fail : Result = " + result;
				if (timer_working)
				{
					step = "enable_device";
					ready = true;
				}
			}

			set_result(txt);
		}

		function get_time_log() {
			var cond = "";
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetFirstGlog";
			messageElem.appendChild(requestElem);

			var posElem = doc.createElement("BeginLogPos");
			posElem.innerHTML = "0";
			messageElem.appendChild(posElem);

			cond = "[Search Result] ";
            cond += "UserId: ";

			var user_id = document.getElementById("user_id").value;
			if (check_user_id(user_id)) {
				var useridElem = doc.createElement("UserID");
				useridElem.innerHTML = document.getElementById("user_id").value;
				messageElem.appendChild(useridElem);
				cond += document.getElementById("user_id").value;
			} else {
				cond += "(Any)";
			}

			var stime = document.getElementById("start_year").value.padStart(4, '0') + "-" +
				document.getElementById("start_month").value.padStart(2, '0') + "-" +
				document.getElementById("start_day").value.padStart(2, '0') + "T" +
				document.getElementById("start_hour").value.padStart(2, '0') + ":" +
				document.getElementById("start_minute").value.padStart(2, '0') + ":" +
				"00";
			var stime_len = document.getElementById("start_year").value.length + 
				document.getElementById("start_month").value.length + 
				document.getElementById("start_day").value.length + 
				document.getElementById("start_hour").value.length + 
				document.getElementById("start_minute").value.length;

			cond += ",  StartTime: ";

			if (stime_len > 0 && !isNaN(Date.parse(stime))) {
				var stimeElem = doc.createElement("StartTime");
				stimeElem.innerHTML = stime.replace("T", "-T") + "Z";
				messageElem.appendChild(stimeElem);
				cond += stime.replace("T", " ");
			} else {
				cond += "(from First log)";
			}

			var etime = document.getElementById("end_year").value.padStart(4, '0') + "-" +
				document.getElementById("end_month").value.padStart(2, '0') + "-" +
				document.getElementById("end_day").value.padStart(2, '0') + "T" +
				document.getElementById("end_hour").value.padStart(2, '0') + ":" +
				document.getElementById("end_minute").value.padStart(2, '0') + ":" +
				"59";
			var etime_len = document.getElementById("end_year").value.length + 
				document.getElementById("end_month").value.length + 
				document.getElementById("end_day").value.length + 
				document.getElementById("end_hour").value.length + 
				document.getElementById("end_minute").value.length;

			cond += ",  EndTime: ";
			if (etime_len > 0 && !isNaN(Date.parse(etime))) {
				var etimeElem = doc.createElement("EndTime");
				etimeElem.innerHTML = etime.replace("T", "-T") + "Z";
				messageElem.appendChild(etimeElem);
				cond += etime.replace("T", " ");
			} else {
				cond += "(to Last log)";
			}

			glog_count = 0;
			log_result = "<tr><th colspan='2'>" + cond + "</th></tr>";
			log_result += "<tr><th>No</th><th>Content</th></tr>";

			document.getElementById("log_list").innerHTML = "";

			send_relay_message(doc, document.getElementById("session").value, messageElem);
		}

		function on_enable_device(xml) {
			var x;
			var txt = "";
			x = xml.getElementsByTagName("Result");
			if (x.length > 0 && 
				x[0].childNodes.length > 0 &&
				x[0].childNodes[0].nodeValue == "OK")
			{
				if (step == "disable_device")
				{
					if (!canceled)
					{
						ready = true;
						txt = "Read Glog: Diable device";
						step = "get_glog_count";
					}
				}
				else if (step == "enable_device")
				{
					timer_working = false;
					txt = document.getElementById("result").innerHTML;
				}
			}
			else 
			{
				txt = "Read Glog: Failed";
				timer_working = false;
			}

			set_result(txt);
		}

		function send_enable_device(enable) {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "EnableDevice";
			messageElem.appendChild(requestElem);
			
			var enableElem = doc.createElement("Enable");
			enableElem.innerHTML = enable;
			messageElem.appendChild(enableElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			if (enable == "No")
				set_result("");
		}

		function on_get_glog_pos_info(xml) {
			var x;
			var txt = "Get Glog Pos Info OK: ";
		
			glog_total_count = 0;
			x = xml.getElementsByTagName("LogCount");
			if (x.length > 0)
			{
				txt += "LogCount = " + x[0].childNodes[0].nodeValue;
				glog_total_count = parseInt(x[0].childNodes[0].nodeValue);
			}
			
			x = xml.getElementsByTagName("MaxCount");
			if (x.length > 0)
				txt += ",  MaxCount = " + x[0].childNodes[0].nodeValue;

			if (timer_working)
			{
				ready = true;
				step = "get_first_glog";
				txt = "Read Glog: Get Glog Count OK, Count = " + glog_total_count;
			}

			set_result(txt);
		}

		function get_glog_pos_info() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetGlogPosInfo";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			if (!timer_working)
				set_result("");
		}

		function on_delete_glog_info(xml) {
			var x;
			var txt = "Delete Time Log: ";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt += "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function delete_time_log() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "DeleteGlogWithPos";
			messageElem.appendChild(requestElem);
			
			var endposElem = doc.createElement("EndPos");
			endposElem.innerHTML = parseInt(document.getElementById("end_pos").value);
			messageElem.appendChild(endposElem);

			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_timer() {
			if (!timer_working)
			{
				document.getElementById("timer_button").innerHTML = "Get Time Log(Timer)";
				if (canceled)
					set_result("Get Glog: Canceled");

				cleanInterval(timer);
				timer = null;
				return;
			}

			if (!ready)
				return;

			ready = false;
			if (step == "disable_device")
				send_enable_device("No");			
			else if (step == "get_glog_count")
				get_glog_pos_info();
			else if (step == "get_first_glog")
				get_time_log();
			else if (step == "get_next_glog")
				get_next_time_log(next_log_id);
			else if (step == "enable_device")
				send_enable_device("Yes");
		}

		function on_timer_click() {
			if (timer_working)
			{
				canceled = true;
				step = "enable_device";
				ready = true;
			}
			else
			{
				canceled = false;
				timer_working = true;
				step = "disable_device";
				ready = true;
				timer = setInterval(on_timer, 100);
				document.getElementById("timer_button").innerHTML = "Stop";
				document.getElementById("log_list").innerHTML = "";
			}			
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
				else if (res == "EnableDevice")
					on_enable_device(xml)
				else if (res == "GetFirstGlog" || res == "GetNextGlog")
					on_get_time_log(xml);
				else if (res == "GetGlogPosInfo")
					on_get_glog_pos_info(xml);
				else if (res == "DeleteGlogWithPos")
					on_delete_glog_info(xml);
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
			<td colspan="2"><button style="width:210px" onclick="get_time_log(); return false;">Get Time Log</button></td>
		</tr>
		<tr>
			<td>User ID:</td>
			<td><input type="text" id='user_id'/></td>
			<td><button style="width:210px" onclick="get_glog_pos_info(); return false;">Get Time Log Pos Info</button></td>
		</tr>
		<tr>
			<td>Start Time:</td>
			<td>
				<input type="text" style="width:60px;" id='start_year' value="2020"/>&nbsp;-&nbsp;
				<input type="text" style="width:40px;" id='start_month' value="01"/>&nbsp;-&nbsp;
				<input type="text" style="width:40px;" id='start_day'/ value="01">&nbsp;&nbsp;
				<input type="text" style="width:40px;" id='start_hour' value="00"/>&nbsp;:&nbsp;
				<input type="text" style="width:40px;" id='start_minute' value="00"/>
			</td>
			<td><button style="width:210px" onclick="delete_time_log(); return false;">Delete Time Log</button></td>
			<td>End Pos:</td>
			<td><input type="text" style="width:100px" id='end_pos'/></td>
		</tr>
		<tr>
			<td>End Time:</td>
			<td>
				<input type="text" style="width:60px;" id='end_year'/>&nbsp;-&nbsp;
				<input type="text" style="width:40px;" id='end_month'/>&nbsp;-&nbsp;
				<input type="text" style="width:40px;" id='end_day'/>&nbsp;&nbsp;
				<input type="text" style="width:40px;" id='end_hour'/>&nbsp;:&nbsp;
				<input type="text" style="width:40px;" id='end_minute'/>
			</td>
		</tr>
		<tr>
			<td colspan="2"><button id="timer_button" style="width:210px" onclick="on_timer_click(); return false;">Get Time Log(Timer)</button></td>
		</tr>
	</table>

	<table id="log_list">
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>