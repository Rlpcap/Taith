using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public List<GameObject> pages = new List<GameObject>();
    int _currentPage = 0;
    public List<GameObject> enemiesDescription = new List<GameObject>();
    int _currentEnemy = 0;
    public List<GameObject> powersDescription = new List<GameObject>();
    int _currentPower = 0;
    public List<GameObject> NPCDescription = new List<GameObject>();
    int _currentNPC = 0;
    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void BtnFlipRight()
    {
        pages[_currentPage].SetActive(false);
        _anim.SetTrigger("right");
    }

    public void BtnFlipLeft()
    {
        pages[_currentPage].SetActive(false);
        _anim.SetTrigger("left");
    }

    public void TurnPage(int pageIndex)
    {
        pages[pageIndex].SetActive(true);
        _currentPage = pageIndex;
    }

    public void BtnActiveEnemy(int enemyIndex)
    {
        enemiesDescription[_currentEnemy].SetActive(false);
        enemiesDescription[enemyIndex].SetActive(true);
        _currentEnemy = enemyIndex;
    }

    public void BtnActivePower(int powerIndex)
    {
        powersDescription[_currentPower].SetActive(false);
        powersDescription[powerIndex].SetActive(true);
        _currentPower = powerIndex;
    }

    public void BtnActiveNPC(int NPCIndex)
    {
        NPCDescription[_currentNPC].SetActive(false);
        NPCDescription[NPCIndex].SetActive(true);
        _currentNPC = NPCIndex;
    }

    private void OnEnable()
    {
        foreach (var page in pages)
        {
            page.SetActive(false);
        }
        for (int i = 0; i < enemiesDescription.Count - 1; i++)
        {
            enemiesDescription[i].SetActive(false);
            powersDescription[i].SetActive(false);
        }
        pages[_currentPage].SetActive(true);
        enemiesDescription[_currentEnemy].SetActive(true);
        powersDescription[_currentPower].SetActive(true);
        //**FALTA RESETEAR LOS NPC**
    }

    private void OnDisable()
    {
        _currentPage = 0;
        _currentEnemy = 0;
        _currentPower = 0;
        //_currentNPC = 0;
    }
}
