using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwitterSendTest : MonoBehaviour {

	[SerializeField]Text DebugOutputText;

	private string OutputText = "";

	public void OnClickTwitterSendButton() {
		SWorker.SocialWorker.PostTwitter("Test", "", (SWorker.SocialWorkerResult swres) => {
				string res = "" + swres;
				AddDebugText(res);
			}
		);

	}
	
	private void AddDebugText(string text) {
		OutputText += text + "\n";
		DebugOutputText.text = OutputText;
	}
}
