using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static event Action OnGridReady;
    public static event Action<TetrisBlock> OnPreviewChanged;

    public static event Action OnRowsHandled;
    public static event Action<int, List<List<Transform>>, Action> OnBeforeRowsCleared;

    [SerializeField] private float cellWidth;
    [SerializeField] private float cellHeight;
    [SerializeField] private int cellSizeX;
    [SerializeField] private int cellSizeY;

    [SerializeField] private Vector2 startPos = Vector2.zero;
    [SerializeField] private Vector2 localSpawnPoint = Vector2.zero;

    [SerializeField] public TetrisBlocksSO tetrisBlocks;
    [SerializeField] private LayerMask blockLayer;

    private Color previewBlockColor;
    private TetrisBlock previewBlock;

    private Vector2[,] gridCoords;
    private Vector2 BlockSpawnPos => transform.TransformPoint(localSpawnPoint);
    private Vector2 GridStartPos => transform.TransformPoint(startPos);
    public float CellWidth => cellWidth;
    public float CellHeight => cellHeight;

    [SerializeField] private AnimationCurve fallingBlockCurve;
    private float fallingBlockCurveIndex;
    private void Start()
    {
        SetupGrid();
    }

    private void SetupGrid()
    {
        gridCoords = new Vector2[cellSizeX, cellSizeY];
        Vector2 localStarPos = GridStartPos;

        for (int i = 0; i < cellSizeX; i++)
        {
            for (int j = 0; j < cellSizeY; j++)
            {
                gridCoords[i, j] = new Vector2(localStarPos.x + i * cellWidth, localStarPos.y + j * cellHeight);
            }
        }

        OnGridReady?.Invoke();
    }
    public bool IsSpawnAreaValid()
    {
        if (previewBlock != null)
        {
            foreach (Transform child in previewBlock.transform)
            {
                Collider2D coll = Physics2D.OverlapBox(child.position, Vector2.one * 0.1f, 0, blockLayer);
                if (coll != null) return false;
            }
        }
        return true;
    }

    public void RotateBlockInGrid(TetrisBlock block)
    {
        List<Vector2Int> rotatedBlockIndexList = GetRotatedBlockIndexList(block);
        int blockOffset = CheckRotatedBlockOffset(rotatedBlockIndexList);
        if (blockOffset != 0)
        {
            rotatedBlockIndexList = ApplyOffset(rotatedBlockIndexList, blockOffset);
        }

        if (RotationIsPossible(block, rotatedBlockIndexList) == false) return;

        int childIndex = 0;
        foreach (Transform child in block.transform)
        {
            Vector2Int indexVector = rotatedBlockIndexList[childIndex++];
            child.position = GetPositionFromIndexVector(indexVector);
        }
    }

    private bool RotationIsPossible(TetrisBlock block, List<Vector2Int> rotatedBlockIndexList)
    {
        foreach (Vector2Int indexVector in rotatedBlockIndexList)
        {
            if (indexVector.x < 0 || indexVector.x >= cellSizeX || indexVector.y < 0 || indexVector.y >= cellSizeY) return false;

            Vector2 pos = GetPositionFromIndexVector(indexVector);
            if (IsEmpty(pos, block) == false) return false;
        }
        return true;
    }

    private int CheckRotatedBlockOffset(List<Vector2Int> rotatedBlockIndexList)
    {
        int maxRightOffset = 0;
        int maxLeftOffset = 0;
        int leftOffset = 0;
        int rightOffset = 0;

        foreach (Vector2Int indexVector in rotatedBlockIndexList)
        {
            if (indexVector.x >= cellSizeX)
            {
                rightOffset = Mathf.RoundToInt(indexVector.x) - cellSizeX + 1;
            }
            else if (indexVector.x < 0)
            {
                leftOffset = Mathf.RoundToInt(indexVector.x) * -1;
            }
            maxRightOffset = maxRightOffset > rightOffset ? maxRightOffset : rightOffset;
            maxLeftOffset = maxLeftOffset > leftOffset ? maxLeftOffset : leftOffset;
        }

        if (maxRightOffset == 0) return -maxLeftOffset;
        return maxRightOffset;
    }
    private List<Vector2Int> ApplyOffset(List<Vector2Int> rotatedBlockIndexList, int offset)
    {
        for (int i = 0; i < rotatedBlockIndexList.Count; i++)
        {
            rotatedBlockIndexList[i] -= new Vector2Int(1, 0) * offset;
        }
        return rotatedBlockIndexList;
    }
    public List<Vector2Int> GetRotatedBlockIndexList(TetrisBlock block)
    {
        List<Vector2Int> rotatedBlockIndexList = new List<Vector2Int>();
        Vector2Int referenceIndexVector = GetIndexFromPosition(block.transform.position);

        foreach (Transform child in block.transform)
        {
            Vector2Int indexVector = GetIndexFromPosition(child.position);
            rotatedBlockIndexList.Add(RotateIndex90Degree(referenceIndexVector, indexVector));
        }
        return rotatedBlockIndexList;
    }

    public bool IsDownValid(TetrisBlock block)
    {
        Vector2 dir = new Vector2(0, -1);
        return CheckDirection(block, dir);
    }
    public bool IsLeftValid(TetrisBlock block)
    {
        Vector2 dir = new Vector2(-1, 0);
        return CheckDirection(block, dir);
    }
    public bool IsRightValid(TetrisBlock block)
    {
        Vector2 dir = new Vector2(1, 0);
        return CheckDirection(block, dir);
    }

    private bool CheckDirection(TetrisBlock block, Vector2 dir)
    {
        foreach (Transform child in block.transform)
        {
            Vector2 indexVector = GetIndexFromPosition(child.position);
            int i = (int)indexVector.x + (int)dir.x;
            int j = (int)indexVector.y + (int)dir.y;

            if (i < 0 || i >= cellSizeX || j < 0 || j >= cellSizeY)
            {
                return false;
            }

            Vector2 nextPos = gridCoords[i, j];
            if (IsEmpty(nextPos, block) == false) return false;
        }

        return true;
    }
    private bool IsEmpty(Vector2 pos, TetrisBlock block)
    {
        Collider2D coll = Physics2D.OverlapBox(pos, Vector2.one * 0.1f, 0);
        if (coll != null && coll.transform.parent != block.transform)
        {
            return false;
        }
        else return true;
    }
    private bool IsFilled(Vector2 pos, out Transform result)
    {
        Collider2D coll = Physics2D.OverlapBox(pos, Vector2.one * 0.1f, 0);
        if (coll != null)
        {
            result = coll.transform;
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }
    private Vector2Int RotateIndex90Degree(Vector2Int referencePoint, Vector2Int pointToRotate)
    {
        int deltaX = pointToRotate.x - referencePoint.x;
        int deltaY = pointToRotate.y - referencePoint.y;

        int new_x = referencePoint.x - deltaY;
        int new_y = referencePoint.y + deltaX;

        return new Vector2Int(new_x, new_y);
    }

    private Vector2Int GetIndexFromPosition(Vector2 pos)
    {
        Vector2Int indexVector = new Vector2Int(Mathf.RoundToInt((pos.x - GridStartPos.x) / cellWidth), Mathf.RoundToInt((pos.y - GridStartPos.y) / cellHeight));
        return indexVector;
    }
    private Vector2 GetPositionFromIndexVector(Vector2Int indexVector)
    {
        return gridCoords[Mathf.RoundToInt(indexVector.x), Mathf.RoundToInt(indexVector.y)];
    }
    public TetrisBlock SpawnTetrisBlock()
    {
        if (previewBlock == null)
        {
            SetupPreview();

            TetrisBlock block = tetrisBlocks.GetRandomBlock();
            TetrisBlock spawnedBlock = Instantiate(block, BlockSpawnPos, Quaternion.identity);
            spawnedBlock.transform.SetParent(transform);
            return spawnedBlock;
        }
        else
        {
            TetrisBlock spawnedBlock = previewBlock;
            spawnedBlock.gameObject.SetActive(true);
            SetupPreview();

            return spawnedBlock;
        }

    }

    private void SetupPreview()
    {
        TetrisBlock block = tetrisBlocks.GetRandomBlock();
        TetrisBlock spawnedBlock = Instantiate(block, BlockSpawnPos, Quaternion.identity);
        spawnedBlock.transform.SetParent(transform);
        previewBlock = spawnedBlock;
        previewBlock.gameObject.SetActive(false);
        OnPreviewChanged?.Invoke(previewBlock);
    }

    public void HandleCompletedRows(TetrisBlock block)
    {
        StartCoroutine(HandleRows(block));
    }
    private IEnumerator HandleRows(TetrisBlock block)
    {
        List<List<Transform>> completedRowList = new List<List<Transform>>();
        List<int> checkedRowIndexes = new List<int>();

        Transform[] children = new Transform[block.transform.childCount];
        for (int i = 0; i < block.transform.childCount; i++)
        {
            children[i] = block.transform.GetChild(i);
        }
        foreach (Transform child in children)
        {
            if (child == null) continue;

            int j = GetIndexFromPosition(child.position).y;
            if (checkedRowIndexes.Contains(j)) continue;
            checkedRowIndexes.Add(j);

            if (j >= 0 && j < cellSizeY)
            {
                if (CheckRow(j, out List<Transform> resultList))
                {
                    completedRowList.Add(resultList);
                }
            }
        }

        if (completedRowList.Count > 0)
        {
            List<List<Transform>> rows = completedRowList.GetRange(0, completedRowList.Count);
            OnBeforeRowsCleared?.Invoke(
                rows.Count,
                rows,
                () =>
                {
                    foreach (List<Transform> row in rows)
                    {
                        DestroyRow(row);
                    }
                });
        }
        yield return null;
        OnRowsHandled?.Invoke();
    }

    //private void ShiftBlocksBy(List<List<Transform>> completedRowList, int rowIndex)
    //{
    //    Vector2 pointA = GetPositionFromIndexVector(new Vector2Int(0, rowIndex + 1));
    //    Vector2 pointB = GetPositionFromIndexVector(new Vector2Int(cellSizeX - 1, cellSizeY - 1));
    //    Collider2D[] allBlocks = Physics2D.OverlapAreaAll(pointA, pointB);
    //    List<Vector2> blockDestinations = new List<Vector2>(allBlocks.Length);

    //    foreach (Collider2D coll in allBlocks)
    //    {
    //        Vector2 destination = (Vector2)coll.transform.position + Vector2.down * cellHeight * completedRowList.Count;
    //        blockDestinations.Add(destination);
    //    }

    //    StartCoroutine(FallBlocks(allBlocks, blockDestinations, () => OnRowsHandled?.Invoke()));
    //}

    //private IEnumerator FallBlocks(Collider2D[] allBlocks, List<Vector2> blockDestinations, Action callback)
    //{
    //    while (fallingBlockCurveIndex < 1)
    //    {
    //        fallingBlockCurveIndex += Time.fixedDeltaTime * 3f;
    //        fallingBlockCurveIndex = Mathf.Clamp(fallingBlockCurveIndex, 0, 1);

    //        for (int i = 0; i < allBlocks.Length; i++)
    //        {
    //            allBlocks[i].transform.position = Vector2.Lerp(allBlocks[i].transform.position, blockDestinations[i], fallingBlockCurve.Evaluate(fallingBlockCurveIndex));
    //        }

    //        yield return new WaitForFixedUpdate();
    //    }
    //    fallingBlockCurveIndex = 0;
    //    callback.Invoke();
    //}

    private void DestroyRow(List<Transform> resultList)
    {
        int rowIndex = GetIndexFromPosition(resultList[0].position).y;
        foreach (Transform singleBlock in resultList)
        {
            Destroy(singleBlock.gameObject);
        }
        ShiftGridDownByOne(rowIndex);
    }


    private void ShiftGridDownByOne(int rowIndex)
    {
        Vector2 pointA = GetPositionFromIndexVector(new Vector2Int(0, rowIndex + 1));
        Vector2 pointB = GetPositionFromIndexVector(new Vector2Int(cellSizeX - 1, cellSizeY - 1));

        Collider2D[] allBlocks = Physics2D.OverlapAreaAll(pointA, pointB);

        foreach (Collider2D coll in allBlocks)
        {
            coll.transform.Translate(Vector2.down * cellHeight, Space.World);
        }
    }
    public void ClearHalf()
    {
        Vector2 pointA = GetPositionFromIndexVector(new Vector2Int(0, (cellSizeY / 2) - 1));
        Vector2 pointB = GetPositionFromIndexVector(new Vector2Int(cellSizeX - 1, cellSizeY - 1));

        Collider2D[] allBlocks = Physics2D.OverlapAreaAll(pointA, pointB + Vector2.up, blockLayer);
        foreach (Collider2D coll in allBlocks)
        {
            Destroy(coll.gameObject);
        }
    }
    private bool CheckRow(int j, out List<Transform> resultList)
    {
        resultList = new List<Transform>();

        for (int i = 0; i < cellSizeX; i++)
        {
            Vector2Int currentIndexVector = new Vector2Int(i, j);
            if (IsFilled(GetPositionFromIndexVector(currentIndexVector), out Transform result))
            {
                resultList.Add(result);
            }
        }
        if (resultList.Count == cellSizeX)
        {
            return true;
        }
        else return false;
    }
    private void OnDrawGizmos()
    {
        if (gridCoords == null) return;

        foreach (Vector2 point in gridCoords)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}
