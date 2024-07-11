namespace Caps_Project.DTOs.OrdenDTOs
{
    public class OrderReceiptDTO
    {
        public int OrderId { get; set; }

        public decimal TotalPaid { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
