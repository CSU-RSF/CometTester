﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CometTester.CFarm
{
    class CroplandValues
    {
        public string ScenarioName { get; set; }
        public string ParcelName { get; set; }
        private List<double> Expected;
        private List<string> Actual;
        private List<string> PYield;

        public CroplandValues()
        {
            Expected = new List<double>();
            Actual = new List<string> { "", "", "", "", "", "" };
            PYield = new List<string> { "", "", "", "", "", "" };
        }

        public void SetExpected(CFarmXmlLister.CroplandsReport report)
        {
            Expected.Add(report.C);
            Expected.Add(report.Co2);
            Expected.Add(report.Co);
            Expected.Add(report.N2o);
            Expected.Add(report.Ch4);
            Expected.Add(report.Total);

        }

        public void SetActual(int key, string value)
        {
            Actual[key] = value;
        }

        public void SetPYield(int key, string value)
        {
            PYield[key] = value;
        }

        public List<double> GetExpected()
        {
            return Expected;
        }

        public List<string> GetActual()
        {
            return Actual;
        }

        public List<string> GetPYield()
        {
            return PYield;
        }

        public void CalculatePYield()
        {

            for (int i = 0; i < 6; i++)
            {
                double actualDouble = double.Parse(Actual[i]);
                double pYieldDouble;
                if (Expected[i] != 0)
                {
                    pYieldDouble = ((actualDouble) / Expected[i]) * 100; //Percent error of report value compared to expected value
                }
                else if (actualDouble == 0)
                {
                    pYieldDouble = 100;
                }
                else
                {
                    pYieldDouble = actualDouble;
                }

                PYield[i] = Math.Round(pYieldDouble).ToString();

                // Sets acceptable percentage color 
                var pYColor = "#04B404";      // green
                if (pYieldDouble >= 105 || pYieldDouble <= 95 && pYieldDouble != 0)
                {
                    pYColor = "#AEB404";         // yellow
                    if (pYieldDouble >= 110 || pYieldDouble <= 90)
                    {
                        pYColor = "#B40404";     // red
                    }
                }

                PYield[i] = "<font color=" + pYColor + ">" + PYield[i] + "%</font>";
            }
        }
    }
}
