using moonstone.core.services.results;

namespace moonstone.core.services
{
    public interface ILoginService
    {
        LoginResult Login(string username, string password, bool isPersistent, bool shouldLockOut);

        void Logout();
    }
}