using LoginRagil.Models;
using LoginRagil.NewFolder;

namespace LoginRagil.Servieces
{
    public class ShortifyService
    {
        private LoginDB _db;
        public ShortifyService(LoginDB db)
        {
            _db = db;
        }

        public async Task<string> Short(string fullUrl)
        {
            if (string.IsNullOrEmpty(fullUrl)) return "";
            //todo: validate fullurl
            var url = _db.UrlPairs.FirstOrDefault(u => u.FullUrl == fullUrl);
            if (url != null) { return url.ShortUrl; }
            //made-todo: check if the fullurl already exists in the user
            string result = "";
            do
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var random = new Random();
                result = new string(
                    Enumerable.Repeat(chars, 6)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());
            } while (_db.UrlPairs.Any(u => u.ShortUrl == result));
            result = "https://localhost:7207/s/" + result;
            // made-todo: check for errors if the code already exists
            _db.UrlPairs.Add(new UrlPair { FullUrl = fullUrl, ShortUrl = result, Created = DateTime.UtcNow });
            await _db.SaveChangesAsync();
            return result;
        }
    }
}
