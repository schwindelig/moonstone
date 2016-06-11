namespace moonstone.core.repositories
{
    public class RepositoryHub
    {
        public IGroupRepository GroupRepository { get; set; }
        public IUserRepository UserRepository { get; set; }

        public RepositoryHub(
            IUserRepository userRepository,
            IGroupRepository groupRepository)
        {
            this.UserRepository = userRepository;
            this.GroupRepository = groupRepository;
        }
    }
}