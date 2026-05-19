<?php

use Workerman\Timer;

class WsConnection
{
	public $_id = 0;
	public $_session_id = null;
	public $_dev_sn = null;
	public $_cloud_id = null;
	public $_last_alive_time = 0;
	public $_timer = null;
	public $_conn = null;
	public $_host_session_id = null;
	public static $connections = array();
	public $_updated_user_ids = null;
	public $_updated_user_count = 0;

    public function __construct($id, $conn)
    {
        $this->_id = $id;
		$this->_session_id = WsConnection::generate_session_id();
		$this->_conn = $conn;
		$this->_last_alive_time = time();
		$this->_updated_user_ids = array();
		$this->_updated_user_count = 0;

		static::$connections[$id] = $this;
    }

    public function __destruct()
    {
    }

	public static function generate_session_id()
	{
		return sprintf( '%04x%04x-%04x-%04x-%04x-%04x%04x%04x',
			// 32 bits for "time_low"
			mt_rand( 0, 0xffff ), mt_rand( 0, 0xffff ),

			// 16 bits for "time_mid"
			mt_rand( 0, 0xffff ),

			// 16 bits for "time_hi_and_version",
			// four most significant bits holds version number 4
			mt_rand( 0, 0x0fff ) | 0x4000,

			// 16 bits, 8 bits for "clk_seq_hi_res",
			// 8 bits for "clk_seq_low",
			// two most significant bits holds zero and one for variant DCE1.1
			mt_rand( 0, 0x3fff ) | 0x8000,

			// 48 bits for "node"
			mt_rand( 0, 0xffff ), mt_rand( 0, 0xffff ), mt_rand( 0, 0xffff )
		);
	}

	public static function getConnectionBySession($session_id)
	{
		foreach (static::$connections as $c)
		{
			if (strcmp($c->_session_id, $session_id) == 0)
				return $c;
		}
		return null;
	}

	public static function clearConnectionBySession($session_id, $id_to_ignore)
	{
		foreach (static::$connections as $c)
		{
			if (strcmp($c->_session_id, $session_id) == 0 && $c->_id != $id_to_ignore)
			{
				echo "==remove duplicated session. " . $session_id . "   id = " . $c->_id . "\n";
				unset(WsConnection::$connections[$c->_id]);
			}
		}
		return null;
	}

	public static function check_timeouted_connections()
	{
		$cur_time = time();
		foreach (static::$connections as $c)
		{
			if ($cur_time - $c->_last_alive_time > 120)
			{
				echo "=connection timeouted. " . $c->_session_id . "\n";
				$c->_conn->close();
				unset(WsConnection::$connections[$c->_id]);
			}
		}
		return null;
	}

	public static function dump_connections()
	{
		echo "============== connections ==============\n";
		echo "No\tSessionId\t\t\t\tDev SN\n";
		foreach(static::$connections as $c)
		{
			echo $c->_id . "\t" . $c->_session_id . "\t" . $c->_dev_sn . "\n";
		}
		echo "\n\n";
	}

	public static function sendData($ws_conn, $data)
	{
		echo "==>TX to ". $ws_conn->_session_id . ":\n" . $data . "\n\n";

		$ws_conn->_conn->send($data);
	}

	public function send_error_message($err)
	{
		$xml = new DOMDocument("1.0");
		$xml->formatOutput=true;
		
		$m = $xml->createElement('Message');
		$xml->appendChild($m);
		
		$m->appendChild($xml->createElement("Response", "ErrorReport"));
		$m->appendChild($xml->createElement("Error", $err));
	
		$retdata = $xml->saveXML();
		WsConnection::sendData($this, $retdata);
	}

	public function on_request_timeouted()
	{
		echo "==request timeouted. session = " . $this->_session_id. "\n";

		$host_conn = WsConnection::getConnectionBySession($this->_host_session_id);

		$this->_conn->close();
		$this->stop_request_timeout_monitor();
		unset(WsConnection::$connections[$this->_id]);

		if ($host_conn != null)
			$host_conn->send_error_message("Reuqest Timeouted");
	}

	public function start_request_timeout_monitor()
	{
		$c = $this;
		$this->_timer = Timer::add(30, function () use ($c) {
			$c->on_request_timeouted();});
	}

	public function stop_request_timeout_monitor()
	{
		if ($this->_timer != null)
		{
			Timer::del($this->_timer);
			$this->_timer = null;
		}
	}

	public function add_updated_user_id($userid)
	{
		$found = false;
		foreach($this->_updated_user_ids as $u)
		{
			if (strcmp($userid, $u) == 0)
			{
				$found = true;
				break;
			}
		}
		if ($found)
			return;

		$this->_updated_user_ids[$this->_updated_user_count] = $userid;
		$this->_updated_user_count++;
	}

	public function clear_updated_user_ids()
	{
		$this->_updated_user_ids = array();
		$this->_updated_user_count = 0;
	}
}
