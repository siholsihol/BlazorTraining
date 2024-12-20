namespace BlazorTraining.Controls.Preload
{
    public class R_PreloadMenuService
    {
        internal event Action OnShow;
        internal event Action OnHide;

        public void Show() => OnShow?.Invoke();
        public void Hide() => OnHide?.Invoke();
    }
}
