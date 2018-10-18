using UnityEngine;
using UnityEngine.Assertions;

public class Spinner : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float xRotationsPerMinute = 1.0f;
    [SerializeField]
    private float yRotationsPerMinute = 1.0f;
    [SerializeField]
    private float zRotationsPerMinute = 1.0f;
    #endregion

    #region Methods
    // Called before the first frame update.
    private void Start()
    {
    }

    // Called once per frame.
    private void Update()
    {
        // "Dimensional Analysis" in code notation to get the formula for
        // transforming RotationsPerMinute into DegreesPerFrame:
        //
        // xDegreesPerFrame     = xRotationsPerMinute       60                      360                     Time.DeltaTime
        // *                      *                         *                       *                       *
        // degrees   frame^-1   = rotation * minute^-1      seconds * minute^-1     degrees * rotation^-1   seconds * frame^-1
        // Cancel out units     ----------------------------------------------------------------------------------------------
        // by finding right     = (rotation ---------   /   seconds ---------)      degrees * rotation^-1   seconds * frame^-1
        // operators            = (--------             /   seconds          )  *   degrees -----------     seconds * frame^-1
        //                      = (1                    /   seconds          )  *   degrees             *   seconds * frame^-1
        //                      = (1                    /   -------          )  *   degrees             *   -------   frame^-1
        // Result:              ----------------------------------------------------------------------------------------------
        // degrees frame^-1     = xRotationsPerMinute   /   60                  *   360                 *   Time.DeltaTime
        // More readable:       = xRotationsPerMinute   *   360                 *   Time.DeltaTime      /   60

        var xDegreesPerFrame = xRotationsPerMinute * 360 * Time.deltaTime / 60;
        transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

        var yDegreesPerFrame = yRotationsPerMinute * 360 * Time.deltaTime / 60;
        transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

        var zDegreesPerFrame = zRotationsPerMinute * 360 * Time.deltaTime / 60;
        transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
    }
    #endregion
}