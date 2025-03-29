using System;

public interface IPopupsController
{
    public enum PopupTag
    {
        None,
        ObjectInfo,
    }
    public void OpenPopup(PopupTag tag, Action<Popup> onComplete = null);

    public void ClosePopup(PopupTag tag);
}
