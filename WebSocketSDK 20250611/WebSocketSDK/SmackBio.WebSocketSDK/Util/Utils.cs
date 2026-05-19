using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmackBio.WebSocketSDK.DB;
using SmackBio.WebSocketSDK.M50;

namespace SmackBio.WebSocketSDK.Util
{
    public class Utils
    {
        public static string Utc2Str(int UtcTimezoneMinutes)
        {
            if (UtcTimezoneMinutes == Int32.MaxValue)
            {
                return "---";   // Invalid
            }
            else if (UtcTimezoneMinutes == 0)
            {
                return "";      // Utc
            }
            else
            {
                return String.Format("{0}{1:D2}:{2:D2}",
                    UtcTimezoneMinutes < 0 ? "-" :"+",
                    Math.Abs(UtcTimezoneMinutes) / 60,
                    Math.Abs(UtcTimezoneMinutes) % 60);
            }
        }
        public static string DateTime2string(DateTime time)
        {
            return time.ToString("yyyy-MM-dd-THH:mm:ssZ");
        }

        public static DateTime ParseDateTime(string time)
        {
            time = time.Replace("-T", " ");
            time = time.Replace("Z", "");
            return DateTime.Parse(time);
        }
/*
        public static bool EnableDevice(string device_model, string device_sn)
        {
            CmdEnableDevice command = new CmdEnableDevice(device_model, device_sn, true);
            BaseMessage response;
            CommandExeResult result;
            if (GetServer().SendCommand(device_model, device_sn, command, out response, out result, false) &&
                result == CommandExeResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DisableDevice(string device_model, string device_sn)
        {
            CmdEnableDevice command = new CmdEnableDevice(device_model, device_sn, false);
            BaseMessage response;
            CommandExeResult result;
            if (GetServer().SendCommand(device_model, device_sn, command, out response, out result, false) &&
                result == CommandExeResult.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DateTime? GetDeviceTime(string device_model, string device_sn)
        {
            CmdGetTime command = new CmdGetTime(device_model, device_sn);
            BaseMessage response;
            CommandExeResult result;

            if (GetServer().SendCommand(device_model, device_sn, command, out response, out result, false) &&
                result == CommandExeResult.OK)
            {
                return ((CmdGetTimeResponse)response).time;
            }

            return null;
        }

        public static bool SetDeviceTime(string device_model, string device_sn)
        {
            CmdSetTime command = new CmdSetTime(device_model, device_sn);
            BaseMessage response;
            CommandExeResult result;
            if (Utils.GetServer().SendCommand(device_model, device_sn, command, out response, out result, false) &&
                result == CommandExeResult.OK)
            {
                return true;
            }
            else
                return false;
        }

        public static string GetUserPassword(string device_model, string device_sn, uint user_id)
        {
            CmdGetUserPassword command = new CmdGetUserPassword(device_model, device_sn, user_id);
            BaseMessage response;
            CommandExeResult result;

            if (Utils.GetServer().SendCommand(device_model, device_sn, command, out response, out result, false) &&
                result == CommandExeResult.OK)
            {
                return ((CmdGetUserPasswordResponse)response).Password;
            }

            return null;
        }

        public static EthernetSetting GetEthernetSetting(string device_model, string device_sn)
        {
            CmdGetEthernetSetting command = new CmdGetEthernetSetting(device_model, device_sn);
            BaseMessage response;
            CommandExeResult result;

            if (Utils.GetServer().SendCommand(device_model, device_sn, command, out response, out result, false) &&
                result == CommandExeResult.OK)
            {
                return ((CmdGetEthernetSettingResponse)response).setting;
            }

            return null;
        }

        public static bool SetEthernetSetting(string device_model, string device_sn, EthernetSetting setting)
        {
            CmdSetEthernet command = new CmdSetEthernet(device_model, device_sn, setting);
            BaseMessage response;
            CommandExeResult result;

            if (Utils.GetServer().SendCommand(device_model, device_sn, command, out response, out result, false) &&
                result == CommandExeResult.OK)
            {
                return true;
            }

            return false;
        }

        public static Finger GetFingerData(string device_model, string device_sn, uint user_id, int fp_no)
        {
            CmdGetFingerData command = new CmdGetFingerData(device_model, device_sn, user_id, fp_no, true);
            BaseMessage response;
            CommandExeResult result;

            if (Utils.GetServer().SendCommand(device_model, device_sn, command, out response, out result, true) &&
                result == CommandExeResult.OK)
            {
                CmdGetFingerDataResponse fp_resp = ((CmdGetFingerDataResponse)response);
                Finger finger = new Finger();
                finger.temporary = false;
                finger.fp_no = fp_resp.FingerNo;
                finger.duress = fp_resp.Duress;
                finger.emp_id = fp_resp.UserId;
                finger.fp_info = fp_resp.FingerData;

                return finger;
            }

            return null;
        }

        public static bool SetFingerData(string device_model, string device_sn, Finger finger, UserPrivilege privilege)
        {
            CmdSetFingerData command = new CmdSetFingerData(device_model, device_sn, finger, privilege, true);
            BaseMessage response;
            CommandExeResult result;

            if (Utils.GetServer().SendCommand(device_model, device_sn, command, out response, out result, true) &&
                result == CommandExeResult.OK)
            {
                return true;
            }

            return false;
        }

        public static Face GetFaceData(string device_model, string device_sn, uint user_id)
        {
            CmdGetFaceData command = new CmdGetFaceData(device_model, device_sn, user_id);
            BaseMessage response;
            CommandExeResult result;

            if (Utils.GetServer().SendCommand(device_model, device_sn, command, out response, out result, true) &&
                result == CommandExeResult.OK)
            {
                CmdGetFaceDataResponse face_resp = ((CmdGetFaceDataResponse)response);
                return face_resp.FaceData;
            }

            return null;
        }

        public static bool SetFaceData(string device_model, string device_sn, Face face, UserPrivilege privilege)
        {
            CmdSetFaceData command = new CmdSetFaceData(device_model, device_sn, face, privilege, true);
            BaseMessage response;
            CommandExeResult result;

            if (Utils.GetServer().SendCommand(device_model, device_sn, command, out response, out result, true) &&
                result == CommandExeResult.OK)
            {
                return true;
            }

            return false;
        }*/

        private static String invert_string(String str)
	    {
            char[] chArray = str.ToCharArray();
            Array.Reverse(chArray);
            return new String(chArray);
	    }
        public static UInt32 convert_string_to_password(String str, UInt32 max_len)
	    {
		    UInt32	ret = 0;

		    if (str == null || str.Length > max_len)
			    return 0;

		    String p = invert_string(str);
            foreach (char c in p)
            {
			    ret <<= 4;
			    if (c >= '0' && c <= '9')
				    ret += (Convert.ToUInt32(c - '0') + 1);
			    else
				    return 0;
		    }
		    return ret;
	    }
        public static String convert_password_to_string(UInt32 password, UInt32 max_len)
	    {
            String ret = "";
            char c;
		    int		i;
		    UInt32	val = password;

		    if (password == 0)
			    return ret;

		    i = 7;
		    while ((val & (0xF << i * 4)) == 0)
			    i--;
		    while (i >= 0) {
			    c = Convert.ToChar(val & 0x0F);
			    if (c >= 1 && c <= 10)
                {
                    c = (char)('0' + c - 1);
                    ret += c.ToString();
                }
			    val >>= 4;
			    i --;
		    }

		    return ret;
	    }
        public static void WriteUserInfo(System.IO.BinaryWriter bw, UserInfo userinfo)
        {
            bw.Write(userinfo.user_id);             // 8bytes
            bw.Write(userinfo.card);                // 4bytes
            bw.Write(convert_string_to_password(userinfo.password, M50Device.MaxUserPasswordLength)); // 4bytes

            bw.Write(userinfo.enroll_mask);         // 2bytes
            bw.Write(userinfo.duress_mask);         // 2bytes

            UInt32 photo_id = 0;
            bw.Write(photo_id);                     // 4bytes

            bw.Write(Convert.ToByte(userinfo.enabled));     // 1byte
            bw.Write(Convert.ToByte(userinfo.privilege));   // 1byte
            bw.Write(userinfo.depart);                      // 1byte
            Byte message_id = 0;
            bw.Write(message_id);                           // 1byte                // for compatibility with U-disk down/upload

            bw.Write(userinfo.timezone);            // 4bytes

            bw.Write(userinfo.name_bytes);          // (24+1)*2 = 50bytes

            byte[] padding = new byte[6];
            for (int i = 0; i < 6; i++)
                padding[i] = 0;
            bw.Write(padding, 0, padding.Length);   // 6bytes
                                                                // Total : 88 bytes

            // Not compatible with U-disk down/upload
            bw.Write(Convert.ToByte(userinfo.period_use));     // 1byte
            bw.Write(((userinfo.period_start.Year - 2000) << 16) + (userinfo.period_start.Month << 8) + userinfo.period_start.Day);      // 4bytes
            bw.Write(((userinfo.period_end.Year - 2000) << 16) + (userinfo.period_end.Month << 8) + userinfo.period_end.Day);            // 4bytes
            bw.Write(userinfo.qr);                // 4bytes
        }
        public static UserInfo ReadUserInfo(System.IO.BinaryReader br)
        {
            UserInfo userinfo = new DB.UserInfo();

            userinfo.user_id = br.ReadInt64();
            userinfo.card = br.ReadUInt32();
            userinfo.password = convert_password_to_string(br.ReadUInt32(), M50Device.MaxUserPasswordLength);

            userinfo.enroll_mask = br.ReadUInt16();
            UInt16 duress_mask = br.ReadUInt16();

            UInt32 photo_id = br.ReadUInt32();

            userinfo.enabled = Convert.ToBoolean(br.ReadByte());
            userinfo.privilege = (UserPrivilege)br.ReadByte();
            userinfo.depart = br.ReadByte();
            Byte message_id = br.ReadByte();                    // for compatibility with U-disk down/upload

            userinfo.timezone = br.ReadUInt32();
   
            userinfo.name_bytes = br.ReadBytes(Convert.ToInt32((M50Device.UserNameLength + 1) * 2));

            byte[] padding = br.ReadBytes(6);
                                // Total : 88 bytes

            // Not compatible with U-disk down/upload
            userinfo.period_use = Convert.ToBoolean(br.ReadByte());
            int yy, mm, dd;
            Int32 v = br.ReadInt32(); yy = v >> 16; mm = (v & 0xFF00) >> 8; dd = v & 0xFF;
            userinfo.period_start = new DateTime(yy + 2000, mm, dd);
            v = br.ReadInt32(); yy = v >> 16; mm = (v & 0xFF00) >> 8; dd = v & 0xFF;
            userinfo.period_end = new DateTime(yy + 2000, mm, dd);

            userinfo.qr = br.ReadUInt32();
            return userinfo;
        }
    }

}
