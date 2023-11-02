namespace PWEBTP.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Possui Varios Carros
        public ICollection<Carro> Carros { get; set; }

        //Possui um gestor
        public int? GestorId { get; set; }
        public Gestor Gestor { get; set; }


    }
}
