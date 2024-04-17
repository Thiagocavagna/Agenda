using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Xml;
using AgendaProjeto.Models;

namespace AgendaProjeto.Services
{
    public interface IAddressBookService 
    {
        AddressBook GetAddressBook(string userName);
        Contact GetContact(string userName, Guid id);
        void CreateDataFile(AddressBook request);
        void AddContact(string userName, Contact newContact);
        void RemoveContact(string userName, Guid contactId);
        void EditContact(string userName, Contact contact);
    }
    public class AddressBookService : IAddressBookService
    {
        const string folderName = "AgendaProjeto";

        public Contact GetContact(string userName, Guid id)
        {
            var pathFile = GetPathFile(userName);

            if (!File.Exists(pathFile))
                throw new Exception(); //Verificar a tratativa em caso de não existir

            var addressBook = File.ReadAllText(pathFile);

            if (string.IsNullOrEmpty(addressBook))
                throw new Exception();

            var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

            if (address == null)
                throw new Exception();

            return address.Contacts.SingleOrDefault(x => x.Id == id)!;
        }

        public AddressBook GetAddressBook(string userName)
        {
            var pathFile = GetPathFile(userName);

            if (!File.Exists(pathFile))
                throw new Exception(); //Verificar a tratativa em caso de não existir

            var addressBook = File.ReadAllText(pathFile);

            if (string.IsNullOrEmpty(addressBook))
                throw new Exception();

            var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

            if (address == null)
                throw new Exception();

            return address;
        }
        private void CreateDataDirectory(string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var path = GetDirectory(userName);

                Directory.CreateDirectory(path);
            }
        }

        public void CreateDataFile(AddressBook request)
        {
            if (!string.IsNullOrEmpty(request.UserName))
            {
                var directory = GetDirectory(request.UserName);

                if (!Directory.Exists(directory))
                    CreateDataDirectory(request.UserName);

                string jsonContent = JsonSerializer.Serialize(request,
                    new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(GetPathFile(request.UserName), jsonContent);

            }
        }

        public void AddContact(string userName, Contact newContact) 
        {    
            //TODO: usar o validate de data annotation para retornar erros de required

            if (!string.IsNullOrEmpty(userName))
            {
                var pathFile = GetPathFile(userName);

                if (!File.Exists(pathFile))
                    throw new Exception();

                var addressBook = File.ReadAllText(pathFile);

                if (string.IsNullOrEmpty(addressBook))
                    throw new Exception();

                var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

                if (address == null)
                    throw new Exception();
                
                foreach(var phone in newContact.Phones)
                {
                    if (phone.Type == Enumerations.PhoneType.Mobile && address.MobilePhoneAlreadyExists(phone.Number))
                        throw new Exception(); //TODO: retornar erro
                }

                address.AddContact(newContact);

                string jsonContent = JsonSerializer.Serialize(address,
                                       new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(pathFile, jsonContent);
            }
        }

        public void RemoveContact(string userName, Guid contactId)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var pathFile = GetPathFile(userName);

                if (!File.Exists(pathFile))
                    throw new Exception(); //Verificar a tratativa em caso de não existir

                var addressBook = File.ReadAllText(pathFile);

                if (string.IsNullOrEmpty(addressBook))
                    throw new Exception();

                var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

                if (address == null)
                    throw new Exception();

                var contactToRemove = address.Contacts.SingleOrDefault(x => x.Id == contactId);

                if(contactToRemove != null)
                    address.Contacts.Remove(contactToRemove);

                string jsonContent = JsonSerializer.Serialize(address,
                                       new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(pathFile, jsonContent);
            }
        }

        public void EditContact(string userName, Contact contact)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var pathFile = GetPathFile(userName);

                if (!File.Exists(pathFile))
                    throw new Exception(); //Verificar a tratativa em caso de não existir

                var addressBook = File.ReadAllText(pathFile);

                if (string.IsNullOrEmpty(addressBook))
                    throw new Exception();

                var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

                if (address == null)
                    throw new Exception();

               var contactToEdit = address.Contacts.SingleOrDefault(x => x.Id == contact.Id);

                if (contactToEdit == null)
                    throw new Exception();

                contactToEdit.Phones = contact.Phones;
                contactToEdit.Name = contact.Name;

                address.RemoveContact(contactToEdit.Id);
                address.AddContact(contactToEdit);

                string jsonContent = JsonSerializer.Serialize(address,
                                       new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(pathFile, jsonContent);
            }
        }

        private string GetDirectory(string userName)
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, userName);

        private string GetPathFile(string userName)
           => Path.Combine(GetDirectory(userName), $"{userName}.json");

    }
}
