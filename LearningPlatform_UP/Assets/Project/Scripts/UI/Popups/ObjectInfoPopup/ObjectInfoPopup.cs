using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private LocalizedTextSetter nameTxtSetter;
    [SerializeField] private LocalizedTextSetter infoTextSetter;
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button nextBtn;
    [SerializeField] private Button completeBtn;
    [SerializeField] private Image objectImg;


    private ObjectInfoSO _data;
    private Popup popup;
    private int curPage;
    

    private void Awake()
    {
        popup = GetComponent<Popup>();
        prevBtn.onClick.AddListener(PrevClick);
        nextBtn.onClick.AddListener(NextClick);
        completeBtn.onClick.AddListener(CompleteClick);
    }

    private void CompleteClick()
    {
        popup.Close();
        AudioManagerGlobal.Instance.PlayOneShot("rewardLight");
        Debug.Log("on complete read");
    }

    public void Setup(ObjectInfoSO data)
    {
        _data = data;
        objectImg.sprite = data.ObjectSprite;
        nameTxtSetter.SetLocalizedText(data.LocalizedObjectName);
        curPage = 0;
        UpdatePage();
    }

    private void PrevClick()
    {
        curPage = Mathf.Max(curPage - 1, 0);
        //AudioManagerGlobal.Instance.PlayOneShot("switchPaper");
        UpdatePage();
    }

    private void NextClick()
    {
        if(curPage >= _data.Facts.Length - 1)
        {
            return;
        }
        curPage = Mathf.Min(curPage + 1, _data.Facts.Length - 1);
        //AudioManagerGlobal.Instance.PlayOneShot("switchPaper");
        UpdatePage();
    }

    private void UpdatePage()
    {
        //text.text = _data.Facts[curPage].text;
        infoTextSetter.SetLocalizedText(_data.Facts[curPage].LocalizedText);
        prevBtn.gameObject.SetActive(curPage > 0);
        nextBtn.gameObject.SetActive(curPage < _data.Facts.Length - 1);
        completeBtn.gameObject.SetActive(curPage == _data.Facts.Length - 1);
    }
}
