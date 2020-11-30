using UnityEngine;

public class Step : MonoBehaviour {
    

    public AudioSource[] footSteps;
    private int randomIndex;

    public void StepSound() {
        randomIndex = Random.Range(0, footSteps.Length);
        footSteps[randomIndex].Play();
    }
}