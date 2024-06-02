using System.ComponentModel.DataAnnotations;

namespace AgendaProjeto.Models
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string telefone = value as string;
            if (telefone == null)
            {
                return false;
            }

            telefone = new string(telefone.Where(char.IsDigit).ToArray());

            if (telefone.Length != 10 && telefone.Length != 11)
            {
                return false; 
            }

            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"O campo {name} não é um número de telefone válido.";
        }
    }

}
