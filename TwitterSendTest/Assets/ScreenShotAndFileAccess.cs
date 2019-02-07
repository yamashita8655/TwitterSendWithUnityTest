using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotAndFileAccess : MonoBehaviour {
	
	[SerializeField]Image TestImage;

	private Texture2D TakeTexture;

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
}
