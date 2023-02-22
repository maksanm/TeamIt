using Domain.Entities.ProjectManager;
using Domain.Entities.Teams;

namespace Domain.Entities.Chats
{
    public class Chat
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ChatPictureId { get; set; }
        public virtual Picture? ChatPicture { get; set; }
        public long? BaseTeamId { get; set; }
        public virtual Team? BaseTeam { get; set; }
        public long? BaseProjectId { get; set; }
        public virtual Project? BaseProject { get; set; }
        public virtual IList<ChatProfile> Profiles { get; set; }
        public virtual IList<Message> Messages { get; set; }
    }
}
