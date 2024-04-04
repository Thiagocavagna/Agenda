using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Xml;
using AgendaProjeto.Models;

namespace AgendaProjeto.Services
{
    public class AddressBookService
    {
        const string folderName = "AgendaProjeto";

        public void CreateDataDirectory(string userName)
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

        public void AddContacts(string userName, List<Contact> newContacts) //TODO: deve receber o request e a lista de contatos já existente
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

                address.Contacts.AddRange(newContacts);

                string jsonContent = JsonSerializer.Serialize(address,
                                       new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(pathFile, jsonContent);
            }
        }

        public void RemoveContacts(string userName, List<Contact> contacts)
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

                var contactsToRemove = address.Contacts.Where(x => contacts.Any(r => r.Id == x.Id)).ToList();;

                if(contactsToRemove.Any())
                    contactsToRemove.ForEach(x => address.Contacts.Remove(x));

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

                string jsonContent = JsonSerializer.Serialize(address,
                                       new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(pathFile, jsonContent);
            }
        }

        public string GetDirectory(string userName)
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, userName);

        public string GetPathFile(string userName)
           => Path.Combine(GetDirectory(userName), $"{userName}.json");
    }
}
