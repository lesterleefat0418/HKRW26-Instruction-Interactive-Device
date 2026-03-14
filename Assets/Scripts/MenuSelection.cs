using System;
using UnityEngine;

public class MenuSelection : MonoBehaviour
{
    public CurrentPage currentPage;
    public Page pages;
    public LanguageUI[] languageUI;
    public CountDownTimer idling;
    public CanvasGroup homeBtn;
    // Start is called before the first frame update
    void Start()
    {
        this.pages.init(null, 0);
        this.ChangePage(0);
        this.idling?.init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setLang(int langId)
    {
        LoaderConfig.Instance.SelectedLanguageId = langId;
        Debug.Log("selected language: " + langId);
        string lang = "";
        switch (langId)
        {
            case 0:
                lang = "TC";
                break;
            case 1:
                lang = "CN";
                break;
            case 2:
                lang = "Eng";
                break;

        }
        Debug.Log("current lang: " + lang);
        for (int i = 0; i < this.languageUI.Length; i++)
        {
            if (this.languageUI[i] != null) this.languageUI[i].setLang();
        }
    }

    public void ChangePage(int toPage)
    {
        this.pages.setPage(toPage);

        SetUI.Set(this.homeBtn, toPage > 0);

        if (Enum.IsDefined(typeof(CurrentPage), toPage))
        {
            this.currentPage = (CurrentPage)toPage;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(toPage), "Invalid page index");
        }
    }

}

public enum CurrentPage
{
    FrontPage = 0,
    GenderSelection = 1,
    UnlockAnimation = 2,
    SelectMagicItem = 3,
    SelectGainHappiness = 4,
    Result = 5
}
