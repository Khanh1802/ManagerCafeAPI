namespace ManagerCafe.CacheItems.Users
{
    public class UserCacheItem
    {
        public Guid Id { get; set; }
        public Guid UserTypeId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
