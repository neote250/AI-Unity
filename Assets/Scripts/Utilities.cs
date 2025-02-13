using UnityEngine;

public static class Utilities
{
    public static float Wrap(float value, float min, float max)
    {
        return (value < min) ? max : (value > max) ? min : value;
    }
    public static Vector3 Wrap(Vector3 value, Vector3 min, Vector3 max)
    {
        value.x = Wrap(value.x, min.x, max.x);
        value.y = Wrap(value.y, min.y, max.y);
        value.z = Wrap(value.z, min.z, max.z);


        return value;
    }
    public static Vector3[] GetDirectionsInCircle(int num, float angle)
    {
        if (num <= 0) return null;
        if (num == 1) return new Vector3[] { Vector3.forward };

        // create array of vector3
        Vector3[] result = new Vector3[num];
        int index = 0;

        // set forward direction if odd number
        if (num%2==1)
        {
            result[index++] = Vector3.forward;
            num--;
        }

        // compute angle between rays (angle * 2 / num rays - 1)
        float angleOffset = angle * 2 / result.Length - 1;

        // add directions symmetrically around the circle
        for (int i = 1; i <= num / 2; i++)
        {
            result[index++] = Quaternion.AngleAxis(+angleOffset * i, Vector3.up) * Vector3.forward;
            result[index++] = Quaternion.AngleAxis(-angleOffset * i, Vector3.up) * Vector3.forward;
        }

        return result;
    }
}
