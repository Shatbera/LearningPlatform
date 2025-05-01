using System;

public enum PopupTag
{
    None,
    ObjectInfo,
    Inventory,
}
public interface IPopupsController
{
    public void OpenPopup(PopupTag tag, Action<Popup> onComplete = null);
    public void ClosePopup(PopupTag tag);
}
