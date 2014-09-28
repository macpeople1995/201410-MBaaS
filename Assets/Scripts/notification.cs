using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Unity;
using KiiCorp.Cloud.Storage;

public class notification : MonoBehaviour {

	public string notif_message;
	private KiiPushPlugin plugin;
	private GameObject notifi; 

	void OnGUI () {
		GUIStyle style = GUI.skin.GetStyle("Label");
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height/4));
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.BeginVertical ();
		GUILayout.Label(notif_message, style , GUILayout.ExpandHeight (true));
		GUILayout.EndVertical ();
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.EndArea ();
	}

	// Use this for initialization
	void Awake() {
		DontDestroyOnLoad(this);
		#if UNITY_IPHONE
		plugin = GameObject.Find("KiiPush").GetComponent<KiiPushPlugin>();
		Debug.Log("Plugin: "+plugin.ToString());
		plugin.OnPushMessageReceived += (ReceivedMessage message) => {
			// This event handler is called when received the push message.
			switch (message.PushMessageType) 
			{
			case ReceivedMessage.MessageType.PUSH_TO_APP:
				// Get the "push_to_app" specific fields.
				PushToAppMessage appMsg = (PushToAppMessage)message;
				Debug.Log("Bucket=" + appMsg.KiiBucket.Uri.ToString());
				Debug.Log("Object=" + appMsg.KiiObject.Uri.ToString());
				break;
			case ReceivedMessage.MessageType.PUSH_TO_USER:
				// "Push to User" message handling...
				Debug.Log("Push to User=" + message.ToString());
				break;
			case ReceivedMessage.MessageType.DIRECT_PUSH:
				// "Direct push" message handling...
				Debug.Log("Direct Push=" + message.ToString());
				break;
			}
			// Dammy Message
 			notif_message = "New HighScore";
		};
		#endif
	}

}
