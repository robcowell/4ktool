using Microsoft.AspNetCore.Mvc;
using _4kampf.Web.Services;

namespace _4kampf.Web.Controllers;

/// <summary>
/// Test controller for verifying Sointu integration.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SointuTestController : ControllerBase
{
    private readonly SointuService _sointuService;
    private readonly ILogger<SointuTestController> _logger;

    public SointuTestController(SointuService sointuService, ILogger<SointuTestController> logger)
    {
        _sointuService = sointuService;
        _logger = logger;
    }

    /// <summary>
    /// Tests if Sointu is available and returns status information.
    /// </summary>
    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            available = _sointuService.IsAvailable,
            message = _sointuService.IsAvailable 
                ? "Sointu is available and ready to use" 
                : "Sointu is not available. Please install Sointu to use music synthesis."
        });
    }

    /// <summary>
    /// Tests Sointu compilation with a simple test song.
    /// </summary>
    [HttpPost("test-compile")]
    public async Task<IActionResult> TestCompile()
    {
        if (!_sointuService.IsAvailable)
        {
            return BadRequest(new { error = "Sointu is not available" });
        }

        try
        {
            // Create a simple test YAML song
            string testYaml = @"song:
  bpm: 120
  rows_per_beat: 4
  length: 64
  patterns:
    - name: pattern1
      rows: 16
      tracks:
        - name: track1
          columns:
            - note: C-4
              length: 4
  sequence:
    - pattern: pattern1
      repeat: 4";

            string testDir = Path.Combine("wwwroot", "test");
            if (!Directory.Exists(testDir))
            {
                Directory.CreateDirectory(testDir);
            }

            string yamlPath = Path.Combine(testDir, "test-song.yml");
            await System.IO.File.WriteAllTextAsync(yamlPath, testYaml);

            string outputPath = Path.Combine(testDir, "test-song.asm");
            bool success = await _sointuService.CompileSongAsync(yamlPath, outputPath);

            if (success && System.IO.File.Exists(outputPath))
            {
                return Ok(new
                {
                    success = true,
                    message = "Sointu compilation test passed",
                    outputFile = outputPath,
                    fileSize = new FileInfo(outputPath).Length
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Sointu compilation test failed - output file not created"
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error testing Sointu compilation");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

