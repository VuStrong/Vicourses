namespace PaymentService.API.Models
{
    public class BatchPayout
    {
        public string Id { get; private set; }
        public DateTime Date { get; private set; }
        public string Method { get; private set; }
        public string? ReferencePaypalPayoutBatchId { get; private set; }
        
        public BatchPayout(string method, string? referencePaypalPayoutBatchId = null)
        {
            Id = Guid.NewGuid().ToString();
            Date = DateTime.Now.Date;
            Method = method;
            ReferencePaypalPayoutBatchId = referencePaypalPayoutBatchId;
        }
    }
}
