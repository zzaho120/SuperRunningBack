using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get => instance;
    }

    public List<UIController> userInterfaces;

    public UIs defaultUIId;
    private UIs currentUIId;

    private void Start()
    {
        instance = this;
        Init();
    }

    private void Init()
    {
        if(userInterfaces.Count > 0)
        {
            for (var i = 0; i < userInterfaces.Count; i++)
            {
                userInterfaces[i].gameObject.SetActive(false);
            }
            currentUIId = defaultUIId;
            userInterfaces[(int)defaultUIId].Open();
        }
    }

    public UIController GetUI(UIs id)
    {
        if(userInterfaces.Count > 0 && (int)id < userInterfaces.Count)
        {
            return userInterfaces[(int)id];
        }
        return null;
    }
    
    public UIController Open(UIs id)
    {
        userInterfaces[(int)currentUIId].Close();
        currentUIId = id;
        userInterfaces[(int)currentUIId].Open();
        return userInterfaces[(int)currentUIId];
    }
}
