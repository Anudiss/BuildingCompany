namespace BuildingCompany.Connection
{
    public partial class Order
    {
        public Stage Stage
        {
            get => (Stage)OrderStage_id;
            set => OrderStage_id = (int)value;
        }
    }
}
