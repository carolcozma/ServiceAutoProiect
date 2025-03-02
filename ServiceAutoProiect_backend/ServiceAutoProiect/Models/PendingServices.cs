namespace ServiceAutoProiect.Models
{
    public class PendingService
    {
        public int ServiceID { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Status { get; set; }
        public decimal TotalCost { get; set; }
        public string Description { get; set; }
        public int MechanicId { get; set; }
        public int ServiceTypeId { get; set; }
        public int CarId { get; set; }
    }
}
