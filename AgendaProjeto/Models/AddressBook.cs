namespace AgendaProjeto.Models
{
    public class AddressBook : Entity
    {
        public string UserName { get; set; }
        public List<Contact> Contacts { get; set; } = new();
    }
}
