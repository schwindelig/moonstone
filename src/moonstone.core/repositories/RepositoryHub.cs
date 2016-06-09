namespace moonstone.core.repositories
{
    public class RepositoryHub
    {
        public IUserRepository UserRepository { get; set; }

        public RepositoryHub(
            IUserRepository userRepository)
        {
            this.UserRepository = userRepository;
        }
    }
}