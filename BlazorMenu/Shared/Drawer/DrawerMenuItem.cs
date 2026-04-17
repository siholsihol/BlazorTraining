using Telerik.SvgIcons;

namespace BlazorMenu.Shared.Drawer
{
    public class DrawerMenuItem
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public ISvgIcon Icon { get; set; } = SvgIcon.Categorize;
        public string ProgramButton { get; set; } = string.Empty;
        public bool Expanded { get; set; }
        public int Level { get; set; }
        public List<DrawerMenuItem> Children { get; set; } = default!;
        public string Title { get; set; } = string.Empty;
        public int Seq { get; set; } = 0;
        public bool Favorite { get; set; } = false;
        public bool IsAnimating { get; set; } = false;
        public string MenuId { get; set; } = string.Empty;
        public string DefaultTitle => $"{Id} - {Text}";
    }
}
