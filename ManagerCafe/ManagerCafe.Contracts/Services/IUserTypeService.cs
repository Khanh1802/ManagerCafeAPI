using ManagerCafe.Commons;
using ManagerCafe.Contracts.Dtos.UserTypeDtos;

namespace ManagerCafe.Contracts.Services
{
    public interface IUserTypeService : IGenericService<UserTypeDto, CreateUserTypeDto, UpdateUserTypeDto, FilterUserTypeDto, Guid>
    {
        Task<CommonPageDto<UserTypeDto>> GetPagedListAsync(FilterUserTypeDto item);
    }
}
