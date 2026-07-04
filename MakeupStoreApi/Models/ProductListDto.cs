namespace MakeupStoreApi.Models
{
    public class ProductListDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string ImageUrl { get; set; }

        public string CategoryName { get; set; }

        public string BrandName { get; set; }
    }
}