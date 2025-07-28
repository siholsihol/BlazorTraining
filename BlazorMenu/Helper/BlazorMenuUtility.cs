using BlazorMenu.Constants;
using System.Collections.Concurrent;

namespace BlazorMenu.Helper
{
    internal static class BlazorMenuUtility
    {
        #region SVG
        private static readonly ConcurrentDictionary<string, string[]> SvgIdDictionary = new();
        private static readonly string DefaultMenuIconPath = AppConstants.MenuIconPath;
        private static readonly string DefaultMenuIconId = AppConstants.MenuIconId;
        internal static string[] GetSVGIds(string key)
        {
            return SvgIdDictionary.TryGetValue(key, out var value) ? value : Array.Empty<string>();
        }

        internal static void SetSVGIds(string key, string[] newIds)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            SvgIdDictionary.AddOrUpdate(key, newIds, (_, _) => newIds);
        }

        internal static void SetMenuIconSVGIds(string[] newIds)
        {
            var key = DefaultMenuIconPath;

            SetSVGIds(key, newIds);
        }

        internal static string GetMenuSVGHref(string id, string path = "")
        {
            var currentPath = DefaultMenuIconPath;

            if (!string.IsNullOrEmpty(path))
            {
                currentPath = path;
            }

            var ids = GetSVGIds(currentPath);

            var normalizedId = string.IsNullOrWhiteSpace(id) ? string.Empty : id.ToLowerInvariant().Replace(" ", "-");

            var currentId = DefaultMenuIconId;

            if (ids.Contains(normalizedId, StringComparer.OrdinalIgnoreCase))
            {
                currentId = normalizedId;
            }

            return currentPath + currentId;
        }

        internal static string GetNormalizedstring(string text)
        {
            return string.IsNullOrWhiteSpace(text) ? string.Empty : text.ToLowerInvariant().Replace(" ", "-");
        }
        #endregion
    }
}
