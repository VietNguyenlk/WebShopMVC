using AppMVC.Data;
using AppMVC.DTOs;
using AppMVC.Helpers;
using AppMVC.Models;

namespace AppMVC.Services
{
    public interface IUserService
    {
        UserDto Register(RegisterDto dto);
        UserDto? Login(LoginDto dto);
    }
    public class UserService(AppDbContext context, JwtService jwtService) : IUserService

    {
        private readonly AppDbContext _context = context;
        private readonly JwtService _jwtService = jwtService;

        public UserDto? Login(LoginDto dto)
        {
            // Chỉ tìm user theo username
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Username);
            Console.WriteLine(dto.Username);
            if (user == null)
            {
                return null; // Không tìm thấy user
            }

            // Sau đó kiểm tra password
            if (!PasswordHasher.VerifyPassword(dto.Password, user.Password))
            {
                return null; // Password không đúng
            }
            return new UserDto
            {
                Id = user.Id,
                Username = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = _jwtService.GenerateToken(user)
            };  
        }

        public UserDto Register(RegisterDto dto)
        {
            var hashedPassword = PasswordHasher.HashPassword(dto.Password);
            var user = new User
            {
                Name = dto.Username,
                Email = dto.Email,
                Password = hashedPassword,
                Role = "Customer"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return new UserDto
            {
                Id = user.Id,
                Username = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = _jwtService.GenerateToken(user)
            };

        }
    }
}
