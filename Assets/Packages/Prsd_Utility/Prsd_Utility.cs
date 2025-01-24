using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.Text;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif


public static class Prsd_Utility
{
    #region Transform

    /// <summary>
    /// Copies the position and rotation from another transform to this transform.
    /// </summary>
    /// <param name="transform">The transform to set the position/rotation at.</param>
    /// <param name="source">The transform to take the position/rotation from.</param>
    public static void CopyPositionAndRotatationFrom(this Transform transform, Transform source)
    {
        transform.position = source.position;
        transform.rotation = source.rotation;
    }

    /// <summary>
    /// Copies the position and rotation from another transform to this transform.
    /// </summary>
    /// <param name="transform">The transform to set the position/rotation at.</param>
    /// <param name="source">The transform to take the position/rotation from.</param>
    public static void CopyLocalPositionAndRotatationFrom(this Transform transform, Transform source)
    {
        transform.localPosition = source.localPosition;
        transform.localRotation = source.localRotation;
    }


    /// <summary>
    /// Sets the x/y/z transform.position using optional parameters, keeping all undefined values as they were before. Can be
    /// called with named parameters like transform.SetPosition(x: 5, z: 10), for example, only changing transform.position.x and z.
    /// </summary>
    /// <param name="transform">The transform to set the transform.position at.</param>
    /// <param name="x">If this is not null, transform.position.x is set to this value.</param>
    /// <param name="y">If this is not null, transform.position.y is set to this value.</param>
    /// <param name="z">If this is not null, transform.position.z is set to this value.</param>
    /// <returns>The transform itself.</returns>
    public static Transform SetPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
    {
        transform.position = transform.position.Change3(x, y, z);
        return transform;
    }

    /// <summary>
    /// Sets the x/y/z transform.localPosition using optional parameters, keeping all undefined values as they were before. Can be
    /// called with named parameters like transform.SetLocalPosition(x: 5, z: 10), for example, only changing transform.localPosition.x and z.
    /// </summary>
    /// <param name="transform">The transform to set the transform.localPosition at.</param>
    /// <param name="x">If this is not null, transform.localPosition.x is set to this value.</param>
    /// <param name="y">If this is not null, transform.localPosition.y is set to this value.</param>
    /// <param name="z">If this is not null, transform.localPosition.z is set to this value.</param>
    /// <returns>The transform itself.</returns>
    public static Transform SetLocalPosition(this Transform transform, float? x = null, float? y = null, float? z = null)
    {
        transform.localPosition = transform.localPosition.Change3(x, y, z);
        return transform;
    }

    /// <summary>
    /// Sets the x/y/z transform.localScale using optional parameters, keeping all undefined values as they were before. Can be
    /// called with named parameters like transform.SetLocalScale(x: 5, z: 10), for example, only changing transform.localScale.x and z.
    /// </summary>
    /// <param name="transform">The transform to set the transform.localScale at.</param>
    /// <param name="x">If this is not null, transform.localScale.x is set to this value.</param>
    /// <param name="y">If this is not null, transform.localScale.y is set to this value.</param>
    /// <param name="z">If this is not null, transform.localScale.z is set to this value.</param>
    /// <returns>The transform itself.</returns>
    public static Transform SetLocalScale(this Transform transform, float? x = null, float? y = null, float? z = null)
    {
        transform.localScale = transform.localScale.Change3(x, y, z);
        return transform;
    }

    /// <summary>
    /// Sets the x/y/z transform.lossyScale using optional parameters, keeping all undefined values as they were before. Can be
    /// called with named parameters like transform.SetLossyScale(x: 5, z: 10), for example, only changing transform.lossyScale.x and z.
    /// </summary>
    /// <param name="transform">The transform to set the transform.lossyScale at.</param>
    /// <param name="x">If this is not null, transform.lossyScale.x is set to this value.</param>
    /// <param name="y">If this is not null, transform.lossyScale.y is set to this value.</param>
    /// <param name="z">If this is not null, transform.lossyScale.z is set to this value.</param>
    /// <returns>The transform itself.</returns>
    public static Transform SetLossyScale(this Transform transform, float? x = null, float? y = null, float? z = null)
    {
        var lossyScale = transform.lossyScale.Change3(x, y, z);

        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(lossyScale.x / transform.lossyScale.x,
                                           lossyScale.y / transform.lossyScale.y,
                                           lossyScale.z / transform.lossyScale.z);

        return transform;
    }

    /// <summary>
    /// Sets the x/y/z transform.eulerAngles using optional parameters, keeping all undefined values as they were before. Can be
    /// called with named parameters like transform.SetEulerAngles(x: 5, z: 10), for example, only changing transform.eulerAngles.x and z.
    /// </summary>
    /// <param name="transform">The transform to set the transform.eulerAngles at.</param>
    /// <param name="x">If this is not null, transform.eulerAngles.x is set to this value.</param>
    /// <param name="y">If this is not null, transform.eulerAngles.y is set to this value.</param>
    /// <param name="z">If this is not null, transform.eulerAngles.z is set to this value.</param>
    /// <returns>The transform itself.</returns>
    public static Transform SetEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
    {
        transform.eulerAngles = transform.eulerAngles.Change3(x, y, z);
        return transform;
    }

    /// <summary>
    /// Sets the x/y/z transform.localEulerAngles using optional parameters, keeping all undefined values as they were before. Can be
    /// called with named parameters like transform.SetLocalEulerAngles(x: 5, z: 10), for example, only changing transform.localEulerAngles.x and z.
    /// </summary>
    /// <param name="transform">The transform to set the transform.localEulerAngles at.</param>
    /// <param name="x">If this is not null, transform.localEulerAngles.x is set to this value.</param>
    /// <param name="y">If this is not null, transform.localEulerAngles.y is set to this value.</param>
    /// <param name="z">If this is not null, transform.localEulerAngles.z is set to this value.</param>
    /// <returns>The transform itself.</returns>
    public static Transform SetLocalEulerAngles(this Transform transform, float? x = null, float? y = null, float? z = null)
    {
        transform.localEulerAngles = transform.localEulerAngles.Change3(x, y, z);
        return transform;
    }

    public static Transform DestroyAllChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        return transform;
    }

    public static Transform DisableAllChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        return transform;
    }

    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }
        return null; // Could not find a parent with given tag.
    }

    public static List<GameObject> FindObjectsWithTag(this Transform parent, string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                taggedGameObjects.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
            }
        }
        return taggedGameObjects;
    }

    public static void ScaleAround(this Transform target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 A = target.localPosition;
        Vector3 B = pivot;

        Vector3 C = A - B; // diff from object pivot to desired pivot/origin

        float RS = newScale.x / target.localScale.x; // relataive scale factor

        // calc final position post-scale
        Vector3 FP = B + C * RS;

        // finally, actually perform the scale/translation
        target.localScale = newScale;
        target.localPosition = FP;
    }

    #endregion

    #region GameObject

    /// <summary>
    /// Assigns a layer to this GameObject and all its children recursively.
    /// </summary>
    /// <param name="gameObject">The GameObject to start at.</param>
    /// <param name="layer">The layer to set.</param>
    public static void AssignLayerToHierarchy(this GameObject gameObject, int layer)
    {
        var transforms = gameObject.GetComponentsInChildren<Transform>();
        for (var i = 0; i < transforms.Length; i++)
        {
            transforms[i].gameObject.layer = layer;
        }
    }

    /// <summary>
    /// When <see cref="Object.Instantiate(Object)"/> is called on a prefab named
    /// "Original", the resulting instance will be named "Original(Clone)". This method changes the name
    /// back to "Original" by stripping everything after and including the first "(Clone)" it finds. If no
    /// "(Clone)" is found, the name is left unchanged.
    /// </summary>
    /// <param name="gameObject">The GameObject to change the name of.</param>
    public static void StripCloneFromName(this GameObject gameObject)
    {
        gameObject.name = gameObject.GetNameWithoutClone();
    }

    /// <summary>
    /// When <see cref="Object.Instantiate(Object)"/> is called on a prefab named
    /// "Original", the resulting instance will be named "Original(Clone)". This method returns the name
    /// without "(Clone)" by stripping everything after and including the first "(Clone)" it finds. If no
    /// "(Clone)" is found, the name is returned unchanged.
    /// </summary>
    /// <param name="gameObject">The GameObject to return the original name of.</param>
    public static string GetNameWithoutClone(this GameObject gameObject)
    {
        var gameObjectName = gameObject.name;

        var clonePartIndex = gameObjectName.IndexOf("(Clone)", StringComparison.Ordinal);
        if (clonePartIndex == -1)
            return gameObjectName;

        return gameObjectName.Substring(0, clonePartIndex);
    }

    public static List<T> GetItemsAsT<T>(this Component[] objects) where T : Component
    {
        var returnList = new List<T>();

        for (int i = 0; i < objects.Length; i++)
        {
            var t = objects[i].GetComponent<T>();
            if (t != null)
                returnList.Add(t);
        }

        return returnList;
    }

    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }


    public static List<T> FindInterfaces<T>()
    {
        List<T> interfaces = new List<T>();
        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var rootGameObject in rootGameObjects)
        {
            T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
            foreach (var childInterface in childrenInterfaces)
            {
                interfaces.Add(childInterface);
            }
        }

        return interfaces;
    }

    public static GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            var o = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            o.transform.SetPositionAndRotation(position, rotation);
            return o;
        }
        else
#endif
            return Object.Instantiate(prefab, position, rotation, parent);
    }

    public static T InstantiatePrefab<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            var o = UnityEditor.PrefabUtility.InstantiatePrefab(prefab, parent) as T;
            o.transform.SetPositionAndRotation(position, rotation);
            return o;
        }
        else
#endif
            return Object.Instantiate(prefab, position, rotation, parent);
    }

    public static void DestroySafe(GameObject o)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) Object.DestroyImmediate(o);
        else
#endif
            Object.Destroy(o);
    }

#if UNITY_EDITOR
    public static List<T> FindInterfacesEditor<T>()
    {
        List<T> interfaces = new List<T>();
        GameObject[] rootGameObjects = EditorSceneManager.GetActiveScene().GetRootGameObjects();

        foreach (var rootGameObject in rootGameObjects)
        {
            T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
            foreach (var childInterface in childrenInterfaces)
            {
                interfaces.Add(childInterface);
            }
        }

        return interfaces;
    }
#endif

    #endregion

    #region Vector2/3/4

    /// <summary>
    /// Makes a copy of the Vector2 with changed x/y values, keeping all undefined values as they were before. Can be
    /// called with named parameters like vector.Change2(y: 5), for example, only changing the y component.
    /// </summary>
    /// <param name="vector">The Vector2 to be copied with changed values.</param>
    /// <param name="x">If this is not null, the x component is set to this value.</param>
    /// <param name="y">If this is not null, the y component is set to this value.</param>
    /// <returns>A copy of the Vector2 with changed values.</returns>
    public static Vector2 Change2(this Vector2 vector, float? x = null, float? y = null)
    {
        if (x.HasValue) vector.x = x.Value;
        if (y.HasValue) vector.y = y.Value;
        return vector;
    }

    /// <summary>
    /// Makes a copy of the Vector3 with changed x/y/z values, keeping all undefined values as they were before. Can be
    /// called with named parameters like vector.Change3(x: 5, z: 10), for example, only changing the x and z components.
    /// </summary>
    /// <param name="vector">The Vector3 to be copied with changed values.</param>
    /// <param name="x">If this is not null, the x component is set to this value.</param>
    /// <param name="y">If this is not null, the y component is set to this value.</param>
    /// <param name="z">If this is not null, the z component is set to this value.</param>
    /// <returns>A copy of the Vector3 with changed values.</returns>
    public static Vector3 Change3(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
        if (x.HasValue) vector.x = x.Value;
        if (y.HasValue) vector.y = y.Value;
        if (z.HasValue) vector.z = z.Value;
        return vector;
    }

    /// <summary>
    /// Makes a copy of the Vector4 with changed x/y/z/w values, keeping all undefined values as they were before. Can be
    /// called with named parameters like vector.Change4(x: 5, z: 10), for example, only changing the x and z components.
    /// </summary>
    /// <param name="vector">The Vector4 to be copied with changed values.</param>
    /// <param name="x">If this is not null, the x component is set to this value.</param>
    /// <param name="y">If this is not null, the y component is set to this value.</param>
    /// <param name="z">If this is not null, the z component is set to this value.</param>
    /// <param name="w">If this is not null, the w component is set to this value.</param>
    /// <returns>A copy of the Vector4 with changed values.</returns>
    public static Vector4 Change4(this Vector4 vector, float? x = null, float? y = null, float? z = null, float? w = null)
    {
        if (x.HasValue) vector.x = x.Value;
        if (y.HasValue) vector.y = y.Value;
        if (z.HasValue) vector.z = z.Value;
        if (w.HasValue) vector.w = w.Value;
        return vector;
    }

    /// <summary>
    /// Rotates a Vector2.
    /// </summary>
    /// <param name="v">The Vector2 to rotate.</param>
    /// <param name="angleRad">How far to rotate the Vector2 in radians.</param>
    /// <returns>The rotated Vector2.</returns>
    public static Vector2 RotateRad(this Vector2 v, float angleRad)
    {
        // http://answers.unity3d.com/questions/661383/whats-the-most-efficient-way-to-rotate-a-vector2-o.html
        var sin = Mathf.Sin(angleRad);
        var cos = Mathf.Cos(angleRad);

        var tx = v.x;
        var ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);

        return v;
    }

    /// <summary>
    /// Rotates a Vector2.
    /// </summary>
    /// <param name="v">The Vector2 to rotate.</param>
    /// <param name="angleDeg">How far to rotate the Vector2 in degrees.</param>
    /// <returns>The rotated Vector2.</returns>
    public static Vector2 RotateDeg(this Vector2 v, float angleDeg)
    {
        return v.RotateRad(angleDeg * Mathf.Deg2Rad);
    }

    /// <summary>
    /// Creates a Vector2 with a length of 1 pointing towards a certain angle.
    /// </summary>
    /// <param name="angleRad">The angle in radians.</param>
    /// <returns>The Vector2 pointing towards the angle.</returns>
    public static Vector2 CreateVector2AngleRad(float angleRad)
    {
        return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    /// <summary>
    /// Creates a Vector2 with a length of 1 pointing towards a certain angle.
    /// </summary>
    /// <param name="angleDeg">The angle in degrees.</param>
    /// <returns>The Vector2 pointing towards the angle.</returns>
    public static Vector2 CreateVector2AngleDeg(float angleDeg)
    {
        return CreateVector2AngleRad(angleDeg * Mathf.Deg2Rad);
    }

    /// <summary>
    /// Gets the rotation of a Vector2.
    /// </summary>
    /// <param name="vector">The Vector2.</param>
    /// <returns>The rotation of the Vector2 in radians.</returns>
    public static float GetAngleRad(this Vector3 vector)
    {
        return Mathf.Atan2(vector.y, vector.x);
    }

    /// <summary>
    /// Gets the rotation of a Vector2.
    /// </summary>
    /// <param name="vector">The Vector2.</param>
    /// <returns>The rotation of the Vector2 in degrees.</returns>
    public static float GetAngleDeg(this Vector3 vector)
    {
        return vector.GetAngleRad() * Mathf.Rad2Deg;
    }

    /// <summary>
    /// Checks if two vectors are approximately the same with a tolerancy
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="tolerancy"></param>
    /// <returns></returns>
    public static bool V3Equal(Vector3 a, Vector3 b, float tolerancy = 0.00001f)
    {
        return Vector3.SqrMagnitude(a - b) < tolerancy;
    }

    public static Vector2 XZ(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public static Vector3 ToXZ(this Vector2 vector)
    {
        return new Vector3(vector.x, 0f, vector.y);
    }

    /// <summary>
    /// Clamsp each vector component between two values.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="min">Clamp min value.</param>
    /// <param name="max">Clamp max value.</param>
    public static Vector2 ClampComponents(this Vector2 vector, float min, float max)
    {
        Vector2 v;
        v.x = Mathf.Clamp(vector.x, min, max);
        v.y = Mathf.Clamp(vector.y, min, max);
        return v;
    }

    /// <summary>
    /// Clamps each vector component between two values.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="min">Clamp min value.</param>
    /// <param name="max">Clamp max value.</param>
    public static Vector3 ClampComponents(this Vector3 vector, float min, float max)
    {
        Vector3 v;
        v.x = Mathf.Clamp(vector.x, min, max);
        v.y = Mathf.Clamp(vector.y, min, max);
        v.z = Mathf.Clamp(vector.z, min, max);
        return v;
    }

    #endregion

    #region Quaternion

    public static Quaternion ClampRotation(this Quaternion q, Vector3 bounds)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -bounds.x, bounds.x);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);
        angleY = Mathf.Clamp(angleY, -bounds.y, bounds.y);
        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        float angleZ = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.z);
        angleZ = Mathf.Clamp(angleZ, -bounds.z, bounds.z);
        q.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZ);

        return q.normalized;
    }

    #endregion

    #region Color

    /// <summary>
    /// Makes a copy of the Color with changed r/g/b/a values, keeping all undefined values as they were before. Can be
    /// called with named parameters like color.Change(g: 0, a: 0.5), for example, only changing the g and a components.
    /// </summary>
    /// <param name="color">The Color to be copied with changed values.</param>
    /// <param name="r">If this is not null, the r component is set to this value.</param>
    /// <param name="g">If this is not null, the g component is set to this value.</param>
    /// <param name="b">If this is not null, the b component is set to this value.</param>
    /// <param name="a">If this is not null, the a component is set to this value.</param>
    /// <returns>A copy of the Color with changed values.</returns>
    public static Color Change(this Color color, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        if (r.HasValue) color.r = r.Value;
        if (g.HasValue) color.g = g.Value;
        if (b.HasValue) color.b = b.Value;
        if (a.HasValue) color.a = a.Value;
        return color;
    }

    /// <summary>
    /// Makes a copy of the vector with a changed alpha value.
    /// </summary>
    /// <param name="color">The Color to copy.</param>
    /// <param name="a">The new a component.</param>
    /// <returns>A copy of the Color with a changed alpha.</returns>
    public static Color ChangeAlpha(this Color color, float a)
    {
        color.a = a;
        return color;
    }

    #endregion

    #region Rect

    /// <summary>
    /// Extends/shrinks the rect by extendDistance to each side and gets a random position from the resulting rect.
    /// </summary>
    /// <param name="rect">The Rect.</param>
    /// <param name="extendDistance">The distance to extend/shrink the rect to each side.</param>
    /// <returns>A random position inside the extended rect.</returns>
    public static Vector2 RandomPosition(this Rect rect, float extendDistance = 0f)
    {
        return new Vector2(Random.Range(rect.xMin - extendDistance, rect.xMax + extendDistance),
                           Random.Range(rect.yMin - extendDistance, rect.yMax + extendDistance));
    }

    /// <summary>
    /// Gets a random subrect of the given width or height inside this rect.
    /// </summary>
    /// <param name="rect">The Rect.</param>
    /// <param name="width">The target width of the subrect. Clamped to the width of the given rect.</param>
    /// <param name="height">The target height of the subrect. Clamped to the height of the given rect.</param>
    /// <returns>A random subrect with the given width and height.</returns>
    public static Rect RandomSubRect(this Rect rect, float width, float height)
    {
        width = Mathf.Min(rect.width, width);
        height = Mathf.Min(rect.height, height);

        var halfWidth = width / 2f;
        var halfHeight = height / 2f;

        var centerX = Random.Range(rect.xMin + halfWidth, rect.xMax - halfWidth);
        var centerY = Random.Range(rect.yMin + halfHeight, rect.yMax - halfHeight);

        return new Rect(centerX - halfWidth, centerY - halfHeight, width, height);
    }

    /// <summary>
    /// Extends/shrinks the rect by extendDistance to each side and then restricts the given vector to the resulting rect.
    /// </summary>
    /// <param name="rect">The Rect.</param>
    /// <param name="position">A position that should be restricted to the rect.</param>
    /// <param name="extendDistance">The distance to extend/shrink the rect to each side.</param>
    /// <returns>The vector, clamped to the Rect.</returns>
    public static Vector2 Clamp2(this Rect rect, Vector2 position, float extendDistance = 0f)
    {
        return new Vector2(Mathf.Clamp(position.x, rect.xMin - extendDistance, rect.xMax + extendDistance),
                           Mathf.Clamp(position.y, rect.yMin - extendDistance, rect.yMax + extendDistance));
    }

    /// <summary>
    /// Extends/shrinks the rect by extendDistance to each side and then restricts the given vector to the resulting rect.
    /// The z component is kept.
    /// </summary>
    /// <param name="rect">The Rect.</param>
    /// <param name="position">A position that should be restricted to the rect.</param>
    /// <param name="extendDistance">The distance to extend/shrink the rect to each side.</param>
    /// <returns>The vector, clamped to the Rect.</returns>
    public static Vector3 Clamp3(this Rect rect, Vector3 position, float extendDistance = 0f)
    {
        return new Vector3(Mathf.Clamp(position.x, rect.xMin - extendDistance, rect.xMax + extendDistance),
                           Mathf.Clamp(position.y, rect.yMin - extendDistance, rect.yMax + extendDistance),
                           position.z);
    }

    /// <summary>
    /// Extends/shrinks the rect by extendDistance to each side.
    /// </summary>
    /// <param name="rect">The Rect.</param>
    /// <param name="extendDistance">The distance to extend/shrink the rect to each side.</param>
    /// <returns>The rect, extended/shrunken by extendDistance to each side.</returns>
    public static Rect Extend(this Rect rect, float extendDistance)
    {
        var copy = rect;
        copy.xMin -= extendDistance;
        copy.xMax += extendDistance;
        copy.yMin -= extendDistance;
        copy.yMax += extendDistance;
        return copy;
    }

    /// <summary>
    /// Extends/shrinks the rect by extendDistance to each side and then checks if a given point is inside the resulting rect.
    /// </summary>
    /// <param name="rect">The Rect.</param>
    /// <param name="position">A position that should be restricted to the rect.</param>
    /// <param name="extendDistance">The distance to extend/shrink the rect to each side.</param>
    /// <returns>True if the position is inside the extended rect.</returns>
    public static bool Contains(this Rect rect, Vector2 position, float extendDistance)
    {
        return (position.x > rect.xMin + extendDistance) &&
               (position.y > rect.yMin + extendDistance) &&
               (position.x < rect.xMax - extendDistance) &&
               (position.y < rect.yMax - extendDistance);
    }

    /// <summary>
    /// Creates an array containing the four corner points of a Rect.
    /// </summary>
    /// <param name="rect">The Rect.</param>
    /// <returns>An array containing the four corner points of the Rect.</returns>
    public static Vector2[] GetCornerPoints(this Rect rect)
    {
        return new[]
                   {
                           new Vector2(rect.xMin, rect.yMin),
                           new Vector2(rect.xMax, rect.yMin),
                           new Vector2(rect.xMax, rect.yMax),
                           new Vector2(rect.xMin, rect.yMax)
                       };
    }

    #endregion

    #region PlayerPrefs

    /// <summary>
    /// Returns the value corresponding to the key in the preference file if it exists.
    /// If it doesn't exist, it will return defaultValue.
    /// (Internally, the value is stored as an int with either 0 or 1.)
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="defaultValue">The default value if none is given.</param>
    /// <returns>The value corresponding to key in the preference file if it exists, else the default value.</returns>
    public static bool PlayerPrefsGetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }

    /// <summary>
    /// Sets the value of the preference entry identified by the key.
    /// (Internally, the value is stored as an int with either 0 or 1.)
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value to set the preference entry to.</param>
    public static void PlayerPrefsSetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    #endregion

    #region LayerMask

    /// <summary>
    /// Is a specific layer actived in the given LayerMask?
    /// </summary>
    /// <param name="mask">The LayerMask.</param>
    /// <param name="layer">The layer to check for.</param>
    /// <returns>True if the layer is activated.</returns>
    public static bool ContainsLayer(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    /// <summary>
    /// Returns an int value from a LayerMask 
    /// https://forum.unity.com/threads/get-the-layernumber-from-a-layermask.114553/
    /// </summary>
    /// <param name="mask"></param>
    /// <returns></returns>
    public static int LayerMaskToInt(this LayerMask mask)
    {
        return (int)(uint)Mathf.Log((uint)mask.value, 2);
    }

    #endregion

    #region Physics

    /// <summary>
    /// Creates a Bounds encapsulating all given colliders bounds.
    /// </summary>
    /// <param name="colliders">The colliders.</param>
    /// <returns>A Bounds encapsulating all given colliders bounds.</returns>
    public static Bounds CombineColliderBounds(Collider[] colliders)
    {
        var bounds = colliders[0].bounds;

        foreach (var colliderComponent in colliders)
        {
            bounds.Encapsulate(colliderComponent.bounds);
        }

        return bounds;
    }

    /// <summary>
    /// Given a CharacterController and a point of origin (the lower point of the capsule), this returns the
    /// point1, point2 and radius needed to fill a CapsuleCast().
    /// </summary>
    /// <param name="characterController">The CharacterController to use as the capsule, providing scale, radius, height and center offset.</param>
    /// <param name="origin">The capsule cast starting point at the lower end of the capsule.</param>
    /// <param name="point1">Outputs the point1 parameter to be used in the CapsuleCast()</param>
    /// <param name="point2">Outputs the point2 parameter to be used in the CapsuleCast()</param>
    /// <param name="radius">Outputs the radius parameter to be used in the CapsuleCast()</param>
    public static void GetCapsuleCastData(CharacterController characterController, Vector3 origin, out Vector3 point1, out Vector3 point2, out float radius)
    {
        var scale = characterController.transform.lossyScale;
        radius = characterController.radius * scale.x;
        var height = characterController.height * scale.y - (radius * 2);
        var center = characterController.center;
        center.Scale(scale);
        point1 = origin + center + Vector3.down * (height / 2f);
        point2 = point1 + Vector3.up * height;
    }

    public static float CalculateLaunchSpeed(float distance, float yOffset, float gravity, float angle)
    {
        return (distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1 / Mathf.Cos(angle))) / Mathf.Sqrt(2 * distance * Mathf.Sin(angle) + 2 * yOffset * Mathf.Cos(angle));
    }

    #endregion

    #region Camera

    /// <summary>
    /// Calculates the size of the viewport at a given distance from a perspective camera.
    /// </summary>
    /// <param name="camera">The Camera.</param>
    /// <param name="distance">The positive distance from the camera.</param>
    /// <param name="aspectRatio">Optionally: An aspect ratio to use. If 0 is set, camera.aspect is used.</param>
    /// <returns>The size of the viewport at the given distance.</returns>
    public static Vector2 CalculateViewportWorldSizeAtDistance(this Camera camera, float distance, float aspectRatio = 0)
    {
        if (aspectRatio == 0)
        {
            aspectRatio = camera.aspect;
        }

        var viewportHeightAtDistance = 2.0f * Mathf.Tan(0.5f * camera.fieldOfView * Mathf.Deg2Rad) * distance;
        var viewportWidthAtDistance = viewportHeightAtDistance * aspectRatio;

        return new Vector2(viewportWidthAtDistance, viewportHeightAtDistance);
    }

    #endregion

    #region Random

    /// <summary>
    /// Gets a random Vector2 of length 1 pointing in a random direction.
    /// </summary>
    public static Vector2 RandomOnUnitCircle
    {
        get
        {
            var angle = Random.Range(0f, Mathf.PI * 2);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }

    /// <summary>
    /// Returns -1 or 1 with equal change.
    /// </summary>
    public static int RandomSign
    {
        get { return (Random.value < 0.5f) ? -1 : 1; }
    }

    /// <summary>
    /// Returns true or false with equal chance.
    /// </summary>
    public static bool RandomBool
    {
        get { return Random.value < 0.5f; }
    }

    /// <summary>
    /// Returns a random index given each index probability.
    /// </summary>
    public static int RandomInidex(params float[] chances)
    {
        if (chances == null || chances.Length == 0) return -1;
        float tot = 0f;
        float[] ranges = new float[chances.Length];
        for (int i = 0; i < chances.Length; i++)
        {
            tot += chances[i];
            ranges[i] = tot;
        }
        float r = Random.value * tot;
        for (int i = 0; i < ranges.Length; i++)
        {
            if (r <= ranges[i])
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Returns an array of random indices from minIndex to maxIndex without repetitions.
    /// </summary>
    public static int[] RandomIndexArray(int length, int minIndex, int maxIndex)
    {
        if (length <= 0) return new int[0];

        int[] array = new int[length];

        if (maxIndex < minIndex) return array;

        List<int> indices = new();
        for (int i = minIndex; i <= maxIndex; i++)
        {
            indices.Add(i);
        }

        int count = indices.Count;
        for (int i = 0; i < length; i++)
        {
            if (count <= 0) break;

            int r = Random.Range(0, count);

            array[i] = indices[r];

            indices[r] = indices[^1];
            indices.RemoveAt(count - 1);
            count--;
        }

        return array;
    }

    #endregion

    #region Float

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static bool RoughlyEqual(float a, float b, float threshold = 0.001f)
    {
        return (Math.Abs(a - b) < threshold);
    }

    public static float SmootherStep(float t)
    {
        return t * t * t * (t * (t * 6f - 15f) + 10f);
    }

    #endregion

    #region Array

    public static bool HasElements(this Array array)
    {
        return array != null && array.Length > 0;
    }

    public static bool HasElements<T>(this List<T> list)
    {
        return list != null && list.Count > 0;
    }

    public static bool HasIndex(this Array array, int index)
    {
        return array != null && index >= 0 && index < array.Length;
    }

    public static bool HasIndex<T>(this List<T> list, int index)
    {
        return list != null && index >= 0 && index < list.Count;
    }

    // Return a random item from an array.
    public static T RandomElement<T>(this T[] items)
    {
        if (!items.HasElements()) return default;

        if (items.Length == 1) return items[0];

        // Return a random item.
        return items[Random.Range(0, items.Length)];
    }

    public static T RandomElement<T>(this List<T> items)
    {
        if (!items.HasElements()) return default;

        if (items.Count == 1) return items[0];

        // Return a random item.
        return items[Random.Range(0, items.Count)];
    }

    /// <summary>
    /// Removes a random element from a list. Setting keepListOrder to false provides a quicker remove.
    /// </summary>
    public static T RemoveRandomElement<T>(this List<T> items, bool keepListOrder = false)
    {
        int count = items.Count;
        if (items == null || count == 0) return default;

        T item;
        if (count == 1)
        {
            item = items[0];
            items.RemoveAt(0);
            return item;
        }

        int r = Random.Range(0, count);
        item = items[r];

        if (keepListOrder)
        {
            items.RemoveAt(r);
        }
        else
        {
            items[r] = items[count - 1];
            items.RemoveAt(count - 1);
        }

        return item;
    }

    #endregion

    #region String

    public static int[] ToIntArray(this string s, string separator, int fixedLength = 0)
    {
        fixedLength = Math.Max(0, fixedLength);
        if (string.IsNullOrEmpty(s)) return new int[fixedLength];
        var sArr = s.Split(separator);
        int length = fixedLength > 0 ? fixedLength : sArr.Length;
        var result = new int[length];
        for (int i = 0; i < length && i < sArr.Length; i++)
        {
            int.TryParse(sArr[i], out int r);
            result[i] = r;
        }
        return result;
    }

    public static string ToString(this int[] array, string separator)
    {
        if (array == null || array.Length == 0) return "";
        StringBuilder s = new(array[0].ToString());
        for (int i = 1; i < array.Length; i++)
        {
            s.Append(separator + array[i].ToString());
        }
        return s.ToString();
    }

    const string digits = "0123456789";
    /// <summary>
    /// Concatenates integer strings without allocating garbage
    /// </summary>
    public static void AppendNonAlloc(this System.Text.StringBuilder s, int i)
    {
        if (i == 0)
        {
            s.Append('0');
        }
        else if (i < 0)
        {
            s.Append('-');
            R(-i);
        }
        else
        {
            R(i);
        }

        void R(int j)
        {
            if (j <= 0) return;
            int frac = j % 10;
            R((j - frac) / 10);
            s.Append(digits[frac]);
        }
    }

    private static readonly int charA = Convert.ToInt32('a');

    private static readonly Dictionary<int, string> units = new Dictionary<int, string>
            {
                {0, ""},
                {1, "K"},
                {2, "M"},
                {3, "B"},
                {4, "T"}
            };

    public static string FormatNumber(double value, bool twoCases = false)
    {
        if (value < 1d)
        {
            if (twoCases)
            {
                return value.ToString("0.##");
            }

            return value.ToString("0.####");
        }

        var n = (int)Math.Log(value, 1000);
        var m = value / Math.Pow(1000, n);
        string unit;

        if (units.ContainsKey(n))
        {
            unit = units[n];
        }
        else
        {
            var unitInt = n - units.Count;
            var secondUnit = unitInt % 26;
            var firstUnit = unitInt / 26;
            if (firstUnit < 0 || firstUnit >= 26) unit = "_";
            else unit = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();
        }

        // Math.Floor(m * 100) / 100) fixes rounding errors
        return (Math.Floor(m * 100) / 100).ToString("0.##") + unit;
    }

    public static string FormatNumberNoCase(double value)
    {
        if (value < 1d)
        {
            return value.ToString("0");
        }

        var n = (int)Math.Log(value, 1000);
        var m = value / Math.Pow(1000, n);
        string unit;

        if (units.ContainsKey(n))
        {
            unit = units[n];
        }
        else
        {
            var unitInt = n - units.Count;
            var secondUnit = unitInt % 26;
            var firstUnit = unitInt / 26;
            if (firstUnit < 0 || firstUnit >= 26) unit = "_";
            else unit = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();
        }

        if (value > 1000)
        {
            return (Math.Floor(m * 100) / 100).ToString("#.##") + unit;
        }

        int integerNumber = (int)m;
        // Math.Floor(m * 100) / 100) fixes rounding errors
        return (Math.Floor(((decimal)integerNumber) * 100) / 100).ToString("#.#") + unit;
    }

    public static string FormatNumberLessDecial(double value)
    {
        if (value < 1d)
        {
            return value.ToString("0.#");
        }

        var n = (int)Math.Log(value, 1000);
        var m = value / Math.Pow(1000, n);
        string unit;

        if (units.ContainsKey(n))
        {
            unit = units[n];
        }
        else
        {
            var unitInt = n - units.Count;
            var secondUnit = unitInt % 26;
            var firstUnit = unitInt / 26;
            if (firstUnit < 0 || firstUnit >= 26) unit = "_";
            else unit = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();
        }

        // Math.Floor(m * 100) / 100) fixes rounding errors
        return (Math.Floor(m * 100) / 100).ToString("0.#") + unit;
    }

    #endregion

    #region Canvas

    public static Vector2 WorldToCanvasPosition(this Canvas canvas, RectTransform canvasRect, Camera camera, Vector3 position)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(camera, position);
        Vector2 result;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : camera, out result);
        return result;
    }

    #endregion

    #region Log

    public static void Log(string message, Color color, LogType type = LogType.Log)
    {
#if UNITY_EDITOR
        message = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + message + "</color>";
#else
            message = "[MN] " + message;
#endif
        switch (type)
        {
            case LogType.Exception:
            case LogType.Error:
                Debug.LogError(message);
                break;
            case LogType.Assert:
                Debug.LogAssertion(message);
                break;
            case LogType.Warning:
                Debug.LogWarning(message);
                break;
            case LogType.Log:
                Debug.Log(message);
                break;
        }
    }

    #endregion
}
