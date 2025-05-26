using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;

[ApiController]
[Route("users/{userId}/avatar")]
public class AvatarController : ControllerBase
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public AvatarController(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["YcStorage:BucketName"]
                      ?? throw new ArgumentNullException("BucketName is not configured.");
    }

    [HttpPost]
    public async Task<IActionResult> UploadAvatar(string userId, IFormFile avatarFile)
    {
        if (avatarFile == null || avatarFile.Length == 0 || Path.GetExtension(avatarFile.FileName) != ".jpg")
        {
            return BadRequest("Файл не предоставлен или предоставлен неверный формат (не .jpg).");
        }

        // Генерируем уникальный ключ для объекта (например, avatars/userId.jpg)
        // Можно добавить GUID или timestamp для предотвращения кеширования или перезаписи
        var fileExtension = Path.GetExtension(avatarFile.FileName);
        var objectKey = $"avatars/{userId}{fileExtension}";

        try
        {
            using (var stream = avatarFile.OpenReadStream())
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = objectKey,
                    InputStream = stream,
                    ContentType = avatarFile.ContentType, // Важно для правильного отображения в браузере
                    CannedACL = S3CannedACL.PublicRead // Или настройте доступ через Bucket Policies
                    // Если хотите сделать аватарку приватной, не используйте CannedACL
                };

                await _s3Client.PutObjectAsync(putRequest);
            }

            // Можно вернуть URL или просто статус OK
            // Внимание: URL будет зависеть от настроек бакета (публичный/приватный)
            return Ok(new { message = "Аватар успешно загружен.", key = objectKey });
        }
        catch (AmazonS3Exception e)
        {
            return StatusCode(500, $"Ошибка при загрузке в S3: {e.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Внутренняя ошибка: {e.Message}");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAvatarUrl(string userId)
    {
        // Предполагаем, что вы знаете/находите ключ объекта (например, из БД)
        // Здесь для примера ищем .jpg, но лучше хранить полный ключ
        var objectKey = $"avatars/{userId}.jpg"; // <-- Это нужно будет определить точнее

        try
        {
            // Проверяем, существует ли объект (опционально, но хорошо бы)
            await _s3Client.GetObjectMetadataAsync(_bucketName, objectKey);

            var getRequest = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                Expires = DateTime.UtcNow.AddMinutes(15) // Ссылка будет действительна 15 минут
            };

            var url = _s3Client.GetPreSignedURL(getRequest);

            // Вы можете вернуть URL как строку
            // return Ok(new { avatarUrl = url });

            // Или сделать редирект, чтобы браузер сразу пошел по ссылке
            return Redirect(url);
        }
        catch (AmazonS3Exception e)
        {
            if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound("Аватар не найден.");
            }
            return StatusCode(500, $"Ошибка S3: {e.Message}");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Внутренняя ошибка: {e.Message}");
        }
    }
}
