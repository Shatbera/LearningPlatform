using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupsController : Singleton<PopupsController>, IPopupsController
{
    [SerializeField] private PopupTintController tintController;

    private List<Popup> popupsList = new();
    private Dictionary<IPopupsController.PopupTag, Popup> popupsDict = new();

    private Stack<Popup> openPopups = new();

    public Popup TopPopup => openPopups.Count == 0 ? null : openPopups.Peek();
    protected override void Awake()
    {
        base.Awake();
        Setup();
    }

    private void Setup()
    {
        foreach (Transform child in transform)
        {
            var popup = child.GetComponent<Popup>();
            if (popup != null)
            {
                popupsList.Add(popup);
                if (popup.PopupTag != IPopupsController.PopupTag.None)
                {
                    popupsDict.Add(popup.PopupTag, popup);
                }
            }
        }
    }

    public void OpenPopup(Popup popup, Action<Popup> onComplete = null)
    {
        popup.SetVisible(this, true);
        onComplete?.Invoke(popup);
        openPopups.Push(popup);
        if(openPopups.Count == 1)
        {
            tintController.ShowTint();
        }
    }
    public void OpenPopup(IPopupsController.PopupTag tag, Action<Popup> onComplete = null)
    {
        if(popupsDict.ContainsKey(tag))
        {   
            OpenPopup(popupsDict[tag], onComplete);
        }
    }


    public void ClosePopup(Popup popup)
    {
        if (openPopups.Peek() != popup)
        {
            return;
        }
        popup.SetVisible(this, false);
        openPopups.Pop();
        if(openPopups.Count == 0)
        {
            tintController.HideTint();
        }
    }

    public void ClosePopup(IPopupsController.PopupTag tag)
    {
        if (popupsDict.ContainsKey(tag))
        {
            ClosePopup(popupsDict[tag]);
        }
    }

    public void CloseTopPopup()
    {
        ClosePopup(openPopups.Peek());
    }

}
