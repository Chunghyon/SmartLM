namespace DoNetDrive.Protocol.Fingerprint.Test.Model
{
    public struct CommandResult
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string SN { get; set; }

        public string Remote { get; set; }

        public string Time { get; set; }
        public string Timemill { get; set; }
    }
}
