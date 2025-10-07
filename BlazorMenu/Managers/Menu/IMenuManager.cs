using BlazorMenuCommon.DTOs;
using R_SecurityPolicyCommon.DTOs;

namespace BlazorMenu.Managers.Menu
{
    public interface IMenuManager : IManager
    {
        Task<GetSecurityPolicyDTO> GetSecurityPolicyAsync(GetSecurityPolicyParameterDTO poParameter);
        Task<InfoDTO> GetInfoAsync(GetInfoParameterDTO poParameter);
        Task<List<MenuListDTO>> GetMenuAsync(GetMenuParameterDTO poParameter);
        Task SetFavoriteAsync(FavoriteParameterDTO poParameter);
        Task SetUnfavoriteAsync(FavoriteParameterDTO poParameter);
        Task SetUserProgramSequenceAsync(SetUserProgramSequenceParameterDTO poParameter);
    }
}
