namespace AgendaProjeto.Models
{
    public class Contact : Entity
    {
        public string Name { get; set; }
        public List<Phone> Phones { get; set; }
    }
}
