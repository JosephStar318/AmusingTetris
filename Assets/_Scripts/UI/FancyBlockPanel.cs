using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class FancyBlockPanel : MonoBehaviour
{
    [SerializeField] private BlockColorsSO blockColorsSO;
    [Range(0, 100)] [SerializeField] private int thresholdIndex = 0;
    private List<Transform> blocks = new List<Transform>();

    public int ThresholdIndex => Mathf.RoundToInt((thresholdIndex / 100f) * blocks.Count);
    private void Start()
    {
        blocks.Clear();
        foreach (Transform child in transform)
        {
            blocks.Add(child);
        }
    }
    public void SetUpColors()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Image>().color = blockColorsSO.GetRandomColor();
        }
    }
    public void ShuffleChildren()
    {
        int childCount = transform.childCount;

        // Generate an array of indices
        int[] indices = new int[childCount];
        for (int i = 0; i < childCount; i++)
        {
            indices[i] = i;
        }

        // Perform Fisher-Yates shuffle on the indices array
        for (int i = childCount - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // Rearrange child transforms based on shuffled indices
        for (int i = 0; i < childCount; i++)
        {
            transform.GetChild(i).SetSiblingIndex(indices[i]);
        }
        blocks.Clear();
        foreach (Transform child in transform)
        {
            blocks.Add(child);
        }
        Debug.Log(blocks.Count);
    }

    private void Update()
    {
        UpdateBlocks();
    }
    private void OnValidate()
    {
        UpdateBlocks();
    }

    private void UpdateBlocks()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (i < ThresholdIndex)
            {
                blocks[i].gameObject.SetActive(false);
            }
            else
            {
                blocks[i].gameObject.SetActive(true);
            }
        }
    }
}


