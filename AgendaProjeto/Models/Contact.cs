namespace AgendaProjeto.Models
{
    public class Contact : Entity
    {
        public string Name { get; set; }
        //public string WorkPhone { get; set; }
        //public string HomePhone { get; set; }
        //public string CellPhone { get; set; }

        public List<Phone> Phones { get; set; } = new();
        public Phone PrincipalPhone => Phones.FirstOrDefault();
    }
}
