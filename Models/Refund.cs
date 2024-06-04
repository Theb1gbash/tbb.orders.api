namespace tbb.orders.api.Models
{
    using System;

    public class Refund
    {
        public int RefundId { get; set; }
        public int OrderId { get; set; }
        public DateTime RefundDate { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
