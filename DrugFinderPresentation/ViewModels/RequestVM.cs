namespace DrugFinderPresentation.ViewModels
{
    public class RequestVM
    {
        public int Id { get; set; }

        public string PatientName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public double DistanceKm { get; set; } 

        public List<DrugItemVM> Drugs { get; set; } = new();
    }

}
