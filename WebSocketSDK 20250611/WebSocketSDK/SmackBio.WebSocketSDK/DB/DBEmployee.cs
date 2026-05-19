using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SmackBio.WebSocketSDK.M50;

namespace SmackBio.WebSocketSDK.DB
{
    public class Fingerprint
    {
        public const int FP_SIZE = 1404;
        public bool enrolled = false;
        public Byte[] fp_info = null;
        public bool duress = false;
    }

    public enum UserPrivilege
    {
        NormalUser,
        Manager,
        Administrator,
        COUNT,
    }

    public enum UserBackup 
    {
		BackupFingerStart	= 0,	
		BackupFingerEnd		= 9,
		BackupPassword		= 10,
		BackupCard			= 11,
		BackupMessage		= 12,
		BackupFace			= 13,	
		BackupQR            = 14,
	};

    public class Face
    {
        public const int FACE_SIZE = 27668;
        public bool enrolled = false;
        public byte[] face_data;
    }

    public class UserInfo
    {
        public const int MAX_PHOTO_SIZE_8K = 8 * 1024;
		public const int MAX_PHOTO_SIZE_32K = 32 * 1024;
        public const int MAX_PHOTO_SIZE_64K = 64 * 1024;
        public UserInfo()
        {
            fingerprints = new Fingerprint[M50Device.MAX_FINGERS_PER_USER];
            for (int i = 0; i < M50Device.MAX_FINGERS_PER_USER; i++)
                fingerprints[i] = new Fingerprint();
            face = new Face();
        }

        public Int64 user_id { get; set; }
        public string name { get; set; }
        public bool enabled { get; set; }
        public UserPrivilege privilege { get; set; }
        public int timeset1 { get; set; }
        public int timeset2 { get; set; }
        public int timeset3 { get; set; }
        public int timeset4 { get; set; }
        public int timeset5 { get; set; }
        
        public bool period_use { get; set; }
        public DateTime period_start { get; set; }
        public DateTime period_end { get; set; }

        public UInt32 card { get; set; }
        public string password { get; set; }
        public UInt32 qr { get; set; }
        public Byte depart { get; set; }
        public Fingerprint[] fingerprints;
        public Face face;
        
        public UInt16 enroll_mask 
        {
            get
            {
                int mask = 0;

                if (password != null && password.Length > 0)
                    mask |= (1 << Convert.ToInt32(UserBackup.BackupPassword));

                if (card > 0)
                    mask |= (1 << Convert.ToInt32(UserBackup.BackupCard));

                if (qr > 0)
                    mask |= (1 << Convert.ToInt32(UserBackup.BackupQR));

                for (int i = Convert.ToInt32(UserBackup.BackupFingerStart); i <= Convert.ToInt32(UserBackup.BackupFingerEnd); i++)
                {
                    if (fingerprints[i].enrolled)
                        mask |= (1 << Convert.ToInt32(i));
                }

                if (face.enrolled)
                    mask |= (1 << Convert.ToInt32(UserBackup.BackupFace));

                return Convert.ToUInt16(mask);
            }
            set
            {
                for (int i = Convert.ToInt32(UserBackup.BackupFingerStart); i <= Convert.ToInt32(UserBackup.BackupFingerEnd); i++)
                    fingerprints[i - Convert.ToInt32(UserBackup.BackupFingerStart)].enrolled = ((value & (1 << Convert.ToInt32(i))) > 0);

                face.enrolled = ((value & (1 << Convert.ToInt32(UserBackup.BackupFace))) > 0);
            }
        }
        public UInt16 duress_mask { get { return 0; } }
        
        public UInt32 timezone
        {
            get
            {
                int ret = 0;
                ret |= timeset1 << 0;
                ret |= timeset2 << 6;
                ret |= timeset3 << 12;
                ret |= timeset4 << 18;
                ret |= timeset5 << 24;
                return Convert.ToUInt32(ret);
            }
            set
            {
                timeset1 = Convert.ToInt32(value & 0x3F);
                value >>= 6;
                timeset2 = Convert.ToInt32(value & 0x3F);
                value >>= 6;
                timeset3 = Convert.ToInt32(value & 0x3F);
                value >>= 6;
                timeset4 = Convert.ToInt32(value & 0x3F);
                value >>= 6;
                timeset5 = Convert.ToInt32(value & 0x3F);
            }
        }
        public byte[] name_bytes
        {
            get
            {
                byte[] value = new byte[(M50Device.UserNameLength + 1) * 2];
                if (name == null)
                {
                    for (int i = 0; i < value.Length; i++)
                        value[i] = 0;
                }
                else
                {
                    byte[] array = Encoding.Unicode.GetBytes(name);

                    for (int i = 0; i < value.Length; i++)
                        if (i < array.Length)
                            value[i] = array[i];

                    value[value.Length - 1] = 0;
                    value[value.Length - 2] = 0;
                }
                return value;
            }
            set
            {
                name = new String(Encoding.Unicode.GetChars(value));
            }
        }
    }
}
