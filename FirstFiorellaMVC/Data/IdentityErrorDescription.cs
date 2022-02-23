using Microsoft.AspNetCore.Identity;

namespace FirstFiorellaMVC.Data
{
    public class IdentityErrorDescription : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "Email",
                Description = "This e-mail already registered",
            };
        }
    }
}
