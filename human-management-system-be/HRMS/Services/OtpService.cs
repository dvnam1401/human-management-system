using System.Collections.Concurrent;

namespace HRMS.Services;

public class OtpService : IOtpService
{
    private readonly ConcurrentDictionary<string, OtpEntry> _otpStore = new();
    private readonly TimeSpan _otpExpiration = TimeSpan.FromMinutes(5);

    public string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    public void StoreOtp(string email, string otp)
    {
        var entry = new OtpEntry
        {
            Otp = otp,
            ExpiresAt = DateTime.Now.Add(_otpExpiration)
        };
        
        _otpStore[email.ToLower()] = entry;
    }

    public bool ValidateOtp(string email, string otp)
    {
        var emailKey = email.ToLower();
        
        if (!_otpStore.TryGetValue(emailKey, out var entry))
            return false;

        if (DateTime.Now > entry.ExpiresAt)
        {
            _otpStore.TryRemove(emailKey, out _);
            return false;
        }

        return entry.Otp == otp;
    }

    public void RemoveOtp(string email)
    {
        _otpStore.TryRemove(email.ToLower(), out _);
    }

    private class OtpEntry
    {
        public string Otp { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}

