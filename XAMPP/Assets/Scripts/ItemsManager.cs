using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;
using TMPro;
using UnityEngine.UI;

public class ItemsManager : MonoBehaviour
{
    Action<string> m_createItemsCallback;
    // Start is called before the first frame update
    void Start()
    {
        m_createItemsCallback = (jsonArray) => { 
            StartCoroutine(CreateItemsRoutine(jsonArray));
        };

        CreateItems();
    }

    public void CreateItems()
    {
        string userID = Main.instance.userInfo.userID;
        StartCoroutine(Main.instance.web.GetItemsIDs(userID, m_createItemsCallback));
    }

    IEnumerator CreateItemsRoutine(string jsonArrayString)
    {
        //Parsing json array as an array.
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++) {
            bool isDone = false; //done downloading?
            string itemID = jsonArray[i].AsObject["itemID"];
            string id = jsonArray[i].AsObject["ID"];
            JSONObject itemInfoJSON = new JSONObject();

            Action<string> getItemInfoCallback = (itemInfo) => { 
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJSON = tempArray[0] as JSONObject;
            };

            StartCoroutine(Main.instance.web.GetItem(itemID, getItemInfoCallback));

            yield return new WaitUntil(() => isDone);

            GameObject itemGO = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            Item item = itemGO.AddComponent<Item>();
            item.ID = id;
            item.ItemID = id;
            itemGO.transform.SetParent(transform);
            itemGO.transform.localScale = Vector3.one;
            itemGO.transform.localPosition = Vector3.zero;

            //fill info
            itemGO.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = itemInfoJSON["name"];
            itemGO.transform.Find("Price Text").GetComponent<TextMeshProUGUI>().text = itemInfoJSON["price"];
            itemGO.transform.Find("Description Text").GetComponent<TextMeshProUGUI>().text = itemInfoJSON["description"];

            // Set Sell button
            itemGO.transform.Find("Sell Button").GetComponent<Button>().onClick.AddListener(() =>
            {
                string userItemId = id;
                string iID = itemID;
                string userID = Main.instance.userInfo.userID;

                StartCoroutine(Main.instance.web.SellItem(userItemId, iID, userID));
            });
        }

        yield return null;
    }
}
