using Diagonal;

namespace DiagTest
{
    [TestClass]
    public class DiagTest
    {
        [TestMethod]
        public void Create()
        {
            Assert.ThrowsException<Diag.NegativeSizeException>(() => _ = new Diag(0));
            Diag a = new(1);
            Assert.AreEqual(a[0, 0], 0);//default value will be 0
            Assert.AreEqual(a.Size, 1);
            Diag b = new(2);
            Assert.AreEqual(b.Size, 2);
            Diag c = new(5);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == j || i + j == 4) // Primary and secondary diagonal elements
                    {
                        Assert.AreEqual(c[i, j], default(int));
                    }
                    else
                    {
                        Assert.AreEqual(c[i, j], 0); // Any other elements should be default(int)
                    }
                }
            }
            Assert.AreEqual(c.Size, 5);
            Assert.ThrowsException<Diag.NegativeSizeException>(() => _ = new Diag(-1));
            Diag d = new Diag(1000);
            Assert.AreEqual(d.Size, 1000);
        }

        [TestMethod]
        public void Change()
        {
            Diag c = new(3);
            c[0, 0] = 1;
            c[1, 1] = 1;
            c[2, 2] = 1;
            c[0, 2] = 5;
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(c[i, i], 1);
            }

            Assert.AreEqual(c[0,  1], 0);
            Assert.ThrowsException<Diag.ReferenceToNullPartException>(() => c[1, 0] = 3);
        }

        [TestMethod]
        public void Assignment()
        {
            Diag a = new Diag(3);
            a[0, 0] = 1;
            a[1, 1] = 2;
            a[2, 2] = 3;
            a[0, 2] = 4;
            a[2, 0] = 5;
            //copy
            Diag b = new Diag(a);
            Diag c = new (2);
            c = a;
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals(c));
            b[0, 0] = 0;
            //3 a
            Assert.IsFalse(a.Equals(b));
            c[0, 0] = 0;
            Assert.IsTrue(a.Equals(b));
            a = b = c;
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals(c));
            a = a;
            Assert.IsTrue(a.Equals(a));
        }


        [TestMethod]
        public void Add()
        {
            Diag a = new(3);
            Diag b = new(3);
            Diag zero = new(3);
            Diag d = new(2);
            Diag c;

            a[0, 0] = 1;
            a[1, 1] = 1;
            a[2, 2] = 1;

            b[0, 0] = 42;
            b[1, 1] = 0;
            b[2, 2] = 0;

            c = a + b;

            Assert.AreEqual(c[0, 0], 43);
            Assert.AreEqual(c[1, 1], 1);
            Assert.IsTrue(a.Equals(a + zero));
            Assert.IsTrue(a.Equals(zero + a));
            Assert.IsTrue((a + b).Equals(b + a));
            Assert.IsTrue(((a + b) + c).Equals(a + (b + c)));
            Assert.ThrowsException<Diag.DifferentSizeException>(() => a + d);
        }

        [TestMethod]
        public void Mul()
        {
            Diag a = new(3);
            Diag b = new(3);
            Diag d = new(2);
            Diag zero = new(3);
            Diag identity = new(3);
            Diag c;

            identity[0, 0] = 1;
            identity[1, 1] = 1;
            identity[2, 2] = 1;

            a[0, 0] = 1;
            a[1, 1] = 1;
            a[0, 2] = 1;
            a[2, 0] = 7;
            a[2, 2] = 1;

            b[0, 0] = 42;
            b[1, 1] = 3;
            b[2, 2] = 4;
            b[0, 2] = 1;
            b[2, 0] = 1;

            c = a * b;

            Assert.AreEqual(c[0, 0], 43);
            Assert.AreEqual(c[1, 1], 3);
            Assert.IsTrue(zero.Equals(a * zero));
            Assert.IsTrue(a.Equals(identity* a));
            Assert.IsFalse(b.Equals(a * b));
            Assert.IsTrue((a * (b * c)).Equals((a * b) * c));
            Assert.IsFalse((b * c).Equals(c * b));

            Assert.ThrowsException<Diag.DifferentSizeException>(() => a * d);
        }

        [TestMethod]
        public void SetMatrix()
        {
            List<double> vec = new List<double>() { 1, 2, 3,5 };
            List<double> vec2 = new List<double>() { 3,2,7,8 };
            Diag a = new (4);
            Diag b = new (2);
            Assert.AreEqual(a[0, 0], 0);
            Assert.AreEqual(a[1, 1], 0);
            Assert.AreEqual(a[2, 2], 0);
            a.Set(vec,vec2);
            Assert.AreEqual(a[0, 0], 1);
            Assert.AreEqual(a[1, 1], 2);
            Assert.AreEqual(a[2, 2], 3);
            Assert.AreEqual(a[0, 2], 0);
            Assert.AreEqual(a[2, 0], 0);
            Assert.AreEqual(a[0, 3], 3);

            Assert.ThrowsException<Diag.DifferentSizeException>(() => b.Set(vec,vec2));
        }
    }
}