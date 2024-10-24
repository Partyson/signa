using FluentAssertions;
using Mapster;
using signa.Dto.user;
using signa.Entities;
using signa.Models;

namespace signa.UnitTests;

public class MappingConfigTests
{
    public MappingConfigTests()
    {
        MappingConfig.RegisterMappings();
    }

    [Test]
    public void MappingCreateUserDtoToUserEntityTest()
    {
        var createUserDto = new CreateUserDto
        {
            FirstName = "John",
            LastName = "Jordan",
            Password = "password",
            Patronymic = "patronymic",
            Gender = "female",
            GroupNumber = "РИ-220930",
            Email = "john.doe@gmail.com"
        };
        var userEntity = createUserDto.Adapt<UserEntity>();
        PasswordHasher.VerifyPassword(createUserDto.Password,
            userEntity.PasswordHash,
            Convert.FromBase64String(userEntity.PasswordSalt)).Should().BeTrue();
    }
}