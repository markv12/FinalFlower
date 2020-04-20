using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HighScoreManager : MonoBehaviour
{

	public GameObject playerScoreTextObject;
	public GameObject highScoreTextObject;
	public GameObject highScorePanel;
	public GameObject playerNameTextObject;
	public GameObject inputZone;
	public int scoreCount = 5;
	string leaderboardName;

	string[] names = { };
	string[] scores = { };
	float playerScore = 100;

	public void setup(float time)
	{
		leaderboardName = "ludumDare46" + SceneManager.GetActiveScene().name;
		highScoreTextObject.GetComponent<TextMeshProUGUI>().text = "Loading...";
		playerNameTextObject.GetComponent<TextMeshProUGUI>().text = "";
		setPlayerScore(time);
		inputZone.SetActive(false);
		StartCoroutine(getHighScores(true));
	}

	public void saveScore()
	{
		string name = playerNameTextObject.GetComponent<TextMeshProUGUI>().text;
		if (name.Length < 2) return;
		if (name.Length > 12) name = name.Substring(0, 12);
		name = name.Replace(',', ' ');
		name = name.Replace('?', '.');
		name = name.Replace('/', '-');
		name = name.Replace('\n', ' ');
		Debug.Log("Adding high score " + playerScore + " for player " + name);
		StartCoroutine(addHighScore(name, playerScore));
		inputZone.SetActive(false);
	}

	public void setPlayerScore(float score)
	{
		playerScore = score;
		playerScoreTextObject.GetComponent<TextMeshProUGUI>().text = "YOUR TIME: " + score.ToString("0.000") + 's';
	}

	void updateHighScoreLabel(bool shouldIncludePlayer)
	{
		highScorePanel.SetActive(true);
		bool hasShownPlayer = !shouldIncludePlayer;
		string scoreLabel = "";
		string playerName = playerNameTextObject.GetComponent<TextMeshProUGUI>().text;
		if (playerName.Length < 2)
		{
			playerName = "You";
		}
		int totalCount = names.Length;
		string delineator = "     ";
		for (int i = 0; i < totalCount; i++)
		{
			if (!hasShownPlayer && playerScore <= float.Parse(scores[i]))
			{
				scoreLabel += "<#FFE100>" + playerName + delineator + playerScore.ToString("0.000") + "s</color>\n";
				hasShownPlayer = true;
				if (totalCount == scoreCount)
				{
					totalCount--;
				}
			}
			if (i < scoreCount)
			{
				string displayName = names[i];
				if (displayName.Length > 12)
				{
					displayName = displayName.Substring(0, 12);
				}
				scoreLabel += displayName + delineator + scores[i] + "s\n";
			}
		}
		if (scoreLabel == "" || (!hasShownPlayer && totalCount < scoreCount))
		{
			scoreLabel += "<#FFE100>" + playerName + delineator + playerScore.ToString("0.000") + "s</color>\n";
		}
		highScoreTextObject.GetComponent<TextMeshProUGUI>().text = scoreLabel;
	}

	IEnumerator getHighScores(bool shouldIncludePlayer)
	{
		Debug.Log("Loading high scores");
		string url = "https://agile-citadel-44322.herokuapp.com/" + leaderboardName + "/bottom/" + scoreCount.ToString() + '/';
		using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError)
			{
				Debug.Log("Network Error: " + webRequest.error);
			}
			else if (webRequest.downloadHandler.text.Substring(0, 1) == "<")
			{
				Debug.Log("Network Error: 404");
				highScorePanel.SetActive(false);
				yield break;
			}
			else if (webRequest.downloadHandler.text == "Forbidden" || webRequest.downloadHandler.text == "Internal Server Error")
			{
				Debug.Log("Network Error: " + webRequest.downloadHandler.text);
				updateHighScoreLabel(true);
				inputZone.SetActive(true);
			}
			else
			{
				Debug.Log("Received high scores: " + webRequest.downloadHandler.text);
				string[] highScoreStrings;
				string[] stringSplitter = new string[] { "\\n" };
				string highScoreString = webRequest.downloadHandler.text.Substring(1, webRequest.downloadHandler.text.Length - 2);
				if (webRequest.downloadHandler.text.IndexOf("\\n") == -1)
				{
					Debug.Log("Only one score: " + highScoreString);
					highScoreStrings = new string[] { highScoreString };
				}
				else
				{
					highScoreStrings = highScoreString.Split(stringSplitter, System.StringSplitOptions.None);
				}

				int scoreCountToDisplay = scoreCount;
				if (highScoreStrings.Length < scoreCountToDisplay)
				{
					scoreCountToDisplay = highScoreStrings.Length;
					if (shouldIncludePlayer)
					{
						Debug.Log("High Score By Default!");
						inputZone.SetActive(true);
					}
				}

				names = new string[scoreCountToDisplay];
				scores = new string[scoreCountToDisplay];

				for (int s = 0; s < highScoreStrings.Length; s++)
				{
					string[] split = highScoreStrings[s].Split(',');
					names[s] = split[0];
					scores[s] = split[1];
					if (shouldIncludePlayer && playerScore <= float.Parse(split[1]))
					{
						inputZone.SetActive(true);
					}
				}
			}
			updateHighScoreLabel(shouldIncludePlayer);
		}
	}

	IEnumerator addHighScore(string name, float score)
	{
		string url = "https://agile-citadel-44322.herokuapp.com/" + leaderboardName + "/add/" + name + '/' + score.ToString("0.000") + '/';
		using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
		{
			yield return webRequest.SendWebRequest();

			if (webRequest.isNetworkError)
			{
				Debug.Log("Network Error: " + webRequest.error);
			}
			else if (webRequest.downloadHandler.text == "Forbidden" || webRequest.downloadHandler.text == "Internal Server Error")
			{
				Debug.Log("Network Error: " + webRequest.downloadHandler.text);
			}
			else
			{
				Debug.Log("Successfully added new score");
				StartCoroutine(getHighScores(false));
			}
		}
	}
}