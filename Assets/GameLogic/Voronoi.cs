using System;
using System.Collections.Generic;

namespace GameLogic {
 
    public struct Region {
        public Point<double> site; // The site that this region is based on
        public List<Point<double>> vertices; // The vertices of this region
        
        public Region(Point<double> site) {
            this.site = site;
            this.vertices = new List<Point<double>>();
        }
    }
    
    public class Breakpoint {
        public Point<double> Left { get; }
        public Point<double> Right { get; }

        public Breakpoint(Point<double> left, Point<double> right) {
            Left = left;
            Right = right;
        }
    }
    
    public class Voronoi {
        
        private List<Point<double>> points;
        private List<Region> regions;
        private BeachLine beachLine;
        private LoggerInterface logger;

        public Voronoi(List<Point<double>> points, LoggerInterface logger) {
            this.points = points;
            this.regions = new List<Region>();
            this.beachLine = new BeachLine();
            this.logger = logger;
            foreach (Point<double> point in points) {
                regions.Add(new Region(point));
            }
        }

        public List<Region> generate() {
            // Initialize event queue with site events
            PriorityQueue<Event> eventQueue = new PriorityQueue<Event>();
            foreach (var point in points) {
                eventQueue.enqueue(new Event(point, EventType.Site), point.y);
            }

            // Process events
            while (eventQueue.Count > 0) {
                Event e = eventQueue.dequeue();
                if (e.Type == EventType.Site) {
                    handleSiteEvent(e, eventQueue);
                } else {
                    handleCircleEvent(e, eventQueue, logger);
                }
            }

            return regions;
        }

        private void handleSiteEvent(Event e, PriorityQueue<Event> eventQueue) {
            // Create a new region for the site
            Region newRegion = new Region(e.Point);
            regions.Add(newRegion);
            
            if (beachLine.isEmpty()) {
                // Create the initial arc with valid breakpoints
                Breakpoint initialBreakpoint = new Breakpoint(e.Point, e.Point);
                Arc initialArc = new Arc(e.Point, initialBreakpoint, initialBreakpoint);
                beachLine.addArc(initialArc);
                return;
            }

            // Find the arc above the new site
            Arc aboveArc = findArcAbove(e.Point);
            
            if (aboveArc == null) {
                return;
            }

            // Create new breakpoints and arcs
            Breakpoint leftBreakpoint = new Breakpoint(aboveArc.Left, e.Point);
            Breakpoint rightBreakpoint = new Breakpoint(e.Point, aboveArc.Right);
            Arc newArc = new Arc(e.Point, leftBreakpoint, rightBreakpoint);

            // Update the beach line
            aboveArc.Right = e.Point;
            aboveArc.RightBreakpoint = leftBreakpoint;
            newArc.LeftBreakpoint = rightBreakpoint;

            // Add circle events for the new breakpoints
            addCircleEvent(aboveArc.LeftBreakpoint, eventQueue);
            addCircleEvent(newArc.RightBreakpoint, eventQueue);
        }

        private Arc findArcAbove(Point<double> site) {
            // Logic to find the arc above the new site
            // This typically involves traversing the beach line
            return beachLine.findArcAbove(site);
        }

        private void addCircleEvent(Breakpoint breakpoint, PriorityQueue<Event> eventQueue) {
            if (breakpoint == null) {
                return;
            }
            // Logic to add a circle event for the given breakpoint
            // This typically involves calculating the circle center and radius
            Point<double> circleCenter = calculateCircleCenter(breakpoint);
            double radius = calculateCircleRadius(breakpoint, circleCenter);
            Point<double> circleEventPoint = new Point<double> { x = circleCenter.x, y = circleCenter.y - radius };

            // Add the circle event to the event queue
            eventQueue.enqueue(new Event(circleEventPoint, EventType.Circle), circleEventPoint.y);
        }

        private Point<double> calculateCircleCenter(Breakpoint breakpoint) {
            // Logic to calculate the center of the circle formed by the breakpoint
            return new Point<double> { x = (breakpoint.Left.x + breakpoint.Right.x) / 2, y = (breakpoint.Left.y + breakpoint.Right.y) / 2 };
        }

        private double calculateCircleRadius(Breakpoint breakpoint, Point<double> center) {
            // Logic to calculate the radius of the circle formed by the breakpoint
            return Math.Sqrt(Math.Pow(breakpoint.Left.x - center.x, 2) + Math.Pow(breakpoint.Left.y - center.y, 2));
        }

        private void handleCircleEvent(Event e, PriorityQueue<Event> eventQueue, LoggerInterface logger) {
            // Remove the arc associated with the circle event
            Arc arcToRemove = beachLine.findArcAbove(e.Point);
            if (arcToRemove == null) {
                logger.Log("Arc not found");
                return;
            } else {
              // log the arc in Unity
                logger.Log("Arc: " + arcToRemove.Site.x + ", " + arcToRemove.Site.y);
            }

            // Remove the arc from the beachline
            beachLine.removeArc(arcToRemove);

            // Create new breakpoints and arcs for the neighboring arcs
            Breakpoint newLeftBreakpoint = new Breakpoint(arcToRemove.Left, arcToRemove.Right);
            Arc newArc = new Arc(arcToRemove.Left, newLeftBreakpoint, arcToRemove.RightBreakpoint);

            // Update the beachline with the new arc
            beachLine.addArc(newArc);

            // Add new circle events for the neighboring arcs
            addCircleEvent(newArc.LeftBreakpoint, eventQueue);
            addCircleEvent(newArc.RightBreakpoint, eventQueue);
        }
        
        private class BeachLine {
            private List<Arc> arcs;

            public BeachLine() {
                arcs = new List<Arc>();
            }
            
            public bool isEmpty() {
                return arcs.Count == 0;
            }

            public Arc findArcAbove(Point<double> site) {
                // Logic to find the arc directly above the given site
                // This typically involves traversing the list of arcs
                foreach (var arc in arcs) {
                    if (isAbove(arc, site)) {
                        return arc;
                    }
                }
                return null;
            }

            public void addArc(Arc arc) {
                arcs.Add(arc);
            }

            public void removeArc(Arc arc) {
                arcs.Remove(arc);
            }

            private bool isAbove(Arc arc, Point<double> site) {
                // Calculate the directrix of the arc's site
                double directrix = arc.Site.y;

                // Calculate the y-coordinate of the parabola at the x-coordinate of the site
                double parabolaY = (Math.Pow(site.x - arc.Site.x, 2) + Math.Pow(directrix, 2) - Math.Pow(site.y, 2)) / (2 * (directrix - site.y));

                // The site is above the arc if its y-coordinate is less than the parabola's y-coordinate
                return site.y >= parabolaY;
            }
        }

        private class Event {
            public Point<double> Point { get; }
            public EventType Type { get; }

            public Event(Point<double> point, EventType type) {
                Point = point;
                Type = type;
            }
        }

        private enum EventType {
            Site,
            Circle
        }
    }
}