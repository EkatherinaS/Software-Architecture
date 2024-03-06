namespace TrackerRepository
{
    public class DBUser : DBEntity
    {
        public long IdChat { get; set; }
        public string? FIO { get; set; }
        public string? Username { get; set; }
        public string? Nickname { get; set; }
        public string? Status { get; set; }
        public bool IsAdmin { get; set; } = false;
        public DBCompany Company { get; set; }
        public DBTask? CurrentTask { get; set; }

        public UserEntity toUserEntity()
        {
            UserEntity userEntity = new UserEntity();
            userEntity.IdChat = IdChat;
            userEntity.Nickname = Nickname;
            userEntity.FIO = FIO;
            userEntity.Username = Username;
            userEntity.Status = Status;
            userEntity.IsAdmin = IsAdmin;
            return userEntity;
        }
    }
}
