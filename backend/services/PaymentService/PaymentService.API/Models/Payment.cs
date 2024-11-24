namespace PaymentService.API.Models
{
    public class Payment
    {
        public string Id { get; private set; }
        public string UserId { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string CourseId { get; private set; }
        public string CourseName { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime PaymentDueDate { get; private set; }
        public decimal ListPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalPrice { get; private set; }
        public string? CouponCode { get; private set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public PaymentMethod Method { get; private set; } = PaymentMethod.Paypal;
        public string? PaypalOrderId { get; private set; }

        public Payment(string userId, string username, string email, string courseId, string courseName, decimal listPrice, decimal discount,
            decimal totalPrice, string? couponCode, PaymentMethod method, string? paypalOrderId)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            Username = username;
            Email = email;
            CourseId = courseId;
            CourseName = courseName;
            CreatedAt = DateTime.Now;
            PaymentDueDate = DateTime.Now.AddHours(1);
            ListPrice = listPrice;
            Discount = discount;
            TotalPrice = totalPrice;
            CouponCode = couponCode;
            Status = PaymentStatus.Pending;
            Method = method;
            PaypalOrderId = paypalOrderId;
        }
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Canceled,
    }

    public enum PaymentMethod
    {
        Paypal
    }
}
