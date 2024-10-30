using EntityFrameworkCore.UnitOfWork.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using signa.Dto.user;
using signa.FunctionalTests.Helpers;
using signa.Interfaces;

namespace signa.FunctionalTests.Services.UsersService;

public class GetUserTests : UsersServicesTestBase
{

    [Test]
    public async Task Should_get_one_user()
    {
        var userId = await TestCreateUser();
        
        var result = await usersService.GetUserResponse(userId);
        
        result.Should().NotBeNull();
    }

    [TestCase(10)]
    public async Task Should_get_all_users_by_ids(int count)
    {
        var userIdList = Enumerable.Range(0, count).Select(_ => TestCreateUser().Result).ToList();
        
        var userResponseList = await usersService.GetUserEntitiesByIds(userIdList);
        
        userResponseList.Should().NotBeEmpty();
        userResponseList.Count.Should().Be(userIdList.Count);
    }

    [TestCase(2)]
    public async Task Should_fail_when_get_user_who_does_not_exist(int count)
    {
        var userIdList = Enumerable.Range(0, count - 1).Select(_ => TestCreateUser().Result).ToList();
        userIdList.Add(Guid.NewGuid());
        
        var userResponseList = await usersService.GetUserEntitiesByIds(userIdList);
        
        userResponseList.Count.Should().NotBe(userIdList.Count);
    }

    [TestCase(10)]
    public async Task Should_fail_when_get_non_existent_users(int count)
    {
        var userIdList = Enumerable.Range(0, count).Select(_ => Guid.NewGuid()).ToList();
        
        var userResponseList = await usersService.GetUserEntitiesByIds(userIdList);
        
        userResponseList.Should().BeEmpty();
    }

    private async Task<Guid> TestCreateUser()
    {
        var email = MailGeneratorHelper.Generate();
        var user = new CreateUserDto
        {
            Email = email,
            FirstName = "Тестгета",
            Gender = "male",
            GroupNumber = "РИ-220930",
            LastName = "Тестгета",
            Password = "!qQqQqQqQqQ322",
            Patronymic = "Тестгета"
        };
        
        var result = await usersService.CreateUser(user);
        unitOfWork.SaveChanges();
        return result;
    }
}