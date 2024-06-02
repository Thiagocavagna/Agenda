using AgendaProjeto.Models;

namespace AgendaProjeto.Services
{
    public interface IAddressBookService 
    {
        AddressBook GetAddressBook();
        Contact GetContact(Guid id);
        void CreateDataFile(AddressBook request);
        void AddContact(Contact newContact);
        void RemoveContact(Guid contactId);
        void EditContact(Contact contact);
    }
}
