using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenOnClick : MonoBehaviour {

	public void LoadGame() {
		SceneManager.LoadScene("nick");
	}
}
