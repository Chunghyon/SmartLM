using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using SmackBio.WebSocketSDK.Cmd;

namespace SmackBio.WebSocketSDK.M50.Cmd
{
    public enum RemoteEnrollBackupType {
        RemoteEnrollFace,
        RemoteEnrollFP,
        RemoteEnrollCard,
    }
    public class CmdRemoteEnroll : CmdBase
    {
        public const string MSG_KEY = "RemoteEnroll";

        Int64 user_id;
        RemoteEnrollBackupType backup;
        string fp_no;
        public CmdRemoteEnroll(Int64 user_id, RemoteEnrollBackupType backup, string fp_no = null)
            : base()
        {
            this.user_id = user_id;
            this.backup = backup;
            this.fp_no = fp_no;
        }

        public override string Build()
        {
            string result = StartBuild();
            AppendTag(ref result, TAG_REQUEST, MSG_KEY);
            AppendTag(ref result, "UserID", user_id);
            AppendTag(ref result, "Backup", backup.ToString());

            if (backup == RemoteEnrollBackupType.RemoteEnrollFP)
            {
                if (fp_no != null && fp_no.Length > 0)
                    AppendTag(ref result, "FingerNo", fp_no);
            }
            AppendEndup(ref result);

            return result;
        }
        public override Type GetResponseType()
        {
            return typeof(CmdRemoteEnrollResponse);
        }
    }

    public enum RemoteEnrollResult
    {
        Success,
        InvalidBackup,
        EnrollNumberError,
        DatabaseFull,
        FaceAlreadyEnrolled,
        FPAllEnrolled,
        FPAlreadyEnrolled,
        InvalidFingerNumber,
        CardAlreadyEnrolled,
        MenuProcessing,
        RemoteEnrollAlreadyStarted,
        Unknown,
    }

    public class CmdRemoteEnrollResponse : Response
    {
        public RemoteEnrollResult remote_enroll_result;

        public RemoteEnrollResult ParseRemoteEnrollResult(XmlDocument doc)
        {
            switch (ParseTag(doc, "ResultCode"))
            {
                case "Success":
                    remote_enroll_result = RemoteEnrollResult.Success;
                    break;
                case "InvalidBackup":
                    remote_enroll_result = RemoteEnrollResult.InvalidBackup;
                    break;
                case "EnrollNumberError":
                    remote_enroll_result = RemoteEnrollResult.EnrollNumberError;
                    break;
                case "FaceAlreadyEnrolled":
                    remote_enroll_result = RemoteEnrollResult.FaceAlreadyEnrolled;
                    break;
                case "FPAllEnrolled":
                    remote_enroll_result = RemoteEnrollResult.FPAllEnrolled;
                    break;
                case "FPAlreadyEnrolled":
                    remote_enroll_result = RemoteEnrollResult.FPAlreadyEnrolled;
                    break;
                case "InvalidFingerNumber":
                    remote_enroll_result = RemoteEnrollResult.InvalidFingerNumber;
                    break;
                case "CardAlreadyEnrolled":
                    remote_enroll_result = RemoteEnrollResult.CardAlreadyEnrolled;
                    break;
                case "DatabaseFull":
                    remote_enroll_result = RemoteEnrollResult.DatabaseFull;
                    break;
                case "MenuProcessing":
                    remote_enroll_result = RemoteEnrollResult.MenuProcessing;
                    break;
                case "RemoteEnrollAlreadyStarted":
                    remote_enroll_result = RemoteEnrollResult.RemoteEnrollAlreadyStarted;
                    break;
                default:
                    remote_enroll_result = RemoteEnrollResult.Unknown;
                    break;
            }
            return remote_enroll_result; 
        }
    }
}