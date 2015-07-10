using System;
using System.ComponentModel;

namespace kampfpanzerin
{
    [Serializable]
	public class Vector3f
	{

        [NonSerialized]
        public static readonly Vector3f INVALID = new Vector3f(-666666.0f);

		public float x, y, z;

        public Vector3f()
            : this(0) {
        }
        public Vector3f(string encoded, System.Globalization.CultureInfo culture) {
            string val = encoded.Substring(1, encoded.Length - 1);
            string[] components = val.Split(new char[] { ',' });
            for (int i = 0; i < components.Length; i++) {
                this[i] = float.Parse(components[i], culture);
            }
        }

        public Vector3f(float initValue)
            : this(initValue, initValue, initValue) {
        }

        public Vector3f(Vector3f v)
            : this(v.x, v.y, v.z) {
        }

		public Vector3f(float x, float y, float z)
		{
            Set(x, y, z);
		}

        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Set(Vector3f that) {
            this.x = that.x;
            this.y = that.y;
            this.z = that.z;
        }

		public float Dot(Vector3f b)
		{
			return (x*b.x + y*b.y + z*b.z);
		}

		public void Normalise()
		{
			float f = (float)(1.0f / Math.Sqrt(Dot(this, this)));

			x *= f;
			y *= f;
			z *= f;
		}

        public override string ToString()
        {
            return "{"+x+","+y+","+z+"}";
        }
            
        public static float Dot(Vector3f a, Vector3f b)
		{
			return (a.x * b.x + a.y * b.y + a.z * b.z);
		}

		public static Vector3f CrossProduct(Vector3f v1, Vector3f v2)
		{
			Vector3f v = new Vector3f();

			v.x = v1.y * v2.z - v1.z * v2.y;
			v.y = v1.z * v2.x - v1.x * v2.z;
			v.z = v1.x * v2.y - v1.y * v2.x;

			return v;
		}

		public Vector3f Clone()
		{
			return new Vector3f(x, y, z);
		}

        public static Vector3f operator +(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3f operator -(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3f operator *(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3f operator -(Vector3f a, float b) {
            return new Vector3f(a.x - b, a.y - b, a.z - b);
        }

        public static Vector3f operator +(Vector3f a, float b) {
            return new Vector3f(a.x + b, a.y + b, a.z + b);
        }

        public static Vector3f operator *(Vector3f a, float b)
        {
            return new Vector3f(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3f operator /(Vector3f a, float b) {
            return new Vector3f(a.x / b, a.y / b, a.z / b);
        }

        public float this [int i] {
            get {
                switch (i) {
                    case 0: return this.x;
                    case 1: return this.y;
                    case 2: return this.z;
                    default: throw new IndexOutOfRangeException("Index must be 0-2");
                }
            }
            set {
                switch (i) {
                    case 0:
                        this.x = value;
                        break;
                    case 1: 
                        this.y = value;
                        break;
                    case 2: 
                        this.z = value;
                        break;
                    default: throw new IndexOutOfRangeException("Index must be 0-2");
                }
            }
        }

        public float Magnitude()
        {
            return ((float)Math.Sqrt(x * x + y * y + z * z));
        }

        public float Max() {
            return Math.Max(x, Math.Max(y, z));
        }

        public float Min() {
            return Math.Min(x, Math.Min(y, z));
        }

        public string ToString(string p, System.Globalization.CultureInfo culture = null) {
            if (culture == null) {
                culture = System.Globalization.CultureInfo.CurrentCulture;
            }
            return "{" + x.ToString(p, culture) + "," + y.ToString(p, culture) + "," + z.ToString(p, culture) + "}";
        }
    }
}
