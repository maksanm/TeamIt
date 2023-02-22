namespace Domain.Entities.Teams
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CreatorUserId { get; set; }
        public virtual User CreatorUser { get; set; }
        public long? PictureId { get; set; }
        public virtual Picture? Picture { get; set; }
        public virtual IList<TeamProfile> Profiles { get; set; }
        public virtual IList<Role> Roles { get; set; }

        public TeamProfile CreatorProfile
        { get { return Profiles.First(profile => profile.UserId == CreatorUserId); } }
    }
}