using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeTrigger : MonoBehaviour
{
    public Book book;
    public int ID;

    private void Start()
    {
        if (!GameManager.Instance.FirstTimeEnemy(ID))
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            UpdateManager.BookGamePaused = true;
            UpdateManager.Instance.PauseGame();
            book.BtnActiveEnemy(GameManager.Instance.lastLevelAchieved);
            Destroy(gameObject);
        }
    }
}
