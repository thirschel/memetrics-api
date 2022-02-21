namespace MeMetrics.Domain.Models.Attachments
{
    public class Attachment
    {
        public string MessageId { get; set; }
        public string AttachmentId { get; set; }
        public string Base64Data { get; set; }
        public string FileName { get; set; }
    }
}
