namespace CVGS.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int GameId { get; set; } 

        public double Total { get; set; }   
    }
}
