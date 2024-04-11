namespace AgendaProjeto.Models
{
    public class User : Entity
    {        
        public string Name { get; set; }
        public string Pasword { get; set; }

        public User(string name, string pasword)
        {
            Name = name;
            Pasword = pasword;
        }

    }
}
