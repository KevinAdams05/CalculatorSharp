using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Calculator
{
    //This class contains all the test cases I made for NUnit
    //If you have NUnit installed you can debug easily by seeing if any test cases become broken

    [TestFixture]
    public class Test_Appending
    {
        [Test]
        public void appendDigit_0_9()
        {
            clearCalcEngine();
            for (int i = 9; i >= 0; i--)
            {
                CalcEngine.AppendNum(i);
                Assert.AreEqual(i, CalcEngine.GetDisplay());
                clearCalcEngine();
            }
        }
        [Test]
        public void appendDigit_multiple()
        {
            clearCalcEngine();
            for (int i = 9; i >= 0; i--)
            {
                CalcEngine.AppendNum(i);
            }
            Assert.AreEqual(9876543210, CalcEngine.GetDisplay());
            clearCalcEngine();
        }
        [Test]
        public void appendDigit_decimal()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(8);
            CalcEngine.AppendNum(4);
            CalcEngine.other_fcns("decimal");
            CalcEngine.AppendNum(2);
            CalcEngine.AppendNum(5);
            Assert.AreEqual(84.25, CalcEngine.GetDisplay());
            clearCalcEngine();
        }
        [Test]
        public void appendNum_multiple()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(829);
            Assert.AreEqual(829, CalcEngine.GetDisplay());
            CalcEngine.AppendNum(204);
            Assert.AreEqual(829204, CalcEngine.GetDisplay());
            clearCalcEngine();
            CalcEngine.AppendNum(5392.634);
            Assert.AreEqual(5392.634, CalcEngine.GetDisplay());
            clearCalcEngine();
        }
        [Test]
        public void appendNum_decimal()
        {
            clearCalcEngine();
            //Try doing two decimals at once
            CalcEngine.AppendNum(84.523);
            Assert.AreEqual(84.523, CalcEngine.GetDisplay());
            CalcEngine.AppendNum(678.935); //Should replae the other one
            Assert.AreEqual(678.935, CalcEngine.GetDisplay());
            clearCalcEngine();
            //Now try agian after doing a normal append
            CalcEngine.AppendNum(8);
            CalcEngine.AppendNum(4);
            CalcEngine.other_fcns("decimal");
            CalcEngine.AppendNum(2);
            CalcEngine.AppendNum(5);
            Assert.AreEqual(84.25, CalcEngine.GetDisplay());
            CalcEngine.AppendNum(678.935); //Should replae the other one
            Assert.AreEqual(678.935, CalcEngine.GetDisplay());
            clearCalcEngine();
            //Now try doing before a normal append (should just append to decimal)
            CalcEngine.AppendNum(678.935); //Should replae the other one
            Assert.AreEqual(678.935, CalcEngine.GetDisplay());
            CalcEngine.AppendNum(8);
            CalcEngine.AppendNum(4);
            CalcEngine.other_fcns("decimal");
            CalcEngine.AppendNum(2);
            CalcEngine.AppendNum(5);
            Assert.AreEqual(678.9358425, CalcEngine.GetDisplay());
            clearCalcEngine();
        }
        public void clearCalcEngine()
        {
            CalcEngine.ClearAll();
            Assert.IsTrue(CalcEngine.AssureCleared());
        }
    }
    [TestFixture]
    public class Test_Memory
    {
        [Test]
        public void clearAll()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(34.223);
            CalcEngine.PrepareOperation("+");
            CalcEngine.other_fcns("switchSign");
            clearCalcEngine();
            CalcEngine.other_fcns("open_paren");
            CalcEngine.AppendNum(34.223);
            CalcEngine.PrepareOperation("add");
            clearCalcEngine();
        }
        [Test]
        public void storeMemory()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(526.42);
            CalcEngine.Memory("memStore");
            Assert.IsTrue(526.42-Math.Abs(Convert.ToDouble(CalcEngine.m_memory)) < 0.000000001);
            clearCalcEngine();
        }
        [Test]
        public void recallMemory()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(526.42);
            CalcEngine.Memory("memStore");
            clearCalcEngine();
            CalcEngine.Memory("memRecall");
            Assert.IsTrue(526.42 - Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
        }
        [Test]
        public void sumToMemory()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(526.42);
            CalcEngine.Memory("memStore");
            clearCalcEngine();
            CalcEngine.Memory("memRecall");
            Assert.IsTrue(526.42 - Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
            CalcEngine.AppendNum(35.56);
            CalcEngine.Memory("memAdd");
            Assert.IsTrue((526.42 + 35.56) - (Convert.ToDouble(CalcEngine.m_memory) + CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
        }
        [Test]
        public void clearMemory()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(526.42);
            CalcEngine.Memory("memStore");
            clearCalcEngine();
            Assert.IsTrue(526.42 - Math.Abs(Convert.ToDouble(CalcEngine.m_memory)) < 0.000000001);
            CalcEngine.Memory("memClear");
            Assert.IsNull(CalcEngine.m_memory);
            clearCalcEngine();
        }
        public void clearCalcEngine()
        {
            CalcEngine.ClearAll();
            Assert.IsTrue(CalcEngine.AssureCleared());
        }
    }
    [TestFixture]
    public class Test_TrigFunctions
    {
         
        [Test]
        public void trig_SIN()
        {
            clearCalcEngine();
            CalcEngine.m_useRadians = true;
            Assert.IsTrue(CalcEngine.m_useRadians);
            CalcEngine.AppendNum(55);
            CalcEngine.trig_fcns("sin");
            Assert.IsTrue(Math.Sin(55) - Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
            clearCalcEngine();
            CalcEngine.m_useRadians = false;
            Assert.IsFalse(CalcEngine.m_useRadians);
            CalcEngine.AppendNum(55);
            CalcEngine.trig_fcns("sin");
            Assert.IsTrue(Math.Sin(55 * Math.PI / 180) - Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
        }
        [Test]
        public void trig_COS()
        {
            clearCalcEngine();
            CalcEngine.m_useRadians = true;
            Assert.IsTrue(CalcEngine.m_useRadians);
            CalcEngine.AppendNum(55);
            CalcEngine.trig_fcns("cos");
            Assert.IsTrue(Math.Cos(55) - Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
            clearCalcEngine();
            CalcEngine.m_useRadians = false;
            Assert.IsFalse(CalcEngine.m_useRadians);
            CalcEngine.AppendNum(55);
            CalcEngine.trig_fcns("cos");
            Assert.IsTrue(Math.Cos(55 * Math.PI / 180) - Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
        }
        [Test]
        public void trig_TAN()
        {
            clearCalcEngine();
            CalcEngine.m_useRadians = true;
            Assert.IsTrue(CalcEngine.m_useRadians);
            CalcEngine.AppendNum(55);
            CalcEngine.trig_fcns("tan");
            Assert.IsTrue(Math.Tan(55) - Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
            clearCalcEngine();
            CalcEngine.m_useRadians = false;
            Assert.IsFalse(CalcEngine.m_useRadians);
            CalcEngine.AppendNum(55);
            CalcEngine.trig_fcns("tan");
            Assert.IsTrue(Math.Tan(55 * Math.PI / 180) - Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
        }
        [Test]
        public void trig_PI()
        {
            clearCalcEngine();
            CalcEngine.trig_fcns("pi");
            Assert.IsTrue(Math.PI-Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
            CalcEngine.AppendNum(234);
            Assert.IsTrue(Math.PI - Math.Abs(CalcEngine.GetDisplay()) < 0.000000001);
            clearCalcEngine();
        }
        public void clearCalcEngine()
        {
            CalcEngine.ClearAll();
            Assert.IsTrue(CalcEngine.AssureCleared());
        }
    }
    [TestFixture]
    public class Test_MathFunctions
    {
        //Tests both postive and negative numbers for math functions
        //Tests consecutive presses of the solve button
        //Tests a change of operation to anything besides current one
        [Test]
        public void math_ADD()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(83);
            CalcEngine.PrepareOperation("add");
            CalcEngine.AppendNum(17);
            CalcEngine.Solve();
            Assert.AreEqual(100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(117, CalcEngine.GetDisplay());
            //If you don't hit the add button again after hitting '=', then it assumes you're typing a new number
            CalcEngine.PrepareOperation("add");
            //Now you may append a numbers
            CalcEngine.AppendNum(3);
            CalcEngine.PrepareOperation("subtract");
            //Make sure display got updated after changing operation
            Assert.AreEqual(120, CalcEngine.GetDisplay());
            clearCalcEngine();
            //Now try every combination of negative values
            //-+
            CalcEngine.AppendNum(-83);
            CalcEngine.PrepareOperation("add");
            CalcEngine.AppendNum(13);
            CalcEngine.Solve();
            Assert.AreEqual(-70, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(-57, CalcEngine.GetDisplay());
            clearCalcEngine();
            //+-
            CalcEngine.AppendNum(83);
            CalcEngine.PrepareOperation("add");
            CalcEngine.AppendNum(-13);
            CalcEngine.Solve();
            Assert.AreEqual(70, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(57, CalcEngine.GetDisplay());
            clearCalcEngine();
            //--
            CalcEngine.AppendNum(-83);
            CalcEngine.PrepareOperation("add");
            CalcEngine.AppendNum(-17);
            CalcEngine.Solve();
            Assert.AreEqual(-100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(-117, CalcEngine.GetDisplay());
            clearCalcEngine();
        }
        [Test]
        public void math_SUBTRACT()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(83);
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(3);
            CalcEngine.Solve();
            Assert.AreEqual(80, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(77, CalcEngine.GetDisplay());
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(7);
            CalcEngine.PrepareOperation("add");
            Assert.AreEqual(70, CalcEngine.GetDisplay());
            clearCalcEngine();
            //-+
            CalcEngine.AppendNum(-83);
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(17);
            CalcEngine.Solve();
            Assert.AreEqual(-100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(-117, CalcEngine.GetDisplay());
            clearCalcEngine();
            //+-
            CalcEngine.AppendNum(83);
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(-17);
            CalcEngine.Solve();
            Assert.AreEqual(100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(117, CalcEngine.GetDisplay());
            clearCalcEngine();
            //--
            CalcEngine.AppendNum(-83);
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(-13);
            CalcEngine.Solve();
            Assert.AreEqual(-70, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(-57, CalcEngine.GetDisplay());
            clearCalcEngine();
        }
        [Test]
        public void math_MULTIPLY()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(10);
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(5);
            CalcEngine.Solve();
            Assert.AreEqual(50, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(250, CalcEngine.GetDisplay());
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(2);
            CalcEngine.PrepareOperation("divide");
            Assert.AreEqual(500, CalcEngine.GetDisplay());
            clearCalcEngine();
            //-+
            CalcEngine.AppendNum(-20);
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(5);
            CalcEngine.Solve();
            Assert.AreEqual(-100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(-500, CalcEngine.GetDisplay());
            clearCalcEngine();
            //+-
            CalcEngine.AppendNum(20);
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(-5);
            CalcEngine.Solve();
            Assert.AreEqual(-100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(500, CalcEngine.GetDisplay());
            clearCalcEngine();
            //--
            CalcEngine.AppendNum(-20);
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(-5);
            CalcEngine.Solve();
            Assert.AreEqual(100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(-500, CalcEngine.GetDisplay());
            clearCalcEngine();
        }
        [Test]
        public void math_DIVIDE()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(500);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(2);
            CalcEngine.Solve();
            Assert.AreEqual(250, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(125, CalcEngine.GetDisplay());
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(0.5);
            CalcEngine.PrepareOperation("multiply");
            Assert.AreEqual(250, CalcEngine.GetDisplay());
            clearCalcEngine();
            //-+
            CalcEngine.AppendNum(-500);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(5);
            CalcEngine.Solve();
            Assert.AreEqual(-100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(-20, CalcEngine.GetDisplay());
            clearCalcEngine();
            //+-
            CalcEngine.AppendNum(500);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(-5);
            CalcEngine.Solve();
            Assert.AreEqual(-100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(20, CalcEngine.GetDisplay());
            clearCalcEngine();
            //--
            CalcEngine.AppendNum(-500);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(-5);
            CalcEngine.Solve();
            Assert.AreEqual(100, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(-20, CalcEngine.GetDisplay());
            clearCalcEngine();
            //Check divide by Zero
            CalcEngine.AppendNum(5);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(0);
            Assert.IsFalse(CalcEngine.Solve());
        }
        [Test]
        public void math_DIF_OPERATIONS()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(3);
            CalcEngine.PrepareOperation("add");
            CalcEngine.AppendNum(5);
            CalcEngine.PrepareOperation("multiply");
            Assert.AreEqual(8, CalcEngine.GetDisplay());
            CalcEngine.AppendNum(3);
            CalcEngine.Solve();
            Assert.AreEqual(24, CalcEngine.GetDisplay());
            CalcEngine.PrepareOperation("divide");
            Assert.AreEqual(24, CalcEngine.GetDisplay());
            CalcEngine.AppendNum(6);
            CalcEngine.PrepareOperation("subtract");
            Assert.AreEqual(4, CalcEngine.GetDisplay());
            CalcEngine.AppendNum(1.5);
            CalcEngine.Solve();
            Assert.AreEqual(2.5, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(1, CalcEngine.GetDisplay());
            CalcEngine.PrepareOperation("add");
            Assert.AreEqual(1, CalcEngine.GetDisplay());
            CalcEngine.AppendNum(-3);
            CalcEngine.Solve();
            Assert.AreEqual(-2, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(-5, CalcEngine.GetDisplay());
            clearCalcEngine();
        }

        public void clearCalcEngine()
        {
            CalcEngine.ClearAll();
            Assert.IsTrue(CalcEngine.AssureCleared());
        }
    }
    [TestFixture]
    public class Test_MiscFunctions
    {
        [Test]
        public void misc_SQRT()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(25);
            CalcEngine.other_fcns("sqrt");
            Assert.IsTrue(5 == CalcEngine.GetDisplay());
            CalcEngine.other_fcns("sqrt");
            Assert.IsTrue(Math.Sqrt(5) - Math.Abs(CalcEngine.GetDisplay()) < 0.0000001);
            clearCalcEngine();
            CalcEngine.AppendNum(-3);
            Assert.IsFalse(CalcEngine.other_fcns("sqrt"));
            clearCalcEngine();
        }
        [Test]
        public void misc_PERCENT()
        {
            //(+)
            clearCalcEngine();
            CalcEngine.AppendNum(50);
            CalcEngine.PrepareOperation("add");
            CalcEngine.AppendNum(25);
            CalcEngine.other_fcns("percent");
            Assert.AreEqual(12.5, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(62.5, CalcEngine.GetDisplay());
            //(-)
            clearCalcEngine();
            CalcEngine.AppendNum(50);
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(25);
            CalcEngine.other_fcns("percent");
            Assert.AreEqual(12.5, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(37.5, CalcEngine.GetDisplay());
            //(*)
            clearCalcEngine();
            CalcEngine.AppendNum(50);
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(25);
            CalcEngine.other_fcns("percent");
            Assert.AreEqual(12.5, CalcEngine.GetDisplay());
            CalcEngine.other_fcns("percent");
            Assert.AreEqual(6.25, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(312.5, CalcEngine.GetDisplay());
            //(/)
            clearCalcEngine();
            CalcEngine.AppendNum(50);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(25);
            CalcEngine.other_fcns("percent");
            Assert.AreEqual(12.5, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(4, CalcEngine.GetDisplay());
            clearCalcEngine();
        }
        [Test]
        public void misc_INVERSE()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(5);
            CalcEngine.other_fcns("inverse");
            Assert.IsTrue(1 / 5 - Math.Abs(CalcEngine.GetDisplay()) < 0.0000001);
            clearCalcEngine();
            CalcEngine.AppendNum(0);
            Assert.IsFalse(CalcEngine.other_fcns("inverse"));
            clearCalcEngine();
        }
        [Test]
        public void misc_BACKSPACE()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(45352.743);
            CalcEngine.other_fcns("backspace");
            Assert.AreEqual(45352.74, CalcEngine.GetDisplay());
            CalcEngine.other_fcns("backspace");
            Assert.AreEqual(45352.7, CalcEngine.GetDisplay());
            CalcEngine.other_fcns("backspace");
            Assert.AreEqual(45352, CalcEngine.GetDisplay());
            Assert.IsTrue(CalcEngine.m_decimal);
            CalcEngine.other_fcns("backspace");
            Assert.AreEqual(45352, CalcEngine.GetDisplay());
            Assert.IsFalse(CalcEngine.m_decimal);
            CalcEngine.other_fcns("backspace");
            Assert.AreEqual(4535, CalcEngine.GetDisplay());
            CalcEngine.other_fcns("backspace");
            Assert.AreEqual(453, CalcEngine.GetDisplay());
            CalcEngine.other_fcns("backspace");
            Assert.AreEqual(45, CalcEngine.GetDisplay());
            CalcEngine.other_fcns("backspace");
            Assert.AreEqual(4, CalcEngine.GetDisplay());
            CalcEngine.other_fcns("backspace");
            Assert.AreEqual(0, CalcEngine.GetDisplay());
            CalcEngine.other_fcns("backspace");
            Assert.IsFalse(CalcEngine.other_fcns("backspace"));
            clearCalcEngine();
        }
        [Test]
        public void misc_PARENTHESES()
        {
            clearCalcEngine();
            CalcEngine.other_fcns("open_paren");
            Assert.IsTrue(CalcEngine.m_openParen);
            Assert.IsFalse(CalcEngine.m_closeParen);
            CalcEngine.AppendNum(1);
            CalcEngine.PrepareOperation("add");
            CalcEngine.AppendNum(-2);
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(88);
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(54);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(36);
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(21);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(2);
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(-13);
            CalcEngine.other_fcns("close_paren");
            Assert.IsTrue(CalcEngine.m_closeParen);
            Assert.IsFalse(CalcEngine.m_openParen);
            Assert.AreEqual(-40,CalcEngine.GetDisplay());
            //Try Again
            CalcEngine.other_fcns("open_paren");
            Assert.IsTrue(CalcEngine.m_openParen);
            Assert.IsFalse(CalcEngine.m_closeParen);
            CalcEngine.AppendNum(1);
            CalcEngine.PrepareOperation("add");
            CalcEngine.AppendNum(-2);
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(88);
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(54);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(36);
            CalcEngine.PrepareOperation("subtract");
            CalcEngine.AppendNum(21);
            CalcEngine.PrepareOperation("divide");
            CalcEngine.AppendNum(2);
            CalcEngine.PrepareOperation("multiply");
            CalcEngine.AppendNum(-13);
            CalcEngine.other_fcns("close_paren");
            Assert.IsTrue(CalcEngine.m_closeParen);
            Assert.IsFalse(CalcEngine.m_openParen);
            Assert.AreEqual(-40, CalcEngine.GetDisplay());
            clearCalcEngine();
        }
        [Test]
        public void misc_clearEntry()
        {
            clearCalcEngine();
            CalcEngine.AppendNum(83);
            CalcEngine.PrepareOperation("add");
            CalcEngine.AppendNum(17);
            CalcEngine.clear();
            CalcEngine.AppendNum(27); //Chagne 17 to 27
            CalcEngine.Solve();
            Assert.AreEqual(110, CalcEngine.GetDisplay());
            CalcEngine.Solve();
            Assert.AreEqual(137, CalcEngine.GetDisplay());
            clearCalcEngine();
        }

        public void clearCalcEngine()
        {
            CalcEngine.ClearAll();
            Assert.IsTrue(CalcEngine.AssureCleared());
        }
    }
}
