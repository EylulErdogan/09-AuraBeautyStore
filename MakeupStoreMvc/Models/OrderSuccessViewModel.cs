using System.Collections.Generic;

namespace MakeupStoreMvc.Models
{
    public class OrderSuccessViewModel
    {
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
