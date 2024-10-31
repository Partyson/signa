using FluentAssertions;
using signa.Dto.user;
using signa.FunctionalTests.Helpers;

namespace signa.FunctionalTests.Services.UsersService;

public class UpdateUserTests : UsersServicesTestBase
{
    [Test]
    public async Task Should_success_update_user()
    {
        var createdUserId = await TestCreateUser();

        var updateUserDto = new UpdateUserDto
        {
            Email = MailGeneratorHelper.Generate()
        };
        
        var updatedId = await usersService.UpdateUser(createdUserId, updateUserDto);

        var user = await usersService.GetUserEntitiesByIds([updatedId]);
        user.First().Email.Should().Be(updateUserDto.Email);
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