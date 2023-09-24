using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Runtime.InteropServices;
public class SaveManager : MonoBehaviour
{

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncDB();
#endif
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

#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif
                Application.Quit(0);
			}
		}
		else
		{
            // Create a new Instance of the FilePicker class

            //Call the OpenFile function with the title and filter
            //FilePicker.Instance.ShowFilePicker();

            secondConfirm = true;
            Invoke(nameof(ConfirmOff), 5);
            wipeSaveText.text = "Wipe Save(Press again to confirm deletion)";
        }
    }
}

