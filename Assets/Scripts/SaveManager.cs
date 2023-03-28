using UnityEngine;
using System.IO;
using UnityEngine.UI;
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
                Directory.Delete(SaveSystem.savePath, true);
                Application.Quit(0);
			}
		}
		else
		{
            // Create a new instance of the FilePicker class

            //Call the OpenFile function with the title and filter
            //FilePicker.instance.ShowFilePicker();

            secondConfirm = true;
            Invoke(nameof(ConfirmOff), 5);
            wipeSaveText.text = "Wipe Save(Press again to confirm deletion)";
        }
    }
}

