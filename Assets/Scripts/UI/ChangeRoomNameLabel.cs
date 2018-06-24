using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChangeRoomNameLabel : MonoBehaviour
{

    RoomManager roomManager;
	void Start ()
    {
        roomManager=(RoomManager) FindObjectOfType(typeof(RoomManager));
        if (roomManager)
        {
            GetComponent<TextMeshProUGUI>().text = roomManager.RoomName.text;
        }
        
	}
  
	
}
