using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;

namespace Domain.Entities.Chats
{
    public class ChatProfile : IProfile
    {
        public long Id { get; set; }
        public long ChatId { get; set; }
        public virtual Chat Chat { get; set; }
        public long? TeamProfileId { get; set; }
        public virtual TeamProfile? TeamProfile { get; set; }
        public long? ProjectProfileId { get; set; }
        public virtual ProjectProfile? ProjectProfile { get; set; }
        public virtual IList<Message> Messages { get; set; }

        public User User => ProjectProfile is not null
            ? ProjectProfile.User
            : TeamProfile!.User;

        public Role Role => ProjectProfile is not null
            ? ProjectProfile.Role
            : TeamProfile!.Role;
    }
}
