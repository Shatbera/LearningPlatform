using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button prevBtn;
    [SerializeField] private Button nextBtn; 


    private ObjectInfoSO _data;
    private Popup popup;
    private int curPage;

    private void Awake()
    {
        popup = GetComponent<Popup>();
        prevBtn.onClick.AddListener(PrevClick);
        nextBtn.onClick.AddListener(NextClick);
    }
    public void Setup(ObjectInfoSO data)
    {
        _data = data;
        curPage = 0;
        UpdatePage();
    }

    private void PrevClick()
    {
        curPage = Mathf.Max(curPage - 1, 0);
        UpdatePage();
    }

    private void NextClick()
    {
        if(curPage >= _data.Facts.Length - 1)
        {
            popup.Close();
            return;
        }
        curPage = Mathf.Min(curPage + 1, _data.Facts.Length - 1);
        UpdatePage();
    }

    private void UpdatePage()
    {
        text.text = _data.Facts[curPage].text;
        prevBtn.gameObject.SetActive(curPage > 0);
    }
}
