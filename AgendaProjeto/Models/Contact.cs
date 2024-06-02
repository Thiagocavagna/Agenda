using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AgendaProjeto.Models
{
    public class Contact : Entity
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo telefone é obrigatório")]
        public List<Phone> Phones { get; set; } = new();

        [JsonIgnore]
        public Phone PrincipalPhone => Phones.FirstOrDefault();
    }
}
