using System;
using System.Collections.Generic;

namespace GameLogic.VoronoiAlgorithm {
    
    public enum EventType {
        Site,
        Circle
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
        }

        public List<Region> generate() {
            // Initialize event queue with site events
            PriorityQueue<Event> eventQueue = new PriorityQueue<Event>();
            foreach (Point<double> point in points) {
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
            Region newRegion = new Region(e.Point);
            regions.Add(newRegion);

            if (beachLine.isEmpty()) {
                // First point: create the initial arc
                Arc initialArc = new Arc(e.Point);
                initialArc.region = newRegion;
                beachLine.addArc(initialArc);
                return;
            }

            Arc aboveArc = beachLine.findArcAbove(e.Point);

            if (aboveArc == null) {
                return;
            }

            if (aboveArc.LeftBreakpoint == null && aboveArc.RightBreakpoint == null) {
                // Second point: split the initial arc into two arcs
                Arc newArc = new Arc(e.Point);
                newArc.region = newRegion;
                Breakpoint newBreakpoint = new Breakpoint(aboveArc.Site, e.Point);

                aboveArc.RightArc = newArc;
                aboveArc.RightBreakpoint = newBreakpoint;
                newArc.LeftArc = aboveArc;
                newArc.LeftBreakpoint = newBreakpoint;

                beachLine.addArc(newArc);
            } else {
                // Third point and beyond: handle normally
                Arc newArc = new Arc(e.Point);
                newArc.region = newRegion;
                beachLine.addArc(newArc);

                addCircleEvent(aboveArc, eventQueue);
            }
        }

        private void addCircleEvent(Arc arc, PriorityQueue<Event> eventQueue) {
            if (arc == null || arc.LeftArc == null || arc.RightArc == null) {
                return;
            }

            Point<double> a = arc.LeftArc.Site;
            Point<double> b = arc.Site;
            Point<double> c = arc.RightArc.Site;

            // Calculate the circumcenter of the triangle formed by points a, b, and c
            double d = 2 * (a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y));
            if (d == 0) return; // Points are collinear

            double ux = ((a.x * a.x + a.y * a.y) * (b.y - c.y) + (b.x * b.x + b.y * b.y) * (c.y - a.y) + (c.x * c.x + c.y * c.y) * (a.y - b.y)) / d;
            double uy = ((a.x * a.x + a.y * a.y) * (c.x - b.x) + (b.x * b.x + b.y * b.y) * (a.x - c.x) + (c.x * c.x + c.y * c.y) * (b.x - a.x)) / d;

            Point<double> circumcenter = new Point<double> { x = ux, y = uy };
            double radius = Math.Sqrt(Math.Pow(a.x - ux, 2) + Math.Pow(a.y - uy, 2));

            // The circle event occurs at the bottom of the circumcircle
            Point<double> circleEventPoint = new Point<double> { x = ux, y = uy - radius };

            // Add the circle event to the event queue
            Event circleEvent = new Event(circleEventPoint, EventType.Circle);
            circleEvent.Arc = arc;
            eventQueue.enqueue(circleEvent, circleEventPoint.y);
        }

        private Point<double> calculateCircleCenter(Breakpoint breakpoint) {
            // Logic to calculate the center of the circle formed by the breakpoint
            return new Point<double> { x = (breakpoint.LeftPoint.x + breakpoint.RightPoint.x) / 2, y = (breakpoint.LeftPoint.y + breakpoint.RightPoint.y) / 2 };
        }

        private double calculateCircleRadius(Breakpoint breakpoint, Point<double> center) {
            // Logic to calculate the radius of the circle formed by the breakpoint
            return Math.Sqrt(Math.Pow(breakpoint.LeftPoint.x - center.x, 2) + Math.Pow(breakpoint.LeftPoint.y - center.y, 2));
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

            // Update regions for the neighboring arcs
            if (arcToRemove.LeftArc != null && arcToRemove.RightArc != null) {
                Region leftRegion = arcToRemove.LeftArc.region;
                Region rightRegion = arcToRemove.RightArc.region;
                // Update the regions with the new Voronoi cell information
                // This typically involves adding the new edge to the regions
                // leftRegion.AddEdge(newEdge);
                // rightRegion.AddEdge(newEdge);
            }

            // Remove the arc from the beachline
            beachLine.removeArc(arcToRemove);

            // Create new breakpoints and arcs for the neighboring arcs
            Breakpoint newLeftBreakpoint = new Breakpoint(arcToRemove.LeftArc.Site, arcToRemove.RightArc.Site);
            Arc newArc = new Arc(arcToRemove.LeftArc.Site);
            newArc.region = arcToRemove.LeftArc.region;

            // Update the beachline with the new arc
            beachLine.addArc(newArc);
        }
    }
}