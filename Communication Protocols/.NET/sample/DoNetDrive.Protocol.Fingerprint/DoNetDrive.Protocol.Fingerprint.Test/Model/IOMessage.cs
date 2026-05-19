namespace DoNetDrive.Protocol.Fingerprint.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    public struct IOMessage
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Type { get; set; }

        public string Remote { get; set; }
        public string Local { get; set; }

        public string Time { get; set; }
    }
}
