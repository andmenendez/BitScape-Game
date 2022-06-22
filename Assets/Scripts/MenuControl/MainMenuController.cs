using System;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

    Animator anim;

    [Header("Options Panel")]
    
    public GameObject TeamMemberPanel;

    public GameObject CreditsPanel;
    public GameObject AssetsUsedPanel;
    public GameObject MainScreenPanel;

    void Start () {
        anim = GetComponent<Animator>();
    }

    public void openCredits()
    {
        CreditsPanel.SetActive(true);
        MainScreenPanel.SetActive(false);
    }

    public void openCredits_TeamMembers()
    {
        CreditsPanel.SetActive(false);
        TeamMemberPanel.SetActive(true);
        AssetsUsedPanel.SetActive(false);
    }

    public void openCredits_AssetsUsed()
    {
        CreditsPanel.SetActive(false);
        TeamMemberPanel.SetActive(false);
        AssetsUsedPanel.SetActive(true);
    }

    public void backToMainMenu()
    {
        MainScreenPanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }

    public void backToCredits()
    {
        CreditsPanel.SetActive(true);
        TeamMemberPanel.SetActive(false);
        AssetsUsedPanel.SetActive(false);
    }
}
