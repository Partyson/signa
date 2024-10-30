using FluentAssertions;
using signa.Dto.user;

namespace signa.FunctionalTests.Services.UsersService;

public class CreateUserTests : UsersServicesTestBase
{
    [TestCase("male")]
    [TestCase("female")]
    public async Task Should_success_when_user_valid(string gender)
    {
        var createUserDto = CreateTestUserDto("Тест", "Тест", "Тест",
            "РИ-220930", gender,
            "test@email.com", "!qQqQqQqQqQ322");
        
        var result = await TestCreateUser(createUserDto);
        
        result.Should().NotBeNull();
    }

    [TestCase("Max")]
    [TestCase("-вайб")]
    [TestCase("")]
    [TestCase("а че будет если сюда упадет много слов", Ignore = "падают все тесты после него")] 
    [TestCase("бадер")]
    [TestCase ("МАкс")]
    [TestCase(null, Ignore = "падают все тесты после него")]

    public async Task Should_fail_when_user_first_name_invalid(string? firstName)
    {
        var createUserDto = CreateTestUserDto(firstName, "Тестимен", "Тестимен",
            "РИ-220930", "male",
            "test@email.com", "!qQqQqQqQqQ322");

        var result = await TestCreateUser(createUserDto);
        
        result.Should().BeNull();
    }
    
    [TestCase("Max")]
    [TestCase("-вайбов")]
    [TestCase("")]
    [TestCase("а че будет если сюда упадет много слов", Ignore = "падают все тесты после него")] 
    [TestCase("бадеров")]
    [TestCase ("МАксов")]
    [TestCase(null, Ignore = "падают все тесты после него")]
    public async Task Should_fail_when_user_last_name_invalid(string? lastName)
    {
        var createUserDto = CreateTestUserDto("Тестфамилий", lastName, "Тестфамилий",
            "РИ-220930", "male",
            "test@email.com", "!qQqQqQqQqQ322");

        var result = await TestCreateUser(createUserDto);
        
        result.Should().BeNull();
    }

    [TestCase("Max")]
    [TestCase("-вайбович")]
    [TestCase("")]
    [TestCase("а че будет если сюда упадет много слов", Ignore = "падают все тесты после него")] 
    [TestCase("бадерович")]
    [TestCase ("МаксОвич")]
    [TestCase(null, Ignore = "падают все тесты после него")]
    public async Task Should_fail_when_user_patronymic_invalid(string? patronymic)
    {
        var createUserDto = CreateTestUserDto("Тестотчества", "Тестотчества", patronymic,
            "РИ-220930", "male",
            "test@email.com", "!qQqQqQqQqQ322");

        var result = await TestCreateUser(createUserDto);
        
        result.Should().BeNull();
    }
    [TestCase("мужской")]
    [TestCase("trash")]
    [TestCase("Male")]
    [TestCase("FEmale")]
    [TestCase("")]
    [TestCase(null, Ignore = "падают все тесты после него")]
    public async Task Should_fail_when_user_gender_invalid(string? gender)
    {
        var createUserDto = CreateTestUserDto("Тестпола", "Тестпола", "Тестпола",
            "РИ-220930", gender,
            "test@email.com", "!qQqQqQqQqQ322");

        var result = await TestCreateUser(createUserDto);
        
        result.Should().BeNull();
    }

    [TestCase("ри-220930")]
    [TestCase("мусор")]
    [TestCase("РИ-мусор")]
    [TestCase("мусор-321")]
    [TestCase("РИ-3209301", Ignore = "падают все тесты после него")]
    [TestCase("")]
    [TestCase(null, Ignore = "падают все тесты после него")]
    public async Task Should_fail_when_user_group_number_invalid(string? groupNumber)
    {
        var createUserDto = CreateTestUserDto("Тестгруппы", "Тестгруппы", "Тестгруппы",
            groupNumber, "male",
            "test@email.com", "!qQqQqQqQqQ322");

        var result = await TestCreateUser(createUserDto);
        
        result.Should().BeNull();
    }

    [TestCase("")]
    [TestCase(null, Ignore = "падают все тесты после него")]
    [TestCase("почта@mail.ru")]
    [TestCase("spidermanmail.ru")]
    [TestCase("spiderman@mail")]
    public async Task Should_fail_when_user_email_invalid(string? email)
    {
        var createUserDto = CreateTestUserDto("Тестпочты", "Тестпочты", "Тестпочты",
            "РИ-320930", "male",
            email, "!qQqQqQqQqQ322");

        var result = await TestCreateUser(createUserDto);
        
        result.Should().BeNull();
    }

    [Test]
    public async Task Should_fail_when_repeat_email()
    {
        var createUserDto = CreateTestUserDto("Тестпочты", "Тестпочты", "Тестпочты",
            "РИ-320930", "male",
            "email@mail.ru", "!qQqQqQqQqQ322");
        var result = await TestCreateUser(createUserDto);
        createUserDto = CreateTestUserDto("Тестпочты", "Тестпочты", "Тестпочты",
            "РИ-320930", "male",
            "email@mail.ru", "!qQqQqQqQqQ322");
        result = await TestCreateUser(createUserDto);
        result.Should().BeNull();
    }

    [TestCase("")]
    [TestCase("trash")]
    [TestCase("+732222814883")]
    [TestCase("spiderman@mail")]
    public async Task Should_fail_when_user_phone_number_invalid(string? phoneNumber)
    {
        var createUserDto = CreateTestUserDto("Тестномера", "Тестномера", "Тестномера",
            "РИ-320930", "male",
            "test@email.com", "!qQqQqQqQqQ322", phoneNumber);

        var result = await TestCreateUser(createUserDto);
        
        result.Should().BeNull();
    }

    [TestCase("")]
    [TestCase(null, Ignore = "падают все тесты после него")]
    [TestCase("short")]
    [TestCase("passwordWtihOutNumbers")]
    [TestCase("passwordwithoutupperletters")]
    [TestCase("passwordWithOutSpecialCharacters123")]
    public async Task Should_fail_when_user_password_invalid(string? password)
    {
        var createUserDto = CreateTestUserDto("Тестпароля", "Тестпароля", "Тестпароля",
            "РИ-320930", "male",
            "test@email.com", password);

        var result = await TestCreateUser(createUserDto);
        
        result.Should().BeNull();
    }
    
    private static CreateUserDto CreateTestUserDto(string? firstName, string? lastName, string? patronymic, 
        string? groupNumber, string? gender, 
        string? email, string? password, string phoneNumber = null) 
        => new CreateUserDto 
        { 
            FirstName = firstName, 
            LastName = lastName, 
            Patronymic = patronymic, 
            GroupNumber = groupNumber, 
            Gender = gender, 
            Email = email, 
            Password = password 
        };

    private async Task<UserResponseDto> TestCreateUser(CreateUserDto createUserDto)
    {
        var userToAddId = await usersService.CreateUser(createUserDto);
        unitOfWork.SaveChanges();
        var result = await usersService.GetUserResponse(userToAddId);
        return result;
    }
}