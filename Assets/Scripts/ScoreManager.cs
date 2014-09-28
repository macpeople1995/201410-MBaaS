using KiiCorp.Cloud.Storage;
using UnityEngine;
using System;
using KiiCorp.Cloud.Unity;

public class ScoreManager : MonoBehaviour{

    private static string BUCKET_NAME = "FlappyDogHighScore";
	private static string USER_KEY = "user";
	private static string NAME_KEY = "name";
	private static string SCORE_KEY = "score";
    private static int cachedHighScore = 0;
    private static int currentScore = 0;

	// Send Local score to KiiCloud bucket.
	// Need already  KiiCloud logined.
    public static void sendHighScore (int score) {
        if (KiiUser.CurrentUser == null || cachedHighScore > score) {
            return;
        }

		KiiBucket bucket = Kii.Bucket(BUCKET_NAME);
		KiiObject kiiObj = bucket.NewKiiObject ();
        kiiObj[SCORE_KEY] = score;
		kiiObj[USER_KEY] = KiiUser.CurrentUser.ID;
		kiiObj[NAME_KEY] = KiiUser.CurrentUser.Username;

		kiiObj.Save((KiiObject obj, Exception e) => {
			if (e != null)
			{
				Debug.LogError(e.ToString());
			} else {
				Debug.Log("High score sent");
			}
		});
    }

	// Get KiiCloud Bucket's score data.
	// Need already KiiCloud logined. 
    public static int getHighScore () {
        if (KiiUser.CurrentUser == null) {
            return 0;
        }
        if (cachedHighScore > 0) {
            return cachedHighScore;
        }
		KiiBucket bucket = Kii.Bucket(BUCKET_NAME);
        KiiQuery query = new KiiQuery(KiiClause.Equals(USER_KEY,KiiUser.CurrentUser.ID));
        query.SortByDesc (SCORE_KEY);
        query.Limit = 1;

		try {
			KiiQueryResult<KiiObject> result = bucket.Query (query);
			foreach (KiiObject obj in result) {
				int score = obj.GetInt (SCORE_KEY, 0);
				cachedHighScore = score;
				return score;
			}
			Debug.Log ("High score fetched");
			return 0;
		} catch (CloudException e) {
			Debug.Log ("Failed to fetch high score: " + e.ToString());
			return 0;
		}

		/* Async version
		bucket.Query(query, (KiiQueryResult<KiiObject> list, Exception e) =>{
			if (e != null)
			{
				Debug.Log ("Failed to fetch high score: " + e.ToString());
			} else {
				Debug.Log ("High score fetched");
				foreach (KiiObject obj in list) {
					int score = obj.GetInt (SCORE_KEY, 0);
					cachedHighScore = score;
					getHighScore = score;
					return;
				}
			}
		});
		*/
    }

    public static void refreshHighScore () {
        if (cachedHighScore < getCurrentScore ()) {
            cachedHighScore = getCurrentScore ();
        }
    }

    public static int getCachedHighScore () {
        return cachedHighScore;
    }

    public static void addCurrentScore (int add) {
        currentScore += add;
    }

    public static int getCurrentScore () {
        return currentScore;
    }

    public static void initCurrentScore () {
        currentScore = 0;
    }

	public static void clearLocalScore(){
		cachedHighScore = 0;
		currentScore = 0;
	}
}
