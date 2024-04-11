using AgendaProjeto.Models;
using System.Net;
using System.Text.Json;

namespace AgendaProjeto.Services
{
    public class UserService : IUserService
    {
        const string folderName = "AgendaProjeto";
        const string userFileName = "Users.json";

        private readonly IFileService _fileService;
        public UserService(IFileService fileService)
        {
            _fileService = fileService;
        }       

        public void Register(UserRequest request)
        {
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Password))
                throw new Exception(); //TODO: refatorar

            var pathFile = _fileService.GetOrCreateUsersPathFile();

            if (!File.Exists(pathFile))
                throw new Exception(); //Verificar a tratativa em caso de não existir

            var users = File.ReadAllText(pathFile);

            var usersDeserialized = JsonSerializer.Deserialize<List<User>>(users);

            if (usersDeserialized == null)
                throw new Exception();

            usersDeserialized.Add(new User(request.Name, request.Password));

            string jsonContent = JsonSerializer.Serialize(usersDeserialized, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(pathFile, jsonContent);
        }

        public bool Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new Exception(); //TODO: refatorar

            var pathFile = _fileService.GetOrCreateUsersPathFile();

            if (!File.Exists(pathFile))
                throw new Exception(); //Verificar a tratativa em caso de não existir

            var users = File.ReadAllText(pathFile);

            var usersDeserialized = JsonSerializer.Deserialize<List<User>>(users);

            if (usersDeserialized == null)
                throw new Exception();

            var user = usersDeserialized.SingleOrDefault(x => x.Name ==  username);

            if (user == null)
                return false;

            var isLogin = user.Pasword == password; //precisa ver a criptografia.

            if (isLogin)
                return true;

            return false;
        }
    }

    public interface IUserService
    {

    }

    public interface IFileService
    {
        string GetOrCreateBaseDirectory();
        string GetOrCreateUsersPathFile();
    }
    public class FileService : IFileService
    {
        const string folderName = "AgendaProjeto";
        const string userFileName = "Users.json";
        public string GetOrCreateBaseDirectory()
        {
            if(!Directory.Exists(folderName))
                Directory.CreateDirectory(GetOrCreateBaseDirectory());
            
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName);
        }            

        public string GetAddressBookDirectory(string userName)
            => Path.Combine(GetOrCreateBaseDirectory(), userName);

        public string GetAddressBookPathFile(string userName)
            => Path.Combine(GetAddressBookDirectory(userName), $"{userName}.json");

        public string GetOrCreateUsersPathFile()
        {
            var filePath = Path.Combine(GetOrCreateBaseDirectory(), userFileName);

            if (!File.Exists(filePath))
                File.Create(filePath);

            return filePath;
        }
    }
}
