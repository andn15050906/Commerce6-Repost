using Commerce6.Data.Domain.Sale;
using Commerce6.Data.Domain.Contact;
using Commerce6.Infrastructure.Models.Sale;
using Commerce6.Infrastructure.Helpers;
using Commerce6.Web.Services.Abstraction;
using Commerce6.Web.Models.Sale.OrderDTOs;
using Commerce6.Data.Domain.AppUser;
using Commerce6.Data.Domain.Merchant;
using Serilog;

namespace Commerce6.Web.Services.SaleServices
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uow;

        public OrderService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public List<OrderDTO>? GetAsCustomer(string? userId)
        {
            return userId == null ? null : _uow.OrderRepo.Get(userId);
        }

        public List<OrderDTO>? GetAsMerchant(int? shopId)
        {
            return shopId == null ? null : _uow.OrderRepo.Get((int)shopId);
        }

        public StatusMessage Create(CreateOrderDTO dto, string? customerId)
        {
            if (customerId == null)
                return StatusMessage.Unauthorized;
            User? customer = _uow.UserRepo.Find(customerId);
            if (customer == null)
                return StatusMessage.Unauthorized;

            try
            {
                _uow.OrderRepo.Insert(Adapt(dto, customerId));
                _uow.Save();
                return StatusMessage.Created;
            }
            catch (Exception e)
            {
                throw e;
                //Ex: User didn't set address but uses defaultAddress
                Log.Debug(e.Message);
                return StatusMessage.BadRequest;
            }
        }

        public StatusMessage Update(MerchantUpdateOrderDTO dto, int? shopId)
        {
            if (shopId == null)
                return StatusMessage.Unauthorized;
            Shop? shop = _uow.ShopRepo.Find(shopId);
            if (shop == null)
                return StatusMessage.Unauthorized;

            Order? order = _uow.OrderRepo.GetByIdMinimum(dto.OrderId);
            if (order == null || order.State.IsNotEdittable())
                return StatusMessage.BadRequest;
            if (dto.State != null &&
                (!Enum.IsDefined(typeof(OrderState), dto.State) || !((OrderState)dto.State).IsApplicableByMerchant(order.State)))
                return StatusMessage.BadRequest;
            if (order.ShopId != shopId)
                return StatusMessage.Unauthorized;
            if (order.State > OrderState.Shipped)
                if (dto.Discount != null || dto.FromAddress != null || dto.DefaultAddress != null)
                    return StatusMessage.BadRequest;

            ApplyChanges(dto, order);
            _uow.Save();
            return StatusMessage.Ok;
        }

        public StatusMessage Update(CustomerUpdateOrderDTO dto, string? customerId)
        {
            if (customerId == null)
                return StatusMessage.Unauthorized;
            User? customer = _uow.UserRepo.Find(customerId);
            if (customer == null)
                return StatusMessage.Unauthorized;

            Order? order = _uow.OrderRepo.GetByIdMinimum(dto.OrderId);
            if (order == null || order.State.IsNotEdittable())
                return StatusMessage.BadRequest;
            if (dto.State != null)
            {
                if (!Enum.IsDefined(typeof(OrderState), dto.State))
                    return StatusMessage.BadRequest;
                OrderState state = (OrderState)dto.State;
                if (!state.IsApplicableByCustomer(order.State))
                    return StatusMessage.BadRequest;
                //prevent updating unalterable
                if ((OrderState)dto.State > OrderState.Pending)
                    if (dto.PaymentMethod != null || dto.Transporter != null || dto.ToAddress != null)
                        return StatusMessage.BadRequest;
            }

            ApplyChanges(dto, order);
            _uow.Save();
            return StatusMessage.Ok;
        }



        private Order Adapt(CreateOrderDTO _, string customerId)
        {
            Order order = new()
            {
                Id = Guid.NewGuid().ToString(),
                PaymentMethod = _.PaymentMethod,
                Transporter = _.Transporter,
                State = OrderState.Pending,
                CreatedTime = DateTime.Now,
                CustomerId = customerId,
                ShopId = _.ShopId,
            };
            if (_.DefaultAddress == true)
                order.ToAddressId = _uow.UserRepo.GetAddressId(customerId);
            else
                order.ToAddress = new Address
                {
                    Province = _.ToAddress.Province,
                    District = _.ToAddress.District,
                    Street = _.ToAddress.Street,
                    StreetNumber = _.ToAddress.StreetNumber
                };


            //Add products
            order.Order_Products = new List<Order_Product>();
            int price = 0;
            foreach (Order_ProductRequestDTO opr in _.Products)
            {
                order.Order_Products.Add(new Order_Product
                {
                    Quantity = opr.Quantity,
                    ProductId = opr.ProductId,
                    OrderId = order.Id
                });
                price += _uow.ProductRepo.GetPriceAfterDiscount(opr.ProductId) * opr.Quantity;
            }
            order.Price = price;

            return order;
        }

        private void ApplyChanges(MerchantUpdateOrderDTO _, Order order)
        {
            if (_.State != null)
            {
                OrderState state = (OrderState)_.State;
                order.State = state;
                if (state.IsNotEdittable())
                    order.CompletedTime = DateTime.Now;
            }
            if (_.Discount != null)
                order.Discount = (double)_.Discount;

            if (_.DefaultAddress != null)
                order.FromAddressId = _uow.ShopRepo.GetAddressId((int)order.ShopId!);
            else if (_.FromAddress != null)
                order.FromAddress = new Address
                {
                    Province = _.FromAddress.Province,
                    District = _.FromAddress.District,
                    Street = _.FromAddress.Street,
                    StreetNumber = _.FromAddress.StreetNumber
                };
        }

        private void ApplyChanges(CustomerUpdateOrderDTO _, Order order)
        {
            if (_.State != null)
            {
                OrderState state = (OrderState)_.State;
                order.State = state;
                if (state.IsNotEdittable())
                    order.CompletedTime = DateTime.Now;
            }
            if (_.PaymentMethod != null)
                order.PaymentMethod = _.PaymentMethod;
            if (_.Transporter != null)
                order.Transporter = _.Transporter;
            if (_.ToAddress != null)
                order.ToAddress = new Address
                {
                    Province = _.ToAddress.Province,
                    District = _.ToAddress.District,
                    Street = _.ToAddress.Street,
                    StreetNumber = _.ToAddress.StreetNumber
                };
        }

        //..Fee
    }
}
