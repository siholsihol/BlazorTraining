using Microsoft.AspNetCore.Components;

namespace BlazorTraining.Controls
{
    public partial class MetaTags : ComponentBase
    {
        [Parameter] public string Description { get; set; } = null!;
    }
}
