using AgendaProjeto.Models;
using System.Text.Json;
using System.Transactions;

namespace AgendaProjeto.Services
{
    public class AddressBookService : IAddressBookService
    {
        const string folderName = "AgendaProjeto";
        const string fileName = "Agenda.json";

        public Contact GetContact(Guid id)
        {
            var pathFile = GetPathFile();

            if (!File.Exists(pathFile))
                throw new Exception("Arquivo não existe");


            var addressBook = File.ReadAllText(pathFile);

            if (string.IsNullOrEmpty(addressBook))
                throw new Exception("A Agenda está vaiza.");

            var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

            if (address == null)
                throw new Exception("Falha ao ler a agenda.");

            return address.Contacts.SingleOrDefault(x => x.Id == id)!;
        }

        public AddressBook GetAddressBook()
        {
            var pathFile = GetPathFile();

            if (!File.Exists(pathFile))
                CreateDataFile(new AddressBook());

            var addressBook = File.ReadAllText(pathFile);

            if (string.IsNullOrEmpty(addressBook))
                throw new Exception("A Agenda está vaiza.");

            var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

            if (address == null)
                throw new Exception("Falha ao ler a agenda.");

            return address;
        }
        private void CreateDataDirectory()
        {
            Directory.CreateDirectory(GetDirectory());            
        }

        public void CreateDataFile(AddressBook request)
        {
            var directory = GetDirectory();

            if (!Directory.Exists(directory))
                CreateDataDirectory();

            string jsonContent = JsonSerializer.Serialize(request,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(GetPathFile(), jsonContent);

        }

        public void AddContact(Contact newContact) 
        {
            ValidateContact(newContact);

            var pathFile = GetPathFile();

            if (!File.Exists(pathFile))
                CreateDataFile(new AddressBook());

            var addressBook = File.ReadAllText(pathFile);

            if (string.IsNullOrEmpty(addressBook))
                throw new Exception("Agenda está vazia");

            var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

            if (address == null)
                throw new Exception("Falha ao ler a agenda.");

            foreach (var phone in newContact.Phones)
            {
                if (phone.Type == Enumerations.PhoneType.Mobile && address.MobilePhoneAlreadyExists(phone.Number))
                    throw new Exception($"O número de celular {phone.Number} já está cadastrado para outro contato");
            }

            address.AddContact(newContact);

            string jsonContent = JsonSerializer.Serialize(address,
                                    new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(pathFile, jsonContent);
            
        }

        public void RemoveContact(Guid contactId)
        {
            var pathFile = GetPathFile();

            if (!File.Exists(pathFile))
                throw new Exception("Arquivo não existe");

            var addressBook = File.ReadAllText(pathFile);

            if (string.IsNullOrEmpty(addressBook))
                throw new Exception("Agenda está vazia");

            var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

            if (address == null)
                throw new Exception("Falha ao ler a agenda.");

            var contactToRemove = address.Contacts.SingleOrDefault(x => x.Id == contactId);

            if(contactToRemove != null)
                address.Contacts.Remove(contactToRemove);

            string jsonContent = JsonSerializer.Serialize(address,
                                    new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(pathFile, jsonContent);
        }

        public void EditContact(Contact contact)
        {
            ValidateContact(contact);

            var pathFile = GetPathFile();

            if (!File.Exists(pathFile))
                throw new Exception("Arquivo não existe");

            var addressBook = File.ReadAllText(pathFile);

            if (string.IsNullOrEmpty(addressBook))
                throw new Exception("Agenda está vazia");

            var address = JsonSerializer.Deserialize<AddressBook>(addressBook);

            if (address == null)
                throw new Exception("Falha ao ler a agenda.");

            var contactToEdit = address.Contacts.SingleOrDefault(x => x.Id == contact.Id);

            if (contactToEdit == null)
                throw new Exception("Contato não encontrado.");

            foreach (var phone in contact.Phones)
            {
                if (phone.Type == Enumerations.PhoneType.Mobile && address.MobilePhoneAlreadyExists(phone.Number))
                    throw new Exception($"O número de celular {phone.Number} já está cadastrado para outro contato");
            }

            contactToEdit.Phones = contact.Phones;
            contactToEdit.Name = contact.Name;

            address.RemoveContact(contactToEdit.Id);
            address.AddContact(contactToEdit);

            string jsonContent = JsonSerializer.Serialize(address,
                                    new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(pathFile, jsonContent);
            
        }

        private string GetDirectory()
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName);

        private string GetPathFile()
           => Path.Combine(GetDirectory(), fileName);

        private void ValidateContact(Contact contact)
        {
            List<string> errors = new();
            if(string.IsNullOrEmpty(contact.Name))
                errors.Add("Nome do contato não pode estar vazio");

            foreach(var phone in contact.Phones)
            {
                if(!string.IsNullOrEmpty(phone.Number))
                {
                    var number = phone.Number.Replace(" ", "").Replace("-", "");

                    if (!number.All(char.IsDigit))
                        errors.Add("Número de telefone inválido");

                    if (number.Length != 10 && number.Length != 11)
                        errors.Add("Formato de telefone inválido");
                } else
                {
                    errors.Add("Número do telefone não pode estar vazio");
                }
            }

            if(errors.Count > 0)
            {
                string message = string.Join("; \n", errors);

                throw new Exception(message);
            }
        }

    }
}
