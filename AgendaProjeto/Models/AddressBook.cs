using AgendaProjeto.Enumerations;

namespace AgendaProjeto.Models
{
    public class AddressBook : Entity
    {
        public List<Contact> Contacts { get; set; } = new();

        public void AddContact(Contact newContact)
        {
            if (Contacts == null)
                Contacts = new List<Contact>();

            if (newContact != null)
            {
                foreach(var phone in newContact.Phones)
                {
                    phone.Number = phone.Number.Replace(" ", "");
                }

                Contacts.Add(newContact);
            }
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
            return Contacts.Any(x => x.Phones.Any(x => x.Number == phoneNumber.Replace(" ", "") && x.Type == PhoneType.Celular));
        }
        public bool MobilePhoneAlreadyExists(string phoneNumber, Guid excludingContactId)
        {
            return Contacts.Where(c => c.Id != excludingContactId)
                .Any(x => x.Phones.Any(x => x.Number == phoneNumber.Replace(" ", "")
                && x.Type == PhoneType.Celular));
        }      
    }
}
