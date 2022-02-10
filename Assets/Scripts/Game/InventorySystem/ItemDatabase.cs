using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ItemDatabase : Singleton<ItemDatabase>
{
    private List<Item> _items = new List<Item>();
    void Start()
    {

        _items.Add(new Item("Scroll", false));
        SaveDataBase();
        BuildDatabase();
    }

    public void SaveDataBase()
    {
        string json = JsonUtility.ToJson(_items[0]);

        File.WriteAllText(Application.dataPath + "/Resources/JSON/Items.json", json);

        _items[0] = JsonUtility.FromJson<Item>(json);

        //TENGO QUE VER COMO PUEDO HACER PARA GUARDAR SIN USAR LISTAS
    }
    private void BuildDatabase()
    {
        //deserializar items

        Debug.Log(_items[0].itemName);
        Debug.Log(_items[0].itemModifier);


    }

}
