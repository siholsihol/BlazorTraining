using BlazorMenu.Constants;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls.Constants;
using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorMenu.Helper
{
    public static class BlazorMenuUtility
    {
        #region SVG
        private static readonly ConcurrentDictionary<string, string[]> SvgIdDictionary = new();
        private static readonly string DefaultMenuIconIdPrefix = $"{AppConstants.MenuIconFilePath}-";
        private static readonly string DefaultMenuIconPath = $"#{DefaultMenuIconIdPrefix}";
        private static readonly string DefaultMenuIconId = AppConstants.MenuIconId;
        public static string[] GetSVGIds(string key)
        {
            return SvgIdDictionary.TryGetValue(key, out var value) ? value : Array.Empty<string>();
        }

        public static string[] GetMenuSVGIds()
        {
            var key = DefaultMenuIconPath;

            return GetSVGIds(key);
        }

        public static void SetSVGIds(string key, string[] newIds)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            SvgIdDictionary.AddOrUpdate(key, newIds, (_, _) => newIds);
        }

        public static void SetMenuIconSVGIds(string[] newIds)
        {
            var key = DefaultMenuIconPath;

            SetSVGIds(key, newIds);
        }

        public static string GetMenuSVGHref(GetMenuSVGHrefParameter poParameter)
        {
            var currentPath = string.IsNullOrEmpty(poParameter.Path)
               ? DefaultMenuIconPath
               : poParameter.Path;

            var ids = GetSVGIds(currentPath);

            if (ids.Length == 0) return string.Empty;

            var defaultId = string.IsNullOrEmpty(poParameter.Default)
                ? DefaultMenuIconId
                : poParameter.Default;

            var currentId = string.IsNullOrEmpty(poParameter.Id)
                ? defaultId
                : poParameter.Id;

            var normalizedId = GetNormalizedString(currentId);

            currentId = ids.FirstOrDefault(x =>
                string.Equals(x, normalizedId, StringComparison.OrdinalIgnoreCase));

            if (currentId is null)
            {
                currentId = defaultId;
            }

            return currentPath + currentId;
        }

        public static string GetNormalizedString(string text)
        {
            return text.Trim().ToLowerInvariant().Replace(" ", "-");
        }

        public static async Task<string[]> GetSvgSymbolIdsFromFile(byte[] file, IJSRuntime JSRuntime, string? prefix = null)
        {
            if (file is null)
                return Array.Empty<string>();

            if (BlazorMenuUtility.GetSVGIds(AppConstants.MenuIconFilePath).Length > 0)
                return Array.Empty<string>();

            if (string.IsNullOrWhiteSpace(prefix))
                prefix = DefaultMenuIconIdPrefix;

            var symbolIds = new List<string>();

            var icons = Encoding.UTF8.GetString(file);

            var matches = Regex.Matches(icons, "symbol id=\"(.*?)\"", RegexOptions.IgnoreCase);
            symbolIds = matches.Cast<Match>().Select(m => m.Groups[1].Value).ToList();

            icons = Regex.Replace(icons, "symbol id=\"(.*?)\"", $"symbol id=\"{prefix}$1\"");

            await JSRuntime.InvokeVoidAsync(JsConstants.InjectSvgToBody, icons);

            return symbolIds.ToArray();
        }
        #endregion
    }

    public class GetMenuSVGHrefParameter
    {
        public string Id { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Default { get; set; } = string.Empty;
    }
}
