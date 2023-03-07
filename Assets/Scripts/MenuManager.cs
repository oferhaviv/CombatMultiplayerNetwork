using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    private bool _allMenusClosed = false;
    private Menu _openMenu;
    [SerializeField] GameObject background;
    [SerializeField] Menu[] menus;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public Menu getOpenedMenu()
    {
        return _openMenu;
    }


    public void OpenMenu(string menuName)
    {
        CloseAllMenus();

        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                menus[i].Open();
                _openMenu = menus[i];
            }

        }
        _allMenusClosed = false;

    }
    public void CloseAllMenus(bool force = false)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].open)
            {
                menus[i].Close();
            }
        }
        if (force)
        {
            Debug.Log("Closing all canvas");
            gameObject.SetActive(false);
            background.SetActive(false);
        }
        _allMenusClosed = true;
    }

    public void setActiveAllMenu(bool active)
    {
        foreach (Menu m  in menus)
        {
            m.gameObject.SetActive(active);
        }
        _allMenusClosed = active;

    }
    public void backFromGame()
    {
        gameObject.SetActive(true);
        background.SetActive(true);
    }

    public void OpenMenu(Menu menu)
    {
        CloseAllMenus();
        menu.Open();
        _openMenu = menu;
    }
    public bool AllMenusClosed()
    {
        return _allMenusClosed;
        
    }
    public void quitApp()
    {
        Launcher.Instance.quit();
        Application.Quit();
        
    }
}

