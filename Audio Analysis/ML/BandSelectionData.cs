using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    class BandSelectionData
    {
        [LoadColumn(0)]
        public double BandIndex;

        [LoadColumn(1)]
        public double HighestPeak1;
        //[LoadColumn(2)]
        //public double HighestPeak2;
        //[LoadColumn(3)]
        //public double HighestPeak3;
        //[LoadColumn(4)]
        //public double HighestPeak4;
        //[LoadColumn(5)]
        //public double HighestPeak5;
        //[LoadColumn(6)]
        //public double HighestPeak6;
        //[LoadColumn(7)]
        //public double HighestPeak7;
        //[LoadColumn(8)]
        //public double HighestPeak8;
        //[LoadColumn(9)]
        //public double HighestPeak9;
        //[LoadColumn(10)]
        //public double HighestPeak10;
        //[LoadColumn(11)]
        //public double HighestPeak11;
        //[LoadColumn(12)]
        //public double HighestPeak12;
        //[LoadColumn(13)]
        //public double HighestPeak13;
        //[LoadColumn(14)]
        //public double HighestPeak14;
        //[LoadColumn(15)]
        //public double HighestPeak15;
        //[LoadColumn(16)]
        //public double HighestPeak16;
        //[LoadColumn(17)]
        //public double HighestPeak17;
        //[LoadColumn(18)]
        //public double HighestPeak18;
        //[LoadColumn(19)]
        //public double HighestPeak19;
        //[LoadColumn(20)]
        //public double HighestPeak20;

        [LoadColumn(21)]
        public double HighestSingleChange21;
        //[LoadColumn(22)]
        //public double HighestSingleChange22;
        //[LoadColumn(23)]
        //public double HighestSingleChange23;
        //[LoadColumn(24)]
        //public double HighestSingleChange24;
        //[LoadColumn(25)]
        //public double HighestSingleChange25;
        //[LoadColumn(26)]
        //public double HighestSingleChange26;
        //[LoadColumn(27)]
        //public double HighestSingleChange27;
        //[LoadColumn(28)]
        //public double HighestSingleChange28;
        //[LoadColumn(29)]
        //public double HighestSingleChange29;
        //[LoadColumn(30)]
        //public double HighestSingleChange30;
        //[LoadColumn(31)]
        //public double HighestSingleChange31;
        //[LoadColumn(32)]
        //public double HighestSingleChange32;
        //[LoadColumn(33)]
        //public double HighestSingleChange33;
        //[LoadColumn(34)]
        //public double HighestSingleChange34;
        //[LoadColumn(35)]
        //public double HighestSingleChange35;
        //[LoadColumn(36)]
        //public double HighestSingleChange36;
        //[LoadColumn(37)]
        //public double HighestSingleChange37;
        //[LoadColumn(38)]
        //public double HighestSingleChange38;
        //[LoadColumn(39)]
        //public double HighestSingleChange39;
        //[LoadColumn(40)]
        //public double HighestSingleChange40;

        //[LoadColumn("41", name: "HighestTotalChange41")]
        //public double HighestTotalChange41;
        //[LoadColumn("42", name: "HighestTotalChange42")]
        //public double HighestTotalChange42;
        //[LoadColumn("43", name: "HighestTotalChange43")]
        //public double HighestTotalChange43;
        //[LoadColumn("44", name: "HighestTotalChange44")]
        //public double HighestTotalChange44;
        //[LoadColumn("45", name: "HighestTotalChange45")]
        //public double HighestTotalChange45;
        //[LoadColumn("46", name: "HighestTotalChange46")]
        //public double HighestTotalChange46;
        //[LoadColumn("47", name: "HighestTotalChange47")]
        //public double HighestTotalChange47;
        //[LoadColumn("48", name: "HighestTotalChange48")]
        //public double HighestTotalChange48;
        //[LoadColumn("49", name: "HighestTotalChange49")]
        //public double HighestTotalChange49;
        //[LoadColumn("50", name: "HighestTotalChange50")]
        //public double HighestTotalChange50;
        //[LoadColumn("51", name: "HighestTotalChange51")]
        //public double HighestTotalChange51;
        //[LoadColumn("52", name: "HighestTotalChange52")]
        //public double HighestTotalChange52;
        //[LoadColumn("53", name: "HighestTotalChange53")]
        //public double HighestTotalChange53;
        //[LoadColumn("54", name: "HighestTotalChange54")]
        //public double HighestTotalChange54;
        //[LoadColumn("55", name: "HighestTotalChange55")]
        //public double HighestTotalChange55;
        //[LoadColumn("56", name: "HighestTotalChange56")]
        //public double HighestTotalChange56;
        //[LoadColumn("57", name: "HighestTotalChange57")]
        //public double HighestTotalChange57;
        //[LoadColumn("58", name: "HighestTotalChange58")]
        //public double HighestTotalChange58;
        //[LoadColumn("59", name: "HighestTotalChange59")]
        //public double HighestTotalChange59;
        //[LoadColumn("60", name: "HighestTotalChange60")]
        //public double HighestTotalChange60;

        //[LoadColumn("61", name: "HighestDynamicRange61")]
        //public double HighestDynamicRange61;
        //[LoadColumn("62", name: "HighestDynamicRange62")]
        //public double HighestDynamicRange62;
        //[LoadColumn("63", name: "HighestDynamicRange63")]
        //public double HighestDynamicRange63;
        //[LoadColumn("64", name: "HighestDynamicRange64")]
        //public double HighestDynamicRange64;
        //[LoadColumn("65", name: "HighestDynamicRange65")]
        //public double HighestDynamicRange65;
        //[LoadColumn("66", name: "HighestDynamicRange66")]
        //public double HighestDynamicRange66;
        //[LoadColumn("67", name: "HighestDynamicRange67")]
        //public double HighestDynamicRange67;
        //[LoadColumn("68", name: "HighestDynamicRange68")]
        //public double HighestDynamicRange68;
        //[LoadColumn("69", name: "HighestDynamicRange69")]
        //public double HighestDynamicRange69;
        //[LoadColumn("70", name: "HighestDynamicRange70")]
        //public double HighestDynamicRange70;
        //[LoadColumn("71", name: "HighestDynamicRange71")]
        //public double HighestDynamicRange71;
        //[LoadColumn("72", name: "HighestDynamicRange72")]
        //public double HighestDynamicRange72;
        //[LoadColumn("73", name: "HighestDynamicRange73")]
        //public double HighestDynamicRange73;
        //[LoadColumn("74", name: "HighestDynamicRange74")]
        //public double HighestDynamicRange74;
        //[LoadColumn("75", name: "HighestDynamicRange75")]
        //public double HighestDynamicRange75;
        //[LoadColumn("76", name: "HighestDynamicRange76")]
        //public double HighestDynamicRange76;
        //[LoadColumn("77", name: "HighestDynamicRange77")]
        //public double HighestDynamicRange77;
        //[LoadColumn("78", name: "HighestDynamicRange78")]
        //public double HighestDynamicRange78;
        //[LoadColumn("79", name: "HighestDynamicRange79")]
        //public double HighestDynamicRange79;
        //[LoadColumn("80", name: "HighestDynamicRange80")]
        //public double HighestDynamicRange80;

        //[LoadColumn("81", name: "BestPeakToAverage81")]
        //public double BestPeakToAverage81;
        //[LoadColumn("82", name: "BestPeakToAverage82")]
        //public double BestPeakToAverage82;
        //[LoadColumn("83", name: "BestPeakToAverage83")]
        //public double BestPeakToAverage83;
        //[LoadColumn("84", name: "BestPeakToAverage84")]
        //public double BestPeakToAverage84;
        //[LoadColumn("85", name: "BestPeakToAverage85")]
        //public double BestPeakToAverage85;
        //[LoadColumn("86", name: "BestPeakToAverage86")]
        //public double BestPeakToAverage86;
        //[LoadColumn("87", name: "BestPeakToAverage87")]
        //public double BestPeakToAverage87;
        //[LoadColumn("88", name: "BestPeakToAverage88")]
        //public double BestPeakToAverage88;
        //[LoadColumn("89", name: "BestPeakToAverage89")]
        //public double BestPeakToAverage89;
        //[LoadColumn("90", name: "BestPeakToAverage90")]
        //public double BestPeakToAverage90;
        //[LoadColumn("91", name: "BestPeakToAverage91")]
        //public double BestPeakToAverage91;
        //[LoadColumn("92", name: "BestPeakToAverage92")]
        //public double BestPeakToAverage92;
        //[LoadColumn("93", name: "BestPeakToAverage93")]
        //public double BestPeakToAverage93;
        //[LoadColumn("94", name: "BestPeakToAverage94")]
        //public double BestPeakToAverage94;
        //[LoadColumn("95", name: "BestPeakToAverage95")]
        //public double BestPeakToAverage95;
        //[LoadColumn("96", name: "BestPeakToAverage96")]
        //public double BestPeakToAverage96;
        //[LoadColumn("97", name: "BestPeakToAverage97")]
        //public double BestPeakToAverage97;
        //[LoadColumn("98", name: "BestPeakToAverage98")]
        //public double BestPeakToAverage98;
        //[LoadColumn("99", name: "BestPeakToAverage99")]
        //public double BestPeakToAverage99;
        //[LoadColumn("100", name: "BestPeakToAverage100")]
        //public double BestPeakToAverage100;

    }
}
