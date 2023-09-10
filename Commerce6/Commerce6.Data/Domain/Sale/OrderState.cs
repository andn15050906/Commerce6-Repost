namespace Commerce6.Data.Domain.Sale
{
    public enum OrderState
    {
        Pending,                //order has been placed but not yet processed or verified
        Processing,             //order being prepared
        Shipped,                //order being shipped out
        Delivered,              //order successfully delivered
        Completed,              //order completed (and no longer editable)
        Cancelled,              //order has been cancelled either by the customer or merchant (and no longer editable)

        Backshipped,            //order being shipped back to merchant
        Refunded                //customer has been refunded
    }
}
