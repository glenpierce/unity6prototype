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
            this.beachLine = new BeachLine(logger);
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
            logger.Log(e.Point.x + ", " + e.Point.y);

            if (beachLine.isEmpty()) {
                logger.Log("beachLine is empty");
                Arc initialArc = new Arc(e.Point);
                initialArc.region = newRegion;
                beachLine.addArc(initialArc);
                return;
            }

            Arc aboveArc = beachLine.findArcAbove(e.Point);

            if (aboveArc == null) {
                logger.Log("aboveArc is null");
                return;
            }

            Arc newArc = new Arc(e.Point);
            newArc.region = newRegion;

            Arc leftArc = new Arc(aboveArc.Site);
            leftArc.region = aboveArc.region;

            Arc rightArc = new Arc(aboveArc.Site);
            rightArc.region = aboveArc.region;

            leftArc.RightArc = newArc;
            newArc.LeftArc = leftArc;
            newArc.RightArc = rightArc;
            rightArc.LeftArc = newArc;

            aboveArc.LeftArc = leftArc;
            aboveArc.RightArc = rightArc;

            beachLine.addArc(leftArc);
            beachLine.addArc(newArc);
            beachLine.addArc(rightArc);

            // Add edges to regions
            aboveArc.region.addEdge(aboveArc.Site, e.Point);
            newRegion.addEdge(aboveArc.Site, e.Point);

            addCircleEvent(leftArc, eventQueue);
            addCircleEvent(rightArc, eventQueue);
        }

        private void addCircleEvent(Arc arc, PriorityQueue<Event> eventQueue) {
            logger.Log("adding circle event");
            if (arc == null || arc.LeftArc == null || arc.RightArc == null) {
                logger.Log("arc is null");
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
            logger.Log("Circle event: " + circleEventPoint.x + ", " + circleEventPoint.y);

            // Add the circle event to the event queue
            Event circleEvent = new Event(circleEventPoint, EventType.Circle);
            circleEvent.Arc = arc;
            eventQueue.enqueue(circleEvent, circleEventPoint.y);
        }

        private void handleCircleEvent(Event e, PriorityQueue<Event> eventQueue, LoggerInterface logger) {
            // Remove the arc associated with the circle event
            logger.Log("HandleCircleEvent: " + e.Point.x + ", " + e.Point.y);
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
                leftRegion.addEdge(arcToRemove.LeftArc.Site, arcToRemove.RightArc.Site);
                rightRegion.addEdge(arcToRemove.LeftArc.Site, arcToRemove.RightArc.Site);
            }

            // Remove the arc from the beachline
            beachLine.removeArc(arcToRemove);
        }
    }
}