using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEBTP.Models.ViewModels
{
    public class PesquisaCarroViewModel
    {
        public List<Carro> ListaDeCarros { get; set; }
        public int NumResultados { get; set; }
        [Display(Name = "Pesquisa de Carros", Prompt = "Introduza o texto a pesquisar")]
        public string TextoAPesquisar { get; set; }
    }
}
