using Commerce6.Data.Domain.Sale;

namespace Commerce6.Infrastructure.Helpers
{
    public static class OrderStatusExtension
    {
        public static bool IsNotEdittable(this OrderState state)
        {
            return
                state == OrderState.Completed ||
                state == OrderState.Cancelled ||
                state == OrderState.Refunded;
        }






        public static bool IsApplicableByCustomer(this OrderState newState, OrderState currentState)
        {
            return newState switch
            {
                OrderState.Delivered => true,
                OrderState.Cancelled => true,
                OrderState.Backshipped => currentState == OrderState.Delivered,
                _ => false,
            };
        }

        public static bool IsApplicableByMerchant(this OrderState newState, OrderState currentState)
        {
            return newState switch
            {
                OrderState.Processing => currentState == OrderState.Pending,
                OrderState.Shipped => currentState < OrderState.Shipped,
                OrderState.Completed => currentState == OrderState.Delivered,
                OrderState.Cancelled => true,
                OrderState.Refunded => currentState == OrderState.Backshipped,
                _ => false,
            };
        }

        //IsApplicableByShipper
    }
}
