using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Unity;
using System;

public class KiiCloudLogin : MonoBehaviour {

    private string username = "";
    private string password = "";
	private KiiUser user = null;
	private bool OnCallback = false;
	private KiiPushPlugin plugin;

 	void OnGUI () {
		if (OnCallback)
			GUI.enabled = false;
		else
			GUI.enabled = true;

        GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
        GUILayout.FlexibleSpace ();
        GUILayout.BeginHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.BeginVertical ();

        GUILayout.Label ("Username");
        username = GUILayout.TextField (username, GUILayout.MinWidth (200));
        GUILayout.Space (10);
        GUILayout.Label ("Password");
        password = GUILayout.PasswordField (password, '*', GUILayout.MinWidth (100));
        GUILayout.Space (30);

        if (GUILayout.Button ("Login", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
			if( username.Length == 0 || password.Length == 0 )
				Debug.Log ("Username/password can't be empty");
			else {
				ScoreManager.clearLocalScore();
            	login ();
			}
        }

        if (GUILayout.Button ("Register", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
			if( username.Length == 0 || password.Length == 0 )
				Debug.Log ("Username/password can't be empty");
			else {
				ScoreManager.clearLocalScore();
            	register ();
			}
        }

		if (user != null) {
			OnCallback = false;
			ScoreManager.getHighScore();
			Application.LoadLevel ("Game");
		}

        if (GUILayout.Button ("Cancel", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
			DontDestroyOnLoad(GameObject.Find("notification"));
            Application.LoadLevel ("Title");
        }
        GUILayout.EndVertical ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndHorizontal ();
        GUILayout.FlexibleSpace ();
        GUILayout.EndArea ();
    }


	private void login () {
		user = null;
		OnCallback = true;
		KiiUser.LogIn(username, password, (KiiUser user2, Exception e) => {
			if (e == null) {
				Debug.Log ("Login completed");
				user = user2;
			} else {
				user = null;
				OnCallback = false;
				Debug.Log ("Login failed : " + e.ToString());
			}
		});
	}
	
	private void register () {
		user = null;
		OnCallback = true;
		KiiUser built_user = KiiUser.BuilderWithName(username).Build();
		built_user.Register(password, (KiiUser user2, Exception e) => {
			if (e == null)
			{
				#if UNITY_IPHONE
				bool development = true;      // choose development/production for iOS
				string USER = username;
				string PASS = password;
				KiiPushInstallation.DeviceType deviceType = KiiPushInstallation.DeviceType.IOS;
				plugin = GameObject.Find("KiiPush").GetComponent<KiiPushPlugin>();
				plugin.RegisterPush((string pushToken, Exception e0) => {
					Debug.Log("Token :"+pushToken);
					if( e0 == null ){
						KiiUser.LogIn(USER, PASS, (KiiUser kiiuser, Exception e1) => {
							if( e1 == null ){
								KiiUser.PushInstallation(development).Install(pushToken, deviceType, (Exception e2) => {
									Debug.Log ("Push registration completed");
									KiiUser user3 = KiiUser.CurrentUser;
									KiiBucket bucket = Kii.Bucket("FlappyDogHighScore");
									KiiPushSubscription ps = user3.PushSubscription;
		 							ps.Subscribe(bucket, (KiiSubscribable target, Exception e3) => {
										if (e3 != null)
										{
											// check fail cause
											Debug.Log("Subscription Failed");
											Debug.Log(e3.ToString());
										}
									}); 
								});
							}else{
								Debug.Log("e1 error: "+e1.ToString());
							}; 
						});
					}else{
						Debug.Log("e0 error: "+e0.ToString());
					};
				});
				#endif

				user = user2;
				Debug.Log ("Register completed");
			} else {
				user = null;
				OnCallback = false;
				Debug.Log ("Register failed : " + e.ToString());
			}
		});
	}

}
