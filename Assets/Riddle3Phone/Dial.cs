using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Dial : MonoBehaviour
{
    public Phone phone;
    private HingeJoint hinge;

    private LimitedGrab grab;

    public float degree = 30f;

    public string[] chars = { "0", "9", "8", "7", "6", "5", "4", "3", "2", "1", "#", "*" };

    void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        
        grab = GetComponent<LimitedGrab>();

        if (grab != null)
        {
            grab.selectExited.AddListener(OnReleased);
        }
    }

    public void OnReleased(SelectExitEventArgs args)
    {
        float angle = Mathf.Abs(hinge.angle);
        
        int index = Mathf.RoundToInt(angle / degree);

        if (index > 0 && index < chars.Length)
        {
            phone.addNum(chars[index-1]);
        }
    }
}
