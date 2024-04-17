using AgendaProjeto.Enumerations;

namespace AgendaProjeto.Models
{
    public class AddressBook : Entity
    {
        public string UserName { get; set; }
        public List<Contact> Contacts { get; set; } = new();

        public void AddContact(Contact newContact)
        {
            if (Contacts == null)
                Contacts = new List<Contact>();

            if (newContact != null)
                Contacts.Add(newContact);
        }        

        public void RemoveContact(Guid contactId)
        {
            if (Contacts == null)
                return;

            var contact = Contacts.SingleOrDefault(x => x.Id == contactId);

            if (contact != null)
                Contacts.Remove(contact);
        }

        public bool MobilePhoneAlreadyExists(string phoneNumber)
        {
            return Contacts.Any(x => x.Phones.Any(x => x.Number == phoneNumber && x.Type == PhoneType.Mobile));
        }

    }
}
