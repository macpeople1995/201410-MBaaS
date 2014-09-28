using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Unity;

public class GameTitle : MonoBehaviour {

	private static bool createNotification;

	// Kii Cloud access with AppID + AppKEY.
	// Those ID&KEY gets in your Kii Cloud dashboard.
	[SerializeField] private string _AppID;
	[SerializeField] private string _AppKey;

    void Awake () {
		if(!createNotification){
			// this is the first instance -make it persist
			DontDestroyOnLoad(GameObject.Find("notification"));
			createNotification = true;
		} else{
			// this must be aduplicate from a scene reload  - DESTROY!
			Destroy(GameObject.Find("notification"));
			Destroy(GameObject.Find("KiiPush"));
		}		// Kii Cloud SDK Initialize.
		Kii.Initialize( _AppID, _AppKey, Kii.Site.JP );
	}
 
	void OnGUI () {
		GUIStyle style = GUI.skin.GetStyle("Label");
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
		GUILayout.FlexibleSpace ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.BeginVertical ();
		GUILayout.Label ("<size=35>Flappy Dog</size>", style , GUILayout.ExpandWidth (false));
		// Don't replace the parameters below please!!
		if (Kii.AppId == null || Kii.AppKey == null || Kii.AppId.Equals ("__KII_APP_ID__") || Kii.AppKey.Equals ("__KII_APP_KEY__")) {
			GUILayout.Space (10);
			GUILayout.Label ("Invalid API keys. See Assets/Readme.txt", GUILayout.ExpandWidth (false));
			GUILayout.Space (20);
			if (GUILayout.Button ("Get API Keys", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
				Application.OpenURL("http://developer.kii.com");
			}
		} else {
			GUILayout.Space (20);
			GUILayout.Label ("Username : " + getCurrentUsername (), GUILayout.ExpandWidth (false));
			if (GUILayout.Button ("Login", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
				// Goto Login Scene with Kii Cloud.
				DontDestroyOnLoad(GameObject.Find("notification"));
				Application.LoadLevel ("KiiCloudLogin");
			}	
		}
		if (GUILayout.Button ("Ranking", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
			DontDestroyOnLoad(GameObject.Find("notification"));
			Application.LoadLevel ("Ranking");
		}	

		GUILayout.EndVertical ();
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.EndArea ();
	}

    private string getCurrentUsername () {
        KiiUser user = KiiUser.CurrentUser;
        if (user != null) {
            return user.Username;
        }
        return "No user";
    }
}
