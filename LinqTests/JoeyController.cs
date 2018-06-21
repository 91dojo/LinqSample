namespace LinqTests
{
    internal class JoeyController
    {
        private readonly IOrderModel _model;

        public JoeyController(IOrderModel model)
        {
            _model = model;
        }

        public void DeleteAdultOrders()
        {
            this._model.Delete(o => o.Customer.Age >= 18);
        }
    }
}