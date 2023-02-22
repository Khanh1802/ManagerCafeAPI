using AutoMapper;
using ManagerCafe.CacheItems.Users;
using ManagerCafe.Commons;
using ManagerCafe.Contracts.Dtos.UsersDtos;
using ManagerCafe.Contracts.Dtos.UsersDtos.ValidateUserDto;
using ManagerCafe.Contracts.Services;
using ManagerCafe.Data.Data;
using ManagerCafe.Data.Models;
using ManagerCafe.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ManagerCafe.Applications.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ManagerCafeDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserValidate _userValidate;
        private readonly IUserCacheService _userCacheService;

        public UserService(IUserRepository userRepository,
            IMapper mapper, IUserValidate userValidate,
            IUserCacheService userCacheService, ManagerCafeDbContext context)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userValidate = userValidate;
            _userCacheService = userCacheService;
            _context = context;
        }

        public async Task<UserDto> AddAsync(CreateUserDto item)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (await CheckUserNameExistAysnc(item.UserName) != false)
                {
                    throw new Exception("User name have been already exist");
                }

                if (await _userValidate.IsValidateEmailAsync(item))
                {
                    throw new Exception("Email have been already exist");
                }

                if (await _userValidate.IsValidatePhoneNumberAsync(item))
                {
                    throw new Exception("Phone number have been already exist");
                }

                var stringMD5 = CommonCreateMD5.Create(item.Password);
                item.Password = stringMD5;
                var user = await _userRepository.AddAsync(_mapper.Map<CreateUserDto, User>(item));
                await transaction.CommitAsync();
                return _mapper.Map<User, UserDto>(user);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        public async Task DeleteAsync(Guid key)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _userRepository.GetByIdAsync(key);
                if (entity == null)
                {
                    throw new Exception("Not found User to deleted");
                }

                await _userRepository.Delete(entity);
            }
            catch (Exception ex)
            {
                throw ex.GetBaseException();
            }
        }

        public Task<List<UserDto>> FilterAsync(FilterUserDto item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return _mapper.Map<List<User>, List<UserDto>>(await _userRepository.GetAllAsync());
        }

        public async Task<UserDto> GetByIdAsync(Guid key)
        {
            return _mapper.Map<User, UserDto>(await _userRepository.GetByIdAsync(key));
        }

        public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto item)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var entity = await _userRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    throw new Exception("Not found User to update");
                }

                item.UserName = entity.UserName;
                if (item.Password == null)
                {
                    item.Password = entity.Password;
                }

                var update = _mapper.Map<UpdateUserDto, User>(item, entity);
                await _userRepository.UpdateAsync(update);
                await transaction.CommitAsync();
                _userCacheService.Set(_mapper.Map<User, UserCacheItem>(update));
                return _mapper.Map<User, UserDto>(update);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex.GetBaseException();
            }
        }

        public async Task<bool> LoginAsync(string userName, string password)
        {
            var hashingPassword = CommonCreateMD5.Create(password);
            var user = await (await _userRepository.GetQueryableAsync())
                .SingleOrDefaultAsync(x => x.UserName == userName && x.Password == hashingPassword);

            if (user != null)
            {
                //Delete last login
                user.LastLoginTime = DateTime.Now;
                _context.Update(user);
                await _context.SaveChangesAsync();
                //AddAsync to cache
                _userCacheService.Set(_mapper.Map<User, UserCacheItem>(user));

                return true;
            }

            return false;
        }

        public async Task<bool> CheckUserNameExistAysnc(string item)
        {
            var query = await _userRepository.GetQueryableAsync();
            return await query.AnyAsync(x => x.UserName == item);
        }

        public async Task<bool> UpdatePassword(string passwordOld, string passwordNew, string passwordNewRepeat)
        {
            var user = _userCacheService.GetOrDefault();
            string hashingPasswordOld = CommonCreateMD5.Create(passwordOld);

            var account = await (await _userRepository.GetQueryableAsync()).AsNoTracking()
                .Where(x => x.Id == user.Id && x.Password == hashingPasswordOld)
                .SingleOrDefaultAsync();
            if (account == null)
            {
                throw new Exception("Password old not correct");
            }

            if (passwordNew != passwordNewRepeat)
            {
                throw new Exception("Password new not same type");
            }

            string hashingPasswordNew = CommonCreateMD5.Create(passwordNew);
            //var update = _mapper.Map<UserCacheItem, User>(user);

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var update = await _userRepository.GetByIdAsync(user.Id);
                update.Password = hashingPasswordNew;
                _context.Update(update);
                await transaction.CommitAsync();
                // cache set nhưng bị lỗi nên đã remove
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateInfomation(Guid id,UpdateUserDto item)
        {
            var hashingPassword = CommonCreateMD5.Create(item.Password);
            try
            {
                await UpdateAsync(id,item);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> Validate(LoginUser loginUser)
        {
            var login = await LoginAsync(loginUser.UserName, loginUser.Password);
            if (login == true)
            {
                var user = _context.Users.FirstOrDefault(x => x.UserName == loginUser.UserName);
                return _mapper.Map<User, UserDto>(user);
            }
            return null;
        }
    }
}