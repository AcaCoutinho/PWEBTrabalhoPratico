using System.Drawing;

namespace PWEBTP.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descricao { get; set; }
        public bool Disponivel { get; set; }

        //Tem varios carros
        public ICollection<Carro> Carros { get; set; }
    }
}
