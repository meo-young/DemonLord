using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class EndingCreditUI : MonoBehaviour
{
    #region 싱글톤 공통
    public static EndingCreditUI instance;

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

    [SerializeField] GameObject MainGo;
    [SerializeField] GameObject CreditImageGo;
    public float Speed = 100f;

    private void Initialize()
    {

    }
    public void Open()
    {
        MainGo.SetActive(true);
        StartCoroutine(CoSlideImage());
    }
    public void Close()
    {
        MainGo.SetActive(false);
    }
    IEnumerator CoSlideImage()
    {
        var rectTrs = CreditImageGo.GetComponent<RectTransform>();
        while (rectTrs.localPosition.y < -600f)
        {
            rectTrs.localPosition += new Vector3(0, Speed * Time.deltaTime * (Input.GetMouseButton(0)?3f:1f));
            yield return null;
        }
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Close();
            }
            yield return null;
        }
    }
}
