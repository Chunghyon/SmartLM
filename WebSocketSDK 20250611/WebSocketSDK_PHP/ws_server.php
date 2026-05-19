<?php

use Workerman\Worker;

require_once __DIR__ . '/vendor/autoload.php';
include "ws_connection.php";
include "ws_dev_event.php";
include "config.inc.php";

// set your timezone
date_default_timezone_set("Asia/Shanghai");

$context = [
    'ssl' => [
        'local_cert'  => 'server.pem',
        'local_pk'    => 'server.key',
        'verify_peer' => false,
    ]
];

// Create a Websocket server
if ($Use_WSS)
{
	$ws_worker = new Worker('websocket://0.0.0.0:' . $Port, $context);
	$ws_worker->transport = 'ssl';
}
else
{
	$ws_worker = new Worker('websocket://0.0.0.0:' . $Port);
}


function onBrowserGetOnlineDevices($ws_conn, $xml)
{
	$retxml = new DOMDocument("1.0");
	$retxml->formatOutput=true;

	$m = $retxml->createElement('Message');
	$retxml->appendChild($m);

	$m->appendChild($retxml->createElement("Response", $xml->Request));

	WsConnection::dump_connections();

	foreach(WsConnection::$connections as $c)
	{
		if ($c->_dev_sn != "")
		{
			$dev = $retxml->createElement("Device");
			$dev->appendChild($retxml->createElement("SN", $c->_dev_sn));
			$dev->appendChild($retxml->createElement("Session", $c->_session_id));
			$dev->appendChild($retxml->createElement("CloudId", $c->_cloud_id));
			$m->appendChild($dev);
		}
	}

	$m->appendChild($retxml->createElement("Result", "OK"));
	$retdata = $retxml->saveXML();

	WsConnection::sendData($ws_conn, $retdata);
}

function onBrowserGetDeviceEvents($ws_conn, $xml)
{
	$retxml = new DOMDocument("1.0");
	$retxml->formatOutput=true;

	$m = $retxml->createElement('Message');
	$retxml->appendChild($m);

	$m->appendChild($retxml->createElement("Response", $xml->Request));

	foreach(DevEvent::$events as $e)
	{
		$ev = $retxml->createElement("Event");
		$ev->appendChild($retxml->createElement("SN", $e->_dev_sn));
		$ev->appendChild($retxml->createElement("Data", $e->get_string()));
		$m->appendChild($ev);
	}

	$m->appendChild($retxml->createElement("Result", "OK"));
	$retdata = $retxml->saveXML();

	WsConnection::sendData($ws_conn, $retdata);
}

function onBrowserClearDeviceEvents($ws_conn, $xml)
{
	DevEvent::clear_all();

	$retxml = new DOMDocument("1.0");
	$retxml->formatOutput=true;

	$m = $retxml->createElement('Message');
	$retxml->appendChild($m);

	$m->appendChild($retxml->createElement("Response", $xml->Request));
	$m->appendChild($retxml->createElement("Result", "OK"));

	$retdata = $retxml->saveXML();
	WsConnection::sendData($ws_conn, $retdata);
}

function onBrowserGetUpdatedUserIds($ws_conn, $xml)
{
	WsConnection::check_timeouted_connections();
	$dev_conn = WsConnection::getConnectionBySession($xml->Device);
	if ($dev_conn != null)
	{
		$retxml = new DOMDocument("1.0");
		$retxml->formatOutput=true;

		$m = $retxml->createElement('Message');
		$retxml->appendChild($m);

		$m->appendChild($retxml->createElement("Response", $xml->Request));

		foreach($dev_conn->_updated_user_ids as $u)
		{
			$user = $retxml->createElement("User", $u);
			$m->appendChild($user);
		}

		$m->appendChild($retxml->createElement("Result", "OK"));
		
		$retdata = $retxml->saveXML();
		WsConnection::sendData($ws_conn, $retdata);
	}
	else
	{
		$ws_conn->send_error_message("Device Disconnected");
	}
}

function onBrowserClearUpdatedUserIds($ws_conn, $xml)
{
	WsConnection::check_timeouted_connections();
	$dev_conn = WsConnection::getConnectionBySession($xml->Device);
	if ($dev_conn != null)
	{
		$dev_conn->clear_updated_user_ids();

		$retxml = new DOMDocument("1.0");
		$retxml->formatOutput=true;

		$m = $retxml->createElement('Message');
		$retxml->appendChild($m);

		$m->appendChild($retxml->createElement("Response", $xml->Request));
		$m->appendChild($retxml->createElement("Result", "OK"));
		
		$retdata = $retxml->saveXML();
		WsConnection::sendData($ws_conn, $retdata);
	}
	else
	{
		$ws_conn->send_error_message("Device Disconnected");
	}
}

function onRegister($ws_conn, $xml)
{
	$ws_conn->_dev_sn = $xml->DeviceSerialNo;
	$ws_conn->_cloud_id = $xml->CloudId;

	$retxml = new DOMDocument("1.0");
	$retxml->formatOutput=true;

	$m = $retxml->createElement('Message');
	$retxml->appendChild($m);

	$m->appendChild($retxml->createElement("Response", $xml->Request));
	$m->appendChild($retxml->createElement("DeviceSerialNo", $ws_conn->_dev_sn));
	$m->appendChild($retxml->createElement("Token", $ws_conn->_session_id));
	$m->appendChild($retxml->createElement("Result", "OK"));

	$retdata = $retxml->saveXML();
	WsConnection::sendData($ws_conn, $retdata);
}

function onLogin($ws_conn, $xml)
{
	$ws_conn->_session_id = $xml->Token;
	$ws_conn->_dev_sn = $xml->DeviceSerialNo;
	$ws_conn->_cloud_id = $xml->CloudId;

	$retxml = new DOMDocument("1.0");
	$retxml->formatOutput=true;

	$m = $retxml->createElement('Message');
	$retxml->appendChild($m);

	$m->appendChild($retxml->createElement("Response", $xml->Request));
	$m->appendChild($retxml->createElement("DeviceSerialNo", $ws_conn->_dev_sn));

	$token_exist = true;	// NOTE: Add the Login logic you need.
	$login_result = true;	//		 Ex, retrieve the database, ...
	if ($login_result)
	{
		$m->appendChild($retxml->createElement("Result", "OK"));
	}
	else
	{
		if (!$token_exist)
			$m->appendChild($retxml->createElement("Result", "FailUnknownToken"));
		else
			$m->appendChild($retxml->createElement("Result", "Fail"));
	}

	$retdata = $retxml->saveXML();
	WsConnection::sendData($ws_conn, $retdata);

	WsConnection::clearConnectionBySession($ws_conn->_session_id, $ws_conn->_id);
}

function extractTime($str) {
	$str = str_replace("-T", " ", $str);
	$str = str_replace("Z", "", $str);
	return $str;
}
function onDeviceEvent($ws_conn, $xml)
{
	$dev_sn = null;
	$event = null;
	$time = null;
	$UtcTimezoneMinutes = null;
	$logid = null;
	$userid = null;
	$adminid = null;
	$action = null;
	$attendstat = null;
	$apstat = null;
	$jobcode = 0;
	$photo = null;
	$latitude = null;
	$longitude = null;
	$attend_only = false;
	$expired = false;

	$dev_sn = $xml->DeviceSerialNo;
	$event = $xml->Event;

	$retxml = new DOMDocument("1.0");
	$retxml->formatOutput=true;

	$m = $retxml->createElement('Message');
	$retxml->appendChild($m);

	$m->appendChild($retxml->createElement("Response", $event));

	if ($event == "AdminLog_v2")
	{
		$time = extractTime($xml->Time);
		$userid = $xml->UserID;
		$adminid = $xml->AdminID;
		$action = $xml->Action;
		$logid = $xml->LogID;
		$attendstat = $xml->Stat;

		if ($action == "BackupFP" ||
			$action == "SetPWD" ||
			$action == "EnrollCard" ||
			$action == "DeleteUser" ||
			$action == "DeleteFP" ||
			$action == "DeletePWD" ||
			$action == "DeleteCard" ||
			$action == "UserTZSet" ||
			$action == "EnrollUser" ||
			$action == "EnrollFace" ||
			$action == "DeleteFace")
			$ws_conn->add_updated_user_id($userid);

		$m->appendChild($retxml->createElement("TransID", $xml->TransID));
	}
	else if ($event == "TimeLog_v2")
	{
		$time = extractTime($xml->Time);
		$UtcTimezoneMinutes = $xml->UtcTimezoneMinutes;
		$userid = $xml->UserID;
		$logid = $xml->LogID;
		$action = $xml->Action;
		$attendstat = $xml->AttendStat;
		$apstat = $xml->APStat;
		$jobcode = $xml->JobCode;
		$photo = $xml->Photo;
		$latitude = $xml->Latitude;
		$longitude = $xml->Longitude;
		$attend_only = ($xml->AttendOnly == "Yes");
		$expired = ($xml->Expired == "Yes");

		$m->appendChild($retxml->createElement("TransID", $xml->TransID));
	}
	else if ($event == "KeepAlive")
	{
		$time = extractTime($xml->DevTime);
		$m->appendChild($retxml->createElement("DevTime", $xml->DevTime));
		$m->appendChild($retxml->createElement("ServerTime", date("Y-m-d") . "-T" . date("H:i:s") . "Z"));
	}
	
	$ev = new DevEvent($dev_sn, $event, $time, $UtcTimezoneMinutes, $logid, $userid, $adminid, $action, $attendstat, $apstat, $jobcode, $photo, $attend_only, $expired, $latitude, $longitude);

	$m->appendChild($retxml->createElement("Result", "OK"));

	$retdata = $retxml->saveXML();
	WsConnection::sendData($ws_conn, $retdata);
}

// Emitted when new connection come
$ws_worker->onConnect = function ($connection) {
    echo "New connection\n";
	$conn = new WsConnection($connection->id, $connection);
};

// Emitted when data received
$ws_worker->onMessage = function ($connection, $data) {

	$cur_conn = WsConnection::$connections[$connection->id];
	$cur_conn->_last_alive_time = time();

	echo "<==RX from ". $cur_conn->_session_id . ":\n" . $data . "\n\n";

	try {
		$xml=simplexml_load_string($data);
		if ($xml == false)
		{
			echo "failed to parse XML.\n";
			return;
		}
		
		if ($xml->UserID_Base36 != "")
			$xml->UserID = $xml->UserID_Base36;
		if ($xml->AdminID_Base36 != "")
			$xml->AdminID = $xml->AdminID_Base36;

		if ($xml->Session != "")
		{
			WsConnection::check_timeouted_connections();
			$dev_conn = WsConnection::getConnectionBySession($xml->Session);
			if ($dev_conn != null)
			{
				if ($dev_conn->_timer == null) // If request already running, ignore second one
				{
					$retxml = new DOMDocument("1.0");	
					$retxml->formatOutput=true;
					$data = $retxml->saveXML() . $xml->Message->saveXML();
					
					$dev_conn->_conn->send($data);
					$dev_conn->_host_session_id = $cur_conn->_session_id; // messages from device should be relayed to this session
					$dev_conn->start_request_timeout_monitor();
				}
			}
			else
			{
				$cur_conn->send_error_message("Device Disconnected");
			}
		}
		else if ($xml->Request != "")
		{
			switch($xml->Request) {
			case "browserGetOnlineDevices": // get all sessions from Browser
				WsConnection::check_timeouted_connections();
				onBrowserGetOnlineDevices($cur_conn, $xml);
				break;
			case "browserGetDeviceEvents": // get all events from Browser
				onBrowserGetDeviceEvents($cur_conn, $xml);
				break;
			case "browserClearAllDeviceEvents": // clear all events from Browser
				onBrowserClearDeviceEvents($cur_conn, $xml);
				break;
			case "browserGetAllUpdatedUsers": // get all update users from Browser
				onBrowserGetUpdatedUserIds($cur_conn, $xml);
				break;
			case "browserClearAllUpdatedUsers": // clear all update users from Browser
				onBrowserClearUpdatedUserIds($cur_conn, $xml);
				break;
			case "Register":		// register cmd from device
				onRegister($cur_conn, $xml);
				break;
			case "Login":			// login cmd from device
				onLogin($cur_conn, $xml);
				break;
			}
		}
		else if ($xml->Response != "") // all response messages should be relayed to Browser
		{
			if ($cur_conn->_host_session_id != null)
			{
				$host_conn = WsConnection::getConnectionBySession($cur_conn->_host_session_id);
				if ($host_conn != null)
					WsConnection::sendData($host_conn, $xml->saveXML());
			}
			$cur_conn->stop_request_timeout_monitor();
		}
		else if ($xml->Event != "")
		{
			onDeviceEvent($cur_conn, $xml);
		}
	} catch (\Exception $e) {
		echo "Exception in " . $e.getFile() . ":" . $e.getLine() . "\n";
		echo $e.getMessage();
		echo "\n";
	} catch (\Error $e) {
		echo "Error in " . $e.getFile() . ":" . $e.getLine() . "\n";
		echo $e.getMessage();
		echo "\n";
	}
};

$ws_worker->onWebSocketPing = function($connection, $data) {
	$cur_conn = WsConnection::$connections[$connection->id];
	$cur_conn->_last_alive_time = time();
	echo "Ping\n";
	$connection->send($data);
};

// Emitted when connection closed
$ws_worker->onClose = function ($connection) {
	echo "Connection closed\n";
    unset(WsConnection::$connections[$connection->id]);
};

// Run worker
Worker::runAll();
?>