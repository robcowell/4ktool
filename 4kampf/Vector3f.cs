using System;
using System.ComponentModel;

namespace kampfpanzerin
{
	public class Vector3f
	{
		public float x, y, z;

        public Vector3f()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
        }

        public Vector3f(Vector3f v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

		public Vector3f(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
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
        public float Magnitude()
        {
            return ((float)Math.Sqrt(x * x + y * y + z * z));
        }
    }
}
