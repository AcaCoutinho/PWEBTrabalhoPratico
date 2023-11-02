namespace PWEBTP.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Reserva> reservas { get; set; }
    }
}
