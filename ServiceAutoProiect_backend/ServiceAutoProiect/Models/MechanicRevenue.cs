namespace ServiceAutoProiect.Models
{
    public class MechanicRevenue
    {
        public int MechanicID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ServiceCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
