using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	public static SimpleConditionalConversation scc;

	// NOTE: When you do not use the google sheet option, it is expecting the file
	// to be named "data.csv" and for it to be in the Resources folder in Assets.
	public bool useGoogleSheet = false;
	public string googleSheetDocID = "";

	// Start is called before the first frame update
	void Start()
	{
		if (useGoogleSheet) {
			// This will start the asyncronous calls to Google Sheets, and eventually
			// it will give a value to scc, and also call LoadInitialHistory().
			GoogleSheetSimpleConditionalConversation gs_ssc = gameObject.AddComponent<GoogleSheetSimpleConditionalConversation>();
			gs_ssc.googleSheetDocID = googleSheetDocID;
		} else {
			scc = new SimpleConditionalConversation("data");
			LoadInitialSCCState();
		}
	}
	
	public static void LoadInitialSCCState()
	{
		// Example of setting the initial state:
		//scc.setGameStateValue("playerWearing", "equals", "Green shirt");
	}
	
	// Update is called once per frame
	void Update()
	{
		// An example of getting a line of dialogue.
	    if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log(DialogueManager.scc.getSCCLine("Patient"));
		}
		
		// An example of modifying the state outside of the DialogueManager (e.g. you could put this in some
		// OnTriggerEnter or something)
	    if (Input.GetKeyDown(KeyCode.G)) {
			scc.setGameStateValue("playerWearing", "equals", "Green shirt");
		}
	}
}
