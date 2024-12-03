namespace CourseService.Domain.Events.User
{
    public class UserInfoUpdatedDomainEvent : DomainEvent
    {
        public Models.User User { get; set; }
        public bool NameOrThumbnailUpdated { get; set; }

        public UserInfoUpdatedDomainEvent(Models.User user)
        {
            User = user;
        }
    }
}
