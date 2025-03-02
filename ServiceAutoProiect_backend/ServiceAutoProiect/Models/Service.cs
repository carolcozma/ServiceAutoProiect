namespace ServiceAutoProiect.Models
{
    public class Service
    {
        public int ServiceId { get; set; }
        public int CarId { get; set; }
        public int ServiceTypeId { get; set; }
        public int MechanicId { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
        public string Status { get; set; }
    }
}
