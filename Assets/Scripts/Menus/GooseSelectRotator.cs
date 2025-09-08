using System;
using System.Collections;
using UnityEngine;

public class GooseSelectRotator : MonoBehaviour
{
    public Transform[] positions; // The three fixed spots
    public GameObject[] items;    // The characters or objects to rotate
    public Material[] gooseMaterials;
    public Transform cameraTransform;
    public float rotationSpeed = 50f;
    public float transitionSpeed = 2f;

    private int currentIndex = 0;
    private bool isTransitioning = false;

    private void Start()
    {
        UpdateAllGooseAnimations();
    }

    void Update()
    {
        if (items.Length > 0 && items[currentIndex] != null)
        {
            // Continuously rotate the currently selected item
            items[currentIndex].transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    public void NextItem()
    {
        if (isTransitioning) return;
        currentIndex = (currentIndex + 1) % items.Length;
        StartCoroutine(ShiftPositions());
        
        SFXManager.Instance?.PlaySoundOnObject("Quack", items[currentIndex], 0.95f);
    }

    public void PreviousItem()
    {
        if (isTransitioning) return;
        currentIndex = (currentIndex - 1 + items.Length) % items.Length;
        StartCoroutine(ShiftPositions());
        
        SFXManager.Instance?.PlaySoundOnObject("Quack", items[currentIndex], 0.95f);
    }

    private IEnumerator ShiftPositions()
    {
        isTransitioning = true;
        Vector3[] startPositions = new Vector3[items.Length];
        Quaternion[] startRotations = new Quaternion[items.Length];

        for (int i = 0; i < items.Length; i++)
        {
            startPositions[i] = items[i].transform.position;
            startRotations[i] = items[i].transform.rotation;

            if (i != currentIndex)
            {
                // Reset rotation of non-selected geese
                items[i].transform.rotation = Quaternion.identity;
            }
        }

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * transitionSpeed;
            for (int i = 0; i < items.Length; i++)
            {
                int targetIndex = (i - currentIndex + items.Length) % items.Length;
                items[i].transform.position = Vector3.Lerp(startPositions[i], positions[targetIndex].position, t);
                items[i].transform.rotation = Quaternion.Lerp(startRotations[i], Quaternion.LookRotation(cameraTransform.position - positions[targetIndex].position), t);
            }
            yield return null;
        }
        
        PlayerPrefs.SetInt("SelectedGooseIndex", currentIndex);
        PlayerPrefs.Save();

        UpdateAllGooseAnimations();
        isTransitioning = false;
    }
    
    private void UpdateAllGooseAnimations()
    {
        for (int i = 0; i < items.Length; i++)
        {
            Animator animator = items[i].GetComponentInChildren<Animator>();
            if (animator == null) continue;

            if (i == currentIndex)
            {
                animator.Play("IdleMoving");
            }
            else
            {
                animator.Play("IdleStill");
            }
        }
    }
}
