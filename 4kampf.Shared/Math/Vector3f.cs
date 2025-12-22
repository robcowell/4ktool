using System;
using System.Globalization;

namespace _4kampf.Shared.Math;

/// <summary>
/// 3D vector class for mathematical operations.
/// Platform-agnostic implementation.
/// </summary>
[Serializable]
public class Vector3f
{
    /// <summary>
    /// Invalid vector marker
    /// </summary>
    [NonSerialized]
    public static readonly Vector3f Invalid = new Vector3f(-666666.0f);

    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    // Legacy property names for compatibility
    public float x
    {
        get => X;
        set => X = value;
    }

    public float y
    {
        get => Y;
        set => Y = value;
    }

    public float z
    {
        get => Z;
        set => Z = value;
    }

    public Vector3f()
        : this(0)
    {
    }

    public Vector3f(string encoded, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.InvariantCulture;
        string val = encoded.Substring(1, encoded.Length - 1);
        string[] components = val.Split(',');
        for (int i = 0; i < components.Length && i < 3; i++)
        {
            this[i] = float.Parse(components[i].Trim(), culture);
        }
    }

    public Vector3f(float initValue)
        : this(initValue, initValue, initValue)
    {
    }

    public Vector3f(Vector3f v)
        : this(v.X, v.Y, v.Z)
    {
    }

    public Vector3f(float x, float y, float z)
    {
        Set(x, y, z);
    }

    public void Set(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void Set(Vector3f that)
    {
        X = that.X;
        Y = that.Y;
        Z = that.Z;
    }

    public float Dot(Vector3f b)
    {
        return (X * b.X + Y * b.Y + Z * b.Z);
    }

    public void Normalise()
    {
        float magnitude = Magnitude();
        if (magnitude > 0.0001f)
        {
            float f = 1.0f / magnitude;
            X *= f;
            Y *= f;
            Z *= f;
        }
    }

    public override string ToString()
    {
        return $"{{{X},{Y},{Z}}}";
    }

    public string ToString(string format, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        return $"{{{X.ToString(format, culture)},{Y.ToString(format, culture)},{Z.ToString(format, culture)}}}";
    }

    public static float Dot(Vector3f a, Vector3f b)
    {
        return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
    }

    public static Vector3f CrossProduct(Vector3f v1, Vector3f v2)
    {
        Vector3f v = new Vector3f();

        v.X = v1.Y * v2.Z - v1.Z * v2.Y;
        v.Y = v1.Z * v2.X - v1.X * v2.Z;
        v.Z = v1.X * v2.Y - v1.Y * v2.X;

        return v;
    }

    public Vector3f Clone()
    {
        return new Vector3f(X, Y, Z);
    }

    public static Vector3f operator +(Vector3f a, Vector3f b)
    {
        return new Vector3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3f operator -(Vector3f a, Vector3f b)
    {
        return new Vector3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    }

    public static Vector3f operator *(Vector3f a, Vector3f b)
    {
        return new Vector3f(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
    }

    public static Vector3f operator -(Vector3f a, float b)
    {
        return new Vector3f(a.X - b, a.Y - b, a.Z - b);
    }

    public static Vector3f operator +(Vector3f a, float b)
    {
        return new Vector3f(a.X + b, a.Y + b, a.Z + b);
    }

    public static Vector3f operator *(Vector3f a, float b)
    {
        return new Vector3f(a.X * b, a.Y * b, a.Z * b);
    }

    public static Vector3f operator /(Vector3f a, float b)
    {
        return new Vector3f(a.X / b, a.Y / b, a.Z / b);
    }

    public float this[int i]
    {
        get
        {
            return i switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new IndexOutOfRangeException("Index must be 0-2")
            };
        }
        set
        {
            switch (i)
            {
                case 0:
                    X = value;
                    break;
                case 1:
                    Y = value;
                    break;
                case 2:
                    Z = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Index must be 0-2");
            }
        }
    }

    public float Magnitude()
    {
        return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
    }

    public float Max()
    {
        return Math.Max(X, Math.Max(Y, Z));
    }

    public float Min()
    {
        return Math.Min(X, Math.Min(Y, Z));
    }
}

