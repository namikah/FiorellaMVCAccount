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
                Description = "Note: This e-mail already registered",
            };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError()
            {
                Code =  "",
                Description= "Note: Just updated"
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError()
            {
                Code="Email",
                Description="Note: Invalid email"
            };
        }
    }
}
