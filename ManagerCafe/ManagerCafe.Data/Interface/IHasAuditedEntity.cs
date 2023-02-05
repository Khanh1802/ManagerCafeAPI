namespace ManagerCafe.Data.Interface
{
    public interface IHasAuditedEntity
    {
        public DateTime CreateTime { get; set; }
        public DateTime? DeletetionTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
