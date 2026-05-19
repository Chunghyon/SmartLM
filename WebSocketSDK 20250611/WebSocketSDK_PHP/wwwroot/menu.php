<html>
<head>
	<meta charset="utf-8">
	<link rel="stylesheet" type="text/css" href="css/style.css">
</head>
<body>
	<form name='frm'>
		<ul>
			<li><button style="width:240px" onclick="frm.action='user_manage.php'; document.frm.submit(); return false;">User Manage</button></li>
			<br><li><button style="width:240px" onclick="frm.action='user_manage_custom.php'; document.frm.submit(); return false;">User Manage (Custom)</button></li>
			<br><li><button style="width:240px" onclick="frm.action='user_manage_german.php'; document.frm.submit(); return false;">User Manage (M91 German)</button></li>
			<br><li><button style="width:240px" onclick="frm.action='dept_manage.php'; document.frm.submit(); return false;">Department</button></li>
			<br><li><button style="width:240px" onclick="frm.action='auto_attendance.php'; document.frm.submit(); return false;">Auto Attendance</button></li>
			<br><li><button style="width:240px" onclick="frm.action='ac_timezone.php'; document.frm.submit(); return false;">Access TimeZone</button></li>
			<br><li><button style="width:240px" onclick="frm.action='bell_time.php'; document.frm.submit(); return false;">Bell Time</button></li>
			<br><li><button style="width:240px" onclick="frm.action='dev_info.php'; document.frm.submit(); return false;">Device Info</button></li>
			<br><li><button style="width:240px" onclick="frm.action='ethernet_setting.php'; document.frm.submit(); return false;">Network Setting</button></li>
			<br><li><button style="width:240px" onclick="frm.action='wifi_setting.php'; document.frm.submit(); return false;">WiFiSetting</button></li>
			<br><li><button style="width:240px" onclick="frm.action='clear_data.php'; document.frm.submit(); return false;">Empty Log, Enroll Data</button></li>
			<br><li><button style="width:240px" onclick="frm.action='time_log.php'; document.frm.submit(); return false;">TimeLog</button></li>
			<br><li><button style="width:240px" onclick="frm.action='ntp_server.php'; document.frm.submit(); return false;">NTP Server</button></li>
			<br><li><button style="width:240px" onclick="frm.action='server_setting.php'; document.frm.submit(); return false;">Server Settings</button></li>
			<br><li><button style="width:240px" onclick="frm.action='fw_upgrade.php'; document.frm.submit(); return false;">Firmware Upgrade</button></li>
			<br><li><button style="width:240px" onclick="frm.action='video_streaming.php'; document.frm.submit(); return false;">Video Streaming</button></li>
		</ul>
		<input type="hidden" name='session' value='<?php echo $_GET["session"]; ?>'></input>
	</form>
</body>
</html>