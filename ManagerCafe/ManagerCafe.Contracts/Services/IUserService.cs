using ManagerCafe.Contracts.Dtos.UsersDtos;
using ManagerCafe.Data.Models;

namespace ManagerCafe.Contracts.Services
{
    public interface IUserService : IGenericService<UserDto, CreateUserDto, UpdateUserDto, FilterUserDto, Guid>
    {
        Task<bool> CheckUserNameExistAysnc(string item);

        Task<UserDto> LoginAsync(string userName, string password);

        Task<bool> UpdatePassword(string passwordOld, string passwordNew, string passworldNewRepeat);
        Task<bool> UpdateInfomation(Guid id, UpdateUserDto item);

        Task<UserDto> Validate(LoginUser loginUser);
    }
}
