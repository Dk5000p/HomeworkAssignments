using UnityEngine;
public class Register : GAction {
    public override bool PrePerform() {

        return true;
    }

    public override bool PostPerform() {
        DialogueManager.scc.setGameStateValue("playerWearing", "equals", "shirt");
        Debug.Log(DialogueManager.scc.getSCCLine("Patient"));
        return true;
    }
}
