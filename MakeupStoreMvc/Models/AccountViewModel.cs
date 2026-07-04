namespace MakeupStoreMvc.Models
{
    public class AccountViewModel
    {
        public LoginViewModel Login { get; set; } = new LoginViewModel();

        public RegisterViewModel Register { get; set; } = new RegisterViewModel();
    }
}