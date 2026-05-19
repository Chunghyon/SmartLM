<?php

class DevEvent
{
	public $_id = 0;
	public $_dev_sn = null;
	public $_event = null;
	public $_time = null;
	public $_UtcTimezoneMinutes = null;
	public $_logid = null;
	public $_userid = null;
	public $_adminid = null;
	public $_action = null;
	public $_attendstat = null;
	public $_apstat = null;
	public $_jobcode = 0;
	public $_photo = null;
	public $_latitude = null;
	public $_longitude = null;
	public $_attend_only = false;
	public $_expired = false;

	public static $events = array();
	public static $counter = 0;
	public static $delete_pos = 0;
    public function __construct($dev_sn, $event, $time, $UtcTimezoneMinutes, $logid, $userid, $adminid, $action, $stat, $apstat, $jobcode, $photo, $attend_only, $expired, $latitude, $longitude)
    {
		if (count(static::$events) >= 2000) // Maximum 2000 events stored in server memory
		{
			unset(static::$events[static::$delete_pos]);
			static::$delete_pos++;
		}

        $this->_id = static::$counter;
		static::$counter++;

		$this->_dev_sn = $dev_sn;
		$this->_event = $event;
		$this->_time = $time;
		$this->_UtcTimezoneMinutes = $UtcTimezoneMinutes;
		$this->_logid = $logid;
		$this->_userid = $userid;
		$this->_adminid = $adminid;
		$this->_action = $action;
		$this->_attendstat = $stat;
		$this->_apstat = $apstat;
		$this->_jobcode = $jobcode;
		$this->_photo = $photo;
		$this->_attend_only = $attend_only;
		$this->_expired = $expired;
		$this->_latitude = $latitude;
		$this->_longitude = $longitude;

		static::$events[$this->_id] = $this;
    }

    public function __destruct()
    {
    }

	public static function clear_all()
	{
		static::$counter = 0;
		static::$delete_pos = 0;
		static::$events = array();		
	}

	public function get_string()
	{
		$ret = "[" . $this->_event ."]";
		if ($this->_event == "AdminLog_v2")
		{
			$ret = $ret . " LogID(" .$this->_logid . ")";
			$ret = $ret . ", Time(" .$this->_time . ")";
			$ret = $ret . ", AdminID(" .$this->_adminid . ")";
			$ret = $ret . ", UserID(" .$this->_userid . ")";
			$ret = $ret . ", Action(" .$this->_action . ")";
			$ret = $ret . ", Stat(" .$this->_attendstat . ")";
		}
		else if ($this->_event == "TimeLog_v2")
		{
			$ret = $ret . " LogID(" .$this->_logid . ")";
			$ret = $ret . ", UTC(" . (($this->_UtcTimezoneMinutes == '') ? "---" : $this->_UtcTimezoneMinutes) . ")";
			$ret = $ret . ", Time(" .$this->_time . ")";
			$ret = $ret . ", UserID(" .$this->_userid . ")";
			$ret = $ret . ", AttendStat(" .$this->_attendstat . ")";
			$ret = $ret . ", Action(" .$this->_action . ")";
			$ret = $ret . ", JobCode(" .$this->_jobcode . ")";
			$ret = $ret . ", Photo(" .$this->_photo . ")";
			if ($this->_attend_only)
				$ret = $ret . ", AttendOnly(Yes)";
			if ($this->_expired)
				$ret = $ret . ", Expired(Yes)";
			if ($this->_latitude != null && !empty($this->_latitude))
				$ret = $ret . ", Latitude(" . $this->_latitude . ")";
			if ($this->_longitude != null && !empty($this->_longitude))
				$ret = $ret . ", Longitude(" . $this->_longitude . ")";
		}
		else if ($this->_event == "KeepAlive")
		{
			$ret = $ret . " DevTime(" .$this->_time . ")";
		}

		return $ret;
	}
}
