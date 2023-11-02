using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWEBTP.Models
{
    public class Gestor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ApplicationUser User { get; set; }

        //Pertence a uma empresa
        [Required]
        [ForeignKey("Empresa")]
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        //Possui varios funcionarios
        public ICollection<Funcionario> Funcionarios { get; set; }
    }
}
