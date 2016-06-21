namespace moonstone.core.repositories
{
    public class RepositoryHub
    {
        public ICategoryRepository CategoryRepository { get; protected set; }
        public IGroupRepository GroupRepository { get; protected set; }
        public IGroupUserRepository GroupUserRepository { get; protected set; }
        public IUserRepository UserRepository { get; protected set; }

        public RepositoryHub(
            IUserRepository userRepository,
            IGroupRepository groupRepository,
            IGroupUserRepository groupUserRepository,
            ICategoryRepository categoryRepository)
        {
            this.UserRepository = userRepository;
            this.GroupRepository = groupRepository;
            this.GroupUserRepository = groupUserRepository;
            this.CategoryRepository = categoryRepository;
        }
    }
}