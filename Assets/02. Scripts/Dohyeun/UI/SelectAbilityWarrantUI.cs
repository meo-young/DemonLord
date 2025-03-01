using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectAbilityWarrantUI : MonoBehaviour
{
    #region 싱글톤 공통
    public static SelectAbilityWarrantUI instance;

    private bool isDestroyOnLoad = true;
    private bool isInitialized = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (!isDestroyOnLoad)
            {
                if (transform.parent)
                    DontDestroyOnLoad(transform.parent);
                else
                    DontDestroyOnLoad(this);
            }
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        if (!isInitialized)
            instance.Initialize();
        isInitialized = true;
    }
    #endregion

    [SerializeField] GameObject MainPanelGo;
    [SerializeField] GameObject DownAbilityGo;
    [SerializeField] GameObject DownWarrantGo;

    [SerializeField] TextMeshProUGUI selectedAbilityText;
    [SerializeField] TextMeshProUGUI selectedWarrantText;
    [SerializeField] Button[] AbilityButtons;
    [SerializeField] Button[] WarrantButtons;
    [SerializeField] TextMeshProUGUI[] AbilityNameTexts;
    [SerializeField] TextMeshProUGUI[] WarrantNameTexts;
    [SerializeField] Button ConfirmAbilityButton;
    [SerializeField] Button ConfirmWarrantButton;

    private AbilityType selectedAbility;
    private WarrantType selectedWarrant;

    private void Initialize()
    {
        MainPanelGo.SetActive(false);
        for (int _i = 0; _i < AbilityButtons.Length; _i++)
        {
            int i = _i;
            AbilityButtons[i].onClick.AddListener(()=>OnClickAbilityCard(i));
            WarrantButtons[i].onClick.AddListener(()=>OnClickWarrantCard(i));
        }
        ConfirmAbilityButton.onClick.AddListener(() => OnClickConfirmAbility());
        ConfirmWarrantButton.onClick.AddListener(() => OnClickConfirmWarrant());
        
    }
    public void Open()
    {
        MainPanelGo.SetActive(true);
        DownAbilityGo.SetActive(true);
        DownWarrantGo.SetActive(false);
    }
    public void Close()
    {
        MainPanelGo.SetActive(false);
    }
    public void OnClickAbilityCard(int idx)
    {
        selectedAbility = (AbilityType)idx;
        for (int i=0;i< AbilityButtons.Count(); i++)
        {
            AbilityButtons[i].GetComponent<Image>().color = idx == i ? Color.white : Color.gray;
        }
        ConfirmAbilityButton.interactable = true;
    }
    public void OnClickWarrantCard(int idx)
    {
        selectedWarrant = (WarrantType)idx;
        for (int i = 0; i < WarrantButtons.Count(); i++)
        {
            WarrantButtons[i].GetComponent<Image>().color = idx == i ? Color.white : Color.gray;
        }
        ConfirmWarrantButton.interactable = true;
    }
    public void OnClickConfirmAbility()
    {
        selectedAbilityText.text = AbilityNameTexts[(int)selectedAbility].text;
        DownAbilityGo.gameObject.SetActive(false);
        DownWarrantGo.gameObject.SetActive(true);
    }
    public void OnClickConfirmWarrant()
    {
        selectedWarrantText.text = WarrantNameTexts[(int)selectedWarrant].text;
    }

}
