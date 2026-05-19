using System;
using System.Collections.Generic;

namespace A3_8190_HTTPServer.Models
{
    // ─── Keepalive ──────────────────────────────────────────────────────────

    /// <summary>Device → Server: 하트비트/킵얼라이브 요청</summary>
    public class KeepaliveRequest
    {
        public string SN { get; set; }
        public int    RelayStatus      { get; set; }  // 0=NC closed, 1=NO closed
        public int    KeepOpenStatus   { get; set; }  // 0=normal, 1=always open
        public int    DoorSensorStatus { get; set; }  // 0=closed,  1=open
        public int    LockDoorStatus   { get; set; }  // 0=unlocked, 1=locked
        public string AlarmStatus      { get; set; }  // "" or "fire,blacklist,…"
    }

    /// <summary>Server → Device: 킵얼라이브 응답 + 대기 명령 플래그</summary>
    public class KeepaliveResponse
    {
        public int Success          { get; set; }  // 0=success, 401=not activated
        public int AddPeople        { get; set; }  // >0: pending personnel to add
        public int DeletePeople     { get; set; }  // >0: pending personnel to delete
        public int Remote           { get; set; }  // 1: remote command waiting
        public int SyncParameter    { get; set; }  // 1: parameter sync required
        public int UploadWorkParameter { get; set; } // 1: upload device params
    }

    // ─── Personnel ──────────────────────────────────────────────────────────

    /// <summary>인원 정보 (서버 → 디바이스 다운로드 / 디바이스 → 서버 푸시 공용)</summary>
    public class PersonInfo
    {
        public string UserID         { get; set; }  // UINT32 최대 4294967295
        public string Name           { get; set; }
        public string Job            { get; set; }
        public string Department     { get; set; }
        public string IdentityCard   { get; set; }
        public string Attachment     { get; set; }
        public string Photo          { get; set; }  // URL 또는 base64
        public string PhotoMD5       { get; set; }
        public int    PhotoLen       { get; set; }
        public string Password       { get; set; }  // 숫자 4–8자리
        public string CardNum        { get; set; }
        public string QRCode         { get; set; }
        public int    AccessType     { get; set; }  // 0=일반, 1=관리자, 2=블랙리스트
        public uint   ExpirationDate { get; set; }  // Unix timestamp, 0=무기한
        public int    OpenTimes      { get; set; }  // 0=불가, 65535=무제한
        public int    KeepOpen       { get; set; }  // 1=상시개방 카드
        public int    Timegroup      { get; set; }  // 1–64, 0=제한없음
        public string Holidays       { get; set; }  // "1,3,5"
        public string Elevators      { get; set; }  // "1,2,3"
        public string FaceFeature    { get; set; }  // URL 또는 base64
        public string FaceFeatureMD5 { get; set; }
        public List<FingerprintItem> Fingerprints { get; set; }
        public List<PalmveinItem>    Palmveins     { get; set; }
    }

    public class FingerprintItem
    {
        public int    Num  { get; set; }
        public string Data { get; set; }
        public string MD5  { get; set; }
    }

    public class PalmveinItem
    {
        public int    Num  { get; set; }
        public string Data { get; set; }
        public string MD5  { get; set; }
    }

    /// <summary>Device → Server: 인원 다운로드 요청</summary>
    public class DownloadPeopleListRequest
    {
        public string SN    { get; set; }
        public int    Limit { get; set; }  // max 1000
    }

    /// <summary>Server → Device: 인원 다운로드 응답</summary>
    public class DownloadPeopleListResponse
    {
        public int             Success     { get; set; }
        public string          Message     { get; set; }
        public int             PeopleCount { get; set; }
        public List<PersonInfo> PeopleList { get; set; }
    }

    /// <summary>Device → Server: 삭제할 인원 목록 요청</summary>
    public class SelectDeleteInfoRequest
    {
        public string SN { get; set; }
    }

    /// <summary>Server → Device: 삭제할 UserID 목록</summary>
    public class SelectDeleteInfoResponse
    {
        public int          Success    { get; set; }
        public string       Message    { get; set; }
        public List<string> DeleteList { get; set; }
    }

    // ─── Remote Control ─────────────────────────────────────────────────────

    /// <summary>Device → Server: 원격 명령 요청</summary>
    public class RemoteCommandRequest
    {
        public string SN { get; set; }
    }

    /// <summary>Server → Device: 원격 명령</summary>
    public class RemoteCommandResponse
    {
        public int    Success      { get; set; }
        public string Message      { get; set; }
        public int    Restart      { get; set; }  // 1=재시작
        public int    Recover      { get; set; }  // 1=공장초기화
        public int    Opendoor     { get; set; }  // 0=없음, 1=열기, 2=상시개방, 3=닫기, 4=잠금, 5=잠금해제
        public int    Closealarm   { get; set; }  // 1=알람 해제
        public int    RepostRecord { get; set; }  // 1=기록 재전송
        public int    PushAllPeople { get; set; } // 1=전체 인원 업로드
        public int    ClearRecord  { get; set; }  // 1=기록 전체 삭제
    }

    // ─── Records ────────────────────────────────────────────────────────────

    /// <summary>Device → Server: 출입 기록</summary>
    public class RecordDetail
    {
        public long   RecordID    { get; set; }
        public int    RecordType  { get; set; }
        public long   RecordDate  { get; set; }  // Unix timestamp (seconds)
        public string UserID      { get; set; }
        public string Name        { get; set; }
        public string IdentityCard { get; set; }
        public string Job         { get; set; }
        public string Department  { get; set; }
        public string CardNum     { get; set; }
        public string QRCode      { get; set; }
        public int    IsEntry     { get; set; }  // 1=입실, 0=퇴실
        public int    BodyTemp    { get; set; }  // 실제 온도 = BodyTemp / 10
        public int    PhotoLen    { get; set; }

        public static string GetRecordTypeText(int t)
        {
            switch (t)
            {
                case 1:  return "카드";
                case 2:  return "지문";
                case 3:  return "얼굴인식";
                case 4:  return "카드+지문";
                case 5:  return "얼굴+지문";
                case 6:  return "카드+얼굴";
                case 7:  return "카드+비밀번호";
                case 8:  return "얼굴+비밀번호";
                case 9:  return "지문+비밀번호";
                case 10: return "비밀번호";
                case 14: return "카드+지문+얼굴";
                case 19: return "미등록 사용자";
                case 25: return "무인증 개문";
                case 32: return "QR코드";
                case 36: return "손바닥 정맥";
                case 40: return "얼굴+손바닥";
                default: return $"타입 {t}";
            }
        }

        public DateTime GetDateTime()
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(RecordDate).ToLocalTime();
        }
    }

    // ─── Generic ────────────────────────────────────────────────────────────

    public class GenericResponse
    {
        public int    Success { get; set; }
        public string Message { get; set; }
    }

    // ─── Pending command container ───────────────────────────────────────────

    public class PendingRemoteCommand
    {
        public int Opendoor  { get; set; }  // 1=열기, 2=상시개방, 3=닫기
        public int Restart   { get; set; }  // 1=재시작
        public int Recover   { get; set; }  // 1=공장초기화
        public int Closealarm { get; set; } // 1=알람 해제
        public int ClearRecord { get; set; }// 1=기록 삭제
    }
}
