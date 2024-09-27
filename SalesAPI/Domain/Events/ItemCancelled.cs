namespace Domain.Events
{
    internal class ItemCancelled
    {
        public int SaleNumber { get; set; }
        public int ItemId { get; set; }
    }
}
