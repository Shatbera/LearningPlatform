public interface IPopupsController
{
    public enum PopupTag
    {
        None,
        ObjectInfo,
    }
    public void OpenPopup(PopupTag tag);

    public void ClosePopup(PopupTag tag);
}
