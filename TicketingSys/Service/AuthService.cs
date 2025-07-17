using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketingSys.Dtos.AuthDtos;
using TicketingSys.Exceptions;
using TicketingSys.Interfaces.ServiceInterfaces;
using TicketingSys.Models;

namespace TicketingSys.Service;

public class AuthService(
    UserManager<User> userManager,
    ITokenService tokenService,
    SignInManager<User> signInManager,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task CreateUser(RegisterDto registerDto)
    {
        User user = new()
        {
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserName = new StringBuilder(registerDto.FirstName)
                    .Append(registerDto.LastName)
                    .ToString(),
        };

        IdentityResult newUser = await userManager.CreateAsync(user, registerDto.Password);

        if (!newUser.Succeeded)
        {
            // errors such as password too short
            List<string> dataError = newUser.Errors.Select(x => x.Description).ToList();

            IdentityError? uniqueFailed = newUser.Errors.FirstOrDefault(x => x.Code == "DuplicateUserName"
                                                                             || x.Code == "DuplicateEmail");

            if (uniqueFailed != null)
            {
                throw new UniqueConstraintFailedException("Duplicate email or username");
            }

            throw new InfeasibleOperationException("Please check your data and try again.");
        }
    }

    public async Task<LoginResponseDto> Login(LoginDto loginDto)
    {
        User? user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        user = user ?? throw new InvalidLoginCredentialsException();

        SignInResult result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            throw new InvalidLoginCredentialsException();
        }

        await userManager.UpdateAsync(user);

        return new LoginResponseDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = tokenService.CreateToken(user)
        };
    }
}