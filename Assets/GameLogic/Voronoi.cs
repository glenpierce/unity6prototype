using System;
using System.Collections.Generic;

namespace GameLogic {
    
    public struct Point {
        public double x, y;
    }
 
    public struct Region {
        public Point site; // The site that this region is based on
        public List<Point> vertices; // The vertices of this region
        
        public Region(Point site) {
            this.site = site;
            this.vertices = new List<Point>();
        }
    }
    
    public class Breakpoint {
        public Point Left { get; }
        public Point Right { get; }

        public Breakpoint(Point left, Point right) {
            Left = left;
            Right = right;
        }
    }
    
    public class Voronoi {
        
        private List<Point> points;
        private List<Region> regions;
        private BeachLine beachLine;

        public Voronoi(List<Point> points) {
            this.points = points;
            this.regions = new List<Region>();
            this.beachLine = new BeachLine();
            foreach (Point point in points) {
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
                    handleCircleEvent(e, eventQueue);
                }
            }

            return regions;
        }

        private void handleSiteEvent(Event e, PriorityQueue<Event> eventQueue) {
            // Create a new region for the site
            Region newRegion = new Region(e.Point);
            regions.Add(newRegion);

            // Find the arc above the new site
            Arc aboveArc = findArcAbove(e.Point);

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

        private Arc findArcAbove(Point site) {
            // Logic to find the arc above the new site
            // This typically involves traversing the beach line
            return beachLine.FindArcAbove(site);
        }

        private void addCircleEvent(Breakpoint breakpoint, PriorityQueue<Event> eventQueue) {
            // Logic to add a circle event for the given breakpoint
            // This typically involves calculating the circle center and radius
            Point circleCenter = calculateCircleCenter(breakpoint);
            double radius = calculateCircleRadius(breakpoint, circleCenter);
            Point circleEventPoint = new Point { x = circleCenter.x, y = circleCenter.y - radius };

            // Add the circle event to the event queue
            eventQueue.enqueue(new Event(circleEventPoint, EventType.Circle), circleEventPoint.y);
        }

        private Point calculateCircleCenter(Breakpoint breakpoint) {
            // Logic to calculate the center of the circle formed by the breakpoint
            return new Point { x = (breakpoint.Left.x + breakpoint.Right.x) / 2, y = (breakpoint.Left.y + breakpoint.Right.y) / 2 };
        }

        private double calculateCircleRadius(Breakpoint breakpoint, Point center) {
            // Logic to calculate the radius of the circle formed by the breakpoint
            return Math.Sqrt(Math.Pow(breakpoint.Left.x - center.x, 2) + Math.Pow(breakpoint.Left.y - center.y, 2));
        }

        private void handleCircleEvent(Event e, PriorityQueue<Event> eventQueue) {
            // Remove the arc associated with the circle event
            Arc arcToRemove = beachLine.FindArcAbove(e.Point);
            if (arcToRemove == null) return;

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

            public Arc FindArcAbove(Point site) {
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

            private bool isAbove(Arc arc, Point site) {
                // Calculate the directrix of the arc's site
                double directrix = arc.Site.y;

                // Calculate the y-coordinate of the parabola at the x-coordinate of the site
                double parabolaY = (Math.Pow(site.x - arc.Site.x, 2) + Math.Pow(directrix, 2) - Math.Pow(site.y, 2)) / (2 * (directrix - site.y));

                // The site is above the arc if its y-coordinate is less than the parabola's y-coordinate
                return site.y < parabolaY;
            }
        }

        private class Event {
            public Point Point { get; }
            public EventType Type { get; }

            public Event(Point point, EventType type) {
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