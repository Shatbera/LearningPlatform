using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupsController : Singleton<IPopupsController>, IPopupsController
{
    private List<Popup> popupsList = new();
    private Dictionary<IPopupsController.PopupTag, Popup> popupsDict = new();
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

    public void OpenPopup(IPopupsController.PopupTag tag, Action<Popup> onComplete = null)
    {
        if(popupsDict.ContainsKey(tag))
        {
            popupsDict[tag].Open();
            onComplete?.Invoke(popupsDict[tag]);
        }
    }

    public void ClosePopup(IPopupsController.PopupTag tag)
    {
        if (popupsDict.ContainsKey(tag))
        {
            popupsDict[tag].Close();
        }
    }
}
