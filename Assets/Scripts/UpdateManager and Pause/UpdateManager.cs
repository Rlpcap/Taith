using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    static UpdateManager _instance;
    public static UpdateManager Instance
    {
        get { return _instance; }
        private set { }
    }

    List<IUpdate> allUpdateElements = new List<IUpdate>();
    List<IFixedUpdate> allFixedUpdateElements = new List<IFixedUpdate>();
    List<ILateUpdate> allLateUpdateElements = new List<ILateUpdate>();

    List<IPause> allPausableElements = new List<IPause>();

    static bool _gamePaused;
    public static bool GamePaused { get { return _gamePaused; } }

    static bool _gameBookPaused;

    public static bool BookGamePaused { get { return _gameBookPaused; } }

    GameObject _bookReference;

    PlayerView _pv;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        //_bookReference =GameObject.Find("Book");
        
        if(_gamePaused || _gameBookPaused)
            UnPauseGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (!_gamePaused && !_gameBookPaused)
            {
                _gamePaused=true;
                PauseGame();
            }
            else
                UnPauseGame();
        }

        if(Input.GetKeyDown(KeyCode.C) && GameManager.Instance.canUseBook)
        {
            if (!_gamePaused && !_gameBookPaused)
            {
                _gameBookPaused=true;
                PauseGame();
            }
            else if(_gameBookPaused)
                UnPauseGame();
        }

        if (!_gamePaused && !_gameBookPaused)
        {
            for (int i = 0; i < allUpdateElements.Count; i++)
            {
                allUpdateElements[i].OnUpdate();
            }
        }
    }

    void FixedUpdate()
    {
        if (!_gamePaused)
        {
            for (int i = 0; i < allFixedUpdateElements.Count; i++)
            {
                allFixedUpdateElements[i].OnFixedUpdate();
            }
        }
    }

    private void LateUpdate()
    {
        if (!_gamePaused && !_gameBookPaused)
        {
            for (int i = 0; i < allLateUpdateElements.Count; i++)
            {
                allLateUpdateElements[i].OnLateUpdate();
            }
        }
    }

    public void AddElementUpdate(IUpdate element)
    {
        if (!allUpdateElements.Contains(element))
            allUpdateElements.Add(element);
    }

    public void RemoveElementUpdate(IUpdate element)
    {
        if (allUpdateElements.Contains(element))
            allUpdateElements.Remove(element);
    }

    public void AddElementFixedUpdate(IFixedUpdate element)
    {
        if (!allFixedUpdateElements.Contains(element))
            allFixedUpdateElements.Add(element);
    }

    public void RemoveElementFixedUpdate(IFixedUpdate element)
    {
        if (allFixedUpdateElements.Contains(element))
            allFixedUpdateElements.Remove(element);
    }

    public void AddElementLateUpdate(ILateUpdate element)
    {
        if (!allLateUpdateElements.Contains(element))
            allLateUpdateElements.Add(element);
    }

    public void RemoveElementLateUpdate(ILateUpdate element)
    {
        if (allLateUpdateElements.Contains(element))
            allLateUpdateElements.Remove(element);
    }

    public void AddElementPausable(IPause element)
    {
        if (!allPausableElements.Contains(element))
            allPausableElements.Add(element);
    }

    public void RemoveElementPausable(IPause element)
    {
        if (allPausableElements.Contains(element))
            allPausableElements.Remove(element);
    }

    public void PauseGame()
    {
       // _gamePaused = true;
        foreach (var item in allPausableElements)
        {
            item.OnPause();
        }
    }

    public void UnPauseGame()
    {
        _gamePaused = false;
        _gameBookPaused= false;
        foreach (var item in allPausableElements)
        {
            item.OnUnpause();
        }
    }

    public static IEnumerator WaitForSecondsCustom(float time)
    {
        float counter = 0;
        while (counter < time)
        {
            if (!_gamePaused && !_gameBookPaused)
            {
                counter += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
