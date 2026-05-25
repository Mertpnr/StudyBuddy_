namespace StudyBuddy.WEB.Models.Chat
{
    public class ChatRoomViewModel
    {
        public int MatchRequestId { get; set; }
        public int CurrentUserId { get; set; }
        public List<ChatListViewModel> Messages { get; set; } = new();
        public ChatCreateViewModel NewMessage { get; set; } = new();
    }
}