namespace AgendaProjeto.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Phone> Phones { get; set; }
    }
}
