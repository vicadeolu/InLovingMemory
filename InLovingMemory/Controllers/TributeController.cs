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

        [HttpPatch("{Id}")]
        public async Task<IActionResult> UpdateTribute(string Id, [FromBody] MemorialTribute request)
        {
            
            try
            {
                // Remove data URI prefix if present (e.g., "data:image/png;base64,")
          
                
                // Update MongoDB document
                var tribute = await _service.GetById(Id);
                if (tribute == null)
                    return NotFound("Tribute not found.");

                // Initialize Images list if null
               tribute.Tribute = request.Tribute;
                tribute.FullName = request.FullName;
                
                tribute.DateModified = DateTime.UtcNow;
                await _service.Update(tribute);

                return Ok(new
                {
                    message = "Tribute updated successfully",
                    data = tribute
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
