﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateItemsOnScene : MonoBehaviour
{
    PickupObject[] _itemsOnScene;

    string _sceneName;

    void Awake()
    {
        _itemsOnScene = FindObjectsOfType<PickupObject>();
        _sceneName = SceneManager.GetActiveScene().name;
    }

    void Start()
    {
        foreach (var playeritem in InventoryController.Instance.itemsOnScenes)
        {
            foreach (var item in _itemsOnScene)
            {
                if (playeritem.id == item.id && _sceneName == item.sceneID && playeritem.gameObjectName == item.gameObjectName)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }
    }
}
