namespace buingocluan_buoi4.Models
{
    public class CartItem
    {
        //Thông tin của giỏ hàng
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
