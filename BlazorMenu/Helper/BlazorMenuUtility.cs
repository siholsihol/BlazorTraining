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

        internal static string GetMenuSVGHref(GetMenuSVGHrefParameter poParameter)
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

        internal static string GetNormalizedString(string text)
        {
            return text.Trim().ToLowerInvariant().Replace(" ", "-");
        }
        #endregion
    }

    internal class GetMenuSVGHrefParameter
    {
        public string Id { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Default { get; set; } = string.Empty;
    }
}
