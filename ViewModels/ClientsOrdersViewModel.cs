namespace AutofacTutorial.ViewModels
{
    public class ClientsOrdersViewModel
    {
        public int ClientId { get; }
        public List<OrderViewModel> Orders { get; }

        public ClientsOrdersViewModel(int clientId, List<OrderViewModel> orders)
        {
            ClientId = clientId;
            Orders = orders;
        }
    }
}
