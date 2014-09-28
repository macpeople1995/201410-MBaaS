using UnityEngine;
using System.Collections;
using KiiCorp.Cloud.Storage;
using KiiCorp.Cloud.Unity;

public class Ranking : MonoBehaviour {

	private static string BUCKET_NAME = "FlappyDogHighScore";
	private static string NAME_KEY = "name";
	private static string SCORE_KEY = "score";
	private string strRanking;
	 
	// Use this for initialization
    void Awake () { 
		KiiBucket bucket = Kii.Bucket(BUCKET_NAME);
		KiiQuery query = new KiiQuery();
		query.SortByDesc (SCORE_KEY);
		query.Limit = 10;
		strRanking = "";
		int number = 1;
		try {
			KiiQueryResult<KiiObject> result = bucket.Query(query);
			Debug.Log(result.ToString());
			foreach (KiiObject obj in result) {
				try{
	 				strRanking = strRanking + "[" + number.ToString() + "] " + obj.GetString(NAME_KEY) + " : " + obj.GetInt(SCORE_KEY) + "\n";
					number++;
				}catch(IllegalKiiBaseObjectFormatException ef){
					Debug.Log("Format Error : "+ef.ToString());
				}
			}
		} catch (CloudException e) {
			Debug.Log ("Failed to fetch high score: " + e.ToString());
		}
	}

    // Update is called once per frame
    void Update () {
    }
	
	void OnGUI () {
		GUIStyle style = GUI.skin.GetStyle("Label");
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
		GUILayout.FlexibleSpace ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.BeginVertical ();
		GUILayout.Label(strRanking,style , GUILayout.ExpandWidth (false));
		if (GUILayout.Button ("Return", GUILayout.MinHeight (50), GUILayout.MinWidth (100))) {
			Application.LoadLevel("Title");
		}
		GUILayout.EndVertical ();
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.EndArea ();
	}
}
