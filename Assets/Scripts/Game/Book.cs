using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.Linq;

public class Book : MonoBehaviour
{
    public List<GameObject> pages = new List<GameObject>();

    int _currentPage = 0;

    [Header("Enemies")]
    public List<Button> enemiesButtons = new List<Button>();
    public List<GameObject> enemiesDescription = new List<GameObject>();
    public List<RenderTexture> enemiesTextures = new List<RenderTexture>();
    public List<VideoClip> enemiesVideos = new List<VideoClip>();

    int _currentEnemy = 0;

    [Header("Powers")]
    public List<GameObject> powersDescription = new List<GameObject>();
    public List<RenderTexture> powersTextures = new List<RenderTexture>();
    public List<VideoClip> powersVideos = new List<VideoClip>();

    int _currentPower = 0;

    [Header("NPC")]
    public List<Button> npcsButtons = new List<Button>();
    public List<GameObject> NPCDescription = new List<GameObject>();
    public List<RenderTexture> NPCTextures = new List<RenderTexture>();
    public List<VideoClip> NPCVideos = new List<VideoClip>();
    int _currentNPC = 0;
    Animator _anim;

    [Header("Video")]
    public VideoPlayer videoPlayerLeft;
    public VideoPlayer videoPlayerRight;
    public RawImage rawImageLeft;
    public RawImage rawImageRight;

    public TMP_Text questsText;

    DialogueWindow _dialogueWindow;

    void Awake()
    {
        _dialogueWindow = FindObjectOfType<DialogueWindow>();
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        SetActiveButtons();
    }

    public void SetActiveButtons()
    {
        if(GameManager.Instance.lastLevelAchieved < enemiesButtons.Count)
        {
            for (int i = 0; i <= GameManager.Instance.lastLevelAchieved; i++)
            {
                enemiesButtons[i].interactable = true;
            }
        }
        else
        {
            for (int i = 0; i < enemiesButtons.Count; i++)
            {
                enemiesButtons[i].interactable = true;
            }
        }

        foreach (var index in GameManager.Instance.metNPCs)
        {
            npcsButtons[index].interactable = true;
        }
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

        if (pageIndex == 0)
        {
            BtnActiveEnemy(_currentEnemy);
            //BtnActivePower(_currentPower);
        }
        else
        {
            BtnActiveNPC(_currentNPC);
            //ShowQuests();
        }

        _currentPage = pageIndex;
    }

    public void BtnActiveEnemy(int enemyIndex)
    {
        enemiesDescription[_currentEnemy].SetActive(false);
        enemiesDescription[enemyIndex].SetActive(true);

        videoPlayerLeft.clip = enemiesVideos[enemyIndex];
        videoPlayerLeft.targetTexture = enemiesTextures[enemyIndex];
        rawImageLeft.texture = enemiesTextures[enemyIndex];

        powersDescription[_currentPower].SetActive(false);
        powersDescription[enemyIndex].SetActive(true);

        videoPlayerRight.clip = powersVideos[enemyIndex];
        videoPlayerRight.targetTexture = powersTextures[enemyIndex];
        rawImageRight.texture = powersTextures[enemyIndex];

        _currentEnemy = enemyIndex;
        _currentPower = enemyIndex;
    }

    public void BtnActivePower(int powerIndex)//**ESTO ESTÁ SIN USARSE PORQUE AHORA EL BOTÓN DEL ENEMIGO INCLUYE ESTO TAMBIÉN**
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

    void ShowQuests()
    {
        var list = QuestManager.Instance._listOfQuests;
        var check = "COMPLETED";

        if (list.Count == 0)
        {
            questsText.text = "You don't have any new quest.";
            return;
        }


        var quests = list.Where(x => x.QuestStatus == QuestState.State.Completed || x.QuestStatus == QuestState.State.Unlocked);

        questsText.text = "";

        if (quests.Count() != 0)
        {
            foreach (var q in quests)
            {
                questsText.text += "" + q.QuestName + ":" + "\n";
                foreach (var t in q.tasks)
                {
                    if (q.tasksList[t] == true)
                        questsText.text += "_ " + t + "." + check + "\n";
                    else
                        questsText.text += "_ " + t + "." + "\n";
                }
            }
        }
        else
        {
            questsText.text = "You don't have any new quest.";
        }
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
        powersDescription[_currentEnemy].SetActive(true);
        rawImageLeft.transform.parent.SetParent(pages[_currentPage].transform);
        BtnActiveEnemy(_currentEnemy);
        //BtnActivePower(_currentPower);
        //**FALTA RESETEAR LOS NPC**
        _dialogueWindow.GetComponent<CanvasGroup>().alpha = 0;

    }

    private void OnDisable()
    {
        _currentPage = 0;
        _currentEnemy = 0;
        _currentPower = 0;

        for (int i = 0; i < enemiesDescription.Count; i++)
        {
            enemiesDescription[i].SetActive(false);
            powersDescription[i].SetActive(false);
        }

        if (_dialogueWindow.isChatting)
            _dialogueWindow.GetComponent<CanvasGroup>().alpha = 1;
        //_currentNPC = 0;
    }
}
