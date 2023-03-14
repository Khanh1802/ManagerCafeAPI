using ManagerCafe.Contracts.Dtos.UserTypeDtos;
using ManagerCafe.Share.Commons;

namespace ManagerCafe.Contracts.Services
{
    public interface IUserTypeService : IGenericService<UserTypeDto, CreateUserTypeDto, UpdateUserTypeDto, FilterUserTypeDto, Guid>
    {
        Task<CommonPageDto<UserTypeDto>> GetPagedListAsync(FilterUserTypeDto item);
    }
}
