namespace AppMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public  User? User { get; set; } // navigation property
        public string Status { get; set; } = "Pending"; // "Pending", "Processing", "Completed", "Cancelled"
        public DateTime OverDateTime { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = [];
    }
}
