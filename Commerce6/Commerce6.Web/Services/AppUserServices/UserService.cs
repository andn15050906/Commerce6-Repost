using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Commerce6.Web.Services.Abstraction;
using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Contact;
using Commerce6.Infrastructure.Models.AppUser;
using Commerce6.Infrastructure.Models.Merchant;
using Commerce6.Web.Models.AppUser.UserDTOs;
using Commerce6.Web.Services.JwtService;
using Commerce6.Web.Helpers.ServerFile;

namespace Commerce6.Web.Services.AppUserServices
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        //NameIdentifier == Id
        private readonly int MAX_ACCESS_FAILED_COUNT = 10;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }






        public UserDTO? GetCustomerInfo(string id)
        {
            return _uow.UserRepo.GetCustomer(id);
        }

        public StatusMessage Register(RegisterDTO dto)
        {
            if (_uow.UserRepo.EmailExisted(dto.Email))
                return StatusMessage.Conflict;

            _uow.UserRepo.Insert(Adapt(dto));
            _uow.Save();
            return StatusMessage.Created;
        }

        public async Task<StatusMessage> Update(UpdateUserDTO dto, string? userId)
        {
            if (userId == null)
                return StatusMessage.Unauthorized;
            User? user = _uow.UserRepo.Find(userId);
            if (user == null)
                return StatusMessage.Unauthorized;              //not BadRequest

            await ApplyChanges(dto, user);
            _uow.Save();
            return StatusMessage.Ok;
        }

        public StatusMessage ChangePassword(ChangePasswordDTO dto, string? userId)
        {
            if (userId == null)
                return StatusMessage.Unauthorized;
            User? user = _uow.UserRepo.Find(userId);
            if (user == null || HashPassword(dto.CurrentPassword) != user.Password)
                return StatusMessage.Unauthorized;

            user.Password = HashPassword(dto.NewPassword);
            _uow.Save();
            return StatusMessage.Ok;
        }

        //Forgot password
        public string? GenerateResetPasswordLink(string email, string domain)
        {
            User? user = _uow.UserRepo.FindByEmail(email);
            if (user == null)
                return null;

            user.Token = Guid.NewGuid().ToString();
            _uow.Save();

            string resetPasswordLink = $"{domain}/resetpassword/{user.Id}/{user.Token}";
            return resetPasswordLink;
        }

        /// <summary>
        /// Return generated password
        /// </summary>
        public string? ResetPassword(string? userId, string token)
        {
            if (userId == null)
                return null;
            User? user = _uow.UserRepo.Find(userId);
            if (user == null || user.Token != token)
                return null;

            user.Password = Guid.NewGuid().ToString()[..10].Replace("-", "");
            _uow.Save();
            return user.Password;
        }






        /// <summary>
        /// Check login result and return tokens
        /// </summary>
        public StatusMessage Login(LoginDTO dto, JwtProvider jwtProvider,
            out string? accessToken, out string? refreshToken)
        {
            accessToken = null;
            refreshToken = null;

            User? user = _uow.UserRepo.FindByPhoneOrEmail(dto.PhoneOrEmail);
            if (user == null)
                return StatusMessage.Unauthorized;
            if (user.AccessFailedCount >= MAX_ACCESS_FAILED_COUNT)
                return StatusMessage.Forbidden;

            //check password
            if (user.Password != HashPassword(dto.Password))
            {
                user.AccessFailedCount++;
                _uow.Save();
                return StatusMessage.Unauthorized;
            }

            //reset access failed count
            user.AccessFailedCount = 0;
            (string, string) tokens = UpdateToken(jwtProvider, user);
            accessToken = tokens.Item1;
            refreshToken = tokens.Item2;
            _uow.Save();
            return StatusMessage.Ok;
        }

        /// <summary>
        /// Check token and return new tokens
        /// </summary>
        public StatusMessage Refresh(string? accessToken, string? refreshToken, JwtProvider jwtProvider,
            out string? newAccessToken, out string? newRefreshToken)
        {
            newAccessToken = null;
            newRefreshToken = null;

            if (accessToken == null || refreshToken == null)
                return StatusMessage.Unauthorized;
            var principal = jwtProvider.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
                return StatusMessage.Unauthorized;
            User? user = FindByClaims(principal);
            if (user == null)
                return StatusMessage.Unauthorized;
            if (user.RefreshToken != refreshToken)
                return StatusMessage.Unauthorized;

            (string, string) tokens = UpdateToken(jwtProvider, user);
            newAccessToken = tokens.Item1;
            newRefreshToken = tokens.Item2;
            _uow.Save();
            return StatusMessage.Ok;
        }





        public ShopDTO? GetShop(string? userId)
        {
            return userId == null ? null : _uow.ShopRepo.Get(userId);
        }





        private User Adapt(RegisterDTO _)
        {
            DateTime now = DateTime.Now;
            return new User
            {
                Id = Guid.NewGuid().ToString(),
                FullName = _.FullName,
                Phone = _.Phone,
                Password = HashPassword(_.Password),
                Email = _.Email,
                DateOfBirth = _.DateOfBirth,
                Role = Role.User,
                JoinDate = now,
                LastSeen = now
            };
        }

        private async Task ApplyChanges(UpdateUserDTO _, User user)
        {
            if (_.FullName != null)
                user.FullName = _.FullName;
            if (_.Phone != null)
                user.Phone = _.Phone;
            if (_.Email != null)
                user.Email = _.Email;
            if (_.DateOfBirth != null)
                user.DateOfBirth = (DateTime)_.DateOfBirth;
            if (_.Facebook != null)
                user.Facebook = _.Facebook;
            if (_.Avatar != null)
            {
                FileHelper fileHelper = new();
                if (user.Avatar != null)
                    fileHelper.DeleteFile(user.Avatar, Dir.User);
                user.Avatar = await fileHelper.SaveFile(_.Avatar, Dir.User);
            }
            if (_.Address != null)
            {
                _uow.UserRepo.LoadAddress(user);

                if (user.Address != null)
                    _uow.AddressRepo.Delete(user.Address);

                user.Address = new Address
                {
                    Province = _.Address.Province,
                    District = _.Address.District,
                    Street = _.Address.Street,
                    StreetNumber = _.Address.StreetNumber
                };
            }
        }






        //= cookieParser.getId, but not in cookie
        private string? GetIdentifier(ClaimsPrincipal userClaim)
        {
            foreach (Claim claim in userClaim.Claims)
                if (claim.Type == ClaimTypes.NameIdentifier)
                    return claim.Value;
            return null;
        }

        private string HashPassword(string password)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));
            return builder.ToString();
        }

        private User? FindByClaims(ClaimsPrincipal userClaim)
        {
            string? id = GetIdentifier(userClaim);
            if (id == null)
                return null;

            return _uow.UserRepo.Find(id);
        }

        //...
        private (string, string) UpdateToken(JwtProvider jwtProvider, User user)
        {
            string authToken = jwtProvider.GenerateAccessToken(user.Id, user.Role.ToString());
            string refreshToken = jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.LastSeen = DateTime.Now;
            return (authToken, refreshToken);
        }






        //Development only
        public StatusMessage RegisterAsAdmin(RegisterDTO dto)
        {
            if (_uow.UserRepo.EmailExisted(dto.Email))
                return StatusMessage.Conflict;

            DateTime now = DateTime.Now;
            User admin = new()
            {
                Id = Guid.NewGuid().ToString(),
                FullName = dto.FullName,
                Phone = dto.Phone,
                Password = HashPassword(dto.Password),
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Role = Role.Admin,
                JoinDate = now,
                LastSeen = now
            };
            _uow.UserRepo.Insert(admin);
            _uow.Save();
            return StatusMessage.Created;
        }
    }
}
