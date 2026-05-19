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

		function on_get_video_streaming(xml) {
			var x;
			var rtsp_enable = false;
			var rtsp_resolution = '0';
			var rtsp_bitrate_mbps = '0';
			var err = "";
			var item = "VideoStreaming Setting";

			x = xml.getElementsByTagName("Error");
			if (x.length > 0)
			{
				err = "Get " + item + " OK: Error = " + x[0].childNodes[0].nodeValue;
			}
			else
			{
				x = xml.getElementsByTagName("Result");
				if (x.length > 0 && x[0].childNodes[0].nodeValue != 'OK')
				{
					err = "Get " + item + " Fail: Result = " + x[0].childNodes[0].nodeValue;
				}
				else
				{
					rtsp_enable = String(xml.getElementsByTagName("rtsp_enable")[0].childNodes[0].nodeValue) == '1';
					rtsp_resolution = String(xml.getElementsByTagName("rtsp_resolution")[0].childNodes[0].nodeValue);
					rtsp_bitrate_mbps = String(xml.getElementsByTagName("rtsp_bitrate_mbps")[0].childNodes[0].nodeValue);
					err = "Get " + item + " OK";
				}
			}

			document.getElementById("rtsp_enable").checked = rtsp_enable;
			document.getElementById("rtsp_resolution").value = rtsp_resolution;
			document.getElementById("rtsp_bitrate_mbps").value = rtsp_bitrate_mbps;
			set_result(err);
		}

		function get_video_streaming() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetVideoStreamSetting";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_video_streaming(xml) {
			var x;
			var ret = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt = "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_video_streaming() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetVideoStreamSetting";

			{
				var paramElem = doc.createElement("rtsp_enable");
				paramElem.innerHTML = document.getElementById("rtsp_enable").checked ? "1" : "0";
				messageElem.appendChild(paramElem);
			}
			{
				var paramElem = doc.createElement("rtsp_resolution");
				paramElem.innerHTML = document.getElementById("rtsp_resolution").value;
				messageElem.appendChild(paramElem);
			}
			{
				var paramElem = doc.createElement("rtsp_bitrate_mbps");
				paramElem.innerHTML = document.getElementById("rtsp_bitrate_mbps").value;
				messageElem.appendChild(paramElem);
			}

			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function validate_color_from_device(color_str)
		{
			return (parseInt(color_str, 16) & 0xFFFFFF).toString(16).toUpperCase().padStart(6, '0');
		}

		function on_get_center_screen_message(xml) {
			var x;
			var verify_disable = false;
			var center_screen_message = '0';
			var center_screen_message_color = '0';
			var center_screen_message_border_color = '0';
			var err = "";
			var item = "CenterScreenMessage Setting";

			x = xml.getElementsByTagName("Error");
			if (x.length > 0)
			{
				err = "Get " + item + " OK: Error = " + x[0].childNodes[0].nodeValue;
			}
			else
			{
				x = xml.getElementsByTagName("Result");
				if (x.length > 0 && x[0].childNodes[0].nodeValue != 'OK')
				{
					err = "Get " + item + " Fail: Result = " + x[0].childNodes[0].nodeValue;
				}
				else
				{
					verify_disable = String(xml.getElementsByTagName("verify_disable")[0].childNodes[0].nodeValue) == '1';
					center_screen_message = utf16Decode(atob(xml.getElementsByTagName("center_screen_message")[0].childNodes[0].nodeValue))
					center_screen_message_color = validate_color_from_device(String(xml.getElementsByTagName("center_screen_message_color")[0].childNodes[0].nodeValue));
					center_screen_message_border_color = validate_color_from_device(String(xml.getElementsByTagName("center_screen_message_border_color")[0].childNodes[0].nodeValue));
					err = "Get " + item + " OK";
				}
			}

			document.getElementById("verify_disable").checked = verify_disable;
			document.getElementById("center_screen_message").value = center_screen_message;
			document.getElementById("center_screen_message_color").value = center_screen_message_color;
			document.getElementById("center_screen_message_border_color").value = center_screen_message_border_color;
			set_result(err);
		}

		function get_center_screen_message() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "GetCenterScreenMessage";
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_center_screen_message(xml) {
			var x;
			var ret = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt = "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function validate_msg_to_device(msg_str)
		{
			return msg_str.length > 100 ? msg_str.substring(0, 100) : msg_str;
		}

		function validate_color_to_device(color_str)
		{
			return 'FF' + (parseInt(color_str, 16) & 0xFFFFFF).toString(16).toUpperCase().padStart(6, '0');
		}
		
		function set_center_screen_message() {
			var doc = document.implementation.createDocument("", "", null);
			
			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			requestElem.innerHTML = "SetCenterScreenMessage";

			{
				var paramElem = doc.createElement("center_screen_message");
				paramElem.innerHTML = btoa(utf16Encode(validate_msg_to_device(document.getElementById("center_screen_message").value)));
				messageElem.appendChild(paramElem);
			}
			{
				var paramElem = doc.createElement("center_screen_message_color");
				paramElem.innerHTML = validate_color_to_device(document.getElementById("center_screen_message_color").value);
				messageElem.appendChild(paramElem);
			}
			{
				var paramElem = doc.createElement("center_screen_message_border_color");
				paramElem.innerHTML = validate_color_to_device(document.getElementById("center_screen_message_border_color").value);
				messageElem.appendChild(paramElem);
			}
			{
				var paramElem = doc.createElement("verify_disable");
				paramElem.innerHTML = document.getElementById("verify_disable").checked ? "1" : "0";
				messageElem.appendChild(paramElem);
			}

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
				else if (res == "GetVideoStreamSetting")
					on_get_video_streaming(xml);
				else if (res == "SetVideoStreamSetting")
					on_set_video_streaming(xml);
				else if (res == "GetCenterScreenMessage")
					on_get_center_screen_message(xml);
				else if (res == "SetCenterScreenMessage")
					on_set_center_screen_message(xml);
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
			<th colspan="2">Video Streaming</th>
		</tr>
		<tr>
			<td><button onclick="get_video_streaming(); return false;">Get VideoStreaming Setting</button></td>
			<td><button onclick="set_video_streaming(); return false;">Set VideoStreaming Setting</button></td>
			<tr>
				<td>RTSP Enable:</td>
				<td><input type="checkbox" id='rtsp_enable'/></td>
			</tr>
			<tr>
				<td>RTSP resolution:</td>
				<td>
					<select id="rtsp_resolution" style="width:250px;">
						<?php 
							echo "<option value='0'>1920x1080</option>";
							echo "<option value='1'>1280x720</option>";
							echo "<option value='2'>960x540</option>";
							echo "<option value='3'>640x360</option>";
						?>
					</select>
				</td>
			</tr>
			<tr>
				<td>RTSP bitrate (Mbps):</td>
				<td>
					<select id="rtsp_bitrate_mbps" style="width:250px;">
						<?php 
							echo "<option value='0'>5 (default)</option>";
							for ($i = 1; $i <= 20; $i++)
								echo "<option value='$i'>$i</option>";
						?>
					</select>
				</td>
			</tr>
		</tr>

		<tr>
			<th colspan="2">Center Screen Message</th>
		</tr>
		<tr>
			<td><button onclick="get_center_screen_message(); return false;">Get CenterScreenMessage</button></td>
			<td><button onclick="set_center_screen_message(); return false;">Set CenterScreenMessage</button></td>
			<tr>
				<td colspan="2"><textarea id="center_screen_message" rows="5" cols="100"></textarea></td>
			</tr>
			<tr>
				<td>Verify Disable:</td>
				<td><input type="checkbox" id='verify_disable'/></td>
			</tr>
			<tr>
				<td>Text Color (RGB, HEX) :</td>
				<td><input type="text" id="center_screen_message_color" /></td>
			</tr>
			<tr>
				<td>Text Border Color  (RGB, HEX) :</td>
				<td><input type="text" id="center_screen_message_border_color" /></td>
			</tr>
		</tr>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>