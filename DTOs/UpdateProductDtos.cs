namespace ProdApi.DTOs
{
    public class UpdateProductDtos
    {
        public string Description { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
