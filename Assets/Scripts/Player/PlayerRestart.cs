using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRestart : MonoBehaviour, IUpdate
{
    public GameObject mesh, staff;
    public float dissolveSpeed;
    bool _restarting = false;
    Material _myMat, _staffMat;
    Animator _anim;

    private void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);

        _myMat = mesh.GetComponent<SkinnedMeshRenderer>().material;
        _staffMat = staff.GetComponent<MeshRenderer>().materials[1];
        _anim = GetComponent<Animator>();

        _myMat.SetFloat("_DissolveAmount1", -0.2f);
        _staffMat.SetFloat("_DissolveAmount1", -0.2f);
    }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R))
            CallRestartLevel();
    }

    public void CallRestartLevel()
    {
        if(!_restarting)
            StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        _restarting = true;
        _anim.SetTrigger("cast");
        float currDissolve = -0.2f;
        while (currDissolve < 1.1f)
        {
            currDissolve += dissolveSpeed;
            _myMat.SetFloat("_DissolveAmount1", currDissolve);
            _staffMat.SetFloat("_DissolveAmount1", currDissolve);
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
