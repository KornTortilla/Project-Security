using UnityEngine;

public static class VectorUtility
{
    public static Vector3 OrientVectorHorizontal(Vector3 vectorToOrient, Vector3 forwardVector, Vector3 rightVector)
    {
        float oldY = vectorToOrient.y;

        Vector3 newForward = forwardVector * vectorToOrient.x;
        Vector3 newRight = rightVector * vectorToOrient.z;

        Vector3 newVector = newForward + newRight;
        newVector.y = oldY;

        return newVector;
    }
}
