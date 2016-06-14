namespace moonstone.core.repositories
{
    public class RepositoryHub
    {
        public IGroupRepository GroupRepository { get; protected set; }
        public IGroupUserRepository GroupUserRepository { get; protected set; }
        public IUserRepository UserRepository { get; protected set; }

        public RepositoryHub(
            IUserRepository userRepository,
            IGroupRepository groupRepository,
            IGroupUserRepository groupUserRepository)
        {
            this.UserRepository = userRepository;
            this.GroupRepository = groupRepository;
            this.GroupUserRepository = groupUserRepository;
        }
    }
}