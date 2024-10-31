using System.Security.Cryptography;
using FluentAssertions;
using signa.Models;

namespace signa.UnitTests;

public class PasswordHasherTests
{
    [Test]
    public void SaltDecodingTest()
    {
        var salt = PasswordHasher.GenerateSalt();
        var saltInBd = Convert.ToBase64String(salt);
        var decodingSalt = Convert.FromBase64String(saltInBd);
        salt.Should().BeEquivalentTo(decodingSalt);
    }

    [Test]
    public void VerifyPasswordHashingTest()
    {
        var salt = PasswordHasher.GenerateSalt();
        var password = "!qQqQqQqQ322";
        var passwordHash = PasswordHasher.HashPassword(password, salt);
        var saltInDb = Convert.ToBase64String(salt);
        PasswordHasher.VerifyPassword(password, passwordHash, Convert.FromBase64String(saltInDb))
            .Should().BeTrue();
    }

    [Test]
    public void WrongPasswordTest()
    {
        var salt = PasswordHasher.GenerateSalt();
        const string password = "password";
        const string inputPassword = "notpassword";
        var passwordHash = PasswordHasher.HashPassword(password, salt);
        var saltInDb = Convert.ToBase64String(salt);
        PasswordHasher.VerifyPassword(inputPassword, passwordHash, Convert.FromBase64String(saltInDb))
            .Should().BeFalse();
    }
}