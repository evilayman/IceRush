using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChangeRoomNameLabel : MonoBehaviour
{

    RoomManager roomManager;
	void Start ()
    {
        StartCoroutine(WaitASecond());
              
	}
    
    IEnumerator WaitASecond()
    {
        yield return new WaitForSeconds(1);
        roomManager = (RoomManager)FindObjectOfType(typeof(RoomManager));
        if (roomManager)
        {
            GetComponent<TextMeshProUGUI>().text = roomManager.RoomName.text;
        }

    }
   
	
}
