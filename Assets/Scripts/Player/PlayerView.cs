using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerView : MonoBehaviour, IUpdate, IPause
{
    //Poderes
    public GameObject stopTimePrefab, earthShield, windJump;

    public float powerFadeSpeed;
    public CanvasGroup powerImageGroup;
    public List<GameObject> powersUI = new List<GameObject>();
    public GameObject crystal, littleStopTimeBubble, blobShadow, canInteractSign, transitionScreen;
    Renderer _crystalRenderer;
    Color _crystalStartColor;
    public List<Color> CrystalColors = new List<Color>();
    public List<GameObject> crystals = new List<GameObject>(), powerIcons = new List<GameObject>();
    public Transform iconPoint;
    public Text powerText;
    public ParticleSystem dust, fireTrail, onFire, onFreeze, doubleJumpParticles, shieldBreak, mudFlood, waterParticles, pickupParticles, iceCastParticle;

    public GameObject pauseScreen, optionsScreen;
    public Button resumeGameButton;

    public CanvasGroup group;

    TMP_Text _questsUI;

    public GameObject book, collectionCanvas;

    public GameObject bookUI, questsUIButton;

    public GameObject questSlotPrefab;

    public List<GameObject> questSlots = new List<GameObject>();

    public Animator questsUIanim;

    public bool canUseQuestsUI;

    bool _showQuestsUI = true;

    GameObject _questUIpanel;


    Animator _anim;
    PlayerModel _playermodel;
    GameObject _currentImage;
    GameObject _currentCrystal, _currentIcon;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _playermodel = GetComponent<PlayerModel>();
        _crystalRenderer = crystal.GetComponent<Renderer>();
        _questsUI = group.GetComponentInChildren<TMP_Text>();
        _questUIpanel = GameObject.Find("QuestUI");

    }

    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        _crystalStartColor = _crystalRenderer.material.color;
        resumeGameButton.onClick.AddListener(UpdateManager.Instance.UnPauseGame);
        group.gameObject.SetActive(true);
        bookUI.gameObject.SetActive(GameManager.Instance.canUseBook);
        questsUIButton.SetActive(false);
        QuestManager.Instance.Set(this);

        questsUIanim.speed = 0;
    }

    public void OnUpdate()
    {
        ProjectBlobShadow();

        if (Input.GetKeyDown(KeyCode.T))
            ShowQuestsUI();
    }

    void ProjectBlobShadow()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 1, 0), -Vector3.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, 1 << 9))
        {
            if (!blobShadow.activeInHierarchy)
                blobShadow.SetActive(true);
            blobShadow.transform.position = hit.point + new Vector3(0, 0.1f, 0);
        }
        else if (blobShadow.activeInHierarchy)
        {
            blobShadow.SetActive(false);
        }
    }

    public void GroundCheck(bool grounded)
    {
        _anim.SetBool("grounded", grounded);

        if ((_anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") || _anim.GetCurrentAnimatorStateInfo(0).IsName("DobleJump Idle")) && grounded)
        {
            _anim.SetTrigger("land");
            dust.Play();
            SoundManager.PlaySound(SoundManager.Sound.PlayerLanding, transform.position);
            //StartCoroutine(ResetAllTriggers());
        }
    }

    public void CallOnStoppedInTime(float time)
    {
        StartCoroutine(OnStoppedInTime(time));
    }

    IEnumerator OnStoppedInTime(float duration)
    {
        littleStopTimeBubble.SetActive(true);
        yield return UpdateManager.WaitForSecondsCustom(duration);
        littleStopTimeBubble.SetActive(false);
    }

    public void CallOnFreeze(float time)
    {
        onFreeze.Play();
        //StartCoroutine(OnFreeze(time));
    }

    IEnumerator OnFreeze(float duration)
    {
        _anim.speed = 0;

        yield return UpdateManager.WaitForSecondsCustom(duration);

        _anim.speed = 1;
    }

    public void CallOnFire(float time)
    {
        StartCoroutine(OnFire(time));
    }

    IEnumerator OnFire(float duration)
    {
        onFire.Play();
        _anim.SetBool("onFire", true);
        yield return UpdateManager.WaitForSecondsCustom(duration);
        onFire.Stop();
        _anim.SetBool("onFire", false);
    }

    public void Attack()
    {
        _anim.SetTrigger("attack");
        //StartCoroutine(ResetAllTriggers());
    }

    public void Cast()
    {
        _anim.SetTrigger("cast");
        //StartCoroutine(ResetAllTriggers());
    }

    public void Jump(bool grounded)
    {
        if (grounded)
            _anim.SetTrigger("jump");
        else
        {
            _anim.SetTrigger("airjump");
            doubleJumpParticles.Play();
        }
        //StartCoroutine(ResetAllTriggers());
    }

    public void MudJump()
    {
        _anim.SetTrigger("mudjump");
        mudFlood.Play();
    }

    public void MudMove()
    {
        if (!mudFlood.isPlaying)
            mudFlood.Play();
    }

    public void RunAnim(float vel)
    {
        _anim.SetFloat("vel", vel);
    }

    public void SpawnStopTimeBubble(float time)
    {
        StartCoroutine(StopTimeBubble(time));
    }

    IEnumerator StopTimeBubble(float duration)
    {
        var b = Instantiate(stopTimePrefab, transform.position, transform.rotation);

        yield return UpdateManager.WaitForSecondsCustom(duration);

        Destroy(b.gameObject);
    }

    public void SpawnLaser(float duration)
    {
        StartCoroutine(IceLaser(duration));
    }

    IEnumerator IceLaser(float duration)
    {
        iceCastParticle.Play();

        Collider[] groundsAround = Physics.OverlapSphere(transform.position, 20.5f, 1 << 9);
        foreach (var ground in groundsAround)
            if (ground.GetComponent<FallingFloor>() != null)
                ground.GetComponent<FallingFloor>().PlayerIceOn(transform.position, 20f);

        yield return UpdateManager.WaitForSecondsCustom(duration);

        foreach (var ground in groundsAround)
            if (ground.GetComponent<FallingFloor>() != null)
                ground.GetComponent<FallingFloor>().PlayerIceOff();
    }

    public void SpawnWindJump()
    {
        //Instanciar el tornadito
        Instantiate(windJump, transform.position, new Quaternion(180, transform.rotation.y, transform.rotation.z, transform.rotation.w));

        //Instantiate(windJump, transform.position, transform.rotation);
    }

    public void SpawnEarthShield(float duration)
    {
        StartCoroutine(EarthShield(duration));
    }

    IEnumerator EarthShield(float time)
    {
        earthShield.SetActive(true);
        yield return UpdateManager.WaitForSecondsCustom(time);
        shieldBreak.Play();
        earthShield.SetActive(false);
    }

    public void PlayFireDash(float duration)
    {
        StartCoroutine(FireDash(duration));
    }

    IEnumerator FireDash(float time)
    {
        fireTrail.Play();
        yield return UpdateManager.WaitForSecondsCustom(time);
        fireTrail.Stop();
    }

    public void NewPower(int index)
    {
        if (_currentImage)
            _currentImage.SetActive(false);
        if (_currentCrystal)
            _currentCrystal.gameObject.SetActive(false);
        if (_currentIcon)
            Destroy(_currentIcon);

        //poweriIcons[index].gameObject.SetActive(true);
        var icon = Instantiate(powerIcons[index], iconPoint.position, iconPoint.rotation);
        icon.transform.SetParent(iconPoint);
        _currentIcon = icon;
        crystals[index].gameObject.SetActive(true);
        _currentCrystal = crystals[index];
        _crystalRenderer.material.color = CrystalColors[index];
        _crystalRenderer.material.SetColor("_EmissionColor", CrystalColors[index] * 5);
        StartCoroutine(ShowPower(index));
    }

    IEnumerator ShowPower(int index)
    {
        float myAlpha = 0.5f;
        _currentImage = powersUI[index];

        _currentImage.SetActive(true);

        while (myAlpha < 1)
        {
            myAlpha += powerFadeSpeed;
            powerImageGroup.alpha = myAlpha;
            yield return null;
        }
    }

    public void UsePower()
    {
        StartCoroutine(HidePower());
        //_currentIcon.gameObject.SetActive(false);
        Destroy(_currentIcon);
        _currentCrystal.gameObject.SetActive(false);
        _crystalRenderer.material.color = _crystalStartColor;
        _crystalRenderer.material.SetColor("_EmissionColor", Color.black);
    }

    IEnumerator HidePower()
    {
        float myAlpha = 1f;

        while (myAlpha > 0.5f)
        {
            myAlpha -= powerFadeSpeed;
            powerImageGroup.alpha = myAlpha;
            yield return null;
        }
        _currentImage.SetActive(false);
    }

    public void InteractEnter()
    {
        canInteractSign.SetActive(true);
    }

    public void InteractExit()
    {
        if (canInteractSign.activeInHierarchy)
            canInteractSign.SetActive(false);
    }

    public void Interact()
    {
        if (canInteractSign.activeInHierarchy)
            canInteractSign.SetActive(false);
    }

    public void OnWaterStart()
    {
        waterParticles.Play();
        _anim.SetBool("water", true);
        SoundManager.PlaySound(SoundManager.Sound.waterSplash, transform.position);
    }

    public void OnWaterEnd()
    {
        _anim.SetBool("water", false);
    }

    public void PortalTrigger(Color tc)
    {
        transitionScreen.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", tc);
        transitionScreen.GetComponent<Animator>().SetTrigger("out");
    }

    public void OnPause()
    {
        _anim.speed = 0;
        if (UpdateManager.GamePaused)
            pauseScreen.SetActive(true);
        else
        {
            book.SetActive(true);
            collectionCanvas.SetActive(true);
        }

        powerImageGroup.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnUnpause()
    {
        _anim.speed = 1;

        if (!UpdateManager.GamePaused && !UpdateManager.BookGamePaused)
        {
            pauseScreen.SetActive(false);
            optionsScreen.SetActive(false);
            book.SetActive(false);
            collectionCanvas.SetActive(false);
        }

        powerImageGroup.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // ShowQuestsUI();
    }

    public void UpdateQuestsUI()
    {
        List<QuestGiver> list = QuestManager.Instance.quests;

        var incompletedQuests = list.Where(x => !x.completed);

        if (questSlots.Count != 0)
        {
            foreach (var q in questSlots)
            {
                GameObject.Destroy(q.gameObject);
            }
            questSlots.Clear();
        }

        questSlots = new List<GameObject>();

        for (int i = 0; i < incompletedQuests.Count(); i++)
        {
            var obj = GameObject.Instantiate(questSlotPrefab);
            obj.transform.position = new Vector3(0, 0, 0);
            Debug.Log(_questUIpanel == null);
            obj.transform.SetParent(GameObject.Find("QuestUI").gameObject.transform, false);
            questSlots.Add(obj);
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - 50 * i, obj.transform.position.z);
        }

        for (int i = 0; i < questSlots.Count; i++)
        {

            var quest = questSlots[i].transform.Find("Quest").GetComponent<TMP_Text>();
            var goal = questSlots[i].transform.Find("Step").GetComponent<TMP_Text>();

            quest.text = "";
            quest.text = "" + incompletedQuests.ToList()[i].questName + ".";
            goal.text = "" + incompletedQuests.ToList()[i].goals.Where(x => !x.completed).First().description + ".";
        }
    }

    public void ShowQuestsUI()
    {
        if (!canUseQuestsUI)
            return;

        if (QuestManager.Instance.quests.Count == 0)
            return;

        _showQuestsUI = !_showQuestsUI;

        questsUIanim.speed = 1;
        questsUIanim.SetBool("toggle", _showQuestsUI);

    }

    public void ToggleQuestsUI()
    {
        questsUIButton.SetActive(true);
        canUseQuestsUI = true;
    }

    public void ShowBookUI()
    {
        GameManager.Instance.canUseBook = true;
        bookUI.SetActive(true);
    }
}
