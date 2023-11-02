using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWEBTP.Models
{
    public class Carro
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public bool Disponivel { get; set; }
        public bool EmDestaque { get; set; }
        public int Kilometros { get; set; }

        //Tem uma categoria
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

    }
}
