using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWEBTP.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        //Possui um gestor

        [Required]
        [ForeignKey("Gestor")]
        public int GestorId { get; set; }
        public Gestor Gestor { get; set; }

        //Possui varias Reservas
        public ICollection<Reserva> Reservas { get; set; }
    }
}
