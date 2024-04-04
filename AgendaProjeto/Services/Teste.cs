using System.Net.Mail;
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

        public void UpdateDataFile(AddressBook request) //TODO: deve receber o request e a lista de contatos já existente
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

        public string GetDirectory(string userName)
            => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName, userName);

        public string GetPathFile(string userName)
           => Path.Combine(GetDirectory(userName), $"{userName}.json");
    }
}
