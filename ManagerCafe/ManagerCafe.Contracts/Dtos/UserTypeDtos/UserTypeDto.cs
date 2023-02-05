using ManagerCafe.Data.Models;

namespace ManagerCafe.Contracts.Dtos.UserTypeDtos
{
    public class UserTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
