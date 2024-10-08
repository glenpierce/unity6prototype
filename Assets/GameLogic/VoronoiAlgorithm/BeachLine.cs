﻿using System;

namespace GameLogic.VoronoiAlgorithm {
    public class BeachLine {
        private AVLTree<Arc> arcs;
        private LoggerInterface logger;

        public BeachLine(LoggerInterface logger) {
            arcs = new AVLTree<Arc>();
            this.logger = logger;
        }
        
        public bool isEmpty() {
            return arcs == null || arcs.root == null;
        }

        public Arc findArcAbove(Point<double> site) {
            Arc arc = getRootArc();
            logger.Log($"Finding arc above ({site.x}, {site.y})");
            if (arc == null) {
                logger.Log("No arcs in the beachline, creating a new arc");
                Arc newArc = new Arc(site);
                addArc(newArc);
                return newArc;
            }

            Arc lastArc = null;
            int i = 0;
            while (arc != null && i < 100) {
                i++; // prevent infinite loop
                lastArc = arc;
                double x = getXOfParabolaIntersection(arc, site.y);
                logger.Log($"Checking arc at ({arc.Site.x}, {arc.Site.y}) with intersection x: {x}");
                if (site.x < x) {
                    if (arc.LeftArc == null) break;
                    arc = arc.LeftArc;
                } else {
                    if (arc.RightArc == null) break;
                    arc = arc.RightArc;
                }
            }

            if (lastArc == null) {
                logger.Log("No arc found above the site, creating a new arc");
                Arc newArc = new Arc(site);
                addArc(newArc);
                return newArc;
            }

            return lastArc;
        }

        private Arc getRootArc() {
            return arcs.root?.value;
        }

        public void addArc(Arc newArc) {
            arcs.Insert(newArc);
        }

        public void removeArc(Arc arc) {
            arcs.Delete(arc);
        }

        private double getXOfParabolaIntersection(Arc arc, double y) {
            double deltaY = 2 * (arc.Site.y - y);
            double inverseDeltaY = 1 / deltaY;
            double negativeTwoTimesArcSiteX = -2 * arc.Site.x / deltaY;
            double constantTerm = (arc.Site.x * arc.Site.x + arc.Site.y * arc.Site.y - y * y) / deltaY;

            if (arc.LeftArc == null) return double.NegativeInfinity;

            double deltaYLeft = 2 * (arc.LeftArc.Site.y - y);
            double inverseDeltaYLeft = 1 / deltaYLeft;
            double negativeTwoTimesArcLeftSiteX = -2 * arc.LeftArc.Site.x / deltaYLeft;
            double constantTermLeft = (arc.LeftArc.Site.x * arc.LeftArc.Site.x + arc.LeftArc.Site.y * arc.LeftArc.Site.y - y * y) / deltaYLeft;

            double coefficientA = inverseDeltaY - inverseDeltaYLeft;
            double coefficientB = negativeTwoTimesArcSiteX - negativeTwoTimesArcLeftSiteX;
            double coefficientC = constantTerm - constantTermLeft;

            double discriminant = coefficientB * coefficientB - 4 * coefficientA * coefficientC;
            double intersectionX1 = (-coefficientB + Math.Sqrt(discriminant)) / (2 * coefficientA);
            double intersectionX2 = (-coefficientB - Math.Sqrt(discriminant)) / (2 * coefficientA);

            return arc.Site.y < arc.LeftArc.Site.y ? Math.Max(intersectionX1, intersectionX2) : Math.Min(intersectionX1, intersectionX2);
        }
    }
}