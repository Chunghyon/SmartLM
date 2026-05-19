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

		function on_get_dept(xml, is_proxy) {
			var x;
			var name = "";
			var err = "";
			var item = "Department";
			if (is_proxy)
				item = "Proxy Department";

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
					x = xml.getElementsByTagName("Name");
					if (x.length > 0)
						name = utf16Decode(atob(x[0].childNodes[0].nodeValue));
					err = "Get " + item + " OK";
				}
			}

			if (is_proxy)
				document.getElementById("proxy_name").value = name;
			else
				document.getElementById("dept_name").value = name;
			set_result(err);
		}

		function get_dept(is_proxy) {
			var doc = document.implementation.createDocument("", "", null);

			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			if (is_proxy)
			{				
				requestElem.innerHTML = "GetProxyDept";
				var paramElem = doc.createElement("ProxyNo");
				paramElem.innerHTML = document.getElementById("proxy_id").value;
				messageElem.appendChild(paramElem);
			}
			else
			{
				requestElem.innerHTML = "GetDepartment";
				var paramElem = doc.createElement("DeptNo");
				paramElem.innerHTML = document.getElementById("dept_id").value;
				messageElem.appendChild(paramElem);
			}
			messageElem.appendChild(requestElem);
			
			send_relay_message(doc, document.getElementById("session").value, messageElem);
			set_result("");
		}

		function on_set_dept(xml) {
			var x;
			var ret = "";

			x = xml.getElementsByTagName("Result");
			if (x.length > 0)
				txt = "Result = " + x[0].childNodes[0].nodeValue;

			set_result(txt);
		}

		function set_dept(is_proxy) {
			var doc = document.implementation.createDocument("", "", null);

			var messageElem = doc.createElement("Message");
			var requestElem = doc.createElement("Request");
			var valueElem = doc.createElement("Data");
			if (is_proxy)
			{
				var paramElem = doc.createElement("ProxyNo");
				paramElem.innerHTML = document.getElementById("proxy_id").value;
				messageElem.appendChild(paramElem);
				requestElem.innerHTML = "SetProxyDept";
				valueElem.innerHTML = btoa(utf16Encode(document.getElementById("proxy_name").value));
			}
			else
			{
				var paramElem = doc.createElement("DeptNo");
				paramElem.innerHTML = document.getElementById("dept_id").value;
				messageElem.appendChild(paramElem);
				requestElem.innerHTML = "SetDepartment";
				valueElem.innerHTML = btoa(utf16Encode(document.getElementById("dept_name").value));
			}

			messageElem.appendChild(requestElem);
			messageElem.appendChild(valueElem);

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
				else if (res == "GetDepartment")
					on_get_dept(xml, false);
				else if (res == "GetProxyDept")
					on_get_dept(xml, true);
				else if (res == "SetDepartment")
					on_set_dept(xml);
				else if (res == "SetProxyDept")
					on_set_dept(xml);
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
			<th colspan="2">User Department</th>
		</tr>
		<tr>
			<td><button onclick="get_dept(false); return false;">Get Department</button></td>
			<td>
				<select id="dept_id" style="width:250px;">
					<?php 
						for ($i = 0; $i < 100; $i++)
							echo "<option value='$i'>$i</option>";
					?>
				</select>
			</td>
		</tr>
		<tr>
			<td><button onclick="set_dept(false); return false;">Set Department</button></td>
			<td><input type='text' id='dept_name' style="width:250px;"/></td>
		</tr>

		<tr>
			<th colspan="2">Proxy Department</th>
		</tr>
		<tr>
			<td><button onclick="get_dept(true); return false;">Get Proxy Dept</button></td>
			<td>
				<select id="proxy_id" style="width:250px;">
					<?php 
						for ($i = 0; $i < 32; $i++)
							echo "<option value='$i'>$i</option>";
					?>
				</select>
			</td>
		</tr>
		<tr>
			<td><button onclick="set_dept(true); return false;">Set Proxy Dept</button></td>
			<td><input type='text' id='proxy_name' style="width:250px;"/></td>
		</tr>
	</table>

	<input type="hidden" id='session' name='session' value='<?php echo $_GET["session"]; ?>'></input>
</body>
</html>