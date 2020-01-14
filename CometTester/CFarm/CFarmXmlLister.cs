using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace CometTester.CFarm
{
    public class CFarmXmlLister
    {
        public CFarmDatatable _xmlDoc;
        public string _path;
        public CFarmXmlLister(string path)
        {
            _path = path;
            var serializer = new XmlSerializer(typeof(CFarmDatatable));
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                _xmlDoc = (CFarmDatatable) serializer.Deserialize(fileStream);
            }
        }

        [XmlRoot("CFarmDatatable")]
        public class CFarmDatatable
        {
            [XmlElement("CFarmCroplandsData")]
            public CFarmCroplandsData CFarmCroplandsData { get; set; }
            [XmlElement("CFarmAnimalAgData")]
            public CFarmAnimalAgData CFarmAnimalAgData { get; set; }
            [XmlElement("CFarmAgroforestryData")]
            public CFarmAgroforestryData CFarmAgroforestryData { get; set; }
            [XmlElement("CFarmForestryData")]
            public CFarmForestryData CFarmForestryData { get; set; }
            [XmlElement("CFarmReportData")]
            public CFarmReportData CFarmReportData { get; set; }
        }

        /// <summary>
        /// Beginning of CFarmCroplands tags
        /// </summary>
        public class CFarmCroplandsData
        {
            [XmlElement("Parcel")]
            public List<Parcel> Parcels { get; set; }
            [XmlElement("CroplandsScenario")]
            public List<CroplandsScenario> CroplandsScenarioList { get; set; }
        }

        public class Parcel
        {
            [XmlAttribute("GpsCoordinates")]
            public string GpsCoordinates { get; set; }
            [XmlAttribute("PointAcres")]
            public string PointAcres { get; set; }
            [XmlAttribute("DragAdjust")]
            public string DragAdjust { get; set; }
            [XmlAttribute("DragCount")]
            public string DragCount { get; set; }
            [XmlAttribute("PointAdjust")]
            public string PointAdjust { get; set; }
            [XmlAttribute("PolygonVertices")]
            public string PolygonVertices { get; set; }
            [XmlAttribute("Scroll")]
            public string Scroll { get; set; }    
            [XmlElement("HistoricManagement")]
            public HistoricManagement HistoricManagement { get; set; }
            
        }
        
        public class HistoricManagement
        {
            [XmlAttribute("Pre1980Management")]
            public string Pre1980Management { get; set; }
            [XmlAttribute("Crp")]
            public bool Crp { get; set; }
            [XmlAttribute("CrpStartYear")]
            public string CrpStartYear { get; set; }
            [XmlAttribute("CrpEndYear")]
            public string CrpEndYear { get; set; }
            [XmlAttribute("CrpType")]
            public string CrpType { get; set; }
            [XmlAttribute("PreCrpManagement")]
            public string PreCrpManagement { get; set; }
            [XmlAttribute("PreCrpTillage")]
            public string PreCrpTillage { get; set; }
            [XmlAttribute("PostCrpManagement")]
            public string PostCrpManagement { get; set; }
            [XmlAttribute("PostCrpTillage")]
            public string PostCrpTillage { get; set; }
            [XmlAttribute("To2000Management")]
            public string To2000Management { get; set; }
            [XmlAttribute("To2000Tillage")]
            public string To2000Tillage { get; set; }            
        }

        public class CroplandsScenario
        {
            [XmlElement("ScenarioData")]
            public List<ScenarioData> ScenarioDataList { get; set; }
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlAttribute("CopyFromBaseline")]
            public bool CopyFromBaseline { get; set; }
        }

        public class ScenarioData
        {
            [XmlAttribute("ParcelName")]
            public string ParcelName { get; set; }
            [XmlAttribute("Years")]
            public string Years { get; set; }
            [XmlElement("CropAndPlant")]
            public CropAndPlant CropAndPlant { get; set; }
            [XmlElement("Tillage")]
            public List<Tillage> TillageList { get; set; }
            [XmlElement("NitrogenApplication")]
            public List<NitrogenApplication> NitrogenApplicationList { get; set; }
            [XmlElement("ManureApplication")]
            public List<ManureApplication> ManureApplicationList { get; set; }
            [XmlElement("Irrigation")]
            public List<Irrigation> IrrigationList { get; set; }
            [XmlElement("Liming")]
            public List<Liming> LimingList { get; set; }
            [XmlElement("Burning")]
            public List<Burning> BurningList { get; set; }
        }

        public class CropAndPlant
        {
            [XmlAttribute("CropType")]
            public string CropType { get; set; }
            [XmlAttribute("PlantingDate")]
            public string PlantingDate { get; set; }
            [XmlAttribute("CoverCrop")]
            public string CoverCrop { get; set; }
            [XmlElement("Harvest")]
            public List<Harvest> HarvestList { get; set; }
            [XmlElement("Grazing")]
            public List<Grazing> GrazingList { get; set; }
        }

        public class Grazing
        {
            [XmlAttribute("StartDate")]
            public string StartDate { get; set; }
            [XmlAttribute("EndDate")]
            public string EndDate { get; set; }
            [XmlAttribute("RestPeriod")]
            public string RestPeriod { get; set; }
            [XmlAttribute("Utilization")]
            public string Utilization { get; set; }
        }

        public class Harvest
        {
            [XmlAttribute("Date")]
            public string Date { get; set; }
            [XmlAttribute("Grain")]
            public bool Grain { get; set; }
            [XmlAttribute("Yield")]
            public string Yield { get; set; }
            [XmlAttribute("Straw")]
            public string Straw { get; set; }
        }

        public class Tillage
        {
            [XmlAttribute("Date")]
            public string Date { get; set; }
            [XmlAttribute("Type")]
            public string Type { get; set; }
        }

        public class NitrogenApplication
        {
            [XmlAttribute("Date")]
            public string Date { get; set; }
            [XmlAttribute("Type")]
            public string Type { get; set; }
            [XmlAttribute("TotalApplied")]
            public string TotalApplied { get; set; }
            [XmlAttribute("Method")]
            public string Method { get; set; }
            [XmlAttribute("Eep")]
            public string Eep { get; set; }
        }

        public class ManureApplication
        {
            [XmlAttribute("Date")]
            public string Date { get; set; }
            [XmlAttribute("Type")]
            public string Type { get; set; }
            [XmlAttribute("AmountApplied")]
            public string AmountApplied { get; set; }
            [XmlAttribute("PercentN")]
            public string PercentN { get; set; }
            [XmlAttribute("CToNRatio")]
            public string CToNRatio { get; set; }
        }

        public class Irrigation
        {
            [XmlAttribute("Date")]
            public string Date { get; set; }
            [XmlAttribute("Applications")]
            public string Applications { get; set; }
            [XmlAttribute("Inches")]
            public string Inches { get; set; }
            [XmlAttribute("EndDate")]
            public string EndDate { get; set; }
            [XmlAttribute("Aerated")]
            public bool Aerated { get; set; }
        }

        public class Liming
        {
            [XmlAttribute("Date")]
            public string Date { get; set; }
            [XmlAttribute("Material")]
            public string Material { get; set; }
            [XmlAttribute("AmountApplied")]
            public string AmountApplied { get; set; }
        }

        public class Burning
        {
            [XmlAttribute("Burn")]
            public string Burn { get; set; }
        }

        /// <summary>
        /// Beginning of CFarmAnimalAg tags
        /// </summary>
        public class CFarmAnimalAgData
        {
            [XmlAttribute("ZipCode")]
            public string ZipCode { get; set; }
            [XmlAttribute("PreferredUnits")]
            public string PreferredUnits { get; set; }
            [XmlAttribute("TypeList")]
            public string TypeList { get; set; }
            [XmlElement("AnimalAgScenario")]
            public List<AnimalAgScenario> AnimalAgScenarioList { get; set; }
        }

        public class AnimalAgScenario
        {
            
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlAttribute("CopyFromFuture")]
            public bool CopyFromFuture { get; set; }
            [XmlElement("AnimalType")]
            public List<AnimalType> AnimalTypeList { get; set; }
        }

        public class AnimalType
        {
            [XmlAttribute("TypeName")]
            public string TypeName { get; set; }
            [XmlElement("HerdOrDiet")]
            public List<HerdOrDiet> HerdOrDietList { get; set; }
        }

        public class HerdOrDiet
        {
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlElement("AnimalDetails")]
            public AnimalDetails AnimalDetails { get; set; }
            [XmlElement("Weight")]
            public Weight Weight { get; set; }
            [XmlElement("HoursOfWork")]
            public HoursOfWork HoursOfWork { get; set; }
            [XmlElement("FeedDetails")]
            public FeedDetails FeedDetails { get; set; }
            [XmlElement("TypesOfFeed")]
            public TypesOfFeed TypesOfFeed { get; set; }
            [XmlElement("FeedingSituation")]
            public FeedingSituation FeedingSituation { get; set; }
            [XmlElement("ManureSystemTypes")]
            public ManureSystemTypes ManureSystemTypes { get; set; }
            [XmlElement("ManureSystemDetails")]
            public ManureSystemDetails ManureSystemDetails { get; set; }
            [XmlElement("ManureDetails")]
            public ManureDetails ManureDetails { get; set; }
            [XmlElement("Pregnancy")]
            public Pregnancy Pregnancy { get; set; }
            [XmlElement("HousingDetails")]
            public HousingDetails HousingDetails { get; set; }
        }

        public class AnimalDetails
        {
            [XmlElement("AverageMonthlyCount")]
            public List<AverageMonthlyCount> AverageMonthlyCountList { get; set; }
            [XmlAttribute("Adfi")]
            public string Adfi { get; set; }
            [XmlAttribute("Abw")]
            public string Abw { get; set; }
            [XmlAttribute("DaysInMilk")]
            public string DaysInMilk { get; set; }
            [XmlAttribute("MilkPerDay")]
            public string MilkPerDay { get; set; }
            [XmlAttribute("DaysOnFeed")]
            public string DaysOnFeed { get; set; }
            [XmlAttribute("CrudeProtein")]
            public string CrudeProtein { get; set; }
            [XmlAttribute("AverageEggWeight")]
            public string AverageEggWeight { get; set; }
            [XmlAttribute("EggsPerDay")]
            public string EggsPerDay { get; set; }
            [XmlAttribute("GestationLength")]
            public string GestationLength { get; set; }
            [XmlAttribute("LitterSize")]
            public string LitterSize { get; set; }
            [XmlAttribute("DressingPercentage")]
            public string DressingPercentage { get; set; }
            [XmlAttribute("FatFreeLeanPercentage")]
            public string FatFreeLeanPercentage { get; set; }
            [XmlAttribute("InitialBodyWeight")]
            public string InitialBodyWeight { get; set; }
            [XmlAttribute("FinalBodyWeight")]
            public string FinalBodyWeight { get; set; }
            [XmlAttribute("LactationLength")]
            public string LactationLength { get; set; }
            [XmlAttribute("LitterWeightAtBirth")]
            public string LitterWeightAtBirth { get; set; }
            [XmlAttribute("LitterWeightAtWeaning")]
            public string LitterWeightAtWeaning { get; set; }
            [XmlAttribute("GrowthCapacity")]
            public string GrowthCapacity { get; set; }
        }

        public class AverageMonthlyCount
        {
            [XmlAttribute("PopName")]
            public string PopName { get; set; }
            [XmlAttribute("Population")]
            public string Population { get; set; }
        }

        public class Weight
        {
            [XmlAttribute("Breed")]
            public string Breed { get; set; }
            [XmlAttribute("Tmw")]
            public string Tmw { get; set; }
            [XmlAttribute("Abw")]
            public string Abw { get; set; }
            [XmlAttribute("Adwg")]
            public string Adwg { get; set; }
            [XmlAttribute("Amw")]
            public string Amw { get; set; }
        }

        public class Pregnancy
        {
            [XmlAttribute("Pregnant")]
            public string Pregnant { get; set; }
            [XmlAttribute("Lactating")]
            public string Lactating { get; set; }
            [XmlAttribute("MilkPerDay")]
            public string MilkPerDay { get; set; }
            [XmlAttribute("FatContent")]
            public string FatContent { get; set; }
        }

        public class HoursOfWork
        {
            [XmlAttribute("Hours")]
            public string Hours { get; set; }
        }

        public class FeedDetails
        {
            [XmlAttribute("Ionophores")]
            public string Ionophores { get; set; }
            [XmlAttribute("FatContent")]
            public string FatContent { get; set; }
            [XmlAttribute("GrainType")]
            public string GrainType { get; set; }
            [XmlAttribute("ConcentratePercentage")]
            public string ConcentratePercentage { get; set; }
        }

        public class TypesOfFeed
        {
            [XmlElement("Feed")]
            public List<Feed> FeedList { get; set; }
        }

        public class Feed
        {
            [XmlAttribute("FeedStuff")]
            public string FeedStuff { get; set; }
            [XmlAttribute("Category")]
            public string Category { get; set; }
            [XmlAttribute("Percent")]
            public string Percent { get; set; }
        }

        public class FeedingSituation
        {
            [XmlAttribute("Type")]
            public string Type { get; set; }
        }

        public class HousingDetails
        {
            [XmlAttribute("Type")]
            public string Type { get; set; }
            [XmlAttribute("RoofedType")]
            public string RoofedType { get; set; }
            [XmlAttribute("DepthOrMix")]
            public string DepthOrMix { get; set; }
            [XmlAttribute("AreaOfBarn")]
            public string AreaOfBarn { get; set; }
            [XmlAttribute("DaysInHousing")]
            public string DaysInHousing { get; set; }
        }

        public class ManureSystemTypes
        {
            [XmlAttribute("Separator")]
            public string Separator { get; set; }
            [XmlAttribute("PrimaryMethod")]
            public string PrimaryMethod { get; set; }
            [XmlAttribute("SeparatorType")]
            public string SeparatorType { get; set; }
            [XmlAttribute("SolidMethod")]
            public string SolidMethod { get; set; }
            [XmlAttribute("LiquidMethod")]
            public string LiquidMethod { get; set; }
        }

        public class ManureSystemDetails
        {
            [XmlElement("TemporaryStackAndLongTermStockpile")]
            public TemporaryStackAndLongTermStockpile TemporaryStackAndLongTermStockpile { get; set; }
            [XmlElement("Composting")]
            public Composting Composting { get; set; }
            [XmlElement("AnaerobicLagoon")]
            public AnaerobicLagoon AnaerobicLagoon { get; set; }
            [XmlElement("AerobicLagoon")]
            public AerobicLagoon AerobicLagoon { get; set; }
            [XmlElement("AnaerobicDigester")]
            public AnaerobicDigester AnaerobicDigester { get; set; }
            [XmlElement("CombinedAerobicTreatment")]
            public CombinedAerobicTreatment CombinedAerobicTreatment { get; set; }
        }

        public class TemporaryStackAndLongTermStockpile
        {
            [XmlAttribute("CoverType")]
            public string CoverType { get; set; }
            [XmlAttribute("StorageTerm")]
            public string StorageTerm { get; set; }
        }

        public class Composting
        {
            [XmlAttribute("Method")]
            public string Method { get; set; }
        }

        public class AnaerobicLagoon
        {
            [XmlAttribute("CoverType")]
            public string CoverType { get; set; }
            [XmlAttribute("Area")]
            public string Area { get; set; }
            [XmlAttribute("Date")]
            public string Date { get; set; }
            [XmlAttribute("StorageLength")]
            public string StorageLength { get; set; }
            [XmlAttribute("SolidRemoval")]
            public string SolidRemoval { get; set; }
        }

        public class AerobicLagoon
        {
            [XmlAttribute("Volume")]
            public string Volume { get; set; }
            [XmlAttribute("Aeration")]
            public string Aeration { get; set; }
        }

        public class AnaerobicDigester
        {
            [XmlAttribute("Type")]
            public string Type { get; set; }
        }

        public class CombinedAerobicTreatment
        {
            [XmlAttribute("CoverType")]
            public string CoverType { get; set; }
            [XmlAttribute("Area")]
            public string Area { get; set; }
        }

        public class ManureDetails
        {
            [XmlAttribute("DryManureTotal")]
            public string DryManureTotal { get; set; }
            [XmlAttribute("PercentNitrogen")]
            public string PercentNitrogen { get; set; }
        }

        

        /// <summary>
        /// Beginning of Agroforestry tags
        /// </summary>
        public class CFarmAgroforestryData
        {
            [XmlElement("AgroforestryLocation")]
            public AgroforestryLocation AgroforestryLocation { get; set; }
            [XmlElement("AgroforestrySpecies")]
            public List<AgroforestrySpecies> AgroforestrySpeciesList { get; set; }
        }

        public class AgroforestryLocation
        {
            [XmlAttribute("States")]
            public string States { get; set; }
            [XmlAttribute("County")]
            public string County { get; set; }
        }

        public class AgroforestrySpecies
        {
            [XmlAttribute("SpeciesGroup")]
            public string SpeciesGroup { get; set; }
            [XmlAttribute("SpeciesName")]
            public string SpeciesName { get; set; }
            [XmlAttribute("Dbh")]
            public string Dbh { get; set; }
            [XmlAttribute("Age")]
            public string Age { get; set; }
            [XmlAttribute("TotalNumber")]
            public string TotalNumber { get; set; }
        }

        /// <summary>
        /// Beginning of Forestry tags
        /// </summary>
        public class CFarmForestryData
        {
            [XmlElement("ForestryParcel")]
            public List<ForestryParcel> ForestryParcelList { get; set; }
        }

        public class ForestryParcel
        {
            [XmlAttribute("GpsCoordinates")]
            public string GpsCoordinates { get; set; }
            [XmlAttribute("ParcelName")]
            public string ParcelName { get; set; }
            [XmlAttribute("PointAcres")]
            public string PointAcres { get; set; }
            [XmlAttribute("PolygonVertices")]
            public string PolygonVertices { get; set; }
            [XmlAttribute("DragAdjust")]
            public string DragAdjust { get; set; }
            [XmlAttribute("PointAdjust")]
            public string PointAdjust { get; set; }
            [XmlAttribute("DragCount")]
            public string DragCount { get; set; }
            [XmlAttribute("Scroll")]
            public string Scroll { get; set; }  
            [XmlElement("ForestryManagement")]
            public ForestryManagement ForestryManagement { get; set; }
        }

        public class ForestryManagement
        {
            [XmlAttribute("ForestType")]
            public string ForestType { get; set; }
            [XmlAttribute("PastCover")]
            public string PastCover { get; set; }
            [XmlAttribute("Age")]
            public string Age { get; set; }
            [XmlAttribute("Volume")]
            public string Volume { get; set; }
            [XmlAttribute("Prescription")]
            public string Prescription { get; set; }
            [XmlAttribute("ActivityYears")]
            public string ActivityYears { get; set; }
        }

        public class CFarmReportData
        {
            [XmlElement("CroplandsReportScenario")]
            public List<CroplandsReportScenario> CroplandsReportScenarioList { get; set; }
            [XmlElement("AnimalAgReportScenario")]
            public List<AnimalAgReportScenario> AnimalAgReportScenarioList { get; set; }
            [XmlElement("AgroforestryReportScenario")]
            public List<AgroforestryReportScenario> AgroforestryReportScenarioList { get; set; }
            [XmlElement("ForestryReportScenario")]
            public List<ForestryReportScenario> ForestryReportScenarioList { get; set; }
        }

        public class CroplandsReportScenario
        {
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlElement("CroplandsReport")]
            public List<CroplandsReport> CroplandsReportList { get; set; }
        }

        public class AnimalAgReportScenario
        {
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlElement("AnimalAgReport")]
            public List<AnimalAgReport> AnimalAgReportList { get; set; }
        }

        public class AgroforestryReportScenario
        {
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlElement("AgroforestryReport")]
            public List<AgroforestryReport> AgroforestryReportList { get; set; }
        }

        public class ForestryReportScenario
        {
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlElement("ForestryReport")]
            public List<ForestryReport> ForestryReportList { get; set; }
        }

        public class CroplandsReport
        {
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlAttribute("C")]
            public double C { get; set; }
            [XmlAttribute("Co2")]
            public double Co2 { get; set; }
            [XmlAttribute("Co")]
            public double Co { get; set; }
            [XmlAttribute("N2o")]
            public double N2o { get; set; }
            [XmlAttribute("Ch4")]
            public double Ch4 { get; set; }
            [XmlAttribute("Total")]
            public double Total { get; set; }
        }

        public class AnimalAgReport
        {
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlAttribute("Methane")]
            public double Methane { get; set; }
            [XmlAttribute("NitrousOxide")]
            public double NitrousOxide { get; set; }
            [XmlAttribute("Total")]
            public double Total { get; set; }
        }

        public class AgroforestryReport
        {
            [XmlAttribute("LiveTrees")]
            public double LiveTrees { get; set; }
            [XmlAttribute("DownedDeadWood")]
            public double DownedDeadWood { get; set; }
            [XmlAttribute("ForestFloor")]
            public double ForestFloor { get; set; }
            [XmlAttribute("StandingDeadTrees")]
            public double StandingDeadTrees { get; set; }
            [XmlAttribute("Understory")]
            public double Understory { get; set; }
            [XmlAttribute("Total")]
            public double Total { get; set; }
        }

        public class ForestryReport
        {
            [XmlAttribute("Name")]
            public string Name { get; set; }
            [XmlAttribute("LiveTrees")]
            public double LiveTrees { get; set; }
            [XmlAttribute("StandingDead")]
            public double StandingDead { get; set; }
            [XmlAttribute("ForestFloor")]
            public double ForestFloor { get; set; }            
            [XmlAttribute("Understory")]
            public double Understory { get; set; }
            [XmlAttribute("DownedDeadWood")]
            public double DownedDeadWood { get; set; }
            [XmlAttribute("SoilOrganic")]
            public double SoilOrganic { get; set; }
            [XmlAttribute("ProductsInUse")]
            public double ProductsInUse { get; set; }
            [XmlAttribute("InLandfills")]
            public double InLandfills { get; set; }
            [XmlAttribute("TotalStandCarbon")]
            public double TotalStandCarbon { get; set; }
            [XmlAttribute("HarvestedCarbon")]
            public double HarvestedCarbon { get; set; }
            [XmlAttribute("Total")]
            public double Total { get; set; }
        }
    }
}
