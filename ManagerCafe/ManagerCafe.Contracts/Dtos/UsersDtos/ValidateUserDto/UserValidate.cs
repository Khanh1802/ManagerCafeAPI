using ManagerCafe.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ManagerCafe.Contracts.Dtos.UsersDtos.ValidateUserDto
{
    public class UserValidate : IUserValidate
    {
        private readonly IUserRepository _userRepository;

        public UserValidate(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Kiểm tra xem email đã tồn tại chưa
        /// </summary>
        /// <param name="hasEmail"></param>
        /// <returns>
        /// true: đã tồn tại
        /// false: chưa tồn tại
        /// </returns>
        public async Task<bool> IsValidateEmailAsync(IHasEmail hasEmail)
        {
            var query = await _userRepository.GetQueryableAsync();
            return await query.AnyAsync(x => x.Email == hasEmail.Email);
        }

        /// <summary>
        /// Kiểm tra xem email đã tồn tại chưa
        /// </summary>
        /// <param name="hasPhoneNumber"></param>
        /// <returns>
        /// true: đã tồn tại
        /// false: chưa tồn tại
        /// </returns>
        public async Task<bool> IsValidatePhoneNumberAsync(IHasPhone hasPhoneNumber)
        {
            var query = await _userRepository.GetQueryableAsync();
            return await query.AnyAsync(x => x.PhoneNumber == hasPhoneNumber.PhoneNumber);
        }
    }
}
