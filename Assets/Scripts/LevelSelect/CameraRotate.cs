using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotate : MonoBehaviour
{
    public Transform[] levelTargets; 
    public float rotationSpeed = 2f;
    private int currentLevel = 0;
    private bool isRotating = false;
    public Button checkpointButton;
    public Button begginerButton;
    public Button advanceButton;
   
    public Button blueArrowNext;
    public Button redArrowNext;
    public Button greenArrowPrevious;
    public Button blueArrowPrevious;
    
    public TextMeshProUGUI checkpointText;
    public TextMeshProUGUI begginerText;
    public TextMeshProUGUI advancedText;

    public void MoveToNextLevel()
    {
        if (!isRotating && currentLevel < levelTargets.Length - 1)
        {
            RotateToLevel(currentLevel + 1);
            SFXManager.Instance?.PlaySoundAtPosition("Arrow", transform.position, 0.7f);
        }
    }

    public void MoveToPreviousLevel()
    {
        if (!isRotating && currentLevel > 0)
        {
            RotateToLevel(currentLevel - 1);
            SFXManager.Instance?.PlaySoundAtPosition("Arrow", transform.position, 0.7f);
        }
    }

    private void RotateToLevel(int levelIndex)
    {
        StartCoroutine(SmoothRotate(levelTargets[levelIndex]));
        currentLevel = levelIndex;
        
        if (currentLevel == 0)
        {
            checkpointButton.gameObject.SetActive(true);
            begginerButton.gameObject.SetActive(false);
            advanceButton.gameObject.SetActive(false);
            
            greenArrowPrevious.gameObject.SetActive(false);
            blueArrowPrevious.gameObject.SetActive(false);
            
            blueArrowNext.gameObject.SetActive(true);
            redArrowNext.gameObject.SetActive(false);
            
            
            checkpointText.gameObject.SetActive(true);
            begginerText.gameObject.SetActive(false);
            advancedText.gameObject.SetActive(false);
   
        } 
        
        else if (currentLevel == 1)
        {
            checkpointButton.gameObject.SetActive(false);
            begginerButton.gameObject.SetActive(true);
            advanceButton.gameObject.SetActive(false);
            
            greenArrowPrevious.gameObject.SetActive(true);
            blueArrowPrevious.gameObject.SetActive(false);
            
            blueArrowNext.gameObject.SetActive(false);
            redArrowNext.gameObject.SetActive(true);
            
            checkpointText.gameObject.SetActive(false);
            begginerText.gameObject.SetActive(true);
            advancedText.gameObject.SetActive(false);
        } 
        
        else
        {
            checkpointButton.gameObject.SetActive(false);
            begginerButton.gameObject.SetActive(false);
            advanceButton.gameObject.SetActive(true);
            
            greenArrowPrevious.gameObject.SetActive(false);
            blueArrowPrevious.gameObject.SetActive(true);
            
            blueArrowNext.gameObject.SetActive(false);
            redArrowNext.gameObject.SetActive(false);
            
            checkpointText.gameObject.SetActive(false);
            begginerText.gameObject.SetActive(false);
            advancedText.gameObject.SetActive(true);
        }
        
    }

    private IEnumerator SmoothRotate(Transform target)
    {
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        isRotating = false;
    }
}
