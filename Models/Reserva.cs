using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWEBTP.Models
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool? Entregue { get; set; }
        public bool? Aceite { get; set; }
        public int? KmsFinais { get; set; }

        //Possui um Carro
        [ForeignKey("Carro")]
        public int CarroId { get; set; }
        public Carro carro { get; set; }

        //Possui um funcionario
        [ForeignKey("Funcionario")]
        public int FuncionarioId { get; set; }
        public Funcionario Funcionario { get; set;}

        //Possui um cliente
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}
