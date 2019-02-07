using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotAndFileAccess : MonoBehaviour {
	
	[SerializeField]Image TestImage;
	[SerializeField]Text DebugText;

	private Texture2D TakeTexture;
	
	private readonly string SaveFileName = "Save1.png";

	public void OnClickTakeScreenShotButton() {
		StartCoroutine(CoTakeScreenShot());
	}

	IEnumerator CoTakeScreenShot() {
		// http://robamemo.hatenablog.com/entry/2017/09/13/111843
		// レンダリングがすべて終わった後に再開という事らしい
		yield return new WaitForEndOfFrame();

		Debug.Log(string.Format("w/h:{0}/{1}", Screen.width, Screen.height));
		TakeTexture = new Texture2D(Screen.width, Screen.height);
		TakeTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		TakeTexture.Apply();
	}
	
	public void OnClickSetImageButton() {
		Sprite sprite = Sprite.Create(TakeTexture, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);
		TestImage.sprite = sprite;
	}
	
	public void OnClickRemoveButton() {
		string filePathAndName = Application.persistentDataPath + "/" + SaveFileName;
		try {
			System.IO.File.Delete(filePathAndName);
		} catch (Exception e) {
			Debug.Log(e.ToString());
			UpdateDebugText(e.ToString());
		}
	}
	
	public void OnClickCheckExistButton() {
		string filePathAndName = Application.persistentDataPath + "/" + SaveFileName;
		if (System.IO.File.Exists(filePathAndName)) {
			Debug.Log("存在する");
			UpdateDebugText("存在する");
		} else {
			Debug.Log("存在しない");
			UpdateDebugText("存在しない");
		}
	}
	
	public void OnClickSaveButton() {
		string filePathAndName = Application.persistentDataPath + "/" + SaveFileName;
		var png = TakeTexture.EncodeToPNG();
		try {
			System.IO.File.WriteAllBytes( filePathAndName, png );
		} catch (Exception e) {
			Debug.Log(e.ToString());
			UpdateDebugText(e.ToString());
		}
	}
	
	public void OnClickLoadAndSetTextureButton() {
		string filePathAndName = Application.persistentDataPath + "/" + SaveFileName;
  
		// https://qiita.com/r-ngtm/items/6cff25643a1a6ba82a6c
		// ここ参考にした
		//ファイルを開く
		System.IO.FileStream fs = new System.IO.FileStream(
		    filePathAndName,
		    System.IO.FileMode.Open,
		    System.IO.FileAccess.Read
		);

		//ファイルを読み込むバイト型配列を作成する
		byte[] bs = new byte[fs.Length];

		//ファイルの内容をすべて読み込む
		fs.Read(bs, 0, bs.Length);

		//閉じる
		fs.Close();

		int pos = 16; // 16バイトから開始
		int width = 0;
		for (int i = 0; i < 4; i++) {
			width = width * 256 + bs[pos++];
		}
		
		int height = 0;
		for (int i = 0; i < 4; i++) {
			height = height * 256 + bs[pos++];
		}
		
		TakeTexture = new Texture2D(width, height);
		TakeTexture.LoadImage(bs);


		Sprite sprite = Sprite.Create(TakeTexture, new Rect(0, 0, width, height), Vector2.zero);
		TestImage.sprite = sprite;
	}
	
	private void UpdateDebugText(string text) {
		DebugText.text += text + "\n";
	}
}
