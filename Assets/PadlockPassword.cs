using UnityEngine;

public class PadlockPassword : MonoBehaviour
{
    public GameObject padlock;
    public GameObject FirstPadlock;
    public GameObject SecondPadlock;
    public GameObject ThirdPadlock;
    public GameObject FourthPadlock;
    public GameObject door;
    public int[] Password = { 1, 2, 3, 4 };
    public int tolerance = 17; // 360/ 10 chiffres / 2 = 17
    
    private bool IsSolved = false;
    private int firstNumber;
    private int secondNumber;
    private int thirdNumber;
    private int fourthNumber;

    private Vector3 previousFirstRotation;
    private Vector3 previousSecondRotation;
    private Vector3 previousThirdRotation;
    private Vector3 previousFourthRotation;




    private void Start()
    {
        previousFirstRotation = FirstPadlock.transform.localEulerAngles;
        previousSecondRotation = SecondPadlock.transform.localEulerAngles;
        previousThirdRotation = ThirdPadlock.transform.localEulerAngles;
        previousFourthRotation = FourthPadlock.transform.localEulerAngles;
    }

    private void Update()
    {
        Vector3 currentFirstRotation = FirstPadlock.transform.localEulerAngles;
        Vector3 currentSecondRotation = SecondPadlock.transform.localEulerAngles;
        Vector3 currentThirdRotation = ThirdPadlock.transform.localEulerAngles;
        Vector3 currentFourthRotation = FourthPadlock.transform.localEulerAngles;


        if(currentFirstRotation != previousFirstRotation)
        {
            firstNumber = GetNumberFromAngle(currentFirstRotation.y, tolerance);
            previousFirstRotation = currentFirstRotation;
        }

        if(currentSecondRotation != previousSecondRotation)
        {
            secondNumber = GetNumberFromAngle(currentSecondRotation.y, tolerance);
            previousSecondRotation = currentSecondRotation;
        }

        if(currentThirdRotation != previousThirdRotation)
        {
            thirdNumber = GetNumberFromAngle(currentThirdRotation.y, tolerance);
            previousThirdRotation = currentThirdRotation;
        }

        if(currentFourthRotation != previousFourthRotation)
        {
            fourthNumber = GetNumberFromAngle(currentFourthRotation.y, tolerance);
            previousFourthRotation = currentFourthRotation;
        }

        IsSolved = isSolved();

        Debug.Log(firstNumber + " " + secondNumber + " " + thirdNumber + " " + fourthNumber);

        if(IsSolved)
        {
            OpenDoor();
            Destroy(padlock);
        }
    }
    private int mod(float x, int m)
    {
        return (int)((x % m + m) % m);
    }


    private int GetNumberFromAngle(float angle, float tolerance)
    {
        var modulo = mod(angle, 360);

        float[] numberAngleRanges = {
            200f,   // 0
            165f,   // 1
            130f,   // 2
            92f,    // 3
            56f,    // 4
            20f,    // 5
            346f,    // 6
            310f,    // 7
            273f,    // 8
            237f, // 9
        };

        for (int i = 0; i < numberAngleRanges.Length; i++)
        {
            if (modulo >= numberAngleRanges[i] - tolerance && modulo <= numberAngleRanges[i] + tolerance)
            {
                return i;
            }
        }

        return -1;
    }

    private bool isSolved()
    {
        return firstNumber == Password[0] && secondNumber == Password[1] && thirdNumber == Password[2] && fourthNumber == Password[3];
    }

    public void Test(float value)
    {
        Debug.Log("Something moved! " + value);
    }

    private void OpenDoor()
    {
        door.GetComponent<OpenDoor>().Open();
    }
}


