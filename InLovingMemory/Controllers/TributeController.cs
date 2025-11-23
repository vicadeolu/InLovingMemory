using InLovingMemory.Data.Entity;
using InLovingMemory.Services;
using Microsoft.AspNetCore.Mvc;

namespace InLovingMemory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TributeController : ControllerBase
    {
        private readonly MemorialTributeService _service;

        public TributeController(MemorialTributeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var tribute = await _service.GetById(id);
            return tribute == null ? NotFound() : Ok(tribute);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MemorialTribute tribute)
        {
            await _service.Create(tribute);
            return CreatedAtAction(nameof(GetById), new { id = tribute.Id }, tribute);
        }

        [HttpPost("upload/{tributeId}")]
        public async Task<IActionResult> UploadImage(string tributeId, [FromBody] ImageUploadRequest request)
        {
            if (string.IsNullOrEmpty(request.Base64String) || string.IsNullOrEmpty(request.FileName))
                return BadRequest("Base64 string and file name are required.");

            try
            {
                // Remove data URI prefix if present (e.g., "data:image/png;base64,")
                var base64Data = request.Base64String;
                if (base64Data.Contains(","))
                {
                    base64Data = base64Data.Split(',')[1];
                }

                // Convert Base64 to bytes
                byte[] fileBytes = Convert.FromBase64String(base64Data);

                // Create uploads folder if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                // Generate unique file name
                var fileExt = Path.GetExtension(request.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExt}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Write file to disk
                await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

                // Update MongoDB document
                var tribute = await _service.GetById(tributeId);
                if (tribute == null)
                    return NotFound("Tribute not found.");

                // Initialize Images list if null
                if (tribute.Images == null)
                    tribute.Images = new List<ImageInfo>();

                tribute.Images.Add(new ImageInfo
                {
                    ImageName = request.FileName,
                    FileExt = fileExt,
                    FilePath = $"/uploads/{fileName}",
                });

                tribute.DateModified = DateTime.UtcNow;
                await _service.Update(tribute);

                return Ok(new
                {
                    message = "Image uploaded successfully",
                    path = $"/uploads/{fileName}",
                    fileName = fileName
                });
            }
            catch (FormatException)
            {
                return BadRequest("Invalid Base64 string format.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading image: {ex.Message}");
            }
        }

        // Request model
        public class ImageUploadRequest
        {
            public string Base64String { get; set; }
            public string FileName { get; set; }
        }

        //public async Task<IActionResult> UploadImage(string tributeId, IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");

        //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        //    if (!Directory.Exists(uploadsFolder))
        //        Directory.CreateDirectory(uploadsFolder);

        //    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //    var filePath = Path.Combine(uploadsFolder, fileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    // Save path in MongoDB
        //    var tribute = await _service.GetById(tributeId);
        //    if (tribute == null) return NotFound();

        //    tribute.Images.Add(new ImageInfo
        //    {
        //        ImageName = file.FileName,
        //        FileExt = Path.GetExtension(file.FileName),
        //        FilePath = $"/uploads/{fileName}"
        //    });

        //    await _service.Update(tribute); // Add an Update method in your service
        //    return Ok(new { path = $"/uploads/{fileName}" });
        //}

    }
}
