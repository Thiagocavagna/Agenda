namespace AgendaProjeto.Models
{
    public class AddressBook
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public List<Contact> Contacts { get; set; }
    }
}
