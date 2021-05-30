using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Book : MonoBehaviour
{
    public List<GameObject> pages = new List<GameObject>();

    int _currentPage = 0;

    public List<GameObject> enemiesDescription = new List<GameObject>();
    public List<RenderTexture> enemiesTextures = new List<RenderTexture>();
    public List<VideoClip> enemiesVideos = new List<VideoClip>();

    int _currentEnemy = 0;

    public List<GameObject> powersDescription = new List<GameObject>();
    public List<RenderTexture> powersTextures = new List<RenderTexture>();
    public List<VideoClip> powersVideos = new List<VideoClip>();

    int _currentPower = 0;

    public List<GameObject> NPCDescription = new List<GameObject>();
    public List<RenderTexture> NPCTextures = new List<RenderTexture>();
    public List<VideoClip> NPCVideos = new List<VideoClip>();
    int _currentNPC = 0;
    Animator _anim;

    public VideoPlayer videoPlayerLeft;
    public VideoPlayer videoPlayerRight;
    public RawImage rawImageLeft;
    public RawImage rawImageRight;



    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void BtnFlipRight()
    {
        pages[_currentPage].SetActive(false);
        _anim.SetTrigger("right");
        SoundManager.PlaySound(SoundManager.Sound.PageFlip);
    }

    public void BtnFlipLeft()
    {
        pages[_currentPage].SetActive(false);
        _anim.SetTrigger("left");
        SoundManager.PlaySound(SoundManager.Sound.PageFlip);
    }

    public void TurnPage(int pageIndex)
    {
        rawImageLeft.transform.parent.SetParent(pages[pageIndex].transform);
        pages[pageIndex].SetActive(true);

        if(pageIndex == 0)
        {
            BtnActiveEnemy(_currentEnemy);
            BtnActivePower(_currentPower);
        }else
        BtnActiveNPC(_currentNPC);

        _currentPage = pageIndex;
    }

    public void BtnActiveEnemy(int enemyIndex)
    {
        enemiesDescription[_currentEnemy].SetActive(false);
        enemiesDescription[enemyIndex].SetActive(true);
        videoPlayerLeft.clip = enemiesVideos[enemyIndex];
        videoPlayerLeft.targetTexture = enemiesTextures[enemyIndex];
        rawImageLeft.texture = enemiesTextures[enemyIndex];
        _currentEnemy = enemyIndex;
    }

    public void BtnActivePower(int powerIndex)
    {
        powersDescription[_currentPower].SetActive(false);
        powersDescription[powerIndex].SetActive(true);
        videoPlayerRight.clip = powersVideos[powerIndex];
        videoPlayerRight.targetTexture = powersTextures[powerIndex];
        rawImageRight.texture = powersTextures[powerIndex];
        _currentPower = powerIndex;
    }

    public void BtnActiveNPC(int NPCIndex)
    {
        NPCDescription[_currentNPC].SetActive(false);
        NPCDescription[NPCIndex].SetActive(true);
        videoPlayerLeft.clip = NPCVideos[NPCIndex];
        videoPlayerLeft.targetTexture = NPCTextures[NPCIndex];
        rawImageLeft.texture = NPCTextures[NPCIndex];
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
        BtnActiveEnemy(_currentEnemy);
        BtnActivePower(_currentPower);
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
