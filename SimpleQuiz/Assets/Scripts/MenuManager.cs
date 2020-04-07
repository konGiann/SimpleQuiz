using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menu;

    public string selectedCategory;

    private void Awake()
    {
        if(menu == null)
        {
            menu = GetComponent<MenuManager>();
        }
    }

    // check which category was clicked and assign it 
    public void OnButtonClick()
    {
        selectedCategory= EventSystem.current.currentSelectedGameObject.name;
        SceneManager.LoadScene(2);
    }
}
