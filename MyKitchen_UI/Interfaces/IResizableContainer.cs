namespace MyKitchen_UI.Interfaces
{
    public interface IResizableContainer
    {
        public event EventHandler controlResizing;
        public event EventHandler controlLoaded;
        Size containerSize { get; }
    }
}
