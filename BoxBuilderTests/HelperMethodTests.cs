using System.Collections.Generic;
using BoxBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BoxBuilderTests
{
    [TestClass]
    public class HelperMethodsTests
    {
        [TestMethod]
        public void FlipHorizontal_CenteredHorizontally()
        {
            List<Point> poly = new List<Point>
            {
                new Point(-2, 2),
                new Point(2, 2),
                new Point(0, -2)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonHorizontally(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(-2, -2),
                new Point(2, -2),
                new Point(0,2)
            };

            foreach(Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach(Point p in rotatedPoly)
                {
                    if(expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }

        [TestMethod]
        public void FlipHorizontal_FromQuadrant1()
        {
            List<Point> poly = new List<Point>
            {
                new Point(2, 5),
                new Point(6, 5),
                new Point(4, 1)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonHorizontally(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(2, 1),
                new Point(6, 1),
                new Point(4, 5)
            };

            foreach (Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach (Point p in rotatedPoly)
                {
                    if (expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }

        [TestMethod]
        public void FlipHorizontal_FromQuadrant2()
        {
            List<Point> poly = new List<Point>
            {
                new Point(2, -5),
                new Point(6, -5),
                new Point(4, -1)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonHorizontally(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(2, -1),
                new Point(6, -1),
                new Point(4, -5)
            };

            foreach (Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach (Point p in rotatedPoly)
                {
                    if (expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }

        [TestMethod]
        public void FlipHorizontal_FromQuadrant3()
        {
            List<Point> poly = new List<Point>
            {
                new Point(-2, -5),
                new Point(-6, -5),
                new Point(-4, -1)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonHorizontally(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(-2, -1),
                new Point(-6, -1),
                new Point(-4, -5)
            };

            foreach (Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach (Point p in rotatedPoly)
                {
                    if (expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }

        [TestMethod]
        public void FlipHorizontal_FromQuadrant4()
        {
            List<Point> poly = new List<Point>
            {
                new Point(-2, 5),
                new Point(-6, 5),
                new Point(-4, 1)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonHorizontally(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(-2, 1),
                new Point(-6, 1),
                new Point(-4, 5)
            };

            foreach (Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach (Point p in rotatedPoly)
                {
                    if (expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }


        [TestMethod]
        public void FlipVertical_CenteredHorizontally()
        {
            List<Point> poly = new List<Point>
            {
                new Point(-2, 2),
                new Point(2, 2),
                new Point(0, -2)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonHorizontally(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(-2, -2),
                new Point(2, -2),
                new Point(0,2)
            };

            foreach (Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach (Point p in rotatedPoly)
                {
                    if (expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }

        [TestMethod]
        public void FlipVertical_FromQuadrant1()
        {
            List<Point> poly = new List<Point>
            {
                new Point(5, 2),
                new Point(5, 6),
                new Point(1, 4)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonVertically(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(1, 2),
                new Point(1, 6),
                new Point(5, 4)
            };
            foreach (Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach (Point p in rotatedPoly)
                {
                    if (expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }

        [TestMethod]
        public void FlipVertical_FromQuadrant2()
        {
            List<Point> poly = new List<Point>
            {
                new Point(5, -2),
                new Point(5, -6),
                new Point(1, -4)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonVertically(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(1, -2),
                new Point(1, -6),
                new Point(5, -4)
            };
            foreach (Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach (Point p in rotatedPoly)
                {
                    if (expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }

        [TestMethod]
        public void FlipVertical_FromQuadrant3()
        {
            List<Point> poly = new List<Point>
            {
                new Point(-5, -2),
                new Point(-5, -6),
                new Point(-1, -4)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonVertically(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(-1, -2),
                new Point(-1, -6),
                new Point(-5, -4)
            };
            foreach (Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach (Point p in rotatedPoly)
                {
                    if (expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }

        [TestMethod]
        public void FlipVertical_FromQuadrant4()
        {
            List<Point> poly = new List<Point>
            {
                new Point(-5, 2),
                new Point(-5, 6),
                new Point(-1, 4)
            };

            var rotatedPoly = BoxBuilder.HelperMethods.FlipPolygonVertically(poly);

            List<Point> ExpectedPoly = new List<Point>
            {
                new Point(-1, 2),
                new Point(-1, 6),
                new Point(-5, 4)
            };

            foreach (Point expectedPoint in ExpectedPoly)
            {
                bool foundPoint = false;

                foreach (Point p in rotatedPoly)
                {
                    if (expectedPoint.X == p.X && expectedPoint.Y == p.Y)
                    {
                        foundPoint = true;
                        break;
                    }
                }

                Assert.IsTrue(foundPoint, "Did not find expected points.");
            }
        }

    }
}
