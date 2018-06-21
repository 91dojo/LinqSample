using ExpectedObjects;
using JoeyIsFat;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using NSubstitute;

namespace LinqTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void find_products_that_price_between_200_and_500()
        {
            var products = RepositoryFactory.GetProducts().ToList();
            var actual = products.JoeyWhere(product => product.IsTopSaleProducts());

            var expected = new List<Product>()
            {
                new Product {Id = 2, Cost = 21, Price = 210, Supplier = "Yahoo"},
                new Product {Id = 3, Cost = 31, Price = 310, Supplier = "Odd-e"},
                new Product {Id = 4, Cost = 41, Price = 410, Supplier = "Odd-e"},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_products_that_price_between_200_and_500_by_where()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.Where(p => p.Price > 200 && p.Price < 500 && p.Cost > 30);

            var expected = new List<Product>()
            {
                //new Product {Id = 2, Cost = 21, Price = 210, Supplier = "Yahoo"},
                new Product {Id = 3, Cost = 31, Price = 310, Supplier = "Odd-e"},
                new Product {Id = 4, Cost = 41, Price = 410, Supplier = "Odd-e"},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void ToHttps()
        {
            var urls = RepositoryFactory.GetUrls();
            IEnumerable<string> actual = WithoutLinq.ToHttps(urls);
            var expected = new List<string>
            {
                "https://tw.yahoo.com",
                "https://facebook.com",
                "https://twitter.com",
                "https://github.com",
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void JoeyWhere_JoeySelect()
        {
            var employees = RepositoryFactory.GetEmployees();
            var sqlActual = from e in employees
                            where e.Age < 25
                            select $"{e.Role}:{e.Name}";

            var actual = employees
                .JoeyWhere(e => e.Age < 25)
                .JoeySelect(e => $"{e.Role}:{e.Name}");

            foreach (var titleName in actual)
            {
                Console.WriteLine(titleName);
            }

            var expected = new List<string>()
            {
                "OP:Andy",
                "Engineer:Frank",
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void Take()
        {
            var employees = RepositoryFactory.GetEmployees();
            var act = WithoutLinq.JoeyTake(employees, 2);
            var expected = new List<Employee>
            {
                new Employee {Name = "Joe", Role = RoleType.Engineer, MonthSalary = 100, Age = 44, WorkingYear = 2.6},
                new Employee {Name = "Tom", Role = RoleType.Engineer, MonthSalary = 140, Age = 33, WorkingYear = 2.6},
            };

            expected.ToExpectedObject().ShouldEqual(act.ToList());
        }

        [TestMethod]
        public void Skip()
        {
            var employees = RepositoryFactory.GetEmployees();
            var act = WithoutLinq.JoeySkip(employees, 6);
            var expected = new List<Employee>
            {
                new Employee {Name = "Frank", Role = RoleType.Engineer, MonthSalary = 120, Age = 16, WorkingYear = 2.6},
                new Employee {Name = "Joey", Role = RoleType.Engineer, MonthSalary = 250, Age = 40, WorkingYear = 2.6},
            };

            expected.ToExpectedObject().ShouldEqual(act.ToList());
        }

        [TestMethod]
        public void Paging_GetSum()
        {
            var employees = RepositoryFactory.GetEmployees();
            var act = WithoutLinq.GetSum(employees, 3, x => x.MonthSalary);
            var expected = new int[] { 620, 540, 370 };
            expected.ToExpectedObject().ShouldEqual(act.ToArray());
        }

        [TestMethod]
        public void TakeWhile()
        {
            var employees = RepositoryFactory.GetEmployees();
            var act = WithoutLinq.TakeWhile(employees, 2, x => x.MonthSalary > 150);
            var expected = new List<Employee>
            {
                new Employee {Name = "Kevin", Role = RoleType.Manager, MonthSalary = 380, Age = 55, WorkingYear = 2.6},
                new Employee {Name = "Bas", Role = RoleType.Engineer, MonthSalary = 280, Age = 36, WorkingYear = 2.6},
            };
            expected.ToExpectedObject().ShouldEqual(act.ToList());
        }

        [TestMethod]
        public void SkipWhile()
        {
            var employees = RepositoryFactory.GetEmployees();
            var act = WithoutLinq.SkipWhile(employees, 3, x => x.MonthSalary < 150);
            var expected = new List<Employee>
            {
                new Employee {Name = "Kevin", Role = RoleType.Manager, MonthSalary = 380, Age = 55, WorkingYear = 2.6},
                new Employee {Name = "Bas", Role = RoleType.Engineer, MonthSalary = 280, Age = 36, WorkingYear = 2.6},
                new Employee {Name = "Mary", Role = RoleType.OP, MonthSalary = 180, Age = 26, WorkingYear = 2.6},
                new Employee {Name = "Frank", Role = RoleType.Engineer, MonthSalary = 120, Age = 16, WorkingYear = 2.6},
                new Employee {Name = "Joey", Role = RoleType.Engineer, MonthSalary = 250, Age = 40, WorkingYear = 2.6},
            };
            expected.ToExpectedObject().ShouldEqual(act.ToList());
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var employees = RepositoryFactory.GetEmployees();
            Assert.IsFalse(employees.JoeyAny(e => e.MonthSalary > 500));
        }

        [TestMethod]
        public void TestAll()
        {
            var employees = RepositoryFactory.GetEmployees();
            Assert.IsFalse(employees.JoeyAll(employee => employee.MonthSalary > 200));
        }

        [TestMethod]
        public void TestFindRich()
        {
            var employees = RepositoryFactory.GetEmployees();
            var findRichEmployee = FindRichEmployee(employees);

            Assert.AreEqual("Joey", findRichEmployee.Name);
        }

        [TestMethod]
        public void OneAndOnlyOne()
        {
            var employees = RepositoryFactory.GetEmployees();
            Assert.AreEqual(RoleType.Manager, employees.JoeySingle(employee => employee.Role == RoleType.Manager).Role);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OneAndOnlyOne_Exception()
        {
            var employees = RepositoryFactory.GetEmployees();
            Assert.AreEqual(RoleType.Engineer,
                employees.JoeySingle(employee => employee.Role == RoleType.Engineer).Role);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestFirst()
        {
            var employees = RepositoryFactory.GetEmployees();
            var employee = employees.EltonFirst(e => e.MonthSalary > 500);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void single()
        {
            var employees = RepositoryFactory.GetEmployees();
            var employee = employees.Where(x => x.Role == RoleType.Engineer).SamSingle();
            //Assert.AreEqual(RoleType.Manager,employee.Role);
        }

        [TestMethod]
        public void Distinct_Employees_Role()
        {
            var expected = new List<Employee>
            {
                new Employee {Name = "Joe", Role = RoleType.Engineer, MonthSalary = 100, Age = 44, WorkingYear = 2.6},
                new Employee {Name = "Kevin", Role = RoleType.Manager, MonthSalary = 380, Age = 55, WorkingYear = 2.6},
                new Employee {Name = "Andy", Role = RoleType.OP, MonthSalary = 80, Age = 22, WorkingYear = 2.6},
            };

            var actual = GetDistinct(RepositoryFactory.GetEmployees(), new MyEqualityComparer());
            //var actual = RepositoryFactory.GetEmployees().Distinct(new MyEqualityComparer());

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void DefaultIfEmpty_Test()
        {
            var employees = RepositoryFactory.GetEmployees();
            var youngers = employees.Where(e => e.Age <= 15).luluDefaultEmpty();
            var expected = new List<Employee>()
            {
                new Employee()
                {
                    Name = "Lulu"
                }
            };

            var emptyEmployees = Enumerable.Empty<Employee>();

            expected.ToExpectedObject().ShouldEqual(youngers.ToList());
        }

        [TestMethod]
        public void MyTestMethod_88_100()
        {
            var colorBalls = Enumerable.Repeat(new ColorBall() { Color = Color.Purple }, 3);
            foreach (var colorBall in colorBalls)
            {
                Console.WriteLine(colorBall.Color);
            }
        }

        [TestMethod]
        public void Contains_LuckyBall_Test()
        {
            var colorBalls = RepositoryFactory.GetBalls();
            var luckyBall =
                new ColorBall() { Color = Color.Purple, Prize = 500, Size = "S" };

            Assert.IsTrue(KaContains(colorBalls, luckyBall));
        }

        [TestMethod]
        public void SequenceEquals_Test()
        {
            var balls = RepositoryFactory.GetBalls();
            var anotherBalls = RepositoryFactory.GetAnotherBalls();
            Assert.IsFalse(JrSequenceEqual(balls, anotherBalls, new LuckyBallCompare()));
        }

        [TestMethod]
        public void SequenceEquals_Test2()
        {
            var balls = RepositoryFactory.GetBalls();
            var anotherBalls = RepositoryFactory.GetBalls();
            Assert.IsTrue(JrSequenceEqual(balls, anotherBalls, new LuckyBallCompare()));
        }

        [TestMethod]
        public void SequenceEquals_Test3()
        {
            var balls = RepositoryFactory.GetBalls();
            var anotherBalls = RepositoryFactory.GetEltonBalls();
            Assert.IsFalse(JrSequenceEqual(balls, anotherBalls, new LuckyBallCompare()));
        }

        [TestMethod]
        public void Test_Delegate()
        {
            //var fakeModel = new FakeOrderModel();
            //var sut = fakeModel.GetDeleteFunc();
            var fakeModel = Substitute.For<IOrderModel>();
            Func<Order, bool> sut = null;

            var joeyController = new JoeyController(fakeModel);
            fakeModel.Delete(Arg.Do<Func<Order, bool>>(x => { sut = x; }));
            joeyController.DeleteAdultOrders();


            var adultOrder = new Order { Customer = new Customer { Age = 19 } };
            Assert.IsTrue(sut(adultOrder));

            var youngerOrder = new Order { Customer = new Customer { Age = 11 } };
            Assert.IsFalse(sut(youngerOrder));
        }

        private bool JrSequenceEqual(IEnumerable<ColorBall> balls, IEnumerable<ColorBall> anotherBalls,
            LuckyBallCompare luckyBallCompare)
        {
            //var ballsCollection = balls as ICollection<ColorBall>;
            //var anotherBallsCollection = anotherBalls as ICollection<ColorBall>;
            //if (ballsCollection != null && anotherBallsCollection != null)
            //{
            //    if (ballsCollection.Count != anotherBallsCollection.Count)
            //    {
            //        return false;
            //    }
            //}

            var enumeratorX = balls.GetEnumerator();
            var enumeratorY = anotherBalls.GetEnumerator();

            while (true)
            {
                var nextX = enumeratorX.MoveNext();
                var nextY = enumeratorY.MoveNext();
                if (IsLengthDifferent(nextX, nextY))
                {
                    return false;
                }

                if (IsEnd(nextX))
                {
                    return true;
                }

                if (!luckyBallCompare.Equals(enumeratorX.Current, enumeratorY.Current))
                {
                    return false;
                }
            }

            //var enumeratorX = balls.GetEnumerator();
            //var enumeratorY = anotherBalls.GetEnumerator();

            //while (enumeratorX.MoveNext() && enumeratorY.MoveNext())
            //{
            //    if (!luckyBallCompare.Equals(enumeratorX.Current, enumeratorY.Current))
            //    {
            //        return false;
            //    }
            //}

            //if (enumeratorX.MoveNext() == enumeratorY.MoveNext()) return true;
            //return false;
        }

        private static bool IsEnd(bool nextX)
        {
            return !nextX;
        }

        private static bool IsLengthDifferent(bool nextX, bool nextY)
        {
            return nextX != nextY;
        }

        private bool KaContains(IEnumerable<ColorBall> colorBalls, ColorBall luckyBall)
        {
            var enumerator = colorBalls.GetEnumerator();
            var luckyBallCompare = new LuckyBallCompare();

            while (enumerator.MoveNext())
            {
                if (luckyBallCompare.Equals(enumerator.Current, luckyBall))
                {
                    return true;
                }

                ;
            }

            return false;
            //return colorBalls.Contains(luckyBall, new LuckyBallCompare());
        }

        private IEnumerable<TSource> GetDistinct<TSource>(IEnumerable<TSource> source,
            IEqualityComparer<TSource> myEqualityComparer)
        {
            var enumerator = source.GetEnumerator();
            var comparer = myEqualityComparer ?? EqualityComparer<TSource>.Default;

            var hashSet = new HashSet<TSource>(comparer);
            while (enumerator.MoveNext())
            {
                if (hashSet.Add(enumerator.Current))
                {
                    yield return enumerator.Current;
                }
            }
        }

        private static Employee FindRichEmployee(IEnumerable<Employee> employees)
        {
            return employees.JasonFirstOrDefault(e => e.MonthSalary > 500, new Employee() { Name = "Joey" });
            //var richMan = employees.FirstOrDefault(e => e.MonthSalary > 500);
            //return richMan ?? new Employee() { Name = "Joey" };
            return employees.JoeyFirstOrDefault(e => e.MonthSalary > 500)
                   ?? new Employee() { Name = "Joey" };
        }
    }

    public class FakeOrderModel : IOrderModel
    {
        private Func<Order, bool> _func;

        internal Func<Order, bool> GetDeleteFunc()
        {
            return _func;
        }

        public void Delete(Func<Order, bool> func)
        {
            _func = func;
        }
    }

    internal class LuckyBallCompare : IEqualityComparer<ColorBall>
    {
        public bool Equals(ColorBall x, ColorBall y)
        {
            return x.Color == y.Color && x.Prize == y.Prize && x.Size == y.Size;
        }

        public int GetHashCode(ColorBall obj)
        {
            return 0;
        }
    }

    internal class MyEqualityComparer : IEqualityComparer<Employee>
    {
        public bool Equals(Employee x, Employee y)
        {
            return x.Role == y.Role;
        }

        public int GetHashCode(Employee obj)
        {
            return obj.Role.GetHashCode();
        }
    }
}