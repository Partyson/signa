using FluentAssertions;
using signa.Dto.user;
using signa.FunctionalTests.Helpers;

namespace signa.FunctionalTests.Services.UsersService;

public class DeleteUserTests : UsersServicesTestBase
{
    [Test]
    public async Task Should_success_delete_user()
    {
        var createdUserId = await TestCreateUser();

        var deletedUserId = await usersService.DeleteUser(createdUserId);
        unitOfWork.SaveChanges();
        
        var user = await usersService.GetUserEntitiesByIds([deletedUserId]);
        user.First().IsDeleted.Should().BeTrue();
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