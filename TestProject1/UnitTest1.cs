using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PSI_Joyce
{
    [TestClass]
    public class UnitTest1
    {
        static byte[] tab = { 20, 0, 0, 0 };
        MyImage test = new MyImage(@"../../../../Probleme_info_Joyce/bin/Debug/net5.0/Test002.bmp");

        QRCode code = new QRCode("HELLO WORLD");
        QRCode code2 = new("HELLO WORLD HELLO WORLD HELLO");

        #region TD2

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(20, MyImage.EndianToInt(tab));
        }

        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(Program.AfficheTableau(tab), Program.AfficheTableau(MyImage.IntToEndian(20, 4)));
        }

        [TestMethod]
        public void TestMethod3()
        {
            Assert.AreEqual(255, test.Image[10, 1].Red); //[x, y]
        }

        [TestMethod]
        public void TestMethod4()
        {
            Assert.AreEqual(20, test.Header.Width);
        }

        #endregion

        #region QR Code

        [TestMethod]
        public void TestMethod5()
        {
            //string test = "HELLO WORLD";
            string[] expected = { "HE", "LL", "O ", "WO", "RL", "D" };
            Assert.AreEqual(Program.AfficheTableau(expected), Program.AfficheTableau(code.GetPairs()));
        }

        [TestMethod]
        public void TestMethod6()
        {
            Assert.AreEqual(17, QRCode.FindIndex('H'));
        }

        [TestMethod]
        public void TestMethod7()
        {
            Assert.AreEqual("01100001011", QRCode.Binary("HE"));
        }

        [TestMethod]
        public void TestMethod8()
        {
            Assert.AreEqual("001101", QRCode.Binary("D"));
        }

        [TestMethod]
        public void TestMethod9()
        {
            MyImage test2 = new(@"../../../../Probleme_info_Joyce/bin/Debug/net5.0/lac.bmp");
            MyImage resTest2 = new(test2.Scale(1.28f));
            Assert.AreEqual(1024, resTest2.Header.Width);
        }

        [TestMethod]
        public void TestMethod10()
        {
            string expected = "0010000001011011000010110111100011010001011100101101110001001101010000110100000011101100000100011110110000010001111011000001000111101100000100011110110011010001111011111100010011001111010011101100001101101101";
            Assert.AreEqual(expected, code.BitChain);
        }

        [TestMethod]
        public void TestMethod11()
        {
            Assert.AreEqual(208, code.BitChain.Length);
        }

        #endregion

    }
}
