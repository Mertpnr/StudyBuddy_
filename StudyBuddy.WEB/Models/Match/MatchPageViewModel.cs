using StudyBuddy.WEB.Models.User;

namespace StudyBuddy.WEB.Models.Match
{
    public class MatchPageViewModel
    {
        public List<UserProfileViewModel> Users { get; set; } = new();

        public MatchResultViewModel? Result { get; set; }

        public int SelectedUserId { get; set; }
    }
}
