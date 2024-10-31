using FluentAssertions;
using signa.Dto.user;
using signa.FunctionalTests.Helpers;
using signa.Models;

namespace signa.FunctionalTests.Services.UsersService;

public class UpdateUserTests : UsersServicesTestBase
{
    [Test]
    public async Task Should_success_update_user()
    {
        var createdUserId = await TestCreateUser();

        var updateUserDto = new UpdateUserDto
        {
            Email = MailGeneratorHelper.Generate(),
            Password = "!qQqQqQqQ322",
            PhoneNumber = "+73222281488"
        };
        
        var updatedId = await usersService.UpdateUser(createdUserId, updateUserDto);

        var users = await usersService.GetUserEntitiesByIds([updatedId]);
        var updatedUser = users.First();
        updatedUser.Email.Should().Be(updateUserDto.Email);
        updatedUser.PhoneNumber.Should().Be(updateUserDto.PhoneNumber);
        PasswordHasher.VerifyPassword(updateUserDto.Password, updatedUser.PasswordHash, 
            Convert.FromBase64String(updatedUser.PasswordSalt)).Should().BeTrue();
    }

    [Test]
    public async Task Should_success_update_user_email()
    {
        var createdUserId = await TestCreateUser();

        var updateUserDto = new UpdateUserDto
        {
            Email = MailGeneratorHelper.Generate()
        };
        
        var updatedId = await usersService.UpdateUser(createdUserId, updateUserDto);

        var users = await usersService.GetUserEntitiesByIds([updatedId]);
        var updatedUser = users.First();
        updatedUser.Email.Should().Be(updateUserDto.Email); 
    }

    [Test]
    public async Task Should_success_update_user_password()
    {
        var createdUserId = await TestCreateUser();

        var updateUserDto = new UpdateUserDto
        {
            Password = "!qQqQqQqQ322"
        };
        
        var updatedId = await usersService.UpdateUser(createdUserId, updateUserDto);

        var users = await usersService.GetUserEntitiesByIds([updatedId]);
        var updatedUser = users.First();
        PasswordHasher.VerifyPassword(updateUserDto.Password, updatedUser.PasswordHash, 
            Convert.FromBase64String(updatedUser.PasswordSalt)).Should().BeTrue();
    }

    [Test]
    public async Task Should_success_update_user_phone_number()
    {
        var createdUserId = await TestCreateUser();

        var updateUserDto = new UpdateUserDto
        {
            PhoneNumber = "+73222281488"
        };
        
        var updatedId = await usersService.UpdateUser(createdUserId, updateUserDto);

        var users = await usersService.GetUserEntitiesByIds([updatedId]);
        var updatedUser = users.First();
        updatedUser.PhoneNumber.Should().Be(updateUserDto.PhoneNumber);
    }
    
    private async Task<Guid> TestCreateUser()
    {
        var email = MailGeneratorHelper.Generate();
        var fullname = MockUsersFio.GenerateFullName().Split();
        var user = new CreateUserDto
        {
            Email = email,
            FirstName = fullname[1],
            Gender = "male",
            GroupNumber = "РИ-220930",
            LastName = fullname[0],
            Password = "!qQqQqQqQqQ322",
            Patronymic = fullname[2]
        };
        
        var result = await usersService.CreateUser(user);
        unitOfWork.SaveChanges();
        return result;
    }
}