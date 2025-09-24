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
        public string Title => $"{Id} - {Text}";
    }
}
