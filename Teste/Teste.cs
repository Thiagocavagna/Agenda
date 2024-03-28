using System.Text;
using System.Text.Json;
using System.Xml;

namespace AgendaProjeto.Services
{
    public class Teste
    {
        const string folderName = "AgendaProjeto";

        public void CreateDataDirectory(string userName)
        {
            if(!string.IsNullOrEmpty(userName))
            {
                var path = GetDirectory(userName);

                Directory.CreateDirectory(path);
            }
        }

        public void CreateDataFile(AddressBook request)
        {
            if(!string.IsNullOrEmpty(request.UserName))
            {
                var directory = GetDirectory(request.UserName);

                if(!Directory.Exists(directory))
                    CreateDataDirectory(request.UserName);

                string jsonContent = JsonSerializer.Serialize(request, 
                    new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(directory, jsonContent);
                
            }
        }

        public string GetDirectory(string userName)
            => Path.Combine("Y", folderName, userName);
        
    }

    public class AddressBook
    {
        public string UserName { get; set; }
        public List<Contact> Contacts { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; } 
    }
}
