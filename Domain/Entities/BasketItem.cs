namespace Domain.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public string BrandId { get; set; }
        public string TypeId { get; set; }
        public int Quantity { get; set; }
    }
}