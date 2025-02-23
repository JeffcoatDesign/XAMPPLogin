using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;
using TMPro;
public class Items : MonoBehaviour
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
            JSONObject itemInfoJSON = new JSONObject();

            Action<string> getItemInfoCallback = (itemInfo) => { 
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJSON = tempArray[0] as JSONObject;
            };

            StartCoroutine(Main.instance.web.GetItem(itemID, getItemInfoCallback));

            yield return new WaitUntil(() => isDone);

            GameObject item = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            item.transform.SetParent(transform);
            item.transform.localScale = Vector3.one;
            item.transform.localPosition = Vector3.zero;

            //fill info
            item.transform.Find("Name Text").GetComponent<TextMeshProUGUI>().text = itemInfoJSON["name"];
            item.transform.Find("Price Text").GetComponent<TextMeshProUGUI>().text = itemInfoJSON["price"];
            item.transform.Find("Description Text").GetComponent<TextMeshProUGUI>().text = itemInfoJSON["description"];
        }

        yield return null;
    }
}
