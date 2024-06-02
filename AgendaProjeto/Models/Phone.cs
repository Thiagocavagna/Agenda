using AgendaProjeto.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace AgendaProjeto.Models
{
    public class Phone
    {
        [Required(ErrorMessage = "O campo número é obrigatório")]
        public string Number { get; set; }

        [Required(ErrorMessage = "O campo tipo é obrigatório")]
        public PhoneType Type { get; set; }
    }
}
