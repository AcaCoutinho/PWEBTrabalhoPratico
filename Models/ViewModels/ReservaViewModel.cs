using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEBTP.Models.ViewModel
{
    public class ReservaViewModel
    {
        [Display(Name = "Data de Início", Prompt = "yyyy-mm-dd")]
        public DateTime DataInicio { get; set; }
        [Display(Name = "Data de Fim", Prompt = "yyyy-mm-dd")]
        public DateTime DataFim { get; set; }
    }
}
