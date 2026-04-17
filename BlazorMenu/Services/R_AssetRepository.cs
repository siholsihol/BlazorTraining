using BlazorMenu.Constants;
using R_BlazorFrontEnd.Deployment.Interfaces;
using R_BlazorFrontEnd.Interfaces;
using System.Text;

namespace BlazorMenu.Services
{
    public class R_AssetRepository : R_IAssetRepository
    {
        private readonly Dictionary<string, byte[]?> _dynamicAssetByteCache = new();
        private readonly Dictionary<string, string?> _dynamicAssetStringCache = new();
        private readonly Dictionary<string, string> _preloadAssetCache = new Dictionary<string, string>();
        private readonly R_IDynamicAssetProvider _assetProvider;

        private bool _hasPreloaded = false;

        public R_AssetRepository(
            R_IDynamicAssetProvider assetProvider
        )
        {
            _assetProvider = assetProvider;
        }

        public async Task<byte[]?> GetAssetBytesAsync(string name)
        {
            if (_dynamicAssetByteCache.TryGetValue(name, out var bytes))
                return bytes;

            var fetched = await _assetProvider.GetAssetAsync(name);
            _dynamicAssetByteCache[name] = fetched;
            return fetched;
        }

        public async Task<string?> GetAssetStringAsync(string name)
        {
            if (_dynamicAssetStringCache.TryGetValue(name, out var text))
                return text;

            var bytes = await GetAssetBytesAsync(name);
            text = bytes == null ? null : Encoding.UTF8.GetString(bytes);
            _dynamicAssetStringCache[name] = text;

            return text;
        }

        public string GetPreloadAsset(string pcKey)
        {
            if (_preloadAssetCache.TryGetValue(pcKey, out var loResult))
            {
                return loResult;
            }

            return string.Empty;
        }
        public async Task PreloadAssetsAsync()
        {
            if (_hasPreloaded) return;

            // Preload logo
            var logoUrl = "assets/img/logo-bimasakti.png";
            _preloadAssetCache.Add(AppConstants.LogoUrlKey, logoUrl);
            var logoStyle = $"width: 125px; height: 35px; background-image: url({logoUrl}); background-size: cover; background-position: left center; background-repeat: no-repeat;";
            _preloadAssetCache.Add(AppConstants.LogoStyleKey, logoStyle);

            // Preload home background
            var homeBgStyle = $"background-image:url(assets/img/home-bg-bimasakti-11.svg); background-size:cover; background-position:right center;";
            _preloadAssetCache.Add(AppConstants.HomeBgStyleKey, homeBgStyle);

            // Preload login background
            var loginBgStyle = $"background-image:url(assets/img/bg-illustration.png); background-size:cover; background-position:right center;";
            _preloadAssetCache.Add(AppConstants.LoginBgStyleKey, loginBgStyle);

            _hasPreloaded = true;
        }
    }
}
