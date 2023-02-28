using ManagerCafe.Contracts.Dtos.UsersDtos.ValidateUserDto;

namespace ManagerCafe.Contracts.Dtos.UsersDtos
{
    public class UpdateUserDto : IValidate, IHasEmail, IHasPhone
    {
        public Guid UserTypeId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public void ValidateUser()
        {
            if (string.IsNullOrEmpty(FullName))
            {
                throw new Exception("Full name is empty");
            }

            if (string.IsNullOrEmpty(Email))
            {
                throw new Exception("Email is empty");
            }

            if (CheckEmailConvert(Email) == false)
            {
                throw new Exception("Email is deficiency @gmail.com");
            }

            if (string.IsNullOrEmpty(PhoneNumber))
            {
                throw new Exception("Phone number is empty");
            }

            if (CheckPhoneNumerConvert(PhoneNumber) == null)
            {
                throw new Exception("Check phone again,something wrong");
            }

            else
            {
                PhoneNumber = CheckPhoneNumerConvert(PhoneNumber);

                if (PhoneNumber.Length != 10)
                {
                    throw new Exception("Check phone again,something wrong");
                }
            }

            if (string.IsNullOrEmpty(UserName))
            {
                throw new Exception("User name is empty");
            }

            if (string.IsNullOrEmpty(Password))
            {
                throw new Exception("Password is empty");
            }
        }

        private string CheckPhoneNumerConvert(string input)
        {
            var checkInput = input.StartsWith("0") || input.StartsWith("84");
            if (checkInput)
            {
                if (input.StartsWith("84"))
                {
                    input = "0" + input.Substring(2);
                }
                var filter = decimal.TryParse(input, out var number);
                if (filter && input.Length == 10)
                {
                    return input;
                }
            }
            return null;
        }

        private bool CheckEmailConvert(string input)
        {
            string[] subs = input.Split("@");
            if (subs.Length > 2)
            {
                throw new Exception("Check emai again");
            }
            var filter = subs[1].Equals("gmail.com");
            return filter;
        }
    }
}
