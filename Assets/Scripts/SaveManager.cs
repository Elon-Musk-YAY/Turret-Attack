using UnityEngine;
using System.IO;
using System.IO.Compression;
using UnityEngine.UI;
using UnityEditor;

public class SaveManager : MonoBehaviour
{
	// Use this for initialization

	bool secondConfirm = false;
	public Text wipeSaveText;
	private void ConfirmOff() {
		secondConfirm = false;
        wipeSaveText.text = "Wipe Save";
    }


	public void WipeSave() {
		if (secondConfirm)
		{
			if (Directory.Exists(SaveSystem.savePath)) {
				FileUtil.DeleteFileOrDirectory(SaveSystem.savePath);
				Application.Quit(0);
			}
		}
		else
		{
			secondConfirm = true;
			Invoke(nameof(ConfirmOff), 5);
			wipeSaveText.text = "Wipe Save(Press again to confirm deletion)";
		}
	}
}

