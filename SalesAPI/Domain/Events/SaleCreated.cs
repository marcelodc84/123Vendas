namespace Domain.Events
{
    public class SaleCreated
    {
        public int SaleNumber { get; set; }
        public string Customer { get; set; }
    }
}
