using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObjectStruct[] selections;
    public GameObjectStruct[] hGates;
    public GameObjectStruct[] xGates;
    public GameObjectStruct[] cxGates;
    public GameObjectStruct[] xcGates;

    public float shooterMoveSpeed = 7.5f;

    public GameObject[] lasers;

    public GateType[,] entries;

    public GameObject[] shooters;

    List<int[]> statePool = new List<int[]>();

    int currentRow = 0;
    int currentCol = 0;

    int bitsCount, cyclesCount;

    private void Start()
    {
        bitsCount = selections.Length;
        cyclesCount = selections[0].cols.Length;

        InitEntryArray();
    }

    private void EnableSelection()
    {
        for (int s = 0; s < selections.Length; s++)
        {
            for (int k = 0; k < selections[0].cols.Length; k++)
            {
                selections[s].cols[k].SetActive(false);
            }
        }
        selections[currentRow].cols[currentCol].SetActive(true);
    }

    public void EnableHGate()
    {
        xGates[currentRow].cols[currentCol].SetActive(false);
        cxGates[currentRow].cols[currentCol].SetActive(false);
        xcGates[currentRow].cols[currentCol].SetActive(false);
        hGates[currentRow].cols[currentCol].SetActive(!hGates[currentRow].cols[currentCol].activeSelf);
    }

    public void EnableXGate()
    {
        hGates[currentRow].cols[currentCol].SetActive(false);
        cxGates[currentRow].cols[currentCol].SetActive(false);
        xcGates[currentRow].cols[currentCol].SetActive(false);
        xGates[currentRow].cols[currentCol].SetActive(!xGates[currentRow].cols[currentCol].activeSelf);
    }

    public void EnableCXGate()
    {
        hGates[currentRow].cols[currentCol].SetActive(false);
        xGates[currentRow].cols[currentCol].SetActive(false);
        xcGates[currentRow].cols[currentCol].SetActive(false);
        cxGates[currentRow].cols[currentCol].SetActive(!cxGates[currentRow].cols[currentCol].activeSelf);
    }

    public void EnableXCGate()
    {
        hGates[currentRow].cols[currentCol].SetActive(false);
        xGates[currentRow].cols[currentCol].SetActive(false);
        cxGates[currentRow].cols[currentCol].SetActive(false);
        xcGates[currentRow].cols[currentCol].SetActive(!xcGates[currentRow].cols[currentCol].activeSelf);
    }

    public void MoveRight()
    {
        currentCol = (currentCol + 1) % selections[0].cols.Length;
    }

    public void MoveLeft()
    {
        currentCol = (currentCol - 1) % selections[0].cols.Length;

        if (currentCol < 0)
            currentCol += selections[0].cols.Length;
    }

    public void MoveUp()
    {
        currentRow = (currentRow - 1) % selections.Length;

        if (currentRow < 0)
            currentRow += selections.Length;
    }

    public void MoveDown()
    {
        currentRow = (currentRow + 1) % selections.Length;
    }

    public void EnableLaser(int index, int shooter)
    {
        // Store the target position for the shooter
        Vector2 targetPosition = new Vector2(lasers[index].transform.position.x, shooters[shooter].transform.position.y);

        // Start moving the shooter towards the target position
        StartCoroutine(MoveShooterToTarget(shooter, targetPosition, index));
    }

    private IEnumerator MoveShooterToTarget(int shooter, Vector2 targetPosition, int laserIndex)
    {
        Vector2 startPosition = shooters[shooter].transform.position;
        float elapsedTime = 0f;
        float journeyLength = Vector2.Distance(startPosition, targetPosition);

        while (elapsedTime < journeyLength / shooterMoveSpeed)
        {
            // Use Lerp to smoothly move the shooter towards the target position
            shooters[shooter].transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / (journeyLength / shooterMoveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the shooter reaches the final target position
        shooters[shooter].transform.position = targetPosition;

        // Set the laser speed now that the shooter has reached the target position
        lasers[laserIndex].SetActive(true);
        lasers[laserIndex].GetComponent<LaserController>().SetNewSprite(shooter);
        lasers[laserIndex].GetComponent<LaserController>().speed = 10;
    }

    public void InitEntryArray()
    {
        entries = new GateType[bitsCount, cyclesCount];
        for (int i = 0; i < bitsCount; i++)
        {
            for (int j = 0; j < cyclesCount; j++)
            {
                entries[i, j] = GateType.Empty;
            }
        }
    }

    public void FillEntryArray()
    {
        for (int i = 0; i < bitsCount; i++)
        {
            for (int j = 0; j < cyclesCount; j++)
            {
                if (xGates[i].cols[j].activeSelf)
                {
                    entries[i, j] = GateType.X;
                }
                else if (hGates[i].cols[j].activeSelf)
                {
                    entries[i, j] = GateType.H;
                }
                else if (cxGates[i].cols[j].activeSelf)
                {
                    entries[i, j] = GateType.CX;
                }
                else if (xcGates[i].cols[j].activeSelf)
                {
                    entries[i, j] = GateType.XC;
                }
                else
                {
                    entries[i, j] = GateType.Empty;
                }
            }
        }
    }

    public void FillStatePool()
    {
        // init with adding one entry: 000..00

        int[] initialBits = Enumerable.Repeat(0, bitsCount).ToArray();
        statePool.Clear();
        statePool.Add(initialBits);

        for (int i = 0; i < cyclesCount; i++)
        {
            for (int j = 0; j < bitsCount; j++)
            {
                if (entries[j,i] == GateType.X)
                {
                    for (int k = 0; k < statePool.Count; k++)
                    {
                        statePool[k][j] = statePool[k][j] == 0 ? 1 : 0;
                    }
                }
                else if (entries[j,i] == GateType.H)
                {
                    int count = statePool.Count;
                    
                    for (int k = 0; k < count; k++)
                    {
                        int[] newBits = new int[bitsCount];
                        for (int t = 0; t < bitsCount; t++)
                        {
                            if (t == j)
                            {
                                newBits[t] = statePool[k][t] == 0 ? 1 : 0;
                            }
                            else
                            {
                                newBits[t] = statePool[k][t];
                            }
                        }
                        
                        statePool.Add(newBits);
                    }
                }
                else if (entries[j,i] == GateType.CX)
                {
                    for (int k = 0; k < statePool.Count; k++)
                    {
                        if (statePool[k][j] == 1)
                        {
                            statePool[k][(j + 1) % bitsCount] = statePool[k][(j + 1) % bitsCount] == 0 ? 1 : 0;
                        }
                    }
                }
                else if (entries[j, i] == GateType.XC)
                {
                    for (int k = 0; k < statePool.Count; k++)
                    {
                        if (statePool[k][j] == 1)
                        {
                            statePool[k][(j - 1 + bitsCount) % bitsCount] = statePool[k][(j - 1 + bitsCount) % bitsCount] == 0 ? 1 : 0;
                        }
                    }
                }
            }
        }

    }

    public int CalculateIndex(int[] state)
    {
        int result = 0;
        for (int j = 0; j < state.Length; j++)
        {
            result += ((int)Mathf.Pow(2, state.Length - 1 - j) * state[j]);
        }

        return result;
    }

    public void ResetLasers()
    {
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].transform.position = new Vector2(lasers[i].transform.position.x, -1.2f);
            lasers[i].GetComponent<LaserController>().speed = 0;
            lasers[i].SetActive(false);
        }
    }
    
    public void ResetGates()
    {
        for (int i = 0; i < hGates.Length; i++)
        {
            for (int j = 0; j < hGates[i].cols.Length; j++)
            {
                xGates[i].cols[j].SetActive(false);
                hGates[i].cols[j].SetActive(false);
                cxGates[i].cols[j].SetActive(false);
                xcGates[i].cols[j].SetActive(false);
            }
        }

        currentRow = 0;
        currentCol = 0;
    }

    public void Fire()
    {
        ResetLasers();
        FillEntryArray();
        FillStatePool();

        if (statePool.Count > 0)
        {
            List<int> totalStates = new List<int>();


            for (int i = 0; i < totalStates.Count; i++)
                totalStates[i] = -1;

            if (statePool.Count <= 2)
            {
                for (int i = 0; i < statePool.Count; i++)
                {
                    totalStates.Add(CalculateIndex(statePool[i]));
                }
            }
            else
            {
                int randCount = Random.Range(1, 4);
                for (int i = 0; i < randCount; i++)
                {
                    totalStates.Add(CalculateIndex(statePool[Random.Range(0, statePool.Count)]));
                }
            }

            totalStates = totalStates.OrderBy(num => num).ToList();

            if (totalStates.Count == 1)
            {
                if (totalStates[0] < 3)
                {
                    EnableLaser(totalStates[0], 0);
                }
                else if (totalStates[0] < 6)
                {
                    EnableLaser(totalStates[0], 1);
                }
                else
                {
                    EnableLaser(totalStates[0], 2);
                }
            }
            else if (totalStates.Count == 2)
            {
                if (totalStates[0] > 2)
                {
                    EnableLaser(totalStates[0], 1);
                    EnableLaser(totalStates[1], 2);
                }
                else if (totalStates[1] < 6)
                {
                    EnableLaser(totalStates[0], 0);
                    EnableLaser(totalStates[1], 1);
                }
                else
                {
                    EnableLaser(totalStates[0], 0);
                    EnableLaser(totalStates[1], 2);
                }
            }
            else
            {
                EnableLaser(totalStates[0], 0);
                EnableLaser(totalStates[1], 1);
                EnableLaser(totalStates[2], 2);
            }
        }

        ResetGates();
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            EnableXGate();
        }

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            EnableHGate();
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            EnableCXGate();
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            EnableXCGate();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Fire();
        }

        EnableSelection();
    }


    [System.Serializable]
    public struct GameObjectStruct
    {
        public GameObject[] cols;
    }

    public enum GateType
    {
        Empty = 0,
        X = 1, 
        H = 2,
        CX = 3,
        XC = 4
    }
}
