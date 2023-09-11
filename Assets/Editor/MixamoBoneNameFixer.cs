using UnityEditor;
using UnityEngine;

public class MixamoBoneNameFixer : MonoBehaviour
{
    [MenuItem("Custom Tools/Fix Mixamo Bone Names")]
    private static void FixMixamoBoneNames()
    {
        // Select your Mixamo model in the Unity editor
        GameObject mixamoModel = Selection.activeGameObject;

        if (mixamoModel == null)
        {
            Debug.Log("Please select the Mixamo model first.");
            return;
        }

        // Get the Animator component from the Mixamo model
        Animator mixamoAnimator = mixamoModel.GetComponent<Animator>();

        if (mixamoAnimator == null)
        {
            Debug.Log("The Mixamo model must have an Animator component.");
            return;
        }

        // Get the HumanDescription of the Mixamo model's humanoid avatar
        HumanDescription mixamoHumanDescription = mixamoAnimator.avatar.humanDescription;

        // Now select your own model in the Unity editor
        GameObject myModel = Selection.activeGameObject;

        if (myModel == null)
        {
            Debug.Log("Please select your model first.");
            return;
        }

        // Get the Animator component from your own model
        Animator myModelAnimator = myModel.GetComponent<Animator>();

        if (myModelAnimator == null)
        {
            Debug.Log("Your model must have an Animator component.");
            return;
        }

        // Get the HumanDescription of your model's humanoid avatar
        HumanDescription myModelHumanDescription = myModelAnimator.avatar.humanDescription;

        // Check if the bone structure of both models match
        if (mixamoHumanDescription.skeleton.Length != myModelHumanDescription.skeleton.Length)
        {
            Debug.Log("The bone structure of the Mixamo model and your model do not match.");
            return;
        }

        // Rename your model's bones to match the Mixamo model
        for (int i = 0; i < mixamoHumanDescription.skeleton.Length; i++)
        {
            string mixamoBoneName = mixamoHumanDescription.skeleton[i].name;

            // Apply the Mixamo bone name to your model's skeleton
            myModelHumanDescription.skeleton[i].name = mixamoBoneName;
        }

        // Update the Avatar with the modified HumanDescription
        Avatar updatedAvatar = AvatarBuilder.BuildHumanAvatar(myModel, myModelHumanDescription);

        // Save the changes to the original prefab
        PrefabUtility.SaveAsPrefabAsset(myModel, PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(myModel), out bool success);

        if (success)
        {
            Debug.Log("Bone names have been updated and changes applied to the prefab.");
        }
        else
        {
            Debug.Log("Failed to save changes to the prefab.");
        }
    }
}
