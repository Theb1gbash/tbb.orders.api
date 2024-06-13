namespace tbb.orders.api.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int OrderId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string TicketStatus { get; set; }
        public byte[] QRCode { get; set; }
        public byte[] Barcode { get; set; }
    }
}
