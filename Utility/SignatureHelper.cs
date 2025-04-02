namespace tradeApi2.Utility;

public static class SignatureHelper
{
    public static string GenerateSignature(Dictionary<string, string> parameters, string apiKey)
    {
        var sortedParams = parameters
            .Where(p => !string.IsNullOrEmpty(p.Value) && p.Key.ToLower() != "sign")
            .OrderBy(p => p.Key, StringComparer.Ordinal)
            .Select(p => $"{p.Key}={p.Value}");

        string stringA = string.Join("&", sortedParams);
        string stringSignTemp = $"{stringA}&key={apiKey}";

        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            var hashBytes = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringSignTemp));
            var sb = new System.Text.StringBuilder();
            foreach (var b in hashBytes)
            {
                sb.Append(b.ToString("X2")); // Uppercase hex
            }
            return sb.ToString();
        }
    }
}
