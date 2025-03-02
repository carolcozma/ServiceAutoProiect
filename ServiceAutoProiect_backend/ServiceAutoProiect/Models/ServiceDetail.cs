namespace ServiceAutoProiect.Models
{
    public class ServiceDetail
    {
        public int ServiceID { get; set; }
        public string Description { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Status { get; set; }
        public string MechanicFirstName { get; set; }
        public string MechanicLastName { get; set; }
    }
}
