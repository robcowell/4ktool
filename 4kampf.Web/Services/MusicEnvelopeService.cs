using System.Globalization;

namespace _4kampf.Web.Services;

/// <summary>
/// Service for loading and managing music envelope data for shader synchronization.
/// Supports both 4klang (legacy) and Sointu (cross-platform) envelope formats.
/// This replaces the 32-bit assembly code dependency by loading pre-rendered envelope data.
/// </summary>
public class MusicEnvelopeService
{
    private float[][]? _envelopeSamples;
    private int _numInstruments = 0;
    private const int SAMPLE_RATE = 44100;
    private const int ENVELOPE_SAMPLE_RATE = 256; // Envelopes are sampled every 256 audio samples

    /// <summary>
    /// Loads envelope data from .dat files.
    /// Supports:
    /// - 4klang format: comma-separated float values (legacy)
    /// - Sointu format: TBD (will support Sointu's envelope output format)
    /// These files contain envelope values over time for shader synchronization.
    /// </summary>
    public async Task LoadEnvelopesAsync(string basePath = ".")
    {
        _envelopeSamples = new float[16][];
        _numInstruments = 0;

        for (int i = 0; i < 16; i++)
        {
            string filename = Path.Combine(basePath, $"envelope-{i}.dat");
            
            if (!File.Exists(filename))
                continue;

            try
            {
                string content = await File.ReadAllTextAsync(filename);
                string[] split = content.Split(',', StringSplitOptions.RemoveEmptyEntries);
                
                _envelopeSamples[i] = new float[split.Length];
                for (int j = 0; j < split.Length; j++)
                {
                    if (float.TryParse(split[j].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                    {
                        _envelopeSamples[i][j] = value;
                    }
                }
                
                _numInstruments++;
            }
            catch (Exception ex)
            {
                // Log error but continue loading other envelopes
                Console.WriteLine($"Error loading envelope {i}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Gets the envelope sync values for the current playback position.
    /// This matches the behavior of the original Get4klangSync() method.
    /// </summary>
    /// <param name="currentPositionSeconds">Current playback position in seconds</param>
    /// <returns>Array of envelope values (one per instrument), or null if no envelopes loaded</returns>
    public float[]? GetEnvelopeSync(double currentPositionSeconds)
    {
        if (_envelopeSamples == null || _envelopeSamples[0] == null)
            return null;

        // Calculate which envelope sample corresponds to current position
        // Envelopes are sampled every 256 audio samples at 44100 Hz
        long currentSample = (long)Math.Min(
            _envelopeSamples[0].Length - 1,
            (currentPositionSeconds * SAMPLE_RATE) / ENVELOPE_SAMPLE_RATE
        );

        float[] result = new float[_numInstruments];
        for (int i = 0; i < _numInstruments; i++)
        {
            if (_envelopeSamples[i] != null && currentSample < _envelopeSamples[i].Length)
            {
                result[i] = _envelopeSamples[i][currentSample];
            }
        }

        return result;
    }

    /// <summary>
    /// Gets the number of instruments with envelope data loaded.
    /// </summary>
    public int NumInstruments => _numInstruments;

    /// <summary>
    /// Checks if envelope data is loaded and available.
    /// </summary>
    public bool HasEnvelopes => _envelopeSamples != null && _numInstruments > 0;
}

