namespace BuildingCompany.Connection
{
    public partial class Order
    {
        public Stage Stage
        {
            get => Stages.AllStages[OrderStage_id];
            set => OrderStage_id = Stages.AllStages.IndexOf(value);
        }

        public void Delete() => IsDeleted = true;
    }
}
